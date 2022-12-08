
//// <GENERATED CODE> //////
namespace Socially.MobileApp.Logic.Models.Mappings
{

    public static class PostDisplayModelMappingExtensions 
    {

        public static Socially.Models.PostDisplayModel ToModel(this Socially.MobileApp.Logic.Models.PostDisplayModel model)
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

        public static Socially.Models.PostDisplayModel ToModel(this Socially.MobileApp.Logic.Models.PostDisplayModel model, Socially.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

        public static Socially.MobileApp.Logic.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model)
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

        public static Socially.MobileApp.Logic.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model, Socially.MobileApp.Logic.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

    }

}

