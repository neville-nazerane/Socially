using Socially.Models.Utils;
using Socially.Website.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public class CachedStorage<TKey, TValue> : ICachedStorage<TKey, TValue>
        where TValue : class, ICachable<TKey, TValue>, new()
    {

        readonly ConcurrentDictionary<TKey, TValue> _data = new();
        readonly ConcurrentBag<TKey> _initialized = new();

        private readonly SemaphoreSlim _locker = new(1, 1);

        public TValue Get(TKey id) => _data.GetOrAdd(id, id => new());


        public IEnumerable<TKey> ExistingIds => _data.Keys;

        /// <summary>
        /// </summary>
        /// <returns>true if there was a lock</returns>
        public Task AwaitLockAsync() => _locker.WaitAsync();

        public async Task UpdateAsync(IEnumerable<TValue> updatedValues)
        {
            await _locker.WaitAsync();
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
            }
            finally
            {
                _locker.Release();
            }
        }

        public bool IsInitialized(params TKey[] ids)
            => _initialized.Intersect(ids).Count() == ids.Length;

        public async Task ClearAllAsync()
        {
            await AwaitLockAsync();
            try
            {
                _data.Clear();
                _initialized.Clear();
            }
            finally
            {
                _locker.Release();
            }

        }

    }
}
