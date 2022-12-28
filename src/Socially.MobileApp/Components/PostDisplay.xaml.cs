using LiteDB;
using Socially.Mobile.Logic.ComponentModels;
using Socially.Mobile.Logic.Models;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class PostDisplay : Grid
{

	public static readonly BindableProperty PostProperty = BindableProperty.Create(nameof(Post),
																				  typeof(PostDisplayModel),
																				  typeof(PostDisplay),
																				  propertyChanged: PostChanged);

    public PostDisplayModel Post
    {
        get => (PostDisplayModel)GetValue(PostProperty);
        set
        {
            SetValue(PostProperty, value);
            ComponentModel.Post = value;
        }
    }

    public PostDisplay()
	{
		InitializeComponent();
    }

	private static void PostChanged(BindableObject bindable, object oldValue, object newValue)
    {
		((PostDisplay)bindable).Post = (PostDisplayModel)newValue;
    }
}