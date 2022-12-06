
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Models.Mappings
{

    public static class PostDisplayModelMappingExtensions 
    {

        public static Socially.Models.PostDisplayModel ToModel(this Socially.MobileApp.Models.PostDisplayModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  Text = model.Text,
                  CreatorId = model.CreatorId,
                  CreatedOn = model.CreatedOn,
                  Comments = model.Comments,
                  LikeCount = model.LikeCount
              };

        public static Socially.MobileApp.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  Text = model.Text,
                  CreatorId = model.CreatorId,
                  CreatedOn = model.CreatedOn,
                  Comments = model.Comments,
                  LikeCount = model.LikeCount
              };

    }

}

