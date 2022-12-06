
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Models.Mappings
{

    public static class SignUpModelMappingExtensions 
    {

        public static Socially.Models.SignUpModel ToModel(this Socially.MobileApp.Models.SignUpModel model)
            => model is null ? null : 
              new() 
              {
                  Email = model.Email,
                  UserName = model.UserName,
                  Password = model.Password,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.MobileApp.Models.SignUpModel ToMobileModel(this Socially.Models.SignUpModel model)
            => model is null ? null : 
              new() 
              {
                  Email = model.Email,
                  UserName = model.UserName,
                  Password = model.Password,
                  ConfirmPassword = model.ConfirmPassword
              };

    }

}

