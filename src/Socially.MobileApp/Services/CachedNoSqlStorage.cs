using Android.Widget;
using AndroidX.Startup;
using LiteDB;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;
using Socially.Models.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    internal class CachedNoSqlStorage<TKey, TValue> : ICachedNoSqlStorage<TKey, TValue>
        where TValue : class, ICachable<TKey, TValue>, new()
    {

        readonly ConcurrentDictionary<TKey, TValue> _data = new();
        private readonly ILiteCollection<TValue> collection;
        private TaskCompletionSource _lock;

        public CachedNoSqlStorage()
        {
            var db = new LiteDatabase(Configs.NoSqlLocation);
            collection = db.GetCollection<TValue>($"cached-{nameof(TValue)}");
        }

        public TValue Get(TKey id)
        {
            return _data.GetOrAdd(id, id =>
            {
                var res = collection.FindById(id.ToBsonValue());
                return res ?? new();
            });
        }

        public bool IsInitialized(TKey id) => collection.Exists(Query.In("_id", id.ToBsonValue()));

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
                foreach (var val in updatedValues)
                {
                    var key = val.GetCacheKey();
                    if (_data.TryGetValue(key, out var existing))
                        existing.CopyFrom(val);
                    collection.Upsert(key.ToBsonValue(), val);
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

        public async Task ClearAllInRamAsync()
        {
            await AwaitLockAsync();
            var localLock = new TaskCompletionSource();
            _lock = localLock;
            try
            {
                _data.Clear();
            }
            finally
            {
                _lock = null;
                localLock.TrySetResult();
            }
        }

        public async Task ClearDbAsync()
        {
            await AwaitLockAsync();
            var localLock = new TaskCompletionSource();
            _lock = localLock;
            try
            {
                collection.DeleteAll();
            }
            finally
            {
                _lock = null;
                localLock.TrySetResult();
            }
        }

    }
}
