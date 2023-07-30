
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class PostDisplayModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.PostDisplayModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.PostDisplayModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToList();

        public static async Task<List<Socially.Models.PostDisplayModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToModel();

        public static List<Socially.Models.PostDisplayModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToList();

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
                  Comments = model.Comments.ToModel(),
                  LikeCount = model.LikeCount,
                  IsLikedByCurrentUser = model.IsLikedByCurrentUser
              };

        public static Socially.Models.PostDisplayModel ToModel(this Socially.Mobile.Logic.Models.PostDisplayModel model, Socially.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments.ToModel();
            dest.LikeCount = model.LikeCount;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.PostDisplayModel CloneFrom(this Socially.Mobile.Logic.Models.PostDisplayModel dest, Socially.Mobile.Logic.Models.PostDisplayModel model)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.PostDisplayModel> ToMobileModel(this IEnumerable<Socially.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToList();   

        public static async Task<List<Socially.Mobile.Logic.Models.PostDisplayModel>> ToMobileModel(this Task<ICollection<Socially.Models.PostDisplayModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static List<Socially.Mobile.Logic.Models.PostDisplayModel> ToMobileModel(this ICollection<Socially.Models.PostDisplayModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToList();   

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
                  Comments = model.Comments.ToMobileModel(),
                  LikeCount = model.LikeCount,
                  IsLikedByCurrentUser = model.IsLikedByCurrentUser
              };

        public static Socially.Mobile.Logic.Models.PostDisplayModel ToMobileModel(this Socially.Models.PostDisplayModel model, Socially.Mobile.Logic.Models.PostDisplayModel dest)
        {
            dest.Id = model.Id;
            dest.Text = model.Text;
            dest.CreatorId = model.CreatorId;
            dest.CreatedOn = model.CreatedOn;
            dest.Comments = model.Comments.ToMobileModel();
            dest.LikeCount = model.LikeCount;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

    }

}

