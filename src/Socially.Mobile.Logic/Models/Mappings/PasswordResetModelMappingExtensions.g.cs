
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class PasswordResetModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.PasswordResetModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.PasswordResetModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.PasswordResetModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.PasswordResetModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.PasswordResetModel> ToModel(this Task<Socially.Mobile.Logic.Models.PasswordResetModel> modelTask)
            => (await modelTask).ToModel();

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

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.PasswordResetModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.PasswordResetModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.PasswordResetModel> ToMobileModel(this IEnumerable<Socially.Models.PasswordResetModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.PasswordResetModel> ToMobileModel(this Task<Socially.Models.PasswordResetModel> modelTask)
            => (await modelTask).ToMobileModel();

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

