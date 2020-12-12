﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public abstract class EndpointsBase
    {

        private ICollection<IMappedMethod> _methods;

        public EndpointsBase() : this(false)
        {

        }

        private EndpointsBase(bool isPrivatelyCreated)
        {
            _methods = new List<IMappedMethod>();
        }

        protected void MapMethods(string pattern, string method, RequestDelegate requestDelegate)
        {
            var methodItem = new MappedMethod(pattern, method, requestDelegate);
            _methods.Add(methodItem);
        }

        public static void AddToServices(IServiceCollection services)
        {
            var allData = new AllMappedData();

            var types = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(a => a.BaseType == typeof(EndpointsBase));

            foreach (var t in types)
            {
                services.AddTransient(t);
                var endpoints = (EndpointsBase) Activator.CreateInstance(t, true);
                allData.AddRange(endpoints._methods.Select(m => new EndpointBuildingData { 
                    MappedMethod = m,
                    SourceType = t
                }));
            }
            services.AddSingleton(allData);
        }

        public static void MapAllEndpoints(IEndpointRouteBuilder endpoints)
        {
            var allData = endpoints.ServiceProvider.GetService<AllMappedData>();
            foreach (var data in allData)
            {
                var mapped = data.MappedMethod;
                endpoints.MapMethods(mapped.Pattern,
                                    new string[] { mapped.Method }, 
                                    context => ((EndpointsBase)context.RequestServices
                                                    .GetService(data.SourceType))
                                                    ._methods
                                                    .Single(m => m.Pattern == mapped.Pattern && m.Method == mapped.Method)
                                                    .RequestDelegate(context));
            }
        }

        private class AllMappedData : List<EndpointBuildingData> { }

        private class EndpointBuildingData
        {
            public IMappedMethod MappedMethod { get; set; }

            public Type SourceType { get; set; }

        }

        public interface IMappedMethod
        {
            RequestDelegate RequestDelegate { get; }
            string Method { get; }
            string Pattern { get; }
        }

        private class MappedMethod : IMappedMethod
        {
            public RequestDelegate RequestDelegate { get; }
            public string Pattern { get; }
            public string Method { get; }

            public MappedMethod(string pattern, 
                                string method,
                                RequestDelegate requestDelegate)
            {
                RequestDelegate = requestDelegate;
                Pattern = pattern;
                Method = method;
            }
        }

    }
}
