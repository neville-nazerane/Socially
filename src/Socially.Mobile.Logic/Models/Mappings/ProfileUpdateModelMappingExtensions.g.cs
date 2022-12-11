
//// <GENERATED CODE> //////
namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class ProfileUpdateModelMappingExtensions 
    {

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

