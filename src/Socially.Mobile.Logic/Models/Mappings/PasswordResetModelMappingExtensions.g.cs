
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Logic.Models.Mappings
{

    public static class PasswordResetModelMappingExtensions 
    {

        public static Socially.Models.PasswordResetModel ToModel(this Socially.MobileApp.Logic.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

        public static Socially.Models.PasswordResetModel ToModel(this Socially.MobileApp.Logic.Models.PasswordResetModel model, Socially.Models.PasswordResetModel dest)
        {
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            dest.CurrentPassword = model.CurrentPassword;
            return dest;
        }

        public static Socially.MobileApp.Logic.Models.PasswordResetModel ToMobileModel(this Socially.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

        public static Socially.MobileApp.Logic.Models.PasswordResetModel ToMobileModel(this Socially.Models.PasswordResetModel model, Socially.MobileApp.Logic.Models.PasswordResetModel dest)
        {
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            dest.CurrentPassword = model.CurrentPassword;
            return dest;
        }

    }

}

