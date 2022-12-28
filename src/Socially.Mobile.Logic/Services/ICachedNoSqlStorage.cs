using Socially.Models.Utils;

namespace Socially.Mobile.Logic.Services
{
    public interface ICachedNoSqlStorage<TKey, TValue>
        where TValue : class, ICachable<TKey, TValue>, new()
    {
        Task<bool> AwaitLockAsync();
        Task ClearAllInRamAsync();
        Task ClearDbAsync();
        TValue Get(TKey id);
        bool IsInitialized(TKey id);
        Task UpdateAsync(IEnumerable<TValue> updatedValues);
    }
}