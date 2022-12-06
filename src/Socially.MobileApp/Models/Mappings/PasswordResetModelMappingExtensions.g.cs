
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Models.Mappings
{

    public static class PasswordResetModelMappingExtensions 
    {

        public static Socially.Models.PasswordResetModel ToModel(this Socially.MobileApp.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

        public static Socially.MobileApp.Models.PasswordResetModel ToMobileModel(this Socially.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

    }

}

