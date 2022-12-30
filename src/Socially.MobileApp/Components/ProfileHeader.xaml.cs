using Microsoft.Maui.Controls.Shapes;

namespace Socially.MobileApp.Components;

public partial class ProfileHeader : AbsoluteLayout
{

    const int buttonCount = 3;

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

    protected override void OnSizeAllocated(double orgWidth, double orgHeight)
    {
        var buttonSize = orgWidth / buttonCount - btnGrid.ColumnSpacing * (buttonCount - 1);

        var cols = Enumerable.Repeat(buttonSize, 3)
                             .Select(s => new ColumnDefinition(s))
                             .ToArray();
        btnGrid.ColumnDefinitions = new(cols);

        btnGrid.RowDefinitions = new RowDefinitionCollection
        {
            new(buttonSize)
        };

        AbsoluteLayout.SetLayoutBounds(btnGrid, new()
        {
            X = 0,
            Y = orgHeight - buttonSize,
            Width = orgWidth,
            Height = buttonSize
        });

        var width = orgWidth;
        var height = orgHeight - buttonSize * (2 / 3);
        var imageSize = Math.Min(width, height) / 3;
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


        base.OnSizeAllocated(orgWidth, orgHeight);
    }

}