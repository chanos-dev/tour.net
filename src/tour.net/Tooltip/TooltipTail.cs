using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace tour.net.Tooltip
{
    internal class TooltipTail : Panel
    {
        private const int TRIANGLE_HEIGHT = 16;

        private readonly Color _color;

        private readonly int _tailWidth;

        internal int TailHeight => TRIANGLE_HEIGHT;
        internal int TailWidth => _tailWidth;

        internal TooltipTail(Color color)
        {
            _color = color;
            _tailWidth = TRIANGLE_HEIGHT / 2;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x00000020;

                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Point p = new Point(_tailWidth, 0);

            e.Graphics.FillPolygon(new SolidBrush(_color),
                new Point[]
                {
                    p,
                    new Point(p.X - _tailWidth, p.Y + TRIANGLE_HEIGHT),
                    new Point(p.X + _tailWidth, p.Y + TRIANGLE_HEIGHT)
                });
        }
    }
}
