using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.Controls
{
    public class CommandToVisibleBehavior : BehaviorBase<View>
    {

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), 
                                                                                          typeof(ICommand), 
                                                                                          typeof(CommandToVisibleBehavior), 
                                                                                          propertyChanged: CommandChanged);


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
                                                                                        typeof(object),
                                                                                        typeof(CommandToVisibleBehavior));


        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private void SetCommand(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
                oldCommand.CanExecuteChanged -= Command_CanExecuteChanged;

            if (newCommand != null)
                newCommand.CanExecuteChanged += Command_CanExecuteChanged;
            UpdateShown();
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateShown();
        }

        private void UpdateShown()
        {

            if (View != null)
            {
                bool canExecute = Command.CanExecute(CommandParameter);
                bool isShown = !canExecute;

                View.IsVisible = isShown;
                if (View is ActivityIndicator indicator)
                {
                    indicator.IsEnabled = isShown;
                    indicator.IsRunning = isShown;
                }
            }



        }

        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CommandToVisibleBehavior)bindable).SetCommand((ICommand)oldValue, (ICommand)newValue);
        }

    }
}
