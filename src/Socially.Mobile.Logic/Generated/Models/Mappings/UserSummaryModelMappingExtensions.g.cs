
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class UserSummaryModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.UserSummaryModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.UserSummaryModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.UserSummaryModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.UserSummaryModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.UserSummaryModel> ToModel(this Task<Socially.Mobile.Logic.Models.UserSummaryModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.UserSummaryModel ToModel(this Socially.Mobile.Logic.Models.UserSummaryModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  FirstName = model.FirstName,
                  LastName = model.LastName,
                  ProfilePicUrl = model.ProfilePicUrl
              };

        public static Socially.Models.UserSummaryModel ToModel(this Socially.Mobile.Logic.Models.UserSummaryModel model, Socially.Models.UserSummaryModel dest)
        {
            dest.Id = model.Id;
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.ProfilePicUrl = model.ProfilePicUrl;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.UserSummaryModel Clone(this Socially.Mobile.Logic.Models.UserSummaryModel model, Socially.Mobile.Logic.Models.UserSummaryModel dest)
        {
            dest.Id = model.Id;
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.ProfilePicUrl = model.ProfilePicUrl;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.UserSummaryModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.UserSummaryModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.UserSummaryModel> ToMobileModel(this IEnumerable<Socially.Models.UserSummaryModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.UserSummaryModel> ToMobileModel(this Task<Socially.Models.UserSummaryModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.UserSummaryModel ToMobileModel(this Socially.Models.UserSummaryModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  FirstName = model.FirstName,
                  LastName = model.LastName,
                  ProfilePicUrl = model.ProfilePicUrl
              };

        public static Socially.Mobile.Logic.Models.UserSummaryModel ToMobileModel(this Socially.Models.UserSummaryModel model, Socially.Mobile.Logic.Models.UserSummaryModel dest)
        {
            dest.Id = model.Id;
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.ProfilePicUrl = model.ProfilePicUrl;
            return dest;
        }

    }

}

