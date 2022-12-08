
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.MobileApp.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.MobileApp.Logic.Models
{
    
    public partial class PostDisplayModel : ObservableObject
    {

        private readonly ValidationContext validationContext;
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
            System.Collections.Generic.ICollection<Socially.Models.DisplayCommentModel> comments;

            [ObservableProperty]
            System.Int32 likeCount;
        


        public PostDisplayModel()
        {
            model = new();
            validationContext = new ValidationContext(model);
        }

        public bool Validate(ICollection<ValidationResult> errors)
        {
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors);
        }

    }

}

