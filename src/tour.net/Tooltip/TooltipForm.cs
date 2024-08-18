using System;
using System.Drawing;
using System.Windows.Forms;

namespace tour.net.Tooltip
{
    public class TooltipForm : Form
    {
        public TooltipForm()
        {
            ShowInTaskbar = false;
        }

        /// <summary>
        /// Adds an event handler for the Exit button click event.
        /// </summary>
        /// <param name="exitEvent">The event handler to add.</param>
        public virtual void AddExitEvent(EventHandler exitEvent) { }

        /// <summary>
        /// Adds an event handler for the Next button click event.
        /// </summary>
        /// <param name="nextEvent">The event handler to add.</param>
        public virtual void AddNextEvent(EventHandler nextEvent) { }

        /// <summary>
        /// Adds an event handler for the Previous button click event.
        /// </summary>
        /// <param name="prevEvent">The event handler to add.</param>
        public virtual void AddPrevEvent(EventHandler prevEvent) { }

        /// <summary>
        /// Removes an event handler for the Exit button click event.
        /// </summary>
        /// <param name="exitEvent">The event handler to remove.</param>
        public virtual void RemoveExitEvent(EventHandler exitEvent) { }

        /// <summary>
        /// Removes an event handler for the Next button click event.
        /// </summary>
        /// <param name="nextEvent">The event handler to remove.</param>
        public virtual void RemoveNextEvent(EventHandler nextEvent) { }

        /// <summary>
        /// Removes an event handler for the Previous button click event.
        /// </summary>
        /// <param name="prevEvent">The event handler to remove.</param>
        public virtual void RemovePrevEvent(EventHandler prevEvent) { }

        /// <summary>
        /// Sets the step information for the tooltip, such as the current step index and the total number of steps.
        /// </summary>
        /// <param name="stepIndex">The current step index.</param>
        /// <param name="totalStepsCount">The total number of steps.</param>
        public virtual void SetStepInfo(int stepIndex, int totalStepsCount) { }

        /// <summary>
        /// Moves the tooltip to the specified location based on the provided bounds of the highlighted control.
        /// </summary>
        /// <param name="highlightControlBounds">The bounds of the control to highlight.</param>
        public virtual void MoveTooltip(Rectangle highlightControlBounds)
        {
            Location = new Point(highlightControlBounds.X, highlightControlBounds.Y);
        }
    }
}
