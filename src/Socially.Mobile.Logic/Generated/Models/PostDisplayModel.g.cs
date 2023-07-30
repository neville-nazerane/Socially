
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class PostDisplayModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.PostDisplayModel model;

        
            [ObservableProperty]
            System.Int32 id;

            [ObservableProperty]
            System.String text;

            [ObservableProperty]
            System.Int32 creatorId;

            [ObservableProperty]
            System.Nullable<System.DateTime> createdOn;

            [ObservableProperty]
            System.Collections.Generic.ICollection<Socially.Mobile.Logic.Models.DisplayCommentModel> comments;

            [ObservableProperty]
            System.Int32 likeCount;

            [ObservableProperty]
            System.Boolean isLikedByCurrentUser;
        


        public PostDisplayModel()
        {
            model = new();
        }

        public bool Validate(ICollection<ValidationResult> errors)
        {
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors, true);
        }

    }

}

