namespace PotionMaster
{
    partial class SettingsForm
    {

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            labelSegmentsCount = new Label();
            labelVialsCount = new Label();
            labelColorTheme = new Label();
            labelDifficulty = new Label();
            btnCancel = new Button();
            btnOK = new Button();
            numMaxSegments = new NumericUpDown();
            numVialCount = new NumericUpDown();
            comboDifficulty = new ComboBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            radioLight = new RadioButton();
            radioDark = new RadioButton();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxSegments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVialCount).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Controls.Add(labelSegmentsCount, 0, 1);
            tableLayoutPanel1.Controls.Add(labelVialsCount, 0, 2);
            tableLayoutPanel1.Controls.Add(labelColorTheme, 0, 3);
            tableLayoutPanel1.Controls.Add(labelDifficulty, 0, 0);
            tableLayoutPanel1.Controls.Add(btnCancel, 0, 4);
            tableLayoutPanel1.Controls.Add(btnOK, 1, 4);
            tableLayoutPanel1.Controls.Add(numMaxSegments, 1, 1);
            tableLayoutPanel1.Controls.Add(numVialCount, 1, 2);
            tableLayoutPanel1.Controls.Add(comboDifficulty, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(284, 261);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // labelSegmentsCount
            // 
            labelSegmentsCount.Anchor = AnchorStyles.None;
            labelSegmentsCount.AutoSize = true;
            labelSegmentsCount.Location = new Point(9, 70);
            labelSegmentsCount.Name = "labelSegmentsCount";
            labelSegmentsCount.Size = new Size(95, 15);
            labelSegmentsCount.TabIndex = 1;
            labelSegmentsCount.Text = "Segments Count";
            // 
            // labelVialsCount
            // 
            labelVialsCount.Anchor = AnchorStyles.None;
            labelVialsCount.AutoSize = true;
            labelVialsCount.Location = new Point(23, 122);
            labelVialsCount.Name = "labelVialsCount";
            labelVialsCount.Size = new Size(67, 15);
            labelVialsCount.TabIndex = 2;
            labelVialsCount.Text = "Vials Count";
            // 
            // labelColorTheme
            // 
            labelColorTheme.Anchor = AnchorStyles.None;
            labelColorTheme.AutoSize = true;
            labelColorTheme.Location = new Point(19, 174);
            labelColorTheme.Name = "labelColorTheme";
            labelColorTheme.Size = new Size(75, 15);
            labelColorTheme.TabIndex = 3;
            labelColorTheme.Text = "Color Theme";
            // 
            // labelDifficulty
            // 
            labelDifficulty.Anchor = AnchorStyles.None;
            labelDifficulty.AutoSize = true;
            labelDifficulty.Location = new Point(29, 18);
            labelDifficulty.Name = "labelDifficulty";
            labelDifficulty.Size = new Size(55, 15);
            labelDifficulty.TabIndex = 0;
            labelDifficulty.Text = "Difficulty";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Right;
            btnCancel.Location = new Point(35, 223);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click_1;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Right;
            btnOK.Location = new Point(194, 223);
            btnOK.Margin = new Padding(3, 3, 15, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click_1;
            // 
            // numMaxSegments
            // 
            numMaxSegments.Anchor = AnchorStyles.None;
            numMaxSegments.Location = new Point(138, 66);
            numMaxSegments.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMaxSegments.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numMaxSegments.Name = "numMaxSegments";
            numMaxSegments.Size = new Size(120, 23);
            numMaxSegments.TabIndex = 7;
            numMaxSegments.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // numVialCount
            // 
            numVialCount.Anchor = AnchorStyles.None;
            numVialCount.Location = new Point(138, 118);
            numVialCount.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
            numVialCount.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            numVialCount.Name = "numVialCount";
            numVialCount.Size = new Size(120, 23);
            numVialCount.TabIndex = 8;
            numVialCount.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // comboDifficulty
            // 
            comboDifficulty.Anchor = AnchorStyles.None;
            comboDifficulty.FormattingEnabled = true;
            comboDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            comboDifficulty.Location = new Point(138, 14);
            comboDifficulty.Name = "comboDifficulty";
            comboDifficulty.Size = new Size(121, 23);
            comboDifficulty.TabIndex = 9;
            comboDifficulty.Text = "Easy";
            comboDifficulty.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(radioLight, 0, 0);
            tableLayoutPanel2.Controls.Add(radioDark, 1, 0);
            tableLayoutPanel2.Location = new Point(116, 159);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(165, 46);
            tableLayoutPanel2.TabIndex = 10;
            // 
            // radioLight
            // 
            radioLight.Anchor = AnchorStyles.None;
            radioLight.AutoSize = true;
            radioLight.Checked = true;
            radioLight.Location = new Point(15, 13);
            radioLight.Name = "radioLight";
            radioLight.Size = new Size(52, 19);
            radioLight.TabIndex = 0;
            radioLight.TabStop = true;
            radioLight.Text = "Light";
            radioLight.UseVisualStyleBackColor = true;
            // 
            // radioDark
            // 
            radioDark.Anchor = AnchorStyles.None;
            radioDark.AutoSize = true;
            radioDark.Location = new Point(99, 13);
            radioDark.Name = "radioDark";
            radioDark.Size = new Size(49, 19);
            radioDark.TabIndex = 1;
            radioDark.Text = "Dark";
            radioDark.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(284, 261);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxSegments).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVialCount).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);

            // Set other properties (layout, positioning, etc.)
            // ... Use appropriate layout controls like TableLayoutPanel ...
        }
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelSegmentsCount;
        private Label labelVialsCount;
        private Label labelColorTheme;
        private Label labelDifficulty;
        private Button btnCancel;
        private Button btnOK;
        private NumericUpDown numMaxSegments;
        private NumericUpDown numVialCount;
        private ComboBox comboDifficulty;
        private TableLayoutPanel tableLayoutPanel2;
        private RadioButton radioLight;
        private RadioButton radioDark;
    }
}