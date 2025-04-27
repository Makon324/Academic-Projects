namespace PotionMaster
{
    public partial class Form1 : Form
    {
        private Stack<Move> moveHistory = new Stack<Move>();
        private int currentScore = 0;
        private int totalUndos;

        public Form1()
        {
            InitializeComponent();

            // Initialize theme based on settings
            string colorTheme = Properties.Settings.Default.ColorTheme;
            Theme.SetTheme(colorTheme == "Dark" ? Theme.DarkPreset : Theme.LightPreset);
            Theme.ApplyTheme(this);

            // Apply menu renderer
            menuStrip1.Renderer = Theme.CreateMenuRenderer();

            StartNewGame(true);
        }


        // POURING ----------------------

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SourceVial") &&
                e.Data.GetDataPresent("Color") &&
                e.Data.GetDataPresent("Quantity"))
            {
                VialControl targetVial = (VialControl)sender;
                Color color = (Color)e.Data.GetData("Color");
                int quant = (int)e.Data.GetData("Quantity");

                if (targetVial.CanPour(color, quant))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SourceVial") &&
                e.Data.GetDataPresent("Color") &&
                e.Data.GetDataPresent("Quantity"))
            {
                VialControl sourceVial = (VialControl)e.Data.GetData("SourceVial");
                Color color = (Color)e.Data.GetData("Color");
                int quant = (int)e.Data.GetData("Quantity");
                VialControl targetVial = (VialControl)sender;

                if (targetVial.CanPour(color, quant))
                {
                    targetVial.Pour(color, quant);
                    sourceVial.RemoveSegments(quant);
                    moveHistory.Push(new Move
                    {
                        Source = sourceVial,
                        Target = targetVial,
                        Color = color,
                        Quantity = quant
                    });
                    UpdateUndoState();
                    UpdateScore();
                    CheckCompletion();
                }
            }
        }


        // NEW GAME/PUZZLE LOGIC ----------------------

        private void StartNewGame(bool resetScore)
        {
            // Set score/undos
            if (resetScore)
            {
                currentScore = 0;
                totalUndos = Properties.Settings.Default.Difficulty switch
                {
                    "Easy" => 3,
                    "Medium" => 2,
                    "Hard" => 1,
                    _ => 3
                };
            }
            moveHistory.Clear();

            // Start new puzzle
            GenerateNewPuzzle();            
            labelCongratulations.Visible = false;
            buttonNextPuzzle.Enabled = false;
            UpdateUndoState();
            UpdateScore();
        }

        private bool isPuzzleCompleted()
        {
            bool allValid = true;
            foreach (Control c in tablePotionLayout.Controls)
            {
                VialControl vial = c as VialControl;
                if (vial == null) continue;

                // Check non-empty vials
                if (vial.Segments.Count > 0)
                {
                    // Must be FULL and have UNIFORM COLOR
                    bool isFull = vial.Segments.Count == Properties.Settings.Default.MaxSegments;
                    bool isUniform = vial.Segments.All(color => color == vial.Segments[0]);

                    if (!isFull || !isUniform)
                    {
                        allValid = false;
                        break;
                    }
                }
            }
            return allValid;
        }

        private void CheckCompletion()
        {
            bool allValid = isPuzzleCompleted();

            labelCongratulations.Visible = allValid;
            labelCongratulations.Enabled = allValid;
            buttonNextPuzzle.Enabled = allValid;
            if (allValid) currentScore += CalculatePuzzleScore();
            UpdateScore();
        }

        private void GenerateNewPuzzle()
        {
            tablePotionLayout.Controls.Clear();
            int vialCount = Properties.Settings.Default.VialCount;
            int maxSegments = Properties.Settings.Default.MaxSegments;
            int emptyVials = GetEmptyVialCount(Properties.Settings.Default.Difficulty);

            // Generate colors and segments
            List<Color> colors = GenerateDistinctColors(vialCount - emptyVials);
            List<Color> allSegments = colors.SelectMany(c => Enumerable.Repeat(c, maxSegments)).ToList();
            Shuffle(allSegments);

            // Initialize vialSegments list
            List<List<Color>> vialSegments = new List<List<Color>>();

            // Distribute segments to non-empty vials
            for (int i = 0; i < vialCount - emptyVials; i++)
            {
                vialSegments.Add(allSegments.Skip(i * maxSegments).Take(maxSegments).ToList());
            }

            // Add empty vials
            for (int i = 0; i < emptyVials; i++)
            {
                vialSegments.Add(new List<Color>());
            }

            // Configure grid layout
            int columns = Math.Min(vialCount, 7);
            int rows = (vialCount + columns - 1) / columns;
            tablePotionLayout.ColumnCount = columns;
            tablePotionLayout.RowCount = rows;
            tablePotionLayout.ColumnStyles.Clear();
            tablePotionLayout.RowStyles.Clear();

            // Set equal spacing for rows/columns
            for (int c = 0; c < columns; c++)
                tablePotionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            for (int r = 0; r < rows; r++)
                tablePotionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));

            // Add vials to the grid
            for (int i = 0; i < vialCount; i++)
            {
                VialControl vial = new VialControl();
                vial.MaxSegments = maxSegments;
                vial.Segments = vialSegments[i];

                // Set fixed size and centering properties
                vial.Size = new Size(100, 200);
                vial.Anchor = AnchorStyles.None;
                vial.Dock = DockStyle.None;
                vial.Margin = new Padding(0);

                // Add to table layout with row/column positioning
                tablePotionLayout.Controls.Add(vial, i % columns, i / columns);

                // Center the control in its cell
                tablePotionLayout.SetCellPosition(vial, new TableLayoutPanelCellPosition(i % columns, i / columns));

                // Event handlers
                vial.DragEnter += HandleDragEnter;
                vial.DragDrop += HandleDragDrop;
            }

            // Set minimum form size
            int minWidth = 100 * columns;
            int minHeight = 260 * rows + 80;
            this.MinimumSize = new Size(minWidth, minHeight);

            if (isPuzzleCompleted())
                GenerateNewPuzzle();
        }

        private void buttonNextPuzzle_Click(object sender, EventArgs e)
        {
            StartNewGame(false);
            currentScore += CalculatePuzzleScore();
            UpdateScore();
        }

        private void newGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            StartNewGame(true);
        }

        private void endGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveBestIfNeeded();
            StartNewGame(true);
        }


        // ----------------------

        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (totalUndos <= 0 || moveHistory.Count == 0) return;

            var move = moveHistory.Pop();
            move.Target.RemoveSegments(move.Quantity);
            move.Source.Pour(move.Color, move.Quantity);
            totalUndos--;
            UpdateUndoState();
            CheckCompletion();
        }

        private void UpdateUndoState()
        {
            buttonUndo.Enabled = totalUndos > 0 && moveHistory.Count > 0;
            labelUndosLeft.Text = $"Left: {totalUndos}";
        }

        private int CalculatePuzzleScore()
        {
            int difficultyScore = Properties.Settings.Default.Difficulty switch
            {
                "Easy" => 1,
                "Medium" => 2,
                "Hard" => 3,
                _ => 1
            };
            int maxSegments = Properties.Settings.Default.MaxSegments;
            int vialCount = Properties.Settings.Default.VialCount;
            int emptyVials = GetEmptyVialCount(Properties.Settings.Default.Difficulty);

            return difficultyScore * maxSegments * (vialCount - emptyVials);
        }

        private void UpdateScore()
        {
            labelScore.Text = $"Score: {currentScore}";
            labelBestScore.Text = $"Best Score: {Properties.Settings.Default.BestScore}";
        }

        private void SaveBestIfNeeded()
        {
            if (currentScore > Properties.Settings.Default.BestScore)
            {
                Properties.Settings.Default.BestScore = currentScore;
                Properties.Settings.Default.Save();
            }
        }

        private int GetEmptyVialCount(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 3,
                "Medium" => 2,
                "Hard" => 1,
                _ => 3
            };
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // HSL to RGB conversion helper
        private Color HSLToRGB(float h, float s, float l)
        {
            float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
            float p = 2 * l - q;
            float[] rgb = new float[3];

            h /= 360f; // Convert hue to 0-1 range

            for (int i = 0; i < 3; i++)
            {
                float t = (h + (1 - i) / 3f) % 1f;
                t = t < 0 ? t + 1 : t;
                rgb[i] = t < 1f / 6f ? p + (q - p) * 6f * t :
                        t < 0.5f ? q :
                        t < 2f / 3f ? p + (q - p) * (2f / 3f - t) * 6f :
                        p;
            }

            return Color.FromArgb(
                (int)(rgb[0] * 255),
                (int)(rgb[1] * 255),
                (int)(rgb[2] * 255)
            );
        }

        private List<Color> GenerateDistinctColors(int count)
        {
            List<Color> colors = new List<Color>();
            double goldenRatioConjugate = 0.618033988749895;

            for (int i = 0; i < count; i++)
            {
                float hue = (float)((i * goldenRatioConjugate * 360) % 360);
                float saturation = 0.85f;
                float lightness = 0.55f;

                colors.Add(HSLToRGB(hue, saturation, lightness));
            }
            return colors;
        }

        private void openSettingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string originalDifficulty = Properties.Settings.Default.Difficulty;
            int originalMaxSegments = Properties.Settings.Default.MaxSegments;
            int originalVialCount = Properties.Settings.Default.VialCount;
            string originalColorTheme = Properties.Settings.Default.ColorTheme;

            using (var settingsForm = new SettingsForm())
            {
                settingsForm.BackColor = this.BackColor;
                settingsForm.ForeColor = this.ForeColor;

                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    Theme.SetTheme(Properties.Settings.Default.ColorTheme == "Dark" ? Theme.DarkPreset : Theme.LightPreset);
                    Theme.ApplyTheme(this);
                    menuStrip1.Renderer = Theme.CreateMenuRenderer();

                    // Check if game-relevant settings changed (exclude ColorTheme)
                    bool gameSettingsChanged =
                        Properties.Settings.Default.Difficulty != originalDifficulty ||
                        Properties.Settings.Default.MaxSegments != originalMaxSegments ||
                        Properties.Settings.Default.VialCount != originalVialCount;

                    if (gameSettingsChanged)
                    {
                        if (gameSettingsChanged)
                        {
                            StartNewGame(true);
                        }
                    }
                }
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    class Move
    {
        public VialControl Source { get; set; }
        public VialControl Target { get; set; }
        public Color Color { get; set; }
        public int Quantity { get; set; }
    }

    
}
