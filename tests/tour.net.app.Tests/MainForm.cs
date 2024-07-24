using System;
using System.Drawing;
using System.Windows.Forms;
using tour.net.Highlight;
using tour.net.Tooltip;
using tour.net.Tutorials;

namespace tour.net.app
{
    public partial class MainForm : Form
    {
        private ITutorial _tutorial;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeTutorial()
        {
            var tutorial = new Tutorial(this)
                .SetTutorialConfig(config =>
                {
                    config.TooltipColor = Color.LightBlue;
                    config.AutoPositionTooltip = true;
                })
                .AddStep(new HighlightForm(button1, 0.1), new DefaultTooltipForm("Step 1", "click the button1.", ETooltipPosition.Right))
                .AddStep(new HighlightForm(button2, 0.3), new DefaultTooltipForm("Step 2", "click the button2.", ETooltipPosition.Right))
                .AddStep(new HighlightForm(checkBox1, 0.5), new DefaultTooltipForm("Step 3", "check the checkBox1.", ETooltipPosition.Right))
                .AddStep(new HighlightForm(radioButton1, 0.7), new DefaultTooltipForm("Step 4", "click the radioButton1.", ETooltipPosition.Right))
                .AddStep(new HighlightForm(radioButton2, 0.9), new DefaultTooltipForm("Step 5", "click the radioButton2.", ETooltipPosition.Right))
                .AddStep(new HighlightForm(radioButton3, 1), new DefaultTooltipForm("Step 6", "click the radioButton3.", ETooltipPosition.Right));

            _tutorial = tutorial.Build();
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
