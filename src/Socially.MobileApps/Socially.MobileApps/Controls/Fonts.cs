using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly:  ExportFont("fa-solid-900.ttf", Alias = "solid")]
//[assembly: ExportFont("fa-regular-400.ttf", Alias = "regular")]
//[assembly: ExportFont("fa-brands-400.ttf", Alias = "brands")]
namespace Socially.MobileApps.Controls
{

    public enum Icon
    {
        None,
        Link,
        Next
    }

    public static class Fonts
    {

        private readonly static Dictionary<string, Dictionary<Icon, string>> fontMappings;

        public static readonly BindableProperty IconProperty =
                                                    BindableProperty.CreateAttached(
                                                                        "Icon",
                                                                        typeof(Icon),
                                                                        typeof(Fonts),
                                                                        Icon.None,
                                                                        propertyChanged: OnIconChanged);

        static Fonts()
        {

            fontMappings = new Dictionary<string, Dictionary<Icon, string>> {

                {
                    "solid", new Dictionary<Icon, string>
                    {
                        { Icon.Link, "\uf6ff" },
                        { Icon.Next, "\uf0a9" }
                    }
                }

            };
        }

        public static Icon GetIcon(BindableObject view) => (Icon)view.GetValue(IconProperty);

        public static void SetIcon(BindableObject view, Icon value) => view.SetValue(IconProperty, value);

        static void OnIconChanged(BindableObject view, object oldValue, object newValue) => OnIconChanged(view, (Icon)newValue);

        static void OnIconChanged(BindableObject view, Icon newValue)
        {
            string text = null;
            string family = null;

            if (newValue == Icon.None) return;

            foreach (var mapping in fontMappings)
            {
                if (mapping.Value.ContainsKey(newValue))
                {
                    family = mapping.Key;
                    text = mapping.Value[newValue];
                }
            }

            if (family is null)
                throw new ArgumentException($"The icon '{newValue}' was never mapped");

            switch (view)
            {
                case Label label:
                    label.Text = text;
                    label.FontFamily = family;
                    break;
                case Button button:
                    button.Text = text;
                    button.FontFamily = family;
                    break;
                case Span span:
                    span.Text = text;
                    span.FontFamily = family;
                    break;
                case SwipeItem swipeItem:
                    swipeItem.IconImageSource = new FontImageSource
                    {
                        Color = Color.White,
                        FontFamily = family,
                        Glyph = text
                    };
                    break;
            }

        }


    }
}