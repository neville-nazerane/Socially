
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Logic.Models.Mappings
{

    public static class SignUpModelMappingExtensions 
    {

        public static Socially.Models.SignUpModel ToModel(this Socially.MobileApp.Logic.Models.SignUpModel model)
            => model is null ? null : 
              new() 
              {
                  Email = model.Email,
                  UserName = model.UserName,
                  Password = model.Password,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.MobileApp.Logic.Models.SignUpModel ToMobileModel(this Socially.Models.SignUpModel model)
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

