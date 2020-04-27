using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class ActivityIndicatorForCommand : ActivityIndicator
    {
        public static readonly BindableProperty CommandToTrackProperty = BindableProperty.Create(nameof(CommandToTrack), typeof(Command), typeof(ActivityIndicatorForCommand));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ActivityIndicatorForCommand));


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
                CommandToTrack.CanExecuteChanged -= CommandToTrack_CanExecuteChanged;
        }

        private void CommandToTrack_CanExecuteChanged(object sender, EventArgs e)
        {
            if (CommandToTrack is null) return;

            UpdateBasedOnCanExecute();
            // check for generic type
        }

        private void UpdateBasedOnCanExecute()
        {
            bool canExecute = CommandToTrack.CanExecute(CommandParameter);

            IsVisible = canExecute;
            IsEnabled = canExecute;
            IsRunning = canExecute;
        }

    }
}
