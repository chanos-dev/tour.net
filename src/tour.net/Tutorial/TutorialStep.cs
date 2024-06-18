using System.Drawing;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public class TutorialStep
    {
        private readonly HighlightForm _highlightForm;
        private readonly TooltipForm _tooltipForm;
        private Point _screenPos;

        public HighlightForm HighlightForm => _highlightForm;
        public TooltipForm TooltipForm => _tooltipForm;

        public TutorialStep(HighlightForm highlightForm, TooltipForm tooltipForm, Point screenPos)
        {
            _highlightForm = highlightForm;
            _tooltipForm = tooltipForm;

            _screenPos = screenPos;
        }

        public void Show()
        {
            _highlightForm.Location = _screenPos;
            _highlightForm.Show();

            _tooltipForm.Owner = _highlightForm;
            _tooltipForm.Show();
            _tooltipForm.Location = _highlightForm.GetToolTipPos();
        }

        public void Hide()
        {
            _highlightForm.Hide();
            _tooltipForm.Hide();
        }

        public void Resize(Size size)
        {
            _highlightForm.Size = size;
        }

        public void Move(Point screenPos)
        {
            _screenPos = screenPos;
            _highlightForm.Location = screenPos;
            _tooltipForm.Location = _highlightForm.GetToolTipPos();
        }
    }
}
