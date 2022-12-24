using CommunityToolkit.Mvvm.ComponentModel;

namespace Socially.MobileApp.Components;

public partial class LoginBanner : Grid
{

    

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), 
                                                                                        typeof(string), 
                                                                                        typeof(LoginBanner), 
                                                                                        null,
                                                                                        propertyChanged: LabelTextChanged);

    public string LabelText 
    {
        get => lbl.Text;
        set
        {
            lbl.Text = value;
            SetValue(LabelTextProperty, value);
        }
    }

    public LoginBanner()
    {
        InitializeComponent();
    }

    static void LabelTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((LoginBanner)bindable).LabelText = (string)newValue;
    }


}