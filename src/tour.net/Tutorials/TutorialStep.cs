using System.Drawing;
using System.Windows.Forms;
using tour.net.Highlight;
using tour.net.Tooltip;

namespace tour.net.Tutorials
{
    internal class TutorialStep
    {
        private readonly HighlightForm _highlightForm;
        private readonly DefaultTooltipForm _tooltipForm;

        internal HighlightForm HighlightForm => _highlightForm;
        internal DefaultTooltipForm TooltipForm => _tooltipForm;

        internal TutorialStep(HighlightForm highlightForm, DefaultTooltipForm tooltipForm)
        {
            _highlightForm = highlightForm;
            _tooltipForm = tooltipForm;
        }

        internal void Show()
        {
            _highlightForm.Show();

            _tooltipForm.Owner = _highlightForm;
            _tooltipForm.Show();
            _tooltipForm.MoveToolTip(_highlightForm.HighlightControlBounds);
        }

        internal void Hide()
        {
            Form owner = _highlightForm.Owner;
            _highlightForm.Owner = null;

            _highlightForm.Hide();
            _tooltipForm.Hide();

            _highlightForm.Owner = owner;
        }

        internal void Resize(Size size)
        {
            _highlightForm.Size = size;
        }

        internal void Move(Point highlightScreenPosition)
        {
            _highlightForm.Location = highlightScreenPosition;
            _tooltipForm.MoveToolTip(_highlightForm.HighlightControlBounds);
        }

        internal void ApplyConfig(TutorialConfig config)
        {
            _tooltipForm.ApplyConfig(config);
        }
    }
}
