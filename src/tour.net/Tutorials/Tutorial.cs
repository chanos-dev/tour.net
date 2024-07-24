using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using tour.net.Exceptions;
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
        private Form _form;
        private int _currentIdx = 0;
        private readonly List<TutorialStep> _steps;
        private readonly TutorialConfig _tutorialConfig;
        private bool _created;
        private bool _disposedValue;

        /// <summary>
        /// Running tutorial.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Count tutorial steps.
        /// </summary>
        public int Count => _steps.Count;

        /// <summary>
        /// New tutorial instance.
        /// </summary>
        /// <param name="mainForm">tutorial mainform</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Tutorial(Form mainForm)
        {
            if (mainForm is null)
                throw new ArgumentNullException(nameof(mainForm));

            _form = mainForm;
            _steps = new List<TutorialStep>();
            _tutorialConfig = TutorialConfig.DefaultTutorialConfig;

            SetAutoResize();
        }

        /// <summary>
        /// Set tutorial config.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>tutorial instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Tutorial SetTutorialConfig(Action<TutorialConfig> config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            config.Invoke(_tutorialConfig);

            return this;
        }

        private void SetAutoResize()
        {
            _form.LocationChanged += ResizeEvent;
            _form.SizeChanged += ResizeEvent;
        }

        private void ResizeEvent(object sender, EventArgs e)
        {
            Resize(_form.PointToScreen(Point.Empty), _form.ClientSize);
        }

        /// <summary>
        /// Add tutorial step.
        /// </summary>
        /// <param name="highlightForm">set highlight form</param>
        /// <param name="tooltipForm">set tooltip form</param>
        /// <returns>tutorial instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Tutorial AddStep(HighlightForm highlightForm, DefaultTooltipForm tooltipForm)
        {
            if (highlightForm is null)
                throw new ArgumentNullException(nameof(highlightForm));

            if (tooltipForm is null)
                throw new ArgumentNullException(nameof(tooltipForm));

            TutorialStep step = new TutorialStep(highlightForm, tooltipForm);

            _steps.Add(step);

            return this;
        } 

        /// <summary>
        /// Remove tutorial step.
        /// </summary>
        /// <param name="idx">index</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveStep(int idx)
        {
            if (idx < 0 || idx >= _steps.Count)
                throw new IndexOutOfRangeException(nameof(idx));

            _steps[idx].TooltipForm.RemovePrevEvent(PrevStep);
            _steps[idx].TooltipForm.RemoveNextEvent(NextStep);
            _steps[idx].TooltipForm.RemoveExitEvent(ExitStep);

            _steps[idx].HighlightForm.Release();

            _steps.RemoveAt(idx);
        }

        /// <summary>
        /// Clear tutorial steps.
        /// </summary>
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

        /// <summary>
        /// Build tutorial.
        /// </summary>
        /// <returns>An ITutorial interface for starting the tutorial.</returns>
        /// <exception cref="EmptyTutorialStepException"></exception>
        public ITutorial Build()
        {
            if (_steps.Count < 1)
                throw new EmptyTutorialStepException();

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

        /// <summary>
        /// Start tutorial.
        /// </summary>
        /// <exception cref="EmptyTutorialStepException"></exception>
        /// <exception cref="TutorialNotBuiltException"></exception>
        void ITutorial.Start()
        {
            if (_steps.Count == 0)
                throw new EmptyTutorialStepException();

            if (!_created)
                throw new TutorialNotBuiltException();

            Running = true;

            ResizeEvent(null, null);
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

        private void Release()
        {
            if (_form is null)
                return;

            _form.LocationChanged -= ResizeEvent;
            _form.SizeChanged -= ResizeEvent;
            _form = null;
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                Clear();
                Release();

                _disposedValue = true;
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
