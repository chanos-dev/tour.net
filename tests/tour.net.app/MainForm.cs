using System;
using System.Windows.Forms;
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
        }

        private void InitializeTutorial()
        {
            var step1 = new HighlightForm(Size, button1.Bounds);
            var step2 = new HighlightForm(Size, button2.Bounds);
            var step3 = new HighlightForm(Size, checkBox1.Bounds);
            var step4 = new HighlightForm(Size, radioButton1.Bounds);
            var step5 = new HighlightForm(Size, radioButton2.Bounds);
            var step6 = new HighlightForm(Size, radioButton3.Bounds);

            _tutorial = new TutorialManager()
                .SetBaseAreaLocation(PointToScreen(panel1.Location))
                .AddStep(step1, new TooltipForm("Step 1", "click the button1."))
                .AddStep(step2, new TooltipForm("Step 2", "click the button2."))
                .AddStep(step3, new TooltipForm("Step 3", "check the checkBox1."))
                .AddStep(step4, new TooltipForm("Step 4", "click the radioButton1."))
                .AddStep(step5, new TooltipForm("Step 5", "click the radioButton2."))
                .AddStep(step6, new TooltipForm("Step 6", "click the radioButton3."));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitializeTutorial();

            _tutorial.Start();
        }
    }
}
