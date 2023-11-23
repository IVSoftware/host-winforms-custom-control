# Host Winforms Custom Control

As I understand it, you're using a third party `AlertViewer` and let's say it exposes properties that control an animated fade in. 

```
public class AlertViewer
{
    double AlertOpacity { get; set; }
    double Step { get; set; }
}
```

And the objective might be to apply a `Style` to instances of `AlertViewer` in order to control the font and animation.


```
<Window.Resources>
    <Style TargetType="{x:Type local:WindowsAlertHost}">
        <Setter Property="FontFamily" Value="Audiowide" />
        <Setter Property="FontSize" Value="24" />
        <Setter Property="AlertOpacity" Value="1" />
        <Setter Property="Step" Value="0.01" />
    </Style>
</Window.Resources>
```
___

Since the the animation properties obviously aren't available in `WindowsFormsHost`, it's necessary to take matters into our own hands, making a custom `WindowsAlertHost : WindowsFormsHost` that lets you do anything you want in terms of style to an `AlertViewer` that it specifically hosts.

```
public class WindowsAlertHost : WindowsFormsHost
{
    public WindowsAlertHost() => Child = new AlertViewer();

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
```
[![animation][1]][1]

```
<Window x:Class="host_winforms_custom_control.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:host_winforms_custom_control"
        mc:Ignorable="d"
        Title="MainWindow" Width="500" Height="300" Background="RoyalBlue">
    <Window.Resources>
        <Style TargetType="{x:Type local:WindowsAlertHost}">
            <Setter Property="FontFamily" Value="Audiowide" />
            <Setter Property="FontSize" Value="24" />
            <Setter Property="AlertOpacity" Value="1" />
            <Setter Property="Step" Value="0.01" />
        </Style>
    </Window.Resources>
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <Grid 
            Grid.Row="1"
            Background="Azure">
            <local:WindowsAlertHost Margin="10" />
        </Grid>
    </Grid>
</Window>
```
___

##### ALTERNATIVE

Your question, and my answer, are about how to **define a style** which implies that there are several of the same kind of control in use, and that the look and behavior of all of them should consistent. I'd be remiss if I didn't mention that setting properties directly with text or a StaticResource is much simpler.
###### Direct set properties on hosted object

```
<Window x:Class="host_winforms_custom_control.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:host_winforms_custom_control"
        mc:Ignorable="d"
        Title="MainWindow" Width="500" Height="300" Background="RoyalBlue">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <Grid 
            Grid.Row="1"
            Background="Azure">
            <WindowsFormsHost 
                x:Name="AlertHost"
                FontFamily="Audiowide"
                FontSize="24"
                Margin="10" >
                <local:AlertViewer
                    x:Name="AlertViewer"
                    Step="0.01"
                    AlertOpacity="1.0"/>
            </WindowsFormsHost>
        </Grid>
    </Grid>
</Window>
```


  [1]: https://i.stack.imgur.com/IJ1JX.png