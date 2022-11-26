using Socially.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public interface ICachedStorage<TKey, TValue> where TValue : class, ICachable<TKey, TValue>, new()
    {
        IEnumerable<TKey> ExistingIds { get; }

        Task<bool> AwaitLockAsync();
        TValue Get(TKey id);
        bool IsInitialized(params TKey[] ids);
        Task UpdateAsync(IEnumerable<TValue> updatedValues);
    }
}