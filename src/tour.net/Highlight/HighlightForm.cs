using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using tour.net.Tooltip;

namespace tour.net.Highlight
{
    public partial class HighlightForm : Form
    {
        private Control _highlightControl;

        public HighlightForm(Size size, Control highlightControl, Form owner, double opacity = 0.1)
        {
            InitializeComponent();

            _highlightControl = highlightControl;

            Opacity = opacity;
            Size = size;
            Owner = owner;

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            StartPosition = FormStartPosition.Manual;

            ShowInTaskbar = false;
        }

        public new Size Size
        {
            get => base.Size;
            set
            {
                SetRegion(value);
                base.Size = value;
            }
        }

        public void Release()
        {
            _highlightControl = null;
            Owner = null;
        }

        public Point GetToolTipPos()
        {
            Point excludeRectRealPos = PointToScreen(_highlightControl.Bounds.Location);

            return new Point(excludeRectRealPos.X + (_highlightControl.Width / 2) - (DefaultTooltipForm.TOOLTIP_FORM_WIDTH / 2),
                excludeRectRealPos.Y + _highlightControl.Height);
        }

        private void SetRegion(Size size)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(new Rectangle(
                        _highlightControl.Location.X - 1,
                        _highlightControl.Location.Y - 1,
                        _highlightControl.Width + 2,
                        _highlightControl.Height + 2));

                Region region = new Region(new Rectangle(0, 0, size.Width, size.Height));
                region.Exclude(path);                
                Region = region;
            }
        }
    }
}
