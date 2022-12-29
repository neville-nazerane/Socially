
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class LoginModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.LoginModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.LoginModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.LoginModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.LoginModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.LoginModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.LoginModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.LoginModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.LoginModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.LoginModel> ToModel(this Task<Socially.Mobile.Logic.Models.LoginModel> modelTask)
            => (await modelTask).ToModel();

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

        public static Socially.Mobile.Logic.Models.LoginModel Clone(this Socially.Mobile.Logic.Models.LoginModel model, Socially.Mobile.Logic.Models.LoginModel dest)
        {
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.Source = model.Source;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.LoginModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.LoginModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.LoginModel> ToMobileModel(this IEnumerable<Socially.Models.LoginModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.LoginModel>> ToMobileModel(this Task<ICollection<Socially.Models.LoginModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.LoginModel> ToMobileModel(this ICollection<Socially.Models.LoginModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.LoginModel> ToMobileModel(this Task<Socially.Models.LoginModel> modelTask)
            => (await modelTask).ToMobileModel();

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

