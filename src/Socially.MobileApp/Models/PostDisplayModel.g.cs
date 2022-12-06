
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;

namespace Socially.MobileApp.Models
{
    
    public partial class PostDisplayModel : ObservableObject
    {
        
            [ObservableProperty]
            System.Int32 id;

            [ObservableProperty]
            System.String text;

            [ObservableProperty]
            System.Int32 creatorId;

            [ObservableProperty]
            System.Nullable<System.DateTime> createdOn;

            [ObservableProperty]
            System.Collections.Generic.ICollection<Socially.Models.DisplayCommentModel> comments;

            [ObservableProperty]
            System.Int32 likeCount;
            
    }

}

