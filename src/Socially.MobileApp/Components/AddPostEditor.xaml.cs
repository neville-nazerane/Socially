namespace Socially.MobileApp.Components;

public partial class AddPostEditor : AbsoluteLayout
{

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                                   typeof(string),
                                                                                   typeof(AddPostEditor),
                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                   propertyChanged: TextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set
        {
            SetValue(TextProperty, value);
            editor.Text = value;
        }
    }

    public AddPostEditor()
    {
        InitializeComponent();
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;
    }

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue) 
        => (bindable as AddPostEditor).Text = (string)newValue;

}