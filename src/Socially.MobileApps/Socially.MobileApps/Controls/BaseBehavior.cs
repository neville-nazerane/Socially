using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Controls
{
	public class BehaviorBase<T> : Behavior<T> where T : BindableObject
	{
		public T View { get; private set; }

		protected override void OnAttachedTo(T bindable)
		{
			base.OnAttachedTo(bindable);
			View = bindable;

			if (bindable.BindingContext != null)
			{
				BindingContext = bindable.BindingContext;
			}

			bindable.BindingContextChanged += OnBindingContextChanged;
		}

		protected override void OnDetachingFrom(T bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.BindingContextChanged -= OnBindingContextChanged;
			View = null;
		}

		void OnBindingContextChanged(object sender, EventArgs e)
		{
			OnBindingContextChanged();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			BindingContext = View.BindingContext;
		}
	}
}
