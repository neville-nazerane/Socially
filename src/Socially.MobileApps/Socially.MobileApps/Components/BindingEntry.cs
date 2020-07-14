using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class BindingEntry : Entry
    {

        public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(nameof(PropertyName), 
                                                                                              typeof(string), 
                                                                                              typeof(BindingEntry),
                                                                                              propertyChanged: Update);

        public string Source { get; set; }

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty); 
            set 
            { 
                SetValue(PropertyNameProperty, value);
                Update();
            }
        }

        void Update()
        {
            this.SetBinding(TextProperty, $"{Source}.{PropertyName}");
            Focus();
        }

        private static void Update(BindableObject bindable, object oldValue, object newValue)
        {
            ((BindingEntry)bindable).Update();
        }

    }
}
