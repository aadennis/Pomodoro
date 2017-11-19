using Windows.UI.Xaml.Controls;
using TinyPrism.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TinyPrism {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MahVm mahVm = new MahVm();
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
