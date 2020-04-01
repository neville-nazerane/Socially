using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Socially.MobileApps.Extensions
{

    [ContentProperty(nameof(Source))]
    class ImageExtension : IMarkupExtension<ImageSource>
    {

        public string Source { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
            => ImageSource.FromResource($"Socially.MobileApps.Images.{Source}", typeof(ImageExtension).GetTypeInfo().Assembly);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
