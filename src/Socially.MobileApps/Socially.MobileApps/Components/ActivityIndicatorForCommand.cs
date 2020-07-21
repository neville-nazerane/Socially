using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class ActivityIndicatorForCommand : ActivityIndicator
    {
        public static readonly BindableProperty CommandToTrackProperty = BindableProperty.Create(nameof(CommandToTrack),
                                                                                                 typeof(ICommand),
                                                                                                 typeof(ActivityIndicatorForCommand),
                                                                                                 propertyChanged: CommandToTrackChanged);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
                                                                                                   typeof(object),
                                                                                                   typeof(ActivityIndicatorForCommand));


        public ICommand CommandToTrack { get => (ICommand) GetValue(CommandToTrackProperty); set => SetCommand(value); }

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

        private void SetCommand(ICommand command)
        {
            if (CommandToTrack != null)
                CommandToTrack.CanExecuteChanged -= CommandToTrack_CanExecuteChanged;

            SetValue(CommandToTrackProperty, command);
            if (Parent != null)
                CommandToTrack.CanExecuteChanged += CommandToTrack_CanExecuteChanged;
        }

        private void CommandToTrack_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            bool needsToRun = !CommandToTrack.CanExecute(CommandParameter);

            //IsVisible = needsToRun;
            IsEnabled = needsToRun;
            IsRunning = needsToRun;
        }

        private static void CommandToTrackChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var comp = (ActivityIndicatorForCommand)bindable;
            comp.SetCommand((ICommand)newValue);
        }

    }
}
