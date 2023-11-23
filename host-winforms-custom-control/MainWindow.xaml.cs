using System.Windows;

namespace host_winforms_custom_control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
                AlertViewer.DisplayAlert(
                    "This is an Alert"
            );
        }
    }
}