using System.Drawing;
using System.Windows.Forms;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public partial class HighlightForm : Form
    {
        private readonly Rectangle _highlightArea;

        public Point ToolTipPos
        {
            get
            {
                Point excludeRectRealPos = PointToScreen(_highlightArea.Location);

                return new Point(excludeRectRealPos.X + (_highlightArea.Width / 2) - (TooltipForm.TOOLTIP_FORM_WIDTH / 2),
                    excludeRectRealPos.Y + _highlightArea.Height);
            }
        }

        public HighlightForm(Size size, Rectangle highlightArea, double opacity = 0.1)
        {
            InitializeComponent();

            _highlightArea = highlightArea;

            Opacity = opacity;
            Size = size;

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            StartPosition = FormStartPosition.Manual;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(Color.White))
                e.Graphics.FillRectangle(brush, _highlightArea);
        }
    }
}
