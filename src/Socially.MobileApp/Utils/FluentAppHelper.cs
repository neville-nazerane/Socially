using Android.Content.Res;
using Google.Android.Material.DatePicker;
using Kotlin.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Utils
{
    public static class FluentAppHelper
    {

        public static XamlAppBuilder WithApp(this MauiAppBuilder builder) => new(builder);



        public class XamlAppBuilder
        {
            private readonly MauiAppBuilder _builder;
            private readonly List<string> _resources;
            private Page _mainPage;

            public XamlAppBuilder(MauiAppBuilder builder)
            {
                _builder = builder;
                _builder.Services.AddSingleton(this);
                _resources = new();
            }

            public XamlAppBuilder AddResource(string resource)
            {
                _resources.Add(resource);
                return this;
            }

            public XamlAppBuilder SetMainPage(Page page)
            {
                _mainPage = page;
                return this;
            }

            public MauiAppBuilder UseMaui()
            {
                if (_mainPage is null) throw new Exception("No Main was set");
                return _builder.UseMauiApp<App>();
            }


            private class App : Application
            {
                public App()
                {
                    var builder = CurrentApp.GetService<XamlAppBuilder>();
                    var resourceDictionary = new ResourceDictionary();

                    foreach (var resourcepath in builder._resources)
                    {
                        var converter = new ResourceDictionary.RDSourceTypeConverter();
                        try
                        {
                            var c = converter.ConvertFromString(resourcepath);
                        }
                        catch (Exception ex)
                        {
                            // thrown not implimented exception
                        }
                        resourceDictionary.MergedDictionaries.Add(new()
                        {
                            Source = new Uri(resourcepath, UriKind.Relative)
                        });
                    }

                    Resources.Add(resourceDictionary);

                    MainPage = builder._mainPage;
                }


                static IServiceProvider CurrentApp =>
#if ANDROID
                        MauiApplication.Current.Services;
#elif IOS || MACCATALYST
                        MauiUIApplicationDelegate.Current.Services;
#else
                        null;
#endif
            }

        }

    }
}
