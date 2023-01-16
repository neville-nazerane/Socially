
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class AddPostModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.AddPostModel model;

        
            [ObservableProperty]
            System.String text;
        


        public AddPostModel()
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

