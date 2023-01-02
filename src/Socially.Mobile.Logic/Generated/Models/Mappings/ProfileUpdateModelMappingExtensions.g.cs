
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class ProfileUpdateModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.ProfileUpdateModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.ProfileUpdateModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.ProfileUpdateModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.ProfileUpdateModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.ProfileUpdateModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.ProfileUpdateModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.ProfileUpdateModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.ProfileUpdateModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.ProfileUpdateModel> ToModel(this Task<Socially.Mobile.Logic.Models.ProfileUpdateModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.ProfileUpdateModel ToModel(this Socially.Mobile.Logic.Models.ProfileUpdateModel model)
            => model is null ? null : 
              new() 
              {
                  FirstName = model.FirstName,
                  LastName = model.LastName,
                  DateOfBirth = model.DateOfBirth,
                  ProfilePictureFileName = model.ProfilePictureFileName
              };

        public static Socially.Models.ProfileUpdateModel ToModel(this Socially.Mobile.Logic.Models.ProfileUpdateModel model, Socially.Models.ProfileUpdateModel dest)
        {
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.DateOfBirth = model.DateOfBirth;
            dest.ProfilePictureFileName = model.ProfilePictureFileName;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.ProfileUpdateModel CloneFrom(this Socially.Mobile.Logic.Models.ProfileUpdateModel dest, Socially.Mobile.Logic.Models.ProfileUpdateModel model)
        {
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.DateOfBirth = model.DateOfBirth;
            dest.ProfilePictureFileName = model.ProfilePictureFileName;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.ProfileUpdateModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.ProfileUpdateModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.ProfileUpdateModel> ToMobileModel(this IEnumerable<Socially.Models.ProfileUpdateModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.ProfileUpdateModel>> ToMobileModel(this Task<ICollection<Socially.Models.ProfileUpdateModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.ProfileUpdateModel> ToMobileModel(this ICollection<Socially.Models.ProfileUpdateModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.ProfileUpdateModel> ToMobileModel(this Task<Socially.Models.ProfileUpdateModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.ProfileUpdateModel ToMobileModel(this Socially.Models.ProfileUpdateModel model)
            => model is null ? null : 
              new() 
              {
                  FirstName = model.FirstName,
                  LastName = model.LastName,
                  DateOfBirth = model.DateOfBirth,
                  ProfilePictureFileName = model.ProfilePictureFileName
              };

        public static Socially.Mobile.Logic.Models.ProfileUpdateModel ToMobileModel(this Socially.Models.ProfileUpdateModel model, Socially.Mobile.Logic.Models.ProfileUpdateModel dest)
        {
            dest.FirstName = model.FirstName;
            dest.LastName = model.LastName;
            dest.DateOfBirth = model.DateOfBirth;
            dest.ProfilePictureFileName = model.ProfilePictureFileName;
            return dest;
        }

    }

}

