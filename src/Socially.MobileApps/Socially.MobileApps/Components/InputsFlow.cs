using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Components
{
    public class InputsFlow : CarouselView
    {

        private INotifyCollectionChanged _currentItemSource;

        protected override void OnParentSet()
        {
            if (Parent is null)
                PropertyChanged -= PropertyChnaged;
            else
            {
                PropertyChanged += PropertyChnaged;
            }

            base.OnParentSet();
        }

        private void PropertyChnaged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemsSource) && ItemsSource != null && Parent != null)
            {
                if (ItemsSource != _currentItemSource)
                {
                    if (_currentItemSource != null)
                        _currentItemSource.CollectionChanged -= CurrentItemSource_CollectionChanged;

                    _currentItemSource = ItemsSource as INotifyCollectionChanged;

                    if (_currentItemSource != null)
                        _currentItemSource.CollectionChanged += CurrentItemSource_CollectionChanged;
                }
            }
        }

        private void CurrentItemSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                ScrollTo(e.NewStartingIndex, position: ScrollToPosition.Start);
        }
    }
}
