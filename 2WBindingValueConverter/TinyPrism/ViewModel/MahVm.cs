using Prism.Mvvm;
using System;
using Windows.UI.Xaml.Data;

namespace TinyPrism.ViewModel {
    public class MahVm : BindableBase {
        const int DEFAULT_DURATION = 20;

        private string _name;

        public string Name {
            get { return _name; }
            set {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string _someNumber;

        public string SomeNumber {
            get { return _someNumber; }
            set {
                if (_someNumber == value) return;
                if (!int.TryParse(value, out int dummy)) {
                    _someNumber = DEFAULT_DURATION.ToString(); //arbitrary value on failure
                }
                else {
                    _someNumber = value;
                }
                RaisePropertyChanged(nameof(SomeNumber));
            }
        }


        public MahVm() {
            Name = "TweetyPie";
            SomeNumber = "22";
        }
    }

   
}
