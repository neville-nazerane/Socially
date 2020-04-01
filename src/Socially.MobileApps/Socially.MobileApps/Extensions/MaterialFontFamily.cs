using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace Socially.MobileApps.Extensions
{
    class MaterialFontFamily : IMarkupExtension<string>
    {
        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
                return "materialdesignicons.ttf#Material Design Icons";
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
                return "Material Design Icons";
            else throw new PlatformNotSupportedException("Material design icons was not setup for this platform");
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
