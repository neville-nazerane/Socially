using Socially.Models.Utils;
using Socially.Website.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
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
            var localLock = new TaskCompletionSource();
            _lock = localLock;

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
                    _initialized.Add(value.GetCacheKey());
                }
                _lock = null;
                localLock.TrySetResult();
            }
            catch (Exception ex)
            {
                _lock = null;
                localLock.TrySetException(ex);
            }
        }

        public bool IsInitialized(params TKey[] ids)
            => _initialized.Intersect(ids).Count() == ids.Length;

        public async Task ClearAllAsync()
        {
            await AwaitLockAsync();
            var localLock = new TaskCompletionSource();
            _lock = localLock;
            try
            {

                _data.Clear();
                _initialized.Clear();
            }
            finally
            {
                _lock = null;
                localLock.TrySetResult();
            }


        }

    }
}
