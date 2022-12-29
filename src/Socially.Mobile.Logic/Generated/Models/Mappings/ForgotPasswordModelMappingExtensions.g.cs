
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class ForgotPasswordModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.ForgotPasswordModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.ForgotPasswordModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.ForgotPasswordModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.ForgotPasswordModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.ForgotPasswordModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.ForgotPasswordModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.ForgotPasswordModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.ForgotPasswordModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.ForgotPasswordModel> ToModel(this Task<Socially.Mobile.Logic.Models.ForgotPasswordModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.ForgotPasswordModel ToModel(this Socially.Mobile.Logic.Models.ForgotPasswordModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Token = model.Token,
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.Models.ForgotPasswordModel ToModel(this Socially.Mobile.Logic.Models.ForgotPasswordModel model, Socially.Models.ForgotPasswordModel dest)
        {
            dest.UserName = model.UserName;
            dest.Token = model.Token;
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.ForgotPasswordModel Clone(this Socially.Mobile.Logic.Models.ForgotPasswordModel model, Socially.Mobile.Logic.Models.ForgotPasswordModel dest)
        {
            dest.UserName = model.UserName;
            dest.Token = model.Token;
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.ForgotPasswordModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.ForgotPasswordModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.ForgotPasswordModel> ToMobileModel(this IEnumerable<Socially.Models.ForgotPasswordModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.ForgotPasswordModel>> ToMobileModel(this Task<ICollection<Socially.Models.ForgotPasswordModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.ForgotPasswordModel> ToMobileModel(this ICollection<Socially.Models.ForgotPasswordModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.ForgotPasswordModel> ToMobileModel(this Task<Socially.Models.ForgotPasswordModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.ForgotPasswordModel ToMobileModel(this Socially.Models.ForgotPasswordModel model)
            => model is null ? null : 
              new() 
              {
                  UserName = model.UserName,
                  Token = model.Token,
                  NewPassword = model.NewPassword,
                  ConfirmPassword = model.ConfirmPassword
              };

        public static Socially.Mobile.Logic.Models.ForgotPasswordModel ToMobileModel(this Socially.Models.ForgotPasswordModel model, Socially.Mobile.Logic.Models.ForgotPasswordModel dest)
        {
            dest.UserName = model.UserName;
            dest.Token = model.Token;
            dest.NewPassword = model.NewPassword;
            dest.ConfirmPassword = model.ConfirmPassword;
            return dest;
        }

    }

}

