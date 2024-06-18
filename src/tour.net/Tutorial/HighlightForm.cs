using System.Drawing;
using System.Windows.Forms;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public partial class HighlightForm : Form
    {
        private readonly Control _highlightControl;

        public HighlightForm(Size size, Control highlightControl, Form owner)
        {
            InitializeComponent();

            _highlightControl = highlightControl;

            Opacity = 0.1;
            Size = size;
            Owner = owner;

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            StartPosition = FormStartPosition.Manual;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(Color.White))
                e.Graphics.FillRectangle(brush, new Rectangle(
                    _highlightControl.Location.X - 1,
                    _highlightControl.Location.Y - 1,
                    _highlightControl.Width + 2,
                    _highlightControl.Height + 2));
        }

        public Point GetToolTipPos()
        {
            Point excludeRectRealPos = PointToScreen(_highlightControl.Bounds.Location);

            return new Point(excludeRectRealPos.X + (_highlightControl.Width / 2) - (TooltipForm.TOOLTIP_FORM_WIDTH / 2),
                excludeRectRealPos.Y + _highlightControl.Height);
        }
    }
}
