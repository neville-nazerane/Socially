using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class TabMenu : AbsoluteLayout
{
    private readonly INavigationControl _navigationControl;

    public TabMenu()
    {
        InitializeComponent();
        _navigationControl = ServicesUtil.Get<INavigationControl>();
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
        }

    }

    void SetAsSelected(TabButton btn)
    {
        btn.IsSelected = true;

        //var bounds = AbsoluteLayout.GetLayoutBounds(btn);
        //bounds.Y = 20;
        //AbsoluteLayout.SetLayoutBounds(btn, bounds);

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
        await _navigationControl.GoToLoginAsync();
    }
}