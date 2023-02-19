
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class AddCommentModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.AddCommentModel model;

        
            [ObservableProperty]
            System.String text;

            [ObservableProperty]
            System.Int32 postId;

            [ObservableProperty]
            System.Nullable<System.Int32> parentCommentId;
        


        public AddCommentModel()
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

