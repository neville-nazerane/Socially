using Socially.Mobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Utils
{
    public static class PostExtensions
    {

        public static IEnumerable<PostDisplayModel> ReverseRecursive(this IEnumerable<PostDisplayModel> posts)
        {
            foreach (var post in posts)
                post.Comments.ReverseRecursive();
            return posts.Reverse();
        }

        public static ICollection<DisplayCommentModel> ReverseRecursive(this IEnumerable<DisplayCommentModel> commentModels)
        {
            if (commentModels is null)
                return null;

            var result = commentModels.Reverse().ToList();

            foreach (var comment in commentModels)
                comment.Comments = comment.Comments.ReverseRecursive();

            return result;
        }

    }
}
