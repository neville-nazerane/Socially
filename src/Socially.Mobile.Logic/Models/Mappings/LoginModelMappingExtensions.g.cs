
//// <GENERATED CODE> //////

namespace Socially.MobileApp.Logic.Models.Mappings
{

    public static class LoginModelMappingExtensions 
    {

        public static Socially.Models.LoginModel ToModel(this Socially.MobileApp.Logic.Models.LoginModel model,
                                                         Socially.Models.LoginModel dest)
        {
            dest.UserName = model.UserName;
            return dest;
        }


        public static Socially.Models.LoginModel ToModel(this Socially.MobileApp.Logic.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

        public static Socially.MobileApp.Logic.Models.LoginModel ToMobileModel(this Socially.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

    }

}

