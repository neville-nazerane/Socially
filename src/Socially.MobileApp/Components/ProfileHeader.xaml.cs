using Microsoft.Maui.Controls.Shapes;

namespace Socially.MobileApp.Components;

public partial class ProfileHeader : AbsoluteLayout
{
    public ProfileHeader()
    {
        InitializeComponent();
    }

    protected override async void OnParentSet()
    {
        if (Parent is not null)
            await ComponentModel.RefreshUserAsync();

        base.OnParentSet();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        var imageSize = Math.Min(width, height) / 2;
        var imageX = (width - imageSize) / 2;
        var imageY = (height - imageSize) / 3;

        AbsoluteLayout.SetLayoutBounds(img, new()
        {
            X = imageX,
            Y = imageY,
            Height = imageSize,
            Width = imageSize
        });

        img.Clip = new RoundRectangleGeometry
        {
            CornerRadius = new(imgBorder.CornerRadius.TopRight),
            Rect = new()
            {
                X = 2,
                Y = 2,
                Height = imageSize - 4,
                Width = imageSize - 4
            }
        };

        AbsoluteLayout.SetLayoutBounds(imgBorder, new()
        {
            X = imageX,
            Y = imageY,
            Height = imageSize,
            Width = imageSize
        });

        AbsoluteLayout.SetLayoutBounds(name, new()
        {
            X = 0,
            Y = imageY + imageSize + 10,
            Height = 60,
            Width = width
        });


        base.OnSizeAllocated(width, height);
    }

}