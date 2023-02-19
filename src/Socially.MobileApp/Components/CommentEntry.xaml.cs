using Javax.Security.Auth;
using Microsoft.Maui.Layouts;
using System.Collections.Concurrent;
using System.Windows.Input;

namespace Socially.MobileApp.Components;

public partial class CommentEntry : AbsoluteLayout
{


    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
                                                                                  typeof(ICommand),
                                                                                  typeof(CommentEntry),
                                                                                  propertyChanged: CommandPropertyChanged);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public CommentEntry()
	{
		InitializeComponent();
	}

    private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((CommentEntry)bindable).Command = (ICommand)newValue;

    private void Entry_Completed(object sender, EventArgs e)
    {
        if (Command?.CanExecute(entry.Text) == true) Command.Execute(entry.Text);
    }

}