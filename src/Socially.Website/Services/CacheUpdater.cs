using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Utils;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class CacheUpdater : ICacheUpdater
    {
        private readonly ICachedStorage<int, PostDisplayModel> _postsCache;

        public CacheUpdater(ICachedStorage<int, PostDisplayModel> postsCache)
        {
            _postsCache = postsCache;
        }

        public Task UpdatePostAsync(PostDisplayModel post)
        {
            ConsoleUtils.Log("Post updated", post);
            return _postsCache.UpdateAsync(new[] { post });
        }
    }
}
