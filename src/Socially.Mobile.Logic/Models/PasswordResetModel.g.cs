
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class PasswordResetModel : ObservableObject, IValidatable
    {

        private readonly ValidationContext validationContext;
        private readonly Socially.Models.PasswordResetModel model;

        
            [ObservableProperty]
            System.String newPassword;

            [ObservableProperty]
            System.String confirmPassword;

            [ObservableProperty]
            System.String currentPassword;
        


        public PasswordResetModel()
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

