
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class PostDisplayModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.PostDisplayModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.PostDisplayModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.PostDisplayModel> ToModel(this Task<Socially.Mobile.Logic.Models.PostDisplayModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.PostDisplayModel ToModel(this Socially.Mobile.Logic.Models.PostDisplayModel model)
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

        public static Socially.Models.PostDisplayModel ToModel(this Socially.Mobile.Logic.Models.PostDisplayModel model, Socially.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.PostDisplayModel Clone(this Socially.Mobile.Logic.Models.PostDisplayModel model, Socially.Mobile.Logic.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel> ToMobileModel(this IEnumerable<Socially.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.PostDisplayModel> ToMobileModel(this Task<Socially.Models.PostDisplayModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model)
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

        public static Socially.Mobile.Logic.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model, Socially.Mobile.Logic.Models.PostDisplayModel dest)
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

