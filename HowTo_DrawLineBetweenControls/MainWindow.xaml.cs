using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        //DrawLineToTarget(btn);
        ShootLineAtTarget(btn);
    }

    private void ShootLineAtTarget(Button btn)
    {
        if (this.Line is not null)
        {
            TheGridParent.Children.Remove(Line);
            this.Line = null;
        }

        var Points = GetPositions(btn);

        // Create a NameScope for this page so that
        // Storyboards can be used.
        NameScope.SetNameScope(this, new NameScope());

        EllipseGeometry myEllipseGeometry = new EllipseGeometry();
        myEllipseGeometry.Center = Points.Source;
        myEllipseGeometry.RadiusX = 8;
        myEllipseGeometry.RadiusY = 8;

        // Assign the EllipseGeometry a name so that
        // it can be targeted by a Storyboard.
        this.RegisterName(
            "MyAnimatedEllipseGeometry", myEllipseGeometry);

        Line = new Path();
        Line.Fill = Brushes.Blue;
        Line.Margin = new Thickness(5);
        Line.Data = myEllipseGeometry;

        PointAnimation myPointAnimation = new PointAnimation();
        myPointAnimation.Duration = TimeSpan.FromSeconds(0.3);

        // Set the animation to repeat forever.
        //myPointAnimation.RepeatBehavior = RepeatBehavior.Forever;

        // Set the From and To properties of the animation.
        myPointAnimation.From = Points.Source;
        myPointAnimation.To = Points.Target;

        // Set the animation to target the Center property
        // of the object named "MyAnimatedEllipseGeometry."
        Storyboard.SetTargetName(myPointAnimation, "MyAnimatedEllipseGeometry");
        Storyboard.SetTargetProperty(
            myPointAnimation, new PropertyPath(EllipseGeometry.CenterProperty));

        myPointAnimation.Completed += MyPointAnimation_Completed;

        // Create a storyboard to apply the animation.
        Storyboard ellipseStoryboard = new Storyboard();
        ellipseStoryboard.Children.Add(myPointAnimation);

        // Start the storyboard when the Path loads.
        Line.Loaded += delegate (object sender, RoutedEventArgs e)
        {
            ellipseStoryboard.Begin(this);
        };

        //Canvas containerCanvas = new Canvas();
        TheGridParent.Children.Add(Line);

        //Content = containerCanvas;

    }

    private void MyPointAnimation_Completed(object? sender, EventArgs e)
    {
        if(Line is not null)
        {
            TheGridParent.Children.Remove(Line);
            Line = null;
        }
    }

    private void DrawLineToTarget(Button source)
    {
        if(Line is not null)
        {
            TheGridParent.Children.Remove(Line);
            Line = null;
        }
        var Points = GetPositions(source);
        string pathString = $"M {Points.Source.X},{Points.Source.Y} L {Points.Target.X},{Points.Target.Y}";
        Line = new()
        {
            Stroke = Brushes.Black,
            Data = Geometry.Parse(pathString)
        };
        TheGridParent.Children.Add(Line);
    }

    private (Point Source, Point Target) GetPositions(Button source)
    {
        var centerOfSourceControl = source.TranslatePoint(new Point(), TheGrid);
        centerOfSourceControl.X += source.Width / 2;
        centerOfSourceControl.Y += source.Height / 2;
        var centerOfTargetControl = Target.TranslatePoint(new Point(), TheGrid);
        centerOfTargetControl.X += Target.Width / 2;
        centerOfTargetControl.Y += Target.Height / 2;

        return (centerOfSourceControl, centerOfTargetControl);
    }
}