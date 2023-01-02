using Android.Service.QuickSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Utils;

public static class ServicesUtil
{

    public static TService Get<TService>() => CurrentServices.GetService<TService>();

    static IServiceProvider CurrentServices =>
#if ANDROID
    MauiApplication.Current.Services;
#elif IOS || MACCATALYST
                    MauiUIApplicationDelegate.Current.Services;
#else
                    null;
#endif
}
