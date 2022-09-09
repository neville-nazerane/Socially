using Microsoft.AspNetCore.Http;
using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Utils
{
    internal static class ModelExtensions
    {

        internal static HttpContent MakeForm<TModel>(this TModel model)
            where TModel : class
        {
            var result = new MultipartFormDataContent();
            foreach (var p in typeof(TModel).GetProperties())
            {
                if (p.PropertyType == typeof(IFormFile))
                    continue;

                if (p.PropertyType == typeof(UploadContext))
                {
                    var value = (UploadContext)p.GetValue(model);
                    string name = p.Name[..^"Context".Length];
                    result.Add(new StreamContent(value.Stream), name);
                }
                else if (p.PropertyType.IsPrimitive)
                {
                    result.Add(new StringContent(p.GetValue(model)?.ToString()), p.Name);
                }
                // account for objects
            }
            return result;
        }

    }
}
