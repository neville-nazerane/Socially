
using Android.Content;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Socially.MobileApp.Helpers;
using System.Windows.Input;

namespace Socially.MobileApp.Components;

public partial class LoginEntry : AbsoluteLayout
{

    #region Bindable property

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                                   typeof(string),
                                                                                   typeof(LoginEntry),
                                                                                   null,
                                                                                   BindingMode.TwoWay,
                                                                                   propertyChanged: TextChanged);

    public static readonly BindableProperty NextElementProperty = BindableProperty.Create(nameof(NextElement),
                                                                                          typeof(VisualElement),
                                                                                          typeof(LoginEntry),
                                                                                          propertyChanged: NextElementChanged);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
                                                                                      typeof(ICommand),
                                                                                      typeof(LoginEntry),
                                                                                      propertyChanged: CommandPropertyChanged);

    public static readonly BindableProperty IsErrorProperty = BindableProperty.Create(nameof(IsError),
                                                                                      typeof(bool),
                                                                                      typeof(LoginEntry),
                                                                                      propertyChanged: IsErrorChanged);

    #endregion

    public bool IsError
    {
        get => (bool)GetValue(IsErrorProperty);
        set
        {
            SetValue(IsErrorProperty, value);
            roundRect.SetStyleClass(value ? "error" : "unselected");
        }
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public VisualElement NextElement
    {
        get => (VisualElement)GetValue(NextElementProperty);
        set
        {
            SetValue(NextElementProperty, value);
            entry.ReturnType = ReturnType.Next;
            var dest = value is LoginEntry le ? le.entry : value;
            SetFocusOnEntryCompletedBehavior.SetNextElement(entry, dest);
        }
    }

    public ReturnType ReturnType
    {
        get => entry.ReturnType;
        set => entry.ReturnType = value;
    }

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

    public LoginEntry()
    {
        InitializeComponent();
    }


    #region events

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (!IsError)
            roundRect.SetStyleClass("selected");
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        roundRect.SetStyleClass("unselected");
    }

    private void Entry_Completed(object sender, EventArgs e)
    {
        if (Command?.CanExecute(null) == true) Command.Execute(null);
    }

    #endregion

    #region static changed

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        => ((LoginEntry)bindable).Text = (string)newValue;

    private static void NextElementChanged(BindableObject bindable, object oldValue, object newValue)
        => ((LoginEntry)bindable).NextElement = (VisualElement)newValue;

    private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((LoginEntry)bindable).Command = (ICommand)newValue;

    private static void IsErrorChanged(BindableObject bindable, object oldValue, object newValue)
        => ((LoginEntry)bindable).IsError = (bool)newValue;

    #endregion
}