using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.Controls
{
    public class CommandToVisibleBehavior : Behavior<View>
    {

        private View _view;


        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), 
                                                                                          typeof(ICommand), 
                                                                                          typeof(CommandToVisibleBehavior), 
                                                                                          null,
                                                                                          propertyChanged: delegate
                                                                                          {

                                                                                          });



        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
                                                                                        typeof(object),
                                                                                        typeof(CommandToVisibleBehavior));



        public static readonly BindableProperty OnlyStringProperty = BindableProperty.CreateAttached(nameof(OnlyString),
                                                                                        typeof(string),
                                                                                        typeof(CommandToVisibleBehavior),
                                                                                        null,
                                                                                        propertyChanged: (a, b, c) =>
                                                                                        {

                                                                                        });

        public string OnlyString
        {
            get => (string)GetValue(OnlyStringProperty);
            set => SetValue(OnlyStringProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty); 
            set => SetCommand(value); 
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private void SetCommand(ICommand command)
        {
            if (Command != null)
                Command.CanExecuteChanged -= Command_CanExecuteChanged;
            SetValue(CommandProperty, command);
            if (Command != null)
                Command.CanExecuteChanged += Command_CanExecuteChanged;
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            _view = bindable;
            BindingContext = _view.BindingContext;
            var cmd = Command;
            //UpdateShown(false);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            if (Command != null)
                Command.CanExecuteChanged -= Command_CanExecuteChanged;
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (_view != null)
            {
                bool canExecute = Command.CanExecute(CommandParameter); 
                UpdateShown(!canExecute);
            }
        }

        private void UpdateShown(bool isShown)
        {
            _view.IsVisible = isShown;
            if (_view is ActivityIndicator indicator)
            {
                indicator.IsEnabled = isShown;
                indicator.IsRunning = isShown;
            }
        }

        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CommandToVisibleBehavior)bindable).Command = (ICommand)newValue;
        }

    }
}
