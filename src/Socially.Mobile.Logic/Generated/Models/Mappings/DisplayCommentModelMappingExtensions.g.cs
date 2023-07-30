
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class DisplayCommentModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.DisplayCommentModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.DisplayCommentModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToList();

        public static async Task<List<Socially.Models.DisplayCommentModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToModel();

        public static List<Socially.Models.DisplayCommentModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToList();

        public static async Task<Socially.Models.DisplayCommentModel> ToModel(this Task<Socially.Mobile.Logic.Models.DisplayCommentModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.DisplayCommentModel ToModel(this Socially.Mobile.Logic.Models.DisplayCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  CreatorId = model.CreatorId,
                  Text = model.Text,
                  Comments = model.Comments.ToModel(),
                  LikeCount = model.LikeCount,
                  CreatedOn = model.CreatedOn,
                  IsLikedByCurrentUser = model.IsLikedByCurrentUser
              };

        public static Socially.Models.DisplayCommentModel ToModel(this Socially.Mobile.Logic.Models.DisplayCommentModel model, Socially.Models.DisplayCommentModel dest)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments.ToModel();
            dest.LikeCount = model.LikeCount;
            dest.CreatedOn = model.CreatedOn;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.DisplayCommentModel CloneFrom(this Socially.Mobile.Logic.Models.DisplayCommentModel dest, Socially.Mobile.Logic.Models.DisplayCommentModel model)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            dest.CreatedOn = model.CreatedOn;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel> ToMobileModel(this IEnumerable<Socially.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToList();   

        public static async Task<List<Socially.Mobile.Logic.Models.DisplayCommentModel>> ToMobileModel(this Task<ICollection<Socially.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static List<Socially.Mobile.Logic.Models.DisplayCommentModel> ToMobileModel(this ICollection<Socially.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToList();   

        public static async Task<Socially.Mobile.Logic.Models.DisplayCommentModel> ToMobileModel(this Task<Socially.Models.DisplayCommentModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.DisplayCommentModel ToMobileModel(this Socially.Models.DisplayCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  CreatorId = model.CreatorId,
                  Text = model.Text,
                  Comments = model.Comments.ToMobileModel(),
                  LikeCount = model.LikeCount,
                  CreatedOn = model.CreatedOn,
                  IsLikedByCurrentUser = model.IsLikedByCurrentUser
              };

        public static Socially.Mobile.Logic.Models.DisplayCommentModel ToMobileModel(this Socially.Models.DisplayCommentModel model, Socially.Mobile.Logic.Models.DisplayCommentModel dest)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments.ToMobileModel();
            dest.LikeCount = model.LikeCount;
            dest.CreatedOn = model.CreatedOn;
            dest.IsLikedByCurrentUser = model.IsLikedByCurrentUser;
            return dest;
        }

    }

}

