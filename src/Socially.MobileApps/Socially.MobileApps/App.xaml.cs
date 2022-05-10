﻿using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Socially.MobileApps.Config;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.Pages;
using Socially.MobileApps.Services;
using Socially.MobileApps.Services.HttpServices;
using Socially.MobileApps.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.FluentInjector;
using Xamarin.Forms;

namespace Socially.MobileApps
{
    public partial class App : Application
    {
        public App()
        {
            ExperimentalFeatures.Enable("AppTheme_Experimental", "Shapes_Experimental");
            InitializeComponent();

            Microsoft.AppCenter.AppCenter
                            .Start(Configs.AppCenter, 
                                    typeof(Analytics), 
                                    typeof(Crashes));

            ThemeControl.Update();
            //FontRegistry.RegisterFonts(FontAwesomeSolid.Font);

            this.StartInjecting()
                .SetInitialPage<SignUpPage>()


                .SetViewModelAssembly(typeof(ViewModelBase).Assembly)

                .AddHttpClient<IApiConsumer, ApiConsumer>(c 
                                    => c.BaseAddress = new Uri(Configs.Endpoint),
                                    builder => builder.ConfigurePrimaryHttpMessageHandler<ApiHttpHandler>())

                .AddTransient<ApiHttpHandler>()
                .AddTransient<IThemeControl, ThemeControl>()

                .Build();

        }


    }
}
