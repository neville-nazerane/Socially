﻿using Socially.MobileApps.Config;
using Socially.MobileApps.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.FluentInjector;
using Xamarin.Forms;

namespace Socially.MobileApps.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

        private async void Signin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(InjectionControl.ResolvePage<SignUpFlowViewModel>());
        }

        private async void ThemeTapped(object sender, EventArgs e)
        {
            var themeControl = new ThemeControl();
            var themes = themeControl.ThemeNames.ToArray();
            var selected = await DisplayActionSheet("Pick a theme", "Go to hell", null, themes);
            if (themes.Contains(selected))
            {
                themeControl.Update(selected);
            }
        }
    }
}
