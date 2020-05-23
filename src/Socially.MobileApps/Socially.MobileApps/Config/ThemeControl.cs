using Socially.MobileApps.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Socially.MobileApps.Config
{
    class ThemeControl : IThemeControl
    {

        private const string key = "selectedTheme";

        private static readonly ICollection<IContainer> themes;

        static ThemeControl()
        {
            themes = new List<IContainer> {
                new Container<PendantTheme>(),
                new Container<MirrorTheme>(),
                new Container<TearsTheme>(),
                new Container<WindyTheme>(),
            };
        }

        public IEnumerable<string> ThemeNames => themes.Select(t => t.Name);

        public void Update(string newTheme)
        {
            Preferences.Set(key, newTheme);
            Update();
        }

        public void Update()
        {
            string selection = Preferences.Get(key, null);
            if (selection is null || !themes.Any(t => t.Name == selection))
            {
                if (themes.Any())
                {
                    selection = themes.First().Name;
                    Preferences.Set(key, selection);
                }
            }

            var resources = Application.Current.Resources.MergedDictionaries;
            resources.Clear();
            resources.Add(themes.Single(t => t.Name == selection).Build());
        }

        class Container<TResource> : IContainer
            where TResource : ResourceDictionary, new()
        {

            public string Name => typeof(TResource).Name.Replace("Theme", string.Empty);

            public ResourceDictionary Build() => new TResource();

        }

        interface IContainer
        {
            string Name { get; }

            ResourceDictionary Build();

        }

    }
}
