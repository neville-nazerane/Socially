
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Mobile.Logic.Models.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    
    public partial class UserSummaryModel : ObservableObject, IValidatable
    {

        private readonly Socially.Models.UserSummaryModel model;

        
            [ObservableProperty]
            System.Int32 id;

            [ObservableProperty]
            System.String firstName;

            [ObservableProperty]
            System.String lastName;

            [ObservableProperty]
            System.String profilePicUrl;
        


        public UserSummaryModel()
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

