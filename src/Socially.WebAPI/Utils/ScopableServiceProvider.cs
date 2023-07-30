using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace Socially.WebAPI.Utils
{
    public class ScopableServiceProvider : IAsyncDisposable
    {
        private readonly AsyncServiceScope _scope;
        protected bool _disposed;

        public ScopableServiceProvider(IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateAsyncScope();
        }

        public TService GetService<TService>() => _scope.ServiceProvider.GetService<TService>();

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _scope.Dispose();

                // Set the flag to indicate that the instance has been disposed.
                _disposed = true;
            }
        }

        public virtual async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await _scope.DisposeAsync();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~ScopableServiceProvider()
        {
            Dispose(false);
        }
    }

}
