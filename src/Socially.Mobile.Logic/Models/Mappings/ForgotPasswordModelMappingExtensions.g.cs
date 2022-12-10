
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Logic.Models.Mappings
{

    public static class ForgotPasswordModelMappingExtensions 
    {

        public static Socially.Models.ForgotPasswordModel ToModel(this Socially.MobileApp.Logic.Models.ForgotPasswordModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Token = model.Token,
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.Models.ForgotPasswordModel ToModel(this Socially.MobileApp.Logic.Models.ForgotPasswordModel model, Socially.Models.ForgotPasswordModel dest)
        {
            dest.UserName = model.UserName;
            dest.Token = model.Token;
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

        public static Socially.MobileApp.Logic.Models.ForgotPasswordModel ToMobileModel(this Socially.Models.ForgotPasswordModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Token = model.Token,
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.MobileApp.Logic.Models.ForgotPasswordModel ToMobileModel(this Socially.Models.ForgotPasswordModel model, Socially.MobileApp.Logic.Models.ForgotPasswordModel dest)
        {
            dest.UserName = model.UserName;
            dest.Token = model.Token;
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

    }

}

