using Socially.Models;
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

    }
}
