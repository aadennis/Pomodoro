using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System;

namespace Pomodoro.ViewModel {
    public class Utilities : BindableBase {
        public bool IsRunning { get; private set; }

        const int MAX_DURATION = 60;
        const int MAX_INTERVAL = 5;
        const int DEFAULT_DURATION = 20;
        const int DEFAULT_INTERVAL = 5;

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }

        public Utilities() {
            IsRunning = false;
            StartCommand = new DelegateCommand(Start, CanStart);
            ResetCommand = new DelegateCommand(Reset, CanReset);
            Duration = DEFAULT_DURATION.ToString();
            Interval = DEFAULT_INTERVAL.ToString();
        }

        private bool CanReset() {
            return IsRunning;
        }

        private void Reset() {
            IsRunning = false;
        }

        public bool CanStart() {
            return !IsRunning;
        }

        public bool CanStartx {
            get { return !IsRunning; }
            private set { }
        }

        private void Start() {
            IsRunning = true;
        }

        private void Stop() {
            IsRunning = false;
        }

        public void ResetToRun() {
            Stop();
        }

        private string _duration;

        public string Duration {
            get { return _duration; }
            set {
                if (_duration == value) return;
                if (!int.TryParse(value, out int validInteger)) {
                    _duration = DEFAULT_DURATION.ToString();
                } else {
                    _duration = value;
                }
                RaisePropertyChanged(nameof(Duration));
            }
        }

        private string _interval;

        /// <summary>
        /// Usual property behaviour defaults, plus...
        /// If value is not an integer, set the property to DEFAULT_INTEGER.
        /// Else if the value is greater than the Duration, set the Interval to the current Duration.
        /// </summary>
        public string Interval {
            get { return _interval; }
            set {
                if (_interval == value) return;
                if (!int.TryParse(value, out int validInteger)) {
                    _interval = DEFAULT_INTERVAL.ToString();
                } else if (validInteger > int.Parse(Duration)) {
                    _interval = Duration;
                }
                else {
                    _interval = value;
                }
                RaisePropertyChanged(nameof(Interval));
            }
        }

        public string MaxDuration {
            get { return MAX_DURATION.ToString(); }
        }

        public string MaxInterval {
            get { return MAX_INTERVAL.ToString(); }
        }

        public string DefaultDuration {
            get { return DEFAULT_DURATION.ToString(); }
        }

        public string DefaultInterval {
            get { return DEFAULT_INTERVAL.ToString(); }
        }
    }
}
