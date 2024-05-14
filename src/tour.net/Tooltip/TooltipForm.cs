using System;
using System.Drawing;
using System.Windows.Forms;

namespace tour.net.Tooltip
{
    public enum EArrowTooltipTail
    {
        Top,
    }

    public partial class TooltipForm : Form
    {
        private readonly TooltipTail _tooltipTail;
        private readonly static Color FORM_COLOR = Color.FromArgb(127, 101, 224);

        public const int TOOLTIP_FORM_WIDTH = 403;

        public TooltipForm(string title, string description, EArrowTooltipTail arrowTooltipTail = EArrowTooltipTail.Top)
        {
            InitializeComponent();
            
            ApplyColor();

            _tooltipTail = new TooltipTail(FORM_COLOR);
            Controls.Add(_tooltipTail);

            MoveTail(arrowTooltipTail);

            Size = new Size(TOOLTIP_FORM_WIDTH, Height + _tooltipTail.TailHeight);

            lbTitle.Text = title;
            lbDescription.Text = description;
        }

        private void ApplyColor()
        {
            lbTitle.BackColor = FORM_COLOR;
            btnNext.BackColor = FORM_COLOR;
            btnPrev.FlatAppearance.BorderColor = FORM_COLOR;
        }

        private void MoveTail(EArrowTooltipTail arrowTooltipTail)
        {
            switch (arrowTooltipTail) 
            {
                case EArrowTooltipTail.Top:
                    _tooltipTail.Location = new Point(Width / 2 - _tooltipTail.TailWidth, _tooltipTail.Location.Y);
                    break;
                default:
                    throw new ArgumentException(nameof(arrowTooltipTail));
            }
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
