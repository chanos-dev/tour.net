using System.Drawing;
using System.Windows.Forms;
using tour.net.Highlight;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public class TutorialStep
    {
        private readonly HighlightForm _highlightForm;
        private readonly DefaultTooltipForm _tooltipForm;

        public HighlightForm HighlightForm => _highlightForm;
        public DefaultTooltipForm TooltipForm => _tooltipForm;

        public TutorialStep(HighlightForm highlightForm, DefaultTooltipForm tooltipForm, Point screenPos)
        {
            _highlightForm = highlightForm;
            _highlightForm.Location = screenPos;

            _tooltipForm = tooltipForm;
        }

        public void Show()
        {
            _highlightForm.Show();

            _tooltipForm.Owner = _highlightForm;
            _tooltipForm.Show();
            _tooltipForm.Location = _highlightForm.GetToolTipPos();
        }

        public void Hide()
        {
            Form owner = _highlightForm.Owner;
            _highlightForm.Owner = null;

            _highlightForm.Hide();
            _tooltipForm.Hide();

            _highlightForm.Owner = owner;
        }

        public void Resize(Size size)
        {
            _highlightForm.Size = size;
        }

        public void Move(Point highlightScreenPosition)
        {
            _highlightForm.Location = highlightScreenPosition;
            _tooltipForm.Location = _highlightForm.GetToolTipPos();
        }

        public void ApplyConfig(TutorialConfig config)
        {
            _tooltipForm.ApplyConfig(config);
        }
    }
}
