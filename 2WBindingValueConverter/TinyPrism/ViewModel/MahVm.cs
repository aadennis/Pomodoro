﻿using Prism.Mvvm;
using System;
using Windows.UI.Xaml.Data;

namespace TinyPrism.ViewModel {
    public class MahVm : BindableBase {
        const int DEFAULT_DURATION = 20;
        string _lastSomeNumber = DEFAULT_DURATION.ToString();

        private string _someNumber;

        public string SomeNumber {
            get { return _someNumber; }
            set {
                if (_someNumber == value) return;
                if (!int.TryParse(value, out int dummy)) {
                    // entered number is not valid - reset to previous value
                    _someNumber = _lastSomeNumber;
                }
                else {
                    // ok...
                    _someNumber = _lastSomeNumber = value;
                 }
                RaisePropertyChanged(nameof(SomeNumber));
            }
        }

        public MahVm() {
            SomeNumber = "22";
        }
    }
}
