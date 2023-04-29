using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.ClientUtils
{
    public class AsyncLocker
    {

        private readonly SemaphoreSlim _locker = new(1, 1);

        public Task WaitAsync() => _locker.WaitAsync();

        public async Task<IDisposable> WaitAndBeginLockAsync()
        {
            await _locker.WaitAsync();
            return new Locker(this);
        }

        class Locker : IDisposable
        {
            private readonly AsyncLocker _src;

            public Locker(AsyncLocker src)
            {
                _src = src;
            }

            public void Dispose()
            {
                _src._locker.Release();
            }

        }

    }
}
