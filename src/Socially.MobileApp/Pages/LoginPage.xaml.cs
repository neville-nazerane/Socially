using Socially.Apps.Consumer.Services;

namespace Socially.MobileApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly IApiConsumer _consumer;

    public LoginPage(IApiConsumer consumer)
	{
		InitializeComponent();
        _consumer = consumer;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}