
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class ForgotPasswordModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.ForgotPasswordModel model;

        
            [ObservableProperty]
            System.String userName;

            [ObservableProperty]
            System.String token;

            [ObservableProperty]
            System.String newPassword;

            [ObservableProperty]
            System.String confirmPassword;
        


        public ForgotPasswordModel()
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

