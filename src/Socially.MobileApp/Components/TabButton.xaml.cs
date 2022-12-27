using Java.Nio.Channels;
using Kotlin.Reflect;
using Socially.MobileApp.Helpers;

namespace Socially.MobileApp.Components;

public partial class TabButton : AbsoluteLayout
{

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected),
                                                                                         typeof(bool),
                                                                                         typeof(TabButton),
                                                                                         propertyChanged: TabButtonChanged);

    private static void TabButtonChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((TabButton)bindable).IsSelected = (bool)newValue;
    }

    private int _selectedThickness;
    private int _outterThickness;
    private int _borderThickness;
    private int _imageMargin;
    private string _fontFamily;
    private string _glyph;

    public string FontFamily
    {
        get => _fontFamily; 
        set
        {
            _fontFamily = value;
            RefreshImage();
        }
    }
    //{
    //    get => (img.Source as FontImageSource).FontFamily;
    //    set => GetOrCreateFontImageSource().FontFamily = value;
    //}

    public string Glyph
    {
        get => _glyph; 
        set
        {
            _glyph = value;
            RefreshImage();
        }
    }
    //{
    //    get => (img.Source as FontImageSource).Glyph;
    //    set => GetOrCreateFontImageSource().Glyph = value;
    //}

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set
        {
            SetValue(IsSelectedProperty, value);
            //_isSelected = value;
            Reset();
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

    //FontImageSource GetOrCreateFontImageSource()
    //{
    //    if (!(img.Source is FontImageSource imageSource))
    //    {
    //        imageSource = new();
    //        img.Source = imageSource;
    //    }
    //    return imageSource;
    //}

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
        img.SetStyleClass(IsSelected ? "selected" : "unselected");
        RefreshImage();

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
                X = 0,
                Y = 0,
                Height = Height,
                Width = Width
            });
            innerCircle.StrokeThickness = BorderThickness;
        }

        rect.IsVisible = IsSelected;
        outterCircle.IsVisible = IsSelected;
    }

    void RefreshImage()
    {
        if (img.Source is FontImageSource imageSource && FontFamily is not null && Glyph is not null)
        {
            imageSource.FontFamily = FontFamily;
            imageSource.Glyph = Glyph;
        }
    }

    private void Img_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Image.Source))
            RefreshImage();
    }
}