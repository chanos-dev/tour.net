using System.Drawing;

namespace tour.net
{
    public class TutorialConfig
    {
        public Point HighlightScreenPosition { get; set; }
        public Color TooltipColor { get; set; }
        public bool AutoPositionTooltip { get; set; }
        private TutorialConfig() { }
        public static TutorialConfig DefaultTutorialConfig => new TutorialConfig()
        {
            HighlightScreenPosition = Point.Empty,
            TooltipColor = Color.FromArgb(127, 101, 224),
            AutoPositionTooltip = false,
        };
    }
}
