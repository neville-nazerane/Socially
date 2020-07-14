using Socially.MobileApps.Contracts;
using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.ViewModels
{
    public class ViewModelBase : BindableBase
    {

        public ICommand ThemeCommand => new Command(async () => await ThemeControl.DisplayThemePickerAsync());

        protected IThemeControl ThemeControl { get; }

        public ViewModelBase(IThemeControl themeControl)
        {
            ThemeControl = themeControl;
        }

    }
}
