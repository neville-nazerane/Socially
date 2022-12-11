
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.MobileApp.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.MobileApp.Logic.Models
{
    
    public partial class ProfileUpdateModel : ObservableObject
    {

        private readonly ValidationContext validationContext;
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
            validationContext = new ValidationContext(model);
        }

        public bool Validate(ICollection<ValidationResult> errors)
        {
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors);
        }

    }

}

