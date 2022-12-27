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
                homeBtn.IsSelected = true;
                break;
            case "//MainPage/profile/posts":
                profileBtn.IsSelected = true;
                break;
        }

        var parentLayout = (Layout)Parent;

        base.OnSizeAllocated(parentLayout.Width, 120);

        AbsoluteLayout.SetLayoutBounds(this, new()
        {
            X = 0,
            Y = parentLayout.Height - 60,
            Height = 120,
            Width = parentLayout.Width
        });

        IsVisible = true;
    }

    private async void Home_Tapped(object sender, TappedEventArgs e)
    {
        await _navigationControl.GoToHomeAsync();
    }

    private async void Profile_Tapped(object sender, TappedEventArgs e)
    {
        await _navigationControl.GoToProfilePostsAsync();
    }
}