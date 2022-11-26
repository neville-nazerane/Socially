using Socially.Models.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class CachedStorage<TKey, TValue> : ICachedStorage<TKey, TValue>
        where TValue : class, ICachable<TKey, TValue>, new()
    {

        readonly ConcurrentDictionary<TKey, TValue> _data = new();
        readonly ConcurrentBag<TKey> _initialized = new();

        private TaskCompletionSource _lock;

        public TValue Get(TKey id) => _data.GetOrAdd(id, id => new());


        public IEnumerable<TKey> ExistingIds => _data.Keys;

        /// <summary>
        /// </summary>
        /// <returns>true if there was a lock</returns>
        public async Task<bool> AwaitLockAsync()
        {
            if (_lock is not null)
            {
                await _lock.Task;
                return true;
            }
            return false;
        }

        public async Task UpdateAsync(IEnumerable<TValue> updatedValues)
        {
            await AwaitLockAsync();
            _lock = new TaskCompletionSource();

            try
            {
                foreach (var value in updatedValues)
                {
                    _data.AddOrUpdate(value.GetCacheKey(), value, (key, existing) =>
                    {
                        existing ??= new();
                        existing.CopyFrom(value);
                        return existing;
                    });
                }
                _lock.TrySetResult();
            }
            catch (Exception ex)
            {
                _lock.SetException(ex);
            }
        }

        public bool IsInitialized(params TKey[] ids)
            => _initialized.Intersect(ids).Count() == ids.Count();

    }
}
