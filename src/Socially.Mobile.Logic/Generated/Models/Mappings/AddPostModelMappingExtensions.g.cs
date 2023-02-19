
//// <GENERATED CODE> //////

namespace Socially.Mobile.Logic.Models.Mappings
{

    public static class AddPostModelMappingExtensions 
    {

        public static async Task<IEnumerable<Socially.Models.AddPostModel>> ToModel(this Task<IEnumerable<Socially.Mobile.Logic.Models.AddPostModel>> modelTask)
            => (await modelTask).ToModel();

        public static IEnumerable<Socially.Models.AddPostModel> ToModel(this IEnumerable<Socially.Mobile.Logic.Models.AddPostModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<ICollection<Socially.Models.AddPostModel>> ToModel(this Task<ICollection<Socially.Mobile.Logic.Models.AddPostModel>> modelTask)
            => (await modelTask).ToModel();

        public static ICollection<Socially.Models.AddPostModel> ToModel(this ICollection<Socially.Mobile.Logic.Models.AddPostModel> model)
            => model == null ? null : model.Select(m => m.ToModel()).ToArray();

        public static async Task<Socially.Models.AddPostModel> ToModel(this Task<Socially.Mobile.Logic.Models.AddPostModel> modelTask)
            => (await modelTask).ToModel();

        public static Socially.Models.AddPostModel ToModel(this Socially.Mobile.Logic.Models.AddPostModel model)
            => model is null ? null : 
              new() 
              {
                  Text = model.Text
              };

        public static Socially.Models.AddPostModel ToModel(this Socially.Mobile.Logic.Models.AddPostModel model, Socially.Models.AddPostModel dest)
        {
            dest.Text = model.Text;
            return dest;
        }

        public static Socially.Mobile.Logic.Models.AddPostModel CloneFrom(this Socially.Mobile.Logic.Models.AddPostModel dest, Socially.Mobile.Logic.Models.AddPostModel model)
        {
            dest.Text = model.Text;
            return dest;
        }

        public static async Task<IEnumerable<Socially.Mobile.Logic.Models.AddPostModel>> ToMobileModel(this Task<IEnumerable<Socially.Models.AddPostModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static IEnumerable<Socially.Mobile.Logic.Models.AddPostModel> ToMobileModel(this IEnumerable<Socially.Models.AddPostModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<ICollection<Socially.Mobile.Logic.Models.AddPostModel>> ToMobileModel(this Task<ICollection<Socially.Models.AddPostModel>> modelTask)
            => (await modelTask).ToMobileModel();

        public static ICollection<Socially.Mobile.Logic.Models.AddPostModel> ToMobileModel(this ICollection<Socially.Models.AddPostModel> model)
            => model == null ? null : model.Select(m => m.ToMobileModel()).ToArray();   

        public static async Task<Socially.Mobile.Logic.Models.AddPostModel> ToMobileModel(this Task<Socially.Models.AddPostModel> modelTask)
            => (await modelTask).ToMobileModel();

        public static Socially.Mobile.Logic.Models.AddPostModel ToMobileModel(this Socially.Models.AddPostModel model)
            => model is null ? null : 
              new() 
              {
                  Text = model.Text
              };

        public static Socially.Mobile.Logic.Models.AddPostModel ToMobileModel(this Socially.Models.AddPostModel model, Socially.Mobile.Logic.Models.AddPostModel dest)
        {
            dest.Text = model.Text;
            return dest;
        }

    }

}

