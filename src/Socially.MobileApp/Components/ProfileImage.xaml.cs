namespace Socially.MobileApp.Components;

public partial class ProfileImage : Grid
{

    public double BorderThickness
    {
        get => border.StrokeThickness;
        set
        {
            border.StrokeThickness = value;
            Repaint();
        }
    }

    public ProfileImage()
    {
        InitializeComponent();
        BorderThickness = 3;
    }

    void Repaint()
    {
        if (Height + Width < 0) return;
        imageClip.Rect = new()
        {
            X = BorderThickness,
            Y = BorderThickness,
            Height = Height - BorderThickness * 2,
            Width = Width - BorderThickness * 2,
        };
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        Repaint();
    }

}