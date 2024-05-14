using System;
using System.Collections.Generic;
using System.Drawing;
using tour.net.Tooltip;

namespace tour.net.Tutorial
{
    public class TutorialManager : IDisposable
    {
        private int _currentIdx = 0;
        private readonly List<TutorialStep> _steps;

        private Point _baseAreaLocation = Point.Empty;
        private bool disposedValue;

        public bool Running { get; set; }

        public TutorialManager()
        {
            _steps = new List<TutorialStep>();
        }

        public TutorialManager SetBaseAreaLocation(Point baseAreaLocation)
        {
            if (baseAreaLocation == Point.Empty)
                throw new ArgumentException(nameof(baseAreaLocation));

            _baseAreaLocation = baseAreaLocation;

            return this;
        } 

        public TutorialManager AddStep(HighlightForm highlightForm, TooltipForm tooltipForm, Point screenPos = new Point())
        {
            if (highlightForm is null)
                throw new ArgumentNullException(nameof(highlightForm));

            if (tooltipForm is null)
                throw new ArgumentNullException(nameof(tooltipForm));

            tooltipForm.AddPrevEvent(PrevStep);
            tooltipForm.AddNextEvent(NextStep);
            tooltipForm.AddExitEvent(ExitStep);

            _steps.Add(new TutorialStep(highlightForm, tooltipForm, _baseAreaLocation != Point.Empty ? _baseAreaLocation : screenPos));

            return this;
        }

        private void NextStep(object sender, EventArgs args)
        {
            if (_currentIdx >= _steps.Count - 1)
                return;

            _steps[_currentIdx].Hide();
            _steps[++_currentIdx].Show();
        }

        private void PrevStep(object sender, EventArgs args)
        {
            if (_currentIdx == 0)
                return;

            _steps[_currentIdx].Hide();
            _steps[--_currentIdx].Show();
        }

        private void ExitStep(object sender, EventArgs args)
        {
            foreach (var step in _steps)
                step.Hide();

            Running = false;
        }

        public void Start()
        {
            Running = true;

            _currentIdx = 0;
            _steps[_currentIdx].Show();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }

                foreach(var step in _steps)
                {
                    step.TooltipForm.RemovePrevEvent(PrevStep);
                    step.TooltipForm.RemoveNextEvent(NextStep);
                    step.TooltipForm.RemoveExitEvent(ExitStep);
                }

                disposedValue = true;
            }
        }

        ~TutorialManager()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
