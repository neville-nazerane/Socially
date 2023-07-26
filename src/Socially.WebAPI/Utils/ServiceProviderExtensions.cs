using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public static class ServiceProviderExtensions
    {

        public static async Task RunScopedLogicAsync<TService>(this IServiceProvider serviceProvider, 
                                                                Func<TService, Task> func)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetService<TService>();
            await func(service);
        }

        public static async Task RunScopedLogicAsync<TService1, TService2>(
                                                                this IServiceProvider serviceProvider,
                                                                Func<TService1, TService2, Task> func)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var scopedProvider = scope.ServiceProvider;
            var service1 = scopedProvider.GetService<TService1>();
            var service2 = scopedProvider.GetService<TService2>();
            await func(service1, service2);
        }

        public static async Task RunScopedLogicAsync<TService1, TService2, TService3>(
                                                                            this IServiceProvider serviceProvider,
                                                                            Func<TService1, TService2, TService3, Task> func)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var scopedProvider = scope.ServiceProvider;
            var service1 = scopedProvider.GetService<TService1>();
            var service2 = scopedProvider.GetService<TService2>();
            var service3 = scopedProvider.GetService<TService3>();
            await func(service1, service2, service3);
        }






    }
}
