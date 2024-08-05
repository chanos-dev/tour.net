using System;
using tour.net.Tooltip;

namespace tour.net.app
{
    public partial class CustomTooltipForm : TooltipForm
    {
        public CustomTooltipForm()
        {
            InitializeComponent();
        }

        public override void AddExitEvent(EventHandler exitEvent)
        {
            button3.Click += exitEvent;
        }

        public override void RemoveExitEvent(EventHandler exitEvent)
        {
            button3.Click -= exitEvent;
        }

        public override void AddNextEvent(EventHandler nextEvent)
        {
            button2.Click += nextEvent;
        }

        public override void RemoveNextEvent(EventHandler nextEvent)
        {
            button2.Click -= nextEvent;
        }

        public override void AddPrevEvent(EventHandler prevEvent)
        {
            button1.Click += prevEvent;
        }

        public override void RemovePrevEvent(EventHandler prevEvent)
        {
            button1.Click -= prevEvent;
        }
    }
}
