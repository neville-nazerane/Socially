
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class SignUpModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.SignUpModel model;

        
            [ObservableProperty]
            System.String email;

            [ObservableProperty]
            System.String userName;

            [ObservableProperty]
            System.String password;

            [ObservableProperty]
            System.String confirmPassword;
        


        public SignUpModel()
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

