


namespace PotionMaster
{
    static class Theme
    {
        public static readonly ColorPreset LightPreset = new ColorPreset(
            Color.White,
            Color.FromKnownColor(KnownColor.Control),
            Color.Black,
            Color.Pink
        );

        public static readonly ColorPreset DarkPreset = new ColorPreset(
            Color.FromArgb(60, 60, 60),
            Color.FromArgb(100, 100, 100),
            Color.White,
            Color.Pink
        );

        private static ColorPreset theme;

        static Theme()
        {
            theme = LightPreset;
        }

        public static void SetTheme(ColorPreset _theme)
        {
            theme = _theme;
        }

        public static void ApplyTheme(Form form)
        {
            form.BackColor = theme.Background;
            form.ForeColor = theme.Text;
            ApplyThemeToControls(form.Controls);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // don't theme these controls
                if (control is NumericUpDown || control is ComboBox)
                    continue;

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
                else
                {
                    control.ForeColor = theme.Text;
                }

                // recurse into children
                if (control.HasChildren)
                    ApplyThemeToControls(control.Controls);
            }
        }


        public static ThemedMenuRenderer CreateMenuRenderer()
        {
            return new ThemedMenuRenderer(
                theme
            );
        }
    }

    public class ColorPreset
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

    class ThemeColorTable : ProfessionalColorTable
    {
        readonly ColorPreset theme;
        public ThemeColorTable(ColorPreset _theme)
        {
            theme = _theme;
        }

        // menu-bar background
        public override Color ToolStripGradientBegin => theme.Background;
        public override Color ToolStripGradientMiddle => theme.Background;
        public override Color ToolStripGradientEnd => theme.Background;
        public override Color MenuItemBorder => theme.Text;
        public override Color ToolStripBorder => theme.Text;

        // dropdown background
        public override Color ToolStripDropDownBackground => theme.Background;

        // hover/pressed highlight
        public override Color MenuItemSelected => theme.Accent;
        public override Color MenuItemSelectedGradientBegin => theme.Accent;
        public override Color MenuItemSelectedGradientEnd => theme.Accent;
        public override Color MenuItemPressedGradientBegin => theme.Accent;
        public override Color MenuItemPressedGradientMiddle => theme.Accent;
        public override Color MenuItemPressedGradientEnd => theme.Accent;

    }

    class ThemedMenuRenderer : ToolStripProfessionalRenderer
    {
        private readonly ColorPreset theme;

        public ThemedMenuRenderer(ColorPreset _theme)
            : base(new ThemeColorTable(_theme))
        {
            theme = _theme;
        }

        // paint the entire drop-down background
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(theme.Background),
                e.AffectedBounds
            );
        }

        // left icon-margin in the same background
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(theme.Background),
                e.AffectedBounds
            );
        }

        // force all menu-item text to theme color
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = theme.Text;
            base.OnRenderItemText(e);
        }


    }
}

