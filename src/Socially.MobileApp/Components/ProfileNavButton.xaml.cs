using Microsoft.Maui.Controls.Shapes;

namespace Socially.MobileApp.Components;

public partial class ProfileNavButton : Grid
{
    const int cornerRadius = 20;

    public string Text
    {
        get => lbl.Text;
        set => lbl.Text = value;
    }

    public string FontFamily
    {
        get => FontImageSource.FontFamily;
        set => FontImageSource.FontFamily = value;
    }

    public string Glyph
    {
        get => FontImageSource.Glyph;
        set => FontImageSource.Glyph = value;
    }

    FontImageSource FontImageSource => img.Source as FontImageSource;

    public ProfileNavButton()
    {
        InitializeComponent();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        Clip = new RoundRectangleGeometry
        {
            CornerRadius = cornerRadius,
            Rect = new()
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            }
        };
        base.OnSizeAllocated(width, height);
    }

}