﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceHelperExtensions
    {

        

        public static IServiceCollection RemoveService<TService>(this IServiceCollection services)
            => services.RemoveService(typeof(TService));

        public static IServiceCollection RemoveService(this IServiceCollection services, Type type)
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == type);

            if (descriptor is not null)
                services.Remove(descriptor);

            return services;
        }

    }
}
