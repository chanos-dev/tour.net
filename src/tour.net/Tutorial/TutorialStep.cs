using System.Drawing;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public class TutorialStep
    {
        private readonly HighlightForm _highlightForm;
        private readonly TooltipForm _tooltipForm;
        private readonly Point _screenPos;

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
            _highlightForm.TopMost = true;
            _highlightForm.Location = _screenPos;
            _highlightForm.Show();

            _tooltipForm.Owner = _highlightForm;
            _tooltipForm.TopMost = true;
            _tooltipForm.Show();
            _tooltipForm.Location = _highlightForm.ToolTipPos;
        }

        public void Hide()
        {
            _highlightForm.Hide();
            _tooltipForm.Hide();
        }
    }
}
