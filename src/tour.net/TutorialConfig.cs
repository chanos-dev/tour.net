﻿using System.Drawing;

namespace tour.net
{
    public class TutorialConfig
    {
        public Color TooltipColor { get; set; }
        private TutorialConfig() { }
        public static TutorialConfig DefaultTutorialConfig => new TutorialConfig()
        {
            TooltipColor = Color.FromArgb(127, 101, 224),
        };
    }
}
