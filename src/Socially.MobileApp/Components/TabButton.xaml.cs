using Java.Nio.Channels;

namespace Socially.MobileApp.Components;

public partial class TabButton : AbsoluteLayout
{
    private bool _isSelected;
    private int _selectedThickness;
    private int _outterThickness;
    private int _borderThickness;
    private int _imageMargin;


    public string FontFamily
    {
        get => (img.Source as FontImageSource).FontFamily;
        set => GetOrCreateFontImageSource().FontFamily = value;
    }

    public string Glyph
    {
        get => (img.Source as FontImageSource).Glyph;
        set => GetOrCreateFontImageSource().Glyph = value;
    }

    public bool IsSelected
    {
        get => _isSelected; 
        set
        {
            _isSelected = value;
        }
    }

    public int SelectedThickness
    {
        get => _selectedThickness; 
        set
        {
            _selectedThickness = value;
            Reset();
        }
    }

    public int OutterThickness
    {
        get => _outterThickness; 
        set
        {
            _outterThickness = value;
            Reset();
        }
    }

    public int BorderThickness
    {
        get => _borderThickness; 
        set
        {
            _borderThickness = value;
            Reset();
        }
    }

    public int ImageMargin
    {
        //get => img.Margin;
        //set => img.Margin = value;
        get => _imageMargin;
        set
        {
            _imageMargin = value;
            Reset();
        }
    }

    FontImageSource GetOrCreateFontImageSource()
    {
        if (!(img.Source is FontImageSource imageSource))
        {
            imageSource = new();
            img.Source = imageSource;
        }
        return imageSource;
    }

    public TabButton()
    {
        InitializeComponent();
    }


    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        Reset();
    }

    void Reset()
    {
        // setting up IMAGE
        int imageReduction = BorderThickness + ImageMargin;
        AbsoluteLayout.SetLayoutBounds(img, new()
        {
            X = imageReduction,
            Y = imageReduction,
            Width = Width - imageReduction * 2,
            Height = Height - imageReduction * 2
        });

        if (IsSelected)
        {
            //// setting up RECT
            AbsoluteLayout.SetLayoutBounds(rect, new()
            {
                X = 0,
                Y = 0,
                Height = Height,
                Width = Width
            });

            // setup up OUTTER
            int outterAddition = SelectedThickness + OutterThickness;
            AbsoluteLayout.SetLayoutBounds(outterCircle, new()
            {
                X = -outterAddition,
                Y = -outterAddition,
                Height = Height + outterAddition * 2,
                Width = Width + outterAddition * 2
            });
            outterCircle.StrokeThickness = BorderThickness + SelectedThickness + OutterThickness;

            // setting up INNER

            AbsoluteLayout.SetLayoutBounds(innerCircle, new()
            {
                X = -SelectedThickness,
                Y = -SelectedThickness,
                Height = Height + SelectedThickness * 2,
                Width = Width + SelectedThickness * 2
            });
            innerCircle.StrokeThickness = SelectedThickness;

        }
        else
        {
            // setting up INNER
            AbsoluteLayout.SetLayoutBounds(innerCircle, new()
            {
                X = -SelectedThickness,
                Y = -SelectedThickness,
                Height = Height,
                Width = Width
            });
            innerCircle.StrokeThickness = BorderThickness;
        }


        //rect.IsVisible = IsSelected;
        //outterCircle.IsVisible = IsSelected;

    }

}