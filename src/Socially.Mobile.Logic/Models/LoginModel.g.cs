
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class LoginModel : ObservableObject, IValidatable
    {

        private readonly ValidationContext validationContext;
        private readonly Socially.Models.LoginModel model;

        
            [ObservableProperty]
            System.String userName;

            [ObservableProperty]
            System.String password;

            [ObservableProperty]
            System.String source;
        


        public LoginModel()
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

