﻿using System;
using System.Drawing;
using System.Windows.Forms;
using tour.net.Highlight;
using tour.net.Tooltip;
using tour.net.Tutorial;

namespace tour.net.app
{
    public partial class MainForm : Form
    {
        private TutorialManager _tutorial;

        public MainForm()
        {
            InitializeComponent();

            LocationChanged += (sender, e) =>
            {
                _tutorial?.Resize(PointToScreen(panel1.Location), panel1.Size);
            };

            SizeChanged += (sender, e) =>
            {
                _tutorial?.Resize(PointToScreen(panel1.Location), panel1.Size);
            };
        }

        private void InitializeTutorial()
        {
            var step1 = new HighlightForm(panel1.Size, button1, this);
            var step2 = new HighlightForm(panel1.Size, button2, this);
            var step3 = new HighlightForm(panel1.Size, checkBox1, this);
            var step4 = new HighlightForm(panel1.Size, radioButton1, this);
            var step5 = new HighlightForm(panel1.Size, radioButton2, this);
            var step6 = new HighlightForm(panel1.Size, radioButton3, this);

            _tutorial = new TutorialManager()
                .SetTutorialConfig(config =>
                {
                    config.HighlightScreenPosition = PointToScreen(panel1.Location);
                    config.TooltipColor = Color.LightBlue;
                })
                .AddStep(step1, new DefaultTooltipForm("Step 1", "click the button1."))
                .AddStep(step2, new DefaultTooltipForm("Step 2", "click the button2."))
                .AddStep(step3, new DefaultTooltipForm("Step 3", "check the checkBox1."))
                .AddStep(step4, new DefaultTooltipForm("Step 4", "click the radioButton1."))
                .AddStep(step5, new DefaultTooltipForm("Step 5", "click the radioButton2."))
                .AddStep(step6, new DefaultTooltipForm("Step 6", "click the radioButton3."));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _tutorial.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeTutorial();
        }
    }
}
