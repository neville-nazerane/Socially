using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.MobileApps.Contracts
{
    public interface IThemeControl
    {
        IEnumerable<string> ThemeNames { get; }

        Task DisplayThemePickerAsync();
        //void Update();
        void Update(string newTheme);

    }
}