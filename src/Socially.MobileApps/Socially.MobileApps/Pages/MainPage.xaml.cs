using Socially.MobileApps.Config;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.FluentInjector;
using Xamarin.FluentInjector.Utilities;
using Xamarin.Forms;

namespace Socially.MobileApps.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IThemeControl _themeControl;
        private readonly IPageControl _pageControl;
        private readonly IInjectionControl _injectionControl;

        public MainPage(IThemeControl themeControl, IPageControl pageControl, IInjectionControl injectionControl)
        {
            InitializeComponent();
            _themeControl = themeControl;
            _pageControl = pageControl;
            _injectionControl = injectionControl;
        }

        private async void Signin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(_injectionControl.ResolvePage<SignUpFlowViewModel>());
        }

        private async void ThemeTapped(object sender, EventArgs e)
        {
            await _themeControl.DisplayThemePickerAsync();
            //var themeControl = new ThemeControl();
            //var themes = themeControl.ThemeNames.ToArray(); 
            //var selected = await DisplayActionSheet("Pick a theme", "Go to hell", null, themes);
            //if (themes.Contains(selected))
            //{
            //    themeControl.Update(selected);
            //}
        }
    }
}
