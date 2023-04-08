﻿using Socially.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Socially.ClientUtils
{
    public static class PostExtensions
    {

        public static IEnumerable<int> GetAllCreatedIds(this IEnumerable<PostDisplayModel> posts)
            => posts.Select(p => p.CreatorId)
                    .Union(posts.SelectMany(p => p.Comments.Select(p => p.CreatorId)))
                    .Distinct()
                    .ToArray();

        public static IEnumerable<PostDisplayModel> Reverse(this IEnumerable<PostDisplayModel> posts)
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
