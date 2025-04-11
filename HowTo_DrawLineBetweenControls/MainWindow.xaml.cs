using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HowTo_DrawLineBetweenControls;

/// <summary>
/// Note: Resizing window is not accounted for.
/// </summary>
public partial class MainWindow : Window
{
    private Button Target { get; set; }
    private Path? Line { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        Target = TargetA;
    }

    private void B_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn is null) return;
        DrawLineToTarget(btn);
    }

    private void DrawLineToTarget(Button source)
    {
        if(Line is not null)
        {
            TheGridParent.Children.Remove(Line);
            Line = null;
        }
        var centerOfSourceControl = source.TranslatePoint(new Point(), TheGrid);
        centerOfSourceControl.X += source.Width / 2;
        centerOfSourceControl.Y += source.Height / 2;
        var centerOfTargetControl = Target.TranslatePoint(new Point(), TheGrid);
        centerOfTargetControl.X += Target.Width / 2;
        centerOfTargetControl.Y += Target.Height / 2;

        string pathString = $"M {centerOfSourceControl.X},{centerOfSourceControl.Y} L {centerOfTargetControl.X},{centerOfTargetControl.Y}";
        Line = new()
        {
            Stroke = Brushes.Black,
            Data = Geometry.Parse(pathString)
        };
        TheGridParent.Children.Add(Line);
    }
}