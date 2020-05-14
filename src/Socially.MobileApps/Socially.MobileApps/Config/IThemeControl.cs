using System.Collections.Generic;

namespace Socially.MobileApps.Config
{
    public interface IThemeControl
    {
        IEnumerable<string> ThemeNames { get; }

        void Update();
        void Update(string newTheme);
    }
}