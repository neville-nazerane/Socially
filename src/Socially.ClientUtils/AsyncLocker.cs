using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ClientUtils
{
    public class AsyncLocker
    {

        private TaskCompletionSource _lock;

        public Task WaitForLockAsync()
        {
            if (_lock is not null) 
                return _lock.Task;
            return Task.CompletedTask;
        }

        public async Task<IDisposable> WaitAndBeginLockAsync()
        {
            await WaitForLockAsync();
            return new Locker(this);
        }

        class Locker : IDisposable
        {
            private readonly AsyncLocker _src;
            private readonly TaskCompletionSource _innerLock;

            public Locker(AsyncLocker src)
            {
                _src = src;
                _innerLock = new();
                _src._lock = _innerLock;
            }

            public void Dispose()
            {
                _innerLock.SetResult();
                _src._lock = null;
            }

        }

    }
}
