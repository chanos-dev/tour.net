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

    public partial class DefaultTooltipForm : Form
    {
        #region inner class
        internal class TooltipTail : Panel
        {
            private const int TRIANGLE_HEIGHT = 16;

            private Color _color;

            private readonly int _tailWidth;

            internal int TailHeight => TRIANGLE_HEIGHT;
            internal int TailWidth => _tailWidth;

            internal TooltipTail(Color color)
            {
                _color = color;
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
        #endregion

        private TooltipTail _tooltipTail;
        private int _stepIndex;
        private int _totalStepsCount;
        internal const int TOOLTIP_FORM_WIDTH = 403;

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
            // set empty color before setting config.
            _tooltipTail = new TooltipTail(Color.Transparent);
            Controls.Add(_tooltipTail);
            MoveTail(tooltipPosition);
        }

        private void MoveTail(ETooltipPosition tooltipPosition)
        {
            switch (tooltipPosition) 
            {
                case ETooltipPosition.Bottom:
                    _tooltipTail.Location = new Point(Width / 2 - _tooltipTail.TailWidth, _tooltipTail.Location.Y);
                    Size = new Size(TOOLTIP_FORM_WIDTH, Height + _tooltipTail.TailHeight);
                    break;
                default:
                    throw new ArgumentException(nameof(tooltipPosition));
            }
        }

        public void SetStepInfo(int stepIndex, int totalStepsCount)
        {
            _stepIndex = stepIndex;
            _totalStepsCount = totalStepsCount;

            lbSeq.Text = $"{_stepIndex} / {_totalStepsCount}";

            if (stepIndex == 1)
            {
                btnPrev.Visible = false;
            }
            else if (stepIndex == totalStepsCount)
            {
                btnNext.Text = "Finish";
            }
        }

        public void ApplyConfig(TutorialConfig config)
        {
            lbTitle.BackColor = config.TooltipColor;

            btnNext.BackColor = config.TooltipColor;
            btnPrev.FlatAppearance.BorderColor = config.TooltipColor;
            btnExit.BackColor = config.TooltipColor;

            pnlTitle.BackColor = config.TooltipColor;

            _tooltipTail.SetColor(config.TooltipColor);
        }

        public void AddPrevEvent(EventHandler prevEvent)
            => btnPrev.Click += prevEvent;

        public void RemovePrevEvent(EventHandler prevEvent)
            => btnPrev.Click -= prevEvent;

        public void AddNextEvent(EventHandler nextEvent)
            => btnNext.Click += nextEvent;

        public void RemoveNextEvent(EventHandler nextEvent)
            => btnNext.Click -= nextEvent;

        public void AddExitEvent(EventHandler exitEvent)
            => btnExit.Click += exitEvent;

        public void RemoveExitEvent(EventHandler exitEvent)
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
