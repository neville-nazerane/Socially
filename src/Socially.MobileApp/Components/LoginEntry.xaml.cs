
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Socially.MobileApp.Components;

public partial class LoginEntry : AbsoluteLayout
{

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                                   typeof(string),
                                                                                   typeof(LoginEntry),
                                                                                   null,
                                                                                   BindingMode.TwoWay,
                                                                                   propertyChanged: TextChanged);
    public bool IsPassword
    {
        get => entry.IsPassword;
        set => entry.IsPassword = value;
    }

    public string Placeholder
    {
        get => entry.Placeholder;
        set => entry.Placeholder = value;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    //public Brush ParentColor
    //{
    //    get => rect.Fill; 
    //    set => rect.Fill = value;
    //}

    public LoginEntry()
    {
        InitializeComponent();
        //ParentColor = Brush.White;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        //AbsoluteLayout.SetLayoutBounds(roundRect, new()
        //{
        //    X = 0,
        //    Y = 0,
        //    Width = 1,
        //    Height = height - 5
        //});

        //AbsoluteLayout.SetLayoutBounds(entry, new()
        //{
        //    X = 4,
        //    Y = 0,
        //    Width = width - 4,
        //    Height = height
        //});

        //AbsoluteLayout.SetLayoutBounds(rect, new()
        //{
        //    X = 0,
        //    Y = height - 12,
        //    Width = 1,
        //    Height = 7
        //});

        base.OnSizeAllocated(width, height);
    }

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((LoginEntry)bindable).Text = (string)newValue;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;
    }
}