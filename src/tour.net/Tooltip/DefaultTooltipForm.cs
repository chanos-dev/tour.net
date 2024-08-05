using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace tour.net.Tooltip
{
    public enum ETooltipPosition
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
    }

    public partial class DefaultTooltipForm : TooltipForm
    {
        #region inner class
        internal class TooltipTail : Panel
        {
            private const int TRIANGLE_HEIGHT = 16;

            private Color _color;
            private readonly ETooltipPosition _tooltipPosition;
            private readonly int _tailWidth;

            internal int TailHeight => TRIANGLE_HEIGHT;
            internal int TailWidth => _tailWidth;

            internal TooltipTail(Color color, ETooltipPosition tooltipPosition)
            {
                _color = color;
                _tooltipPosition = tooltipPosition;
                _tailWidth = TRIANGLE_HEIGHT / 2;
            }

            internal void SetColor(Color color)
            {
                _color = color;
                Invalidate();
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

                Point[] triangle = null;

                switch (_tooltipPosition)
                {                    
                    case ETooltipPosition.Bottom:
                    default:
                        Point pb = new Point(_tailWidth, 0);
                        
                        triangle = new Point[]
                        {
                            pb,
                            new Point(pb.X - _tailWidth, pb.Y + TRIANGLE_HEIGHT),
                            new Point(pb.X + _tailWidth, pb.Y + TRIANGLE_HEIGHT)
                        };
                        break;
                    case ETooltipPosition.Top:
                        Point pt = new Point(_tailWidth, TRIANGLE_HEIGHT);

                        triangle = new Point[]
                        {
                            pt,
                            new Point(pt.X - _tailWidth, pt.Y - TRIANGLE_HEIGHT),
                            new Point(pt.X + _tailWidth, pt.Y - TRIANGLE_HEIGHT)
                        };
                        break;
                    case ETooltipPosition.Right:
                        Point pr = new Point(0, TRIANGLE_HEIGHT / 2);

                        triangle = new Point[]
                        {
                            new Point(TRIANGLE_HEIGHT, pr.Y - _tailWidth),
                            pr,
                            new Point(TRIANGLE_HEIGHT, pr.Y + _tailWidth)
                        };
                        break;
                    case ETooltipPosition.Left:
                        Point pl = new Point(TRIANGLE_HEIGHT, _tailWidth);

                        triangle = new Point[]
                        {
                            new Point(0, pl.Y - _tailWidth),
                            pl,
                            new Point(0, pl.Y + _tailWidth)
                        };
                        break;
                }

                e.Graphics.FillPolygon(new SolidBrush(_color), triangle);
            }
        }
        #endregion

        private TooltipTail _tooltipTail;
        private ETooltipPosition _tooltipPosition;

        public DefaultTooltipForm(string title, string description, ETooltipPosition tooltipPosition = ETooltipPosition.Bottom)
        {
            InitializeComponent();
            InitializeTooltipTail(tooltipPosition);
            InitializeProperties(title, description);
        }

        private void InitializeProperties(string title, string description)
        {
            lbTitle.Text = title;
            lbDescription.Text = description;
            
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            // avoid focusing close button.
            ActiveControl = lbDescription;
        }

        private void InitializeTooltipTail(ETooltipPosition tooltipPosition)
        {
            _tooltipPosition = tooltipPosition;
            // set empty color before setting config.
            _tooltipTail = new TooltipTail(Color.Transparent, tooltipPosition);
            Controls.Add(_tooltipTail);
            MoveTail();
        }

        private void MoveTail()
        {
            switch (_tooltipPosition) 
            {
                case ETooltipPosition.Bottom:
                default:
                    pnlMain.Dock = DockStyle.Bottom;
                    _tooltipTail.Location = new Point(Width / 2 - _tooltipTail.TailWidth, _tooltipTail.Location.Y);
                    Size = new Size(Width, Height + _tooltipTail.TailHeight);
                    break;
                case ETooltipPosition.Top:
                    pnlMain.Dock = DockStyle.Top;
                    _tooltipTail.Location = new Point(Width / 2 - _tooltipTail.TailWidth, Height);
                    Size = new Size(Width, Height + _tooltipTail.TailHeight);
                    break;
                case ETooltipPosition.Right:
                    pnlMain.Dock = DockStyle.Right;
                    _tooltipTail.Location = new Point(0, Height / 2 - _tooltipTail.TailWidth);
                    Size = new Size(Width + _tooltipTail.TailHeight, Height);
                    break;
                case ETooltipPosition.Left:
                    pnlMain.Dock = DockStyle.Left;
                    _tooltipTail.Location = new Point(Width, Height / 2 - _tooltipTail.TailWidth);
                    Size = new Size(Width + _tooltipTail.TailHeight, Height);
                    break;
            }
        }

        internal void ApplyConfig(TutorialConfig config)
        {
            lbTitle.BackColor = config.TooltipColor;

            btnNext.BackColor = config.TooltipColor;
            btnPrev.FlatAppearance.BorderColor = config.TooltipColor;
            btnExit.BackColor = config.TooltipColor;

            pnlTitle.BackColor = config.TooltipColor;

            _tooltipTail.SetColor(config.TooltipColor);
        }
        public override void MoveTooltip(Rectangle highlightControlBounds)
        {
            switch (_tooltipPosition)
            {
                case ETooltipPosition.Bottom:
                default:
                    Location = new Point(highlightControlBounds.X - Width / 2 + highlightControlBounds.Width / 2,
                        highlightControlBounds.Y + highlightControlBounds.Height);
                    break;
                case ETooltipPosition.Top:
                    Location = new Point(highlightControlBounds.X - Width / 2 + highlightControlBounds.Width / 2,
                        highlightControlBounds.Y - Height);
                    break;
                case ETooltipPosition.Right:
                    Location = new Point(highlightControlBounds.X + highlightControlBounds.Width,
                        highlightControlBounds.Y - (Height / 2) + (highlightControlBounds.Height / 2));
                    break;
                case ETooltipPosition.Left:
                    Location = new Point(highlightControlBounds.X - Width,
                        highlightControlBounds.Y - (Height / 2) + (highlightControlBounds.Height / 2));
                    break;
            }
        }

        public override void SetStepInfo(int stepIndex, int totalStepsCount)
        {
            lbSeq.Text = $"{stepIndex} / {totalStepsCount}";

            if (stepIndex == 1)
            {
                btnPrev.Visible = false;
            }
            else if (stepIndex == totalStepsCount)
            {
                btnNext.Text = "Finish";
            }
        }

        public override void AddPrevEvent(EventHandler prevEvent)
            => btnPrev.Click += prevEvent;

        public override void RemovePrevEvent(EventHandler prevEvent)
            => btnPrev.Click -= prevEvent;

        public override void AddNextEvent(EventHandler nextEvent)
            => btnNext.Click += nextEvent;

        public override void RemoveNextEvent(EventHandler nextEvent)
            => btnNext.Click -= nextEvent;

        public override void AddExitEvent(EventHandler exitEvent)
            => btnExit.Click += exitEvent;

        public override void RemoveExitEvent(EventHandler exitEvent)
            => btnExit.Click -= exitEvent;

        #region override methods
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
        #endregion
    }
}
