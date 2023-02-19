using Socially.Mobile.Logic.Models;
using System.Windows.Input;

namespace Socially.MobileApp.Components;

public partial class CommentDisplay : Grid
{

    public static readonly BindableProperty CommentProperty = BindableProperty.Create(nameof(Comment),
                                                                                     typeof(DisplayCommentModel),
                                                                                     typeof(CommentDisplay),
                                                                                     propertyChanged: CommentChanged);

    public static readonly BindableProperty AddCommandProperty = BindableProperty.Create(nameof(AddCommand),
                                                                              typeof(ICommand),
                                                                              typeof(CommentDisplay),
                                                                              propertyChanged: AddCommandPropertyChanged);

    public static readonly BindableProperty PostIdProperty = BindableProperty.Create(nameof(PostId),
                                                                                     typeof(int),
                                                                                     typeof(CommentDisplay),
                                                                                     propertyChanged: PostIdPropertyChanged);
    public int PostId
    {
        get => (int)GetValue(PostIdProperty);
        set
        {
            SetValue(PostIdProperty, value);
            ComponentModel.PostId = value;
        }
    }

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set
        {
            SetValue(AddCommandProperty, value);
            //commentEntry.Command = value;
        }
    }

    public DisplayCommentModel Comment
    {
        get => (DisplayCommentModel)GetValue(CommentProperty);
        set
        {
            SetValue(CommentProperty, value);
            ComponentModel.Comment = value;
        }
    }

    public CommentDisplay()
    {
        InitializeComponent();
    }

    private static void CommentChanged(BindableObject bindable, object oldValue, object newValue) => ((CommentDisplay)bindable).Comment = (DisplayCommentModel)newValue;

    private static void AddCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((CommentDisplay)bindable).AddCommand = (ICommand)newValue;
    
    private static void PostIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((CommentDisplay)bindable).PostId = (int)newValue;



}