namespace PotionMaster
{
    public partial class Form1 : Form
    {
        private Stack<Move> moveHistory = new Stack<Move>();
        private int currentScore = 0;
        private int totalUndos;
        private int usedUndos;

        private readonly ColorPreset LightPreset = new ColorPreset(
            Color.White,
            Color.FromKnownColor(KnownColor.Control),
            Color.Black,
            Color.Pink
        );

        private readonly ColorPreset DarkPreset = new ColorPreset(
            Color.FromArgb(60, 60, 60),
            Color.FromArgb(100, 100, 100),
            Color.White,
            Color.Pink
        );

        public Form1()
        {
            InitializeComponent();
            ApplyTheme();
            InitializeNewGame();
            GenerateNewPuzzle();
            UpdateScore();
            UpdateUndoState();
        }

        private void tablePotionLayout_Paint(object sender, PaintEventArgs e)
        {

        }





















        private void vialControl1_Load(object sender, EventArgs e)
        {

        }

        private void vialControl2_Load(object sender, EventArgs e)
        {

        }

        private void vialControl3_Load(object sender, EventArgs e)
        {

        }

        private void vialControl4_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void vialControl1_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(sender, e);
        }

        private void vialControl1_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(sender, e);
        }

        private void vialControl2_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(sender, e);
        }

        private void vialControl2_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(sender, e);
        }

        private void vialControl3_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(sender, e);
        }

        private void vialControl3_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(sender, e);
        }

        private void vialControl4_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(sender, e);
        }

        private void vialControl4_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(sender, e);
        }



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

        private void buttonNextPuzzle_Click(object sender, EventArgs e)
        {
            // Reset game state
            labelCongratulations.Visible = false;
            buttonNextPuzzle.Enabled = false;
            UpdateUndoState();
            GenerateNewPuzzle();
            UpdateScore();
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

        private void CheckCompletion()
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

            labelCongratulations.Visible = allValid;
            labelCongratulations.Enabled = allValid;
            buttonNextPuzzle.Enabled = allValid;
            if (allValid) currentScore += CalculatePuzzleScore();
            UpdateScore();
        }

        private void InitializeNewGame()
        {
            totalUndos = Properties.Settings.Default.Difficulty switch
            {
                "Easy" => 3,
                "Medium" => 2,
                "Hard" => 1,
                _ => 3
            };
            moveHistory.Clear();
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

            // Calculate minimum form size
            int minWidth = 100 * columns;
            int minHeight = 260 * rows + 80;

            this.MinimumSize = new Size(minWidth, minHeight);
        }




        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ApplyTheme()
        {
            var theme = Properties.Settings.Default.ColorTheme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? DarkPreset
                : LightPreset;

            // Apply to entire form and controls
            this.BackColor = theme.Background;
            this.ForeColor = theme.Text;
            ApplyThemeToControls(this.Controls, theme);

            // Apply to MenuStrip
            menuStrip1.BackColor = theme.Background;
            menuStrip1.ForeColor = theme.Text;
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Renderer = new ThemedMenuRenderer(
                theme.Accent,      
                theme.Background,  
                theme.Text        
            );
        }




        private void ApplyThemeToControls(Control.ControlCollection controls, ColorPreset theme)
        {
            foreach (Control control in controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = theme.Button;
                    btn.ForeColor = theme.Text;
                }
                else if (control is Label lbl)
                {
                    lbl.ForeColor = theme.Text;
                }
                else if (control is Panel || control is TableLayoutPanel)
                {
                    control.BackColor = theme.Background;
                    control.ForeColor = theme.Text;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, theme);
                }
            }
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
                    ApplyTheme();

                    // Check if game-relevant settings changed (exclude ColorTheme)
                    bool gameSettingsChanged =
                        Properties.Settings.Default.Difficulty != originalDifficulty ||
                        Properties.Settings.Default.MaxSegments != originalMaxSegments ||
                        Properties.Settings.Default.VialCount != originalVialCount;

                    if (gameSettingsChanged)
                    {
                        InitializeNewGame();
                        GenerateNewPuzzle();
                        currentScore = 0;
                        UpdateScore();
                        moveHistory.Clear();
                        UpdateUndoState();
                        labelCongratulations.Visible = false;
                        buttonNextPuzzle.Enabled = false;
                    }
                }
            }
        }

        private void newGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            InitializeNewGame();
            GenerateNewPuzzle();
            currentScore = 0;
            UpdateScore();
            labelCongratulations.Visible = false;
            buttonNextPuzzle.Enabled = false;
            moveHistory.Clear();
            UpdateUndoState();
        }

        private void endGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // On End Game: save best score if needed
            UpdateScore();
            SaveBestIfNeeded();

            // Then reset game state
            InitializeNewGame();
            GenerateNewPuzzle();
            currentScore = 0;
            UpdateScore();
            labelCongratulations.Visible = false;
            buttonNextPuzzle.Enabled = false;
            moveHistory.Clear();
            UpdateUndoState();
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

    class ColorPreset
    {
        public Color Background { get; }
        public Color Button { get; }
        public Color Text { get; }
        public Color Accent { get; }

        public ColorPreset(Color background, Color button, Color text, Color accent)
        {
            Background = background;
            Button = button;
            Text = text;
            Accent = accent;
        }
    }

    public class ThemeColorTable : ProfessionalColorTable
    {
        readonly Color _accent, _background;
        public ThemeColorTable(Color accent, Color background)
        {
            _accent = accent;
            _background = background;
        }

        // menu-bar background
        public override Color ToolStripGradientBegin => _background;
        public override Color ToolStripGradientMiddle => _background;
        public override Color ToolStripGradientEnd => _background;

        // dropdown background
        public override Color ToolStripDropDownBackground
            => _background;

        // hover/pressed highlight
        public override Color MenuItemSelected
            => _accent;
        public override Color MenuItemSelectedGradientBegin
            => _accent;
        public override Color MenuItemSelectedGradientEnd
            => _accent;
        public override Color MenuItemPressedGradientBegin
            => _accent;
        public override Color MenuItemPressedGradientMiddle
            => _accent;
        public override Color MenuItemPressedGradientEnd
            => _accent;

    }

    public class ThemedMenuRenderer : ToolStripProfessionalRenderer
    {
        private readonly Color _text, _background;

        public ThemedMenuRenderer(Color accent, Color background, Color text)
            : base(new ThemeColorTable(accent, background))
        {
            _text = text;
            _background = background;
        }

        // paint the entire drop-down background
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(_background),
                e.AffectedBounds
            );
        }

        // left icon-margin in the same background
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(_background),
                e.AffectedBounds
            );
        }

        // force *all* menu-item text to theme color
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = _text;
            base.OnRenderItemText(e);
        }


    }
}
