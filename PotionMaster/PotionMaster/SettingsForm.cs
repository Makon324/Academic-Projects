﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotionMaster
{
    public partial class SettingsForm : Form
    {
        private string originalDifficulty;
        private int originalMaxSegments;
        private int originalVialCount;
        private string originalColorTheme;

        private ColorPreset CurrentTheme =>
            Properties.Settings.Default.ColorTheme == "Light"
                ? new ColorPreset(
                    Color.White,
                    Color.FromKnownColor(KnownColor.Control),
                    Color.Black,
                    Color.Pink
                  )
                : new ColorPreset(
                    Color.FromArgb(60, 60, 60),
                    Color.FromArgb(100, 100, 100),
                    Color.White,
                    Color.Pink
                  );

        public SettingsForm()
        {
            InitializeComponent();
            Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Load current settings
            originalDifficulty = Properties.Settings.Default.Difficulty;
            originalMaxSegments = Properties.Settings.Default.MaxSegments;
            originalVialCount = Properties.Settings.Default.VialCount;
            originalColorTheme = Properties.Settings.Default.ColorTheme;

            // Set controls
            comboDifficulty.SelectedItem = originalDifficulty;
            UpdateVialCountConstraints();
            numMaxSegments.Value = originalMaxSegments;
            numVialCount.Value = originalVialCount;

            switch (originalColorTheme)
            {
                case "Light": radioLight.Checked = true; break;
                case "Dark": radioDark.Checked = true; break;
            }

            ApplyTheme(); // Apply theme after loading settings
        }






        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVialCountConstraints();
        }

        private void UpdateVialCountConstraints()
        {
            string difficulty = comboDifficulty.SelectedItem?.ToString() ?? "Easy";
            int minVials = difficulty switch
            {
                "Hard" => 3,
                "Medium" => 4,
                "Easy" => 5,
                _ => 5
            };
            numVialCount.Minimum = minVials;
            if (numVialCount.Value < minVials)
                numVialCount.Value = minVials;
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            // Discard changes by not saving and close
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            bool changesMade = false;

            // Check ALL settings first before modifying
            string newDifficulty = comboDifficulty.SelectedItem?.ToString();
            int newMaxSegments = (int)numMaxSegments.Value;
            int newVialCount = (int)numVialCount.Value;
            string newTheme = radioLight.Checked ? "Light" : "Dark";

            // Detect changes
            if (newDifficulty != originalDifficulty ||
                newMaxSegments != originalMaxSegments ||
                newVialCount != originalVialCount ||
                newTheme != originalColorTheme)
            {
                changesMade = true;
            }

            // Apply changes only if detected
            if (changesMade)
            {
                Properties.Settings.Default.Difficulty = newDifficulty;
                Properties.Settings.Default.MaxSegments = newMaxSegments;
                Properties.Settings.Default.VialCount = newVialCount;
                Properties.Settings.Default.ColorTheme = newTheme;
                Properties.Settings.Default.Save();
            }

            DialogResult = DialogResult.OK;
            Close();
        }


        private void ApplyTheme()
        {
            var theme = CurrentTheme;

            // Apply to entire form
            this.BackColor = theme.Background;
            this.ForeColor = theme.Text;

            // Apply to all child controls
            ApplyThemeToControls(this.Controls, theme);
        }

        private void ApplyThemeToControls(Control.ControlCollection controls, ColorPreset theme)
        {
            foreach (Control c in controls)
            {
                // Set background for panels/layouts
                if (c is Panel || c is TableLayoutPanel || c is GroupBox)
                {
                    c.BackColor = theme.Background;
                    c.ForeColor = theme.Text;
                }
                // Style buttons
                else if (c is Button btn)
                {
                    btn.BackColor = theme.Button;
                    btn.ForeColor = theme.Text;
                }
                // Style labels
                else if (c is Label lbl)
                {
                    lbl.ForeColor = theme.Text;
                }

                // Recurse into child controls
                if (c.HasChildren)
                {
                    ApplyThemeToControls(c.Controls, theme);
                }
            }
        }
    }
}
