

using Socially.Mobile.Logic.ViewModels;
using Socially.MobileApp.Pages;

namespace Socially.MobileApp;

public static partial class MauiProgram
{

    static partial void AppPageInjections(IServiceCollection services)
    {
        services.AddTransient<ForgotPasswordPage>().AddTransient<ForgotPasswordViewModel>()
.AddTransient<HomePage>().AddTransient<HomeViewModel>()
.AddTransient<InitialPage>().AddTransient<InitialViewModel>()
.AddTransient<LoginPage>().AddTransient<LoginViewModel>()
.AddTransient<ProfileFriendsPage>().AddTransient<ProfileFriendsViewModel>()
.AddTransient<ProfilePostsPage>().AddTransient<ProfilePostsViewModel>()
.AddTransient<SignupPage>().AddTransient<SignupViewModel>();
    }
        
}

