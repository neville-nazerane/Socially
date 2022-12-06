
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Models.Mappings
{

    public static class LoginModelMappingExtensions 
    {

        public static Socially.Models.LoginModel ToModel(this Socially.MobileApp.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

        public static Socially.MobileApp.Models.LoginModel ToMobileModel(this Socially.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

    }

}

