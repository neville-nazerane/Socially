
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class ProfileUpdateModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.ProfileUpdateModel model;

        
            [ObservableProperty]
            System.String firstName;

            [ObservableProperty]
            System.String lastName;

            [ObservableProperty]
            System.Nullable<System.DateTime> dateOfBirth;

            [ObservableProperty]
            System.String profilePictureFileName;
        


        public ProfileUpdateModel()
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

