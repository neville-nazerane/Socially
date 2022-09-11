using Microsoft.AspNetCore.Http;
using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                    if (value is not null && value.Stream is not null)
                        result.Add(new StreamContent(File.OpenRead(@"C:\Setup.log")), name, value.FileName);
                }
                else if (p.PropertyType.IsPrimitive)
                {
                    var value = p.GetValue(model);
                    if (value is not null)
                        result.Add(new StringContent(value.ToString()), p.Name);
                }
                // account for objects
            }
            return result;
        }

    }
}
