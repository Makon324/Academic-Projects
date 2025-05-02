using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotionMaster
{
    public partial class VialControl : UserControl
    {
        private int maxSegments = 4;
        private int initSegmentCount = 0;
        private List<Color> segments = new List<Color>();

        [Browsable(true)]
        [Category("Vial Settings"), Description("Maximum number of segments")]
        public int MaxSegments
        {
            get => maxSegments;
            set { maxSegments = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Vial Settings"), Description("Maximum number of segments")]
        public int InitSegmentCount
        {
            get => initSegmentCount;
            set { initSegmentCount = value;  InitSegments(); Invalidate(); }
        }

        [Browsable(true)]
        [Category("Vial Settings"), Description("Maximum number of segments")]
        public List<Color> Segments
        {
            get => segments;
            set { segments = value; Invalidate(); }
        }

        private void InitSegments()
        {
            segments.Clear();

            Random R = new Random();

            Color[] possibleColors = { Color.Red, Color.Green, Color.Blue };

            for (int i = 0; i < initSegmentCount; i++)
            {
                segments.Add(possibleColors[R.Next(0, 3)]);
            }

        }

        public VialControl()
        {
            InitializeComponent();
            InitSegments();
            this.AllowDrop = true;
            this.MouseDown += VialControl_MouseDown;            
            this.DragEnter += VialControl_DragEnter;
            this.DragDrop += VialControl_DragDrop;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate(); // Force redraw on resize
        }

        private void VialControl_Load(object sender, EventArgs e)
        {
            //
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int _borderWidth = 12;
            float halfPen = _borderWidth / 2f;

            // Vial dimensions (excluding border)
            float vialWidth = 60;
            float vialHeight = 200;

            // Calculate the total space the vial + border will occupy
            float totalWidth = vialWidth + _borderWidth;
            float totalHeight = vialHeight + _borderWidth;

            // Center the total space (vial + border) within the control
            float x = (ClientSize.Width - totalWidth) / 2f;
            float y = (ClientSize.Height - totalHeight) / 2f;

            // Adjust the outer rectangle to account for the border's inner half
            var outer = new RectangleF(
                x + halfPen,
                y + 14f,
                vialWidth - halfPen,
                vialHeight);

            // Build the bottom-rounded, open-top path
            using (var borderPath = new GraphicsPath())
            {
                float w = outer.Width;
                float h = outer.Height;
                float arcDiameter = w;            // Full-width semi-circle
                float straightHeight = h - w / 2f;  // Height of the two straight sides


                // setting clip

                GraphicsPath clipPath = new GraphicsPath();

                clipPath.StartFigure();

                // Rectangle sides
                clipPath.AddLine(outer.Left, outer.Top, outer.Left, outer.Top + straightHeight - arcDiameter);

                // Bottom half-circle
                clipPath.AddArc(
                    outer.Left,
                    outer.Top + straightHeight - arcDiameter,
                    arcDiameter, arcDiameter,
                    -180, -180);

                clipPath.AddLine(outer.Left + w, outer.Top + straightHeight - arcDiameter, outer.Left + w, outer.Top);

                clipPath.CloseFigure();

                // Set clip to the custom shape
                e.Graphics.SetClip(clipPath);

                // -------------

                // Draw segments
                if (MaxSegments > 0 && Segments.Count > 0)
                {
                    int segmentHeight = 167 / MaxSegments;
                    for (int i = 0; i < Segments.Count && i < MaxSegments; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Segments[i]))
                        {
                            int yPos = Height - (i + 1) * segmentHeight;
                            g.FillRectangle(brush, 2, yPos-20, Width - 4, segmentHeight - 2);
                        }
                    }
                }


                // Draw vial


                // Left side down
                borderPath.StartFigure();
                borderPath.AddLine(
                    outer.Left, outer.Top,
                    outer.Left, outer.Top + straightHeight - arcDiameter);

                // Bottom arc
                borderPath.AddArc(
                    outer.Left,
                    outer.Top + straightHeight - arcDiameter,
                    arcDiameter, arcDiameter,
                    -180, -180);

                // Right side up
                borderPath.AddLine(
                    outer.Left + w, outer.Top + straightHeight - arcDiameter,
                    outer.Left + w, outer.Top);

                // Draw the border
                using (var pen = new Pen(ForeColor, _borderWidth))
                {
                    pen.Alignment = PenAlignment.Center;
                    g.DrawPath(pen, borderPath);
                }
            }



        }

        // pouring
        public bool CanPour(Color C, int quant)
        {
            if (segments.Count == 0)
            {
                return quant <= maxSegments;
            }
            else
            {
                Color targetColor = segments.Last(); // Check top color
                return (C == targetColor) && (maxSegments - segments.Count >= quant);
            }
        }

        public void Pour(Color C, int quant)
        {
            for(int i = 0; i < quant; i++)
                segments.Add(C);
            Invalidate();
        }

        public (Color C, int q) GetPour()
        {
            if (segments.Count == 0)
                return (Color.Empty, 0);

            Color topColor = segments.Last();
            int count = 0;
            for (int i = segments.Count - 1; i >= 0; i--)
            {
                if (segments[i] == topColor)
                    count++;
                else
                    break;
            }
            return (topColor, count);
        }

        public void RemoveSegments(int quant)
        {
            if (quant <= 0 || quant > segments.Count)
                return;

            segments.RemoveRange(segments.Count - quant, quant);
            Invalidate();
        }

        private void VialControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (segments.Count == 0)
                return;

            var (color, quant) = GetPour();
            if (quant == 0)
                return;

            DataObject data = new DataObject();
            data.SetData("SourceVial", this);
            data.SetData("Color", color);
            data.SetData("Quantity", quant);
            DoDragDrop(data, DragDropEffects.Move);
        }

        private void VialControl_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void VialControl_DragDrop(object sender, DragEventArgs e)
        {

        }





    }
}
