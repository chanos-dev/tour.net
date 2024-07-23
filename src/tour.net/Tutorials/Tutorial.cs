using System;
using System.Collections.Generic;
using System.Drawing;
using tour.net.Highlight;
using tour.net.Tooltip;

namespace tour.net.Tutorials
{
    public interface ITutorial
    {
        void Start();
    }

    public class Tutorial : ITutorial, IDisposable
    {
        private int _currentIdx = 0;
        private readonly List<TutorialStep> _steps;
        private readonly TutorialConfig _tutorialConfig;

        private bool _created;
        private bool disposedValue;

        public bool Running { get; set; }

        public Tutorial()
        {
            _steps = new List<TutorialStep>();
            _tutorialConfig = TutorialConfig.DefaultTutorialConfig;
        }

        public TutorialStep this[int idx]
        {
            get
            {
                if (idx < 0 || idx >= _steps.Count)
                    throw new IndexOutOfRangeException();

                return _steps[idx];
            }
        }

        public Tutorial SetTutorialConfig(Action<TutorialConfig> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            config.Invoke(_tutorialConfig);

            return this;
        } 

        public Tutorial AddStep(HighlightForm highlightForm, DefaultTooltipForm tooltipForm, Point highlightScreenPosition = new Point())
        {
            if (highlightForm is null)
                throw new ArgumentNullException(nameof(highlightForm));

            if (tooltipForm is null)
                throw new ArgumentNullException(nameof(tooltipForm));

            Point pos = highlightScreenPosition != Point.Empty ? highlightScreenPosition : _tutorialConfig.HighlightScreenPosition;
            TutorialStep step = new TutorialStep(highlightForm, tooltipForm, pos);

            _steps.Add(step);

            return this;
        } 

        public void RemoveStep(int idx)
        {
            if (idx < 0 || idx >= _steps.Count)
                throw new ArgumentOutOfRangeException(nameof(idx));

            _steps[idx].TooltipForm.RemovePrevEvent(PrevStep);
            _steps[idx].TooltipForm.RemoveNextEvent(NextStep);
            _steps[idx].TooltipForm.RemoveExitEvent(ExitStep);

            _steps[idx].HighlightForm.Release();

            _steps.RemoveAt(idx);
        }

        public void Clear()
        {
            if (_steps.Count == 0)
                return;

            foreach (TutorialStep step in _steps)
            {
                step.TooltipForm.RemovePrevEvent(PrevStep);
                step.TooltipForm.RemoveNextEvent(NextStep);
                step.TooltipForm.RemoveExitEvent(ExitStep);

                step.HighlightForm.Release();
            }

            _steps.Clear();
        }

        private void NextStep(object sender, EventArgs args)
        {
            if (_currentIdx >= _steps.Count - 1)
            {
                _steps[_currentIdx].Hide();
                return;
            }

            _steps[++_currentIdx].Show();
            _steps[_currentIdx - 1].Hide(); 
        }

        private void PrevStep(object sender, EventArgs args)
        {
            if (_currentIdx == 0)
                return;

            _steps[--_currentIdx].Show();
            _steps[_currentIdx + 1].Hide();
        }

        private void ExitStep(object sender, EventArgs args)
        {
            _steps[_currentIdx].Hide();

            Running = false;
        }

        public ITutorial Build()
        {
            if (_steps.Count < 1)
                throw new Exception();

            if (_created)
                return this;

            for (int idx = 0; idx < _steps.Count; idx++)
            {
                _steps[idx].TooltipForm.AddPrevEvent(PrevStep);
                _steps[idx].TooltipForm.AddNextEvent(NextStep);
                _steps[idx].TooltipForm.AddExitEvent(ExitStep);

                _steps[idx].TooltipForm.SetStepInfo(idx + 1, _steps.Count);

                _steps[idx].ApplyConfig(_tutorialConfig);
            }

            _created = true;

            return this;
        }

        void ITutorial.Start()
        {
            if (_steps.Count == 0)
                throw new InvalidOperationException("Has no step.");

            if (!_created)
                throw new InvalidOperationException("You have to call the Build method.");

            Running = true;

            _currentIdx = 0;
            _steps[_currentIdx].Show();
        }

        public void Resize(Point highlightScreenPosition, Size size)
        {
            foreach (TutorialStep step in _steps)
            {
                step.Resize(size);
                step.Move(highlightScreenPosition);
            }
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }

                foreach(TutorialStep step in _steps)
                {
                    step.TooltipForm.RemovePrevEvent(PrevStep);
                    step.TooltipForm.RemoveNextEvent(NextStep);
                    step.TooltipForm.RemoveExitEvent(ExitStep);

                    step.HighlightForm.Release();
                }

                disposedValue = true;
            }
        }

        ~Tutorial()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
