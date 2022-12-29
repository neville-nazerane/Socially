using Socially.Mobile.Logic.Models;

namespace Socially.MobileApp.Components;

public partial class CommentDisplay : Grid
{

    public static readonly BindableProperty CommentProperty = BindableProperty.Create(nameof(Comment),
                                                                                     typeof(DisplayCommentModel),
                                                                                     typeof(CommentDisplay),
                                                                                     propertyChanged: CommentChanged);


    public DisplayCommentModel Comment
    {
        get => (DisplayCommentModel)GetValue(CommentProperty);
        set => SetValue(CommentProperty, value);
    }

    public CommentDisplay()
    {
        InitializeComponent();
    }

    private static void CommentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CommentDisplay)bindable).Comment = (DisplayCommentModel)newValue;
    }

}