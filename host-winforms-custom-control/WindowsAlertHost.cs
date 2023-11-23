using System.Windows;
using System.Windows.Forms.Integration;

namespace host_winforms_custom_control
{
    public class WindowsAlertHost : WindowsFormsHost
    {
        public WindowsAlertHost() => Child = new AlertViewer();

        public static implicit operator AlertViewer(WindowsAlertHost host) =>
            (AlertViewer)host.Child;

        public static readonly DependencyProperty AlertOpacityProperty =
            DependencyProperty.Register(
                "AlertOpacity",
                typeof(float),
                typeof(WindowsAlertHost),
                new PropertyMetadata(default(float), OnAlertOpacityChanged));
        public float AlertOpacity
        {
            get => (float)GetValue(AlertOpacityProperty);
            set => SetValue(AlertOpacityProperty, value);
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                "Step",
                typeof(float),
                typeof(WindowsAlertHost),
                new PropertyMetadata(default(float), OnStepChanged));

        public float Step
        {
            get => (float)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        private static void OnAlertOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowsAlertHost host && host.Child is AlertViewer viewer)
            {
                viewer.Opacity = (float)e.NewValue;
            }
        }

        private static void OnStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowsAlertHost host && host.Child is AlertViewer viewer)
            {
                viewer.Step = (float)e.NewValue;
            }
        }
    }
}
