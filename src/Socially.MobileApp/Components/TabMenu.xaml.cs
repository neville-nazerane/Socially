using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class TabMenu : AbsoluteLayout
{
    private readonly INavigationControl _navigationControl;
    private readonly IMessaging _messaging;

    public TabMenu()
    {
        InitializeComponent();
        _navigationControl = ServicesUtil.Get<INavigationControl>();
        _messaging = ServicesUtil.Get<IMessaging>();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        var location = Shell.Current.CurrentState.Location.OriginalString;

        switch (location)
        {
            case "//MainPage/home":
                SetAsSelected(homeBtn);
                break;
            case "//MainPage/profile/posts":
                SetAsSelected(profileBtn);
                break;
            case "//MainPage/account":
                SetAsSelected(settingsBtn);
                break;
        }

    }

    void SetAsSelected(TabButton btn)
    {
        btn.IsSelected = true;
    }

    private async void Home_Tapped(object sender, TappedEventArgs e)
    {
        await _navigationControl.GoToHomeAsync();
    }

    private async void Profile_Tapped(object sender, TappedEventArgs e)
    {
        await _navigationControl.GoToProfilePostsAsync();
    }

    private async void Settings_Tapped(object sender, TappedEventArgs e)
    {
        settingsBtn.IsSelected = true;
        try
        {
            var response = await _messaging.DisplayActionSheet("Settings", null, "Edit Your Account", "Sign out");
            switch (response)
            {
                case "Edit Your Account":
                    await _navigationControl.GoToAccountAsync();
                    break;
                case "Sign out":
                    await _navigationControl.GoToLoginAsync();
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            settingsBtn.IsSelected = false;
        }
    }
}