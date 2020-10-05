using Socially.MobileApps.Contracts;
using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        protected Command BuildCommand(Func<Task> asyncAction)
        {
            bool canExecute = true;
            Command cmd = null;
            async Task execute()
            {
                canExecute = false;
                cmd.ChangeCanExecute();
                await asyncAction();
                canExecute = true;
                cmd.ChangeCanExecute();
            };
            cmd = new Command(async () => await execute(), () => canExecute);
            return cmd;
        }

        protected Command BuildCommand(Action action)
        {
            bool canExecute = true;
            Command cmd = null;
            void execute()
            {
                canExecute = false;
                cmd.ChangeCanExecute();
                action();
                canExecute = true;
                cmd.ChangeCanExecute();
            };
            cmd = new Command(execute, () => canExecute);
            return cmd;
        }

        protected Command<T> BuildCommand<T>(Func<T, Task> asyncAction)
        {
            bool canExecute = true;
            Command<T> cmd = null;
            async Task execute(T param)
            {
                canExecute = false;
                cmd.ChangeCanExecute();
                await asyncAction(param);
                canExecute = true;
                cmd.ChangeCanExecute();
            };
            cmd = new Command<T>(async param => await execute(param), param => canExecute);
            return cmd;
        }

        protected Command<T> BuildCommand<T>(Action<T> action)
        {
            bool canExecute = true;
            Command<T> cmd = null;
            void execute(T param)
            {
                canExecute = false;
                cmd.ChangeCanExecute();
                action(param);
                canExecute = true;
                cmd.ChangeCanExecute();
            };
            cmd = new Command<T>(execute, param => canExecute);
            return cmd;
        }


    }
}
