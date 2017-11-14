using Prism.Commands;
using System.Windows.Input;
using System;

namespace Pomodoro.ViewModel {
    public class Utilities {
        public bool IsRunning { get; private set; } 

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }

        public Utilities() {
            IsRunning = false;
            StartCommand = new DelegateCommand(Start, CanStart);
            ResetCommand = new DelegateCommand(Reset, CanReset);
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
    }
}
