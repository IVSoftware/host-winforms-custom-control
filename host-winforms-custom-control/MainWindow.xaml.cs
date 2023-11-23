using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using IWin32Window = System.Windows.Forms.IWin32Window;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace host_winforms_custom_control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWin32Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AlertViewer = AlertHost;
            Loaded += (sender, e) =>
                AlertViewer.DisplayAlert(
                    "This is an Alert"
            );
        }

        public nint Handle => ((HwndSource)PresentationSource.FromVisual(this)).Handle;

        AlertViewer AlertViewer { get; } 
    }
}