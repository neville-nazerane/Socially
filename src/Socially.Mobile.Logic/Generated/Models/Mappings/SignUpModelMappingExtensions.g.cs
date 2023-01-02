
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class SignUpModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.SignUpModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.SignUpModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.SignUpModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.SignUpModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.SignUpModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.SignUpModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.SignUpModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.SignUpModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.SignUpModel> ToModel(this Task<Socially.Mobile.Logic.Models.SignUpModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.SignUpModel ToModel(this Socially.Mobile.Logic.Models.SignUpModel model)
            => model is null ? null : 
              new() 
              {
                  Email = model.Email,
                  UserName = model.UserName,
                  Password = model.Password,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.Models.SignUpModel ToModel(this Socially.Mobile.Logic.Models.SignUpModel model, Socially.Models.SignUpModel dest)
        {
            dest.Email = model.Email;
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.SignUpModel CloneFrom(this Socially.Mobile.Logic.Models.SignUpModel dest, Socially.Mobile.Logic.Models.SignUpModel model)
        {
            dest.Email = model.Email;
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.SignUpModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.SignUpModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.SignUpModel> ToMobileModel(this IEnumerable<Socially.Models.SignUpModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.SignUpModel>> ToMobileModel(this Task<ICollection<Socially.Models.SignUpModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.SignUpModel> ToMobileModel(this ICollection<Socially.Models.SignUpModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.SignUpModel> ToMobileModel(this Task<Socially.Models.SignUpModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.SignUpModel ToMobileModel(this Socially.Models.SignUpModel model)
            => model is null ? null : 
              new() 
              {
                  Email = model.Email,
                  UserName = model.UserName,
                  Password = model.Password,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.Mobile.Logic.Models.SignUpModel ToMobileModel(this Socially.Models.SignUpModel model, Socially.Mobile.Logic.Models.SignUpModel dest)
        {
            dest.Email = model.Email;
            dest.UserName = model.UserName;
            dest.Password = model.Password;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

    }

}

