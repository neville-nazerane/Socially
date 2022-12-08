
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.MobileApp.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.MobileApp.Logic.Models
{
    
    public partial class SignUpModel : ObservableValidator
    {

        private readonly ValidationContext validationContext;
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
            validationContext = new ValidationContext(model);
        }

        public bool Validate(ICollection<ValidationResult> errors)
        {
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors);
        }

    }

}

