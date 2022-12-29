
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class DisplayCommentModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.DisplayCommentModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.DisplayCommentModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.DisplayCommentModel> ToModel(this Task<Socially.Mobile.Logic.Models.DisplayCommentModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.DisplayCommentModel ToModel(this Socially.Mobile.Logic.Models.DisplayCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  CreatorId = model.CreatorId,
                  Text = model.Text,
                  Comments = model.Comments,
                  LikeCount = model.LikeCount
              };

        public static Socially.Models.DisplayCommentModel ToModel(this Socially.Mobile.Logic.Models.DisplayCommentModel model, Socially.Models.DisplayCommentModel dest)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.DisplayCommentModel Clone(this Socially.Mobile.Logic.Models.DisplayCommentModel model, Socially.Mobile.Logic.Models.DisplayCommentModel dest)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.DisplayCommentModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.DisplayCommentModel> ToMobileModel(this IEnumerable<Socially.Models.DisplayCommentModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.DisplayCommentModel> ToMobileModel(this Task<Socially.Models.DisplayCommentModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.DisplayCommentModel ToMobileModel(this Socially.Models.DisplayCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Id = model.Id,
                  CreatorId = model.CreatorId,
                  Text = model.Text,
                  Comments = model.Comments,
                  LikeCount = model.LikeCount
              };

        public static Socially.Mobile.Logic.Models.DisplayCommentModel ToMobileModel(this Socially.Models.DisplayCommentModel model, Socially.Mobile.Logic.Models.DisplayCommentModel dest)
        {
            dest.Id = model.Id;
            dest.CreatorId = model.CreatorId;
            dest.Text = model.Text;
            dest.Comments = model.Comments;
            dest.LikeCount = model.LikeCount;
            return dest;
        }

    }

}

