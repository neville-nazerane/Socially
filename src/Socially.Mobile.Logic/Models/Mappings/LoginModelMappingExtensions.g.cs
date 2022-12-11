
//// <GENERATED CODE> //////
namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class LoginModelMappingExtensions 
    {

        public static Socially.Models.LoginModel ToModel(this Socially.Mobile.Logic.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

        public static Socially.Models.LoginModel ToModel(this Socially.Mobile.Logic.Models.LoginModel model, Socially.Models.LoginModel dest)
        {
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.Source = model.Source;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.LoginModel ToMobileModel(this Socially.Models.LoginModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Password = model.Password,
                  Source = model.Source
              };

        public static Socially.Mobile.Logic.Models.LoginModel ToMobileModel(this Socially.Models.LoginModel model, Socially.Mobile.Logic.Models.LoginModel dest)
        {
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.Source = model.Source;
            return dest;
        }

    }

}

