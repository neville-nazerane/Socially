
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class DisplayCommentModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.DisplayCommentModel model;

        
            [ObservableProperty]
            System.Int32 id;

            [ObservableProperty]
            System.Int32 creatorId;

            [ObservableProperty]
            System.String text;

            [ObservableProperty]
            System.Collections.Generic.ICollection<Socially.Mobile.Logic.Models.DisplayCommentModel> comments;

            [ObservableProperty]
            System.Int32 likeCount;

            [ObservableProperty]
            System.DateTime createdOn;

            [ObservableProperty]
            System.Boolean isLikedByCurrentUser;
        


        public DisplayCommentModel()
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

