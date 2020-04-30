using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class ActivityIndicatorForCommand : ActivityIndicator
    {
        public static readonly BindableProperty CommandToTrackProperty = BindableProperty.Create(nameof(CommandToTrack),
                                                                                                 typeof(Command),
                                                                                                 typeof(ActivityIndicatorForCommand),
                                                                                                 propertyChanged: CommandToTrackChanged);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
                                                                                                   typeof(object),
                                                                                                   typeof(ActivityIndicatorForCommand),
                                                                                                   propertyChanged: CommandParameterChanged);


        public Command CommandToTrack { get => (Command) GetValue(CommandToTrackProperty); set => SetCommand(value); }

        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent is null)
            {
                if (CommandToTrack != null)
                    CommandToTrack.CanExecuteChanged -= CommandToTrack_CanExecuteChanged;
            }
            else
            {
                if (CommandToTrack != null)
                    CommandToTrack.CanExecuteChanged += CommandToTrack_CanExecuteChanged;
            }
        }

        private void SetCommand(Command command)
        {
            if (CommandToTrack != null)
                CommandToTrack.CanExecuteChanged -= CommandToTrack_CanExecuteChanged;

            SetValue(CommandToTrackProperty, command);
            if (Parent != null)
                CommandToTrack.CanExecuteChanged += CommandToTrack_CanExecuteChanged;
        }

        private void CommandToTrack_CanExecuteChanged(object sender, EventArgs e)
        {
            if (CommandToTrack is null) return;

            UpdateBasedOnCanExecute();
            // check for generic type
        }

        private void UpdateBasedOnCanExecute()
        {
            bool needsToRun = !CommandToTrack.CanExecute(CommandParameter);

            IsVisible = needsToRun;
            IsEnabled = needsToRun;
            IsRunning = needsToRun;
        }

        private static void CommandToTrackChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var comp = (ActivityIndicatorForCommand)bindable;
            comp.CommandToTrack = (Command)newValue;
        }

        private static void CommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var comp = (ActivityIndicatorForCommand)bindable;
            comp.CommandParameter = newValue;
        }
    }
}
