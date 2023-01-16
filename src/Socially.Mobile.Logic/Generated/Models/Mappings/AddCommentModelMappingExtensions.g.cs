
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class AddCommentModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.AddCommentModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.AddCommentModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.AddCommentModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.AddCommentModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.AddCommentModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.AddCommentModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.AddCommentModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.AddCommentModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.AddCommentModel> ToModel(this Task<Socially.Mobile.Logic.Models.AddCommentModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.AddCommentModel ToModel(this Socially.Mobile.Logic.Models.AddCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Text = model.Text,
                  PostId = model.PostId,
                  ParentCommentId = model.ParentCommentId
              };

        public static Socially.Models.AddCommentModel ToModel(this Socially.Mobile.Logic.Models.AddCommentModel model, Socially.Models.AddCommentModel dest)
        {
            dest.Text = model.Text;
            dest.PostId = model.PostId;
            dest.ParentCommentId = model.ParentCommentId;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.AddCommentModel CloneFrom(this Socially.Mobile.Logic.Models.AddCommentModel dest, Socially.Mobile.Logic.Models.AddCommentModel model)
        {
            dest.Text = model.Text;
            dest.PostId = model.PostId;
            dest.ParentCommentId = model.ParentCommentId;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.AddCommentModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.AddCommentModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.AddCommentModel> ToMobileModel(this IEnumerable<Socially.Models.AddCommentModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.AddCommentModel>> ToMobileModel(this Task<ICollection<Socially.Models.AddCommentModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.AddCommentModel> ToMobileModel(this ICollection<Socially.Models.AddCommentModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.AddCommentModel> ToMobileModel(this Task<Socially.Models.AddCommentModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.AddCommentModel ToMobileModel(this Socially.Models.AddCommentModel model)
            => model is null ? null : 
              new() 
              {
                  Text = model.Text,
                  PostId = model.PostId,
                  ParentCommentId = model.ParentCommentId
              };

        public static Socially.Mobile.Logic.Models.AddCommentModel ToMobileModel(this Socially.Models.AddCommentModel model, Socially.Mobile.Logic.Models.AddCommentModel dest)
        {
            dest.Text = model.Text;
            dest.PostId = model.PostId;
            dest.ParentCommentId = model.ParentCommentId;
            return dest;
        }

    }

}

