using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Pages;

public partial class ImagePickerPage : ContentPage
{
	public ImagePickerPage()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        ServicesUtil.Get<INavigationControl>().ImagePopupResponse.TrySetResult(null);
        return base.OnBackButtonPressed();
    }

}