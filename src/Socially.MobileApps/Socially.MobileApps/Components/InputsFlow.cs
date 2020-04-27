using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class InputsFlow : CarouselView
    {

        protected override void OnParentSet()
        {
            if (Parent is null)
                PropertyChanged -= Batman;
            else
                PropertyChanged += Batman;

            base.OnParentSet();
        }

        private void Batman(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemsSource) && ItemsSource != null && Parent != null)
            {

            }
        }
    }
}
