

using Socially.Mobile.Logic.ViewModels;
using Socially.MobileApp.Pages;

namespace Socially.MobileApp;

public static partial class MauiProgram
{

    static partial void AppPageInjections(IServiceCollection services)
    {
        services.AddTransient<HomePage>().AddTransient<HomeViewModel>()
.AddTransient<InitialPage>().AddTransient<InitialViewModel>()
.AddTransient<LoginPage>().AddTransient<LoginViewModel>();
    }
        
}

