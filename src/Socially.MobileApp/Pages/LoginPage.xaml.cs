using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;

namespace Socially.MobileApp.Pages;

public partial class LoginPage : ContentPage
{
    private IApiConsumer _consumer;

    


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            var consumer = Handler.MauiContext.Services.GetService<IApiConsumer>();
            _consumer = consumer;
            var res = await _consumer.LoginAsync(new Models.LoginModel
            {
                Source = "website",
                UserName = "user",
                Password = "password"
            });
        }
        catch (Exception ex)
        {

        }
    }

}