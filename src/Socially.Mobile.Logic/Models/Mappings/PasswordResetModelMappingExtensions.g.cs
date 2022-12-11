
//// <GENERATED CODE> //////
namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class PasswordResetModelMappingExtensions 
    {

        public static Socially.Models.PasswordResetModel ToModel(this Socially.Mobile.Logic.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

        public static Socially.Models.PasswordResetModel ToModel(this Socially.Mobile.Logic.Models.PasswordResetModel model, Socially.Models.PasswordResetModel dest)
        {
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            dest.CurrentPassword = model.CurrentPassword;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.PasswordResetModel ToMobileModel(this Socially.Models.PasswordResetModel model)
            => model is null ? null : 
              new() 
              {
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword,
                  CurrentPassword = model.CurrentPassword
              };

        public static Socially.Mobile.Logic.Models.PasswordResetModel ToMobileModel(this Socially.Models.PasswordResetModel model, Socially.Mobile.Logic.Models.PasswordResetModel dest)
        {
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            dest.CurrentPassword = model.CurrentPassword;
            return dest;
        }

    }

}

