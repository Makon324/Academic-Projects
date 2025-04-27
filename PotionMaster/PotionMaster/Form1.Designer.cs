namespace PotionMaster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            gameToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            endGameToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            openSettingsToolStripMenuItem = new ToolStripMenuItem();
            tablePotionLayout = new TableLayoutPanel();
            controlPanel = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            labelUndosLeft = new Label();
            buttonUndo = new Button();
            buttonNextPuzzle = new Button();
            labelCongratulations = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            labelScore = new Label();
            labelBestScore = new Label();
            menuStrip1.SuspendLayout();
            controlPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { gameToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(784, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newGameToolStripMenuItem, endGameToolStripMenuItem, exitToolStripMenuItem });
            gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            gameToolStripMenuItem.Size = new Size(50, 20);
            gameToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(132, 22);
            newGameToolStripMenuItem.Text = "New Game";
            newGameToolStripMenuItem.Click += newGameToolStripMenuItem_Click_1;
            // 
            // endGameToolStripMenuItem
            // 
            endGameToolStripMenuItem.Name = "endGameToolStripMenuItem";
            endGameToolStripMenuItem.Size = new Size(132, 22);
            endGameToolStripMenuItem.Text = "End Game";
            endGameToolStripMenuItem.Click += endGameToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(132, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSettingsToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // openSettingsToolStripMenuItem
            // 
            openSettingsToolStripMenuItem.Name = "openSettingsToolStripMenuItem";
            openSettingsToolStripMenuItem.Size = new Size(148, 22);
            openSettingsToolStripMenuItem.Text = "Open Settings";
            openSettingsToolStripMenuItem.Click += openSettingsToolStripMenuItem_Click_1;
            // 
            // tablePotionLayout
            // 
            tablePotionLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tablePotionLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tablePotionLayout.ColumnCount = 4;
            tablePotionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tablePotionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tablePotionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tablePotionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tablePotionLayout.Location = new Point(0, 24);
            tablePotionLayout.Name = "tablePotionLayout";
            tablePotionLayout.RowCount = 1;
            tablePotionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePotionLayout.Size = new Size(784, 357);
            tablePotionLayout.TabIndex = 1;
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(tableLayoutPanel1);
            controlPanel.Dock = DockStyle.Bottom;
            controlPanel.Location = new Point(0, 381);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(784, 80);
            controlPanel.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonNextPuzzle, 3, 0);
            tableLayoutPanel1.Controls.Add(labelCongratulations, 2, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(784, 80);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(labelUndosLeft);
            panel1.Controls.Add(buttonUndo);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(150, 74);
            panel1.TabIndex = 0;
            // 
            // labelUndosLeft
            // 
            labelUndosLeft.Anchor = AnchorStyles.Left;
            labelUndosLeft.AutoSize = true;
            labelUndosLeft.Font = new Font("Segoe UI", 6F, FontStyle.Regular, GraphicsUnit.Point);
            labelUndosLeft.Location = new Point(25, 40);
            labelUndosLeft.Name = "labelUndosLeft";
            labelUndosLeft.Size = new Size(31, 11);
            labelUndosLeft.TabIndex = 1;
            labelUndosLeft.Text = "(Left: 0)";
            // 
            // buttonUndo
            // 
            buttonUndo.Anchor = AnchorStyles.Left;
            buttonUndo.Enabled = false;
            buttonUndo.Location = new Point(3, 14);
            buttonUndo.Name = "buttonUndo";
            buttonUndo.Size = new Size(75, 23);
            buttonUndo.TabIndex = 0;
            buttonUndo.Text = "Undo";
            buttonUndo.UseVisualStyleBackColor = true;
            buttonUndo.Click += buttonUndo_Click;
            // 
            // buttonNextPuzzle
            // 
            buttonNextPuzzle.Anchor = AnchorStyles.Right;
            buttonNextPuzzle.Enabled = false;
            buttonNextPuzzle.Location = new Point(686, 28);
            buttonNextPuzzle.Name = "buttonNextPuzzle";
            buttonNextPuzzle.Size = new Size(95, 23);
            buttonNextPuzzle.TabIndex = 5;
            buttonNextPuzzle.Text = "Next Puzzle";
            buttonNextPuzzle.UseVisualStyleBackColor = true;
            buttonNextPuzzle.Click += buttonNextPuzzle_Click;
            // 
            // labelCongratulations
            // 
            labelCongratulations.Anchor = AnchorStyles.Right;
            labelCongratulations.AutoSize = true;
            labelCongratulations.Enabled = false;
            labelCongratulations.Font = new Font("Snap ITC", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelCongratulations.Location = new Point(463, 27);
            labelCongratulations.Name = "labelCongratulations";
            labelCongratulations.Size = new Size(199, 25);
            labelCongratulations.TabIndex = 7;
            labelCongratulations.Text = "Congratulations!";
            labelCongratulations.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(labelScore, 0, 0);
            tableLayoutPanel2.Controls.Add(labelBestScore, 0, 1);
            tableLayoutPanel2.Location = new Point(159, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(268, 74);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // labelScore
            // 
            labelScore.Anchor = AnchorStyles.Left;
            labelScore.AutoSize = true;
            labelScore.Location = new Point(3, 11);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(48, 15);
            labelScore.TabIndex = 2;
            labelScore.Text = "Score: 0";
            // 
            // labelBestScore
            // 
            labelBestScore.Anchor = AnchorStyles.Left;
            labelBestScore.AutoSize = true;
            labelBestScore.Location = new Point(3, 48);
            labelBestScore.Name = "labelBestScore";
            labelBestScore.Size = new Size(73, 15);
            labelBestScore.TabIndex = 0;
            labelBestScore.Text = "Best Score: 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(controlPanel);
            Controls.Add(tablePotionLayout);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(500, 380);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Potion Master";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            controlPanel.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem openSettingsToolStripMenuItem;
        private TableLayoutPanel tablePotionLayout;
        private Panel controlPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonUndo;
        private Label labelScore;
        private Button buttonNextPuzzle;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem endGameToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Label labelBestScore;
        private Label labelCongratulations;
        private Panel panel1;
        private Label labelUndosLeft;
        private TableLayoutPanel tableLayoutPanel2;
    }
}
