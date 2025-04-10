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
        Target = TA1;
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
        var pos1 = source.TranslatePoint(new Point(), TheGrid);
        pos1.X += source.Width / 2;
        pos1.Y += source.Height / 2;
        var pos2 = Target.TranslatePoint(new Point(), TheGrid);
        pos2.X += Target.Width / 2;
        pos2.Y += Target.Height / 2;

        string pathString = $"M {pos1.X},{pos1.Y} L {pos2.X},{pos2.Y}";
        Line = new();
        Line.Stroke = Brushes.Black;
        Line.Data = Geometry.Parse(pathString);
        TheGridParent.Children.Add(Line);
    }
}