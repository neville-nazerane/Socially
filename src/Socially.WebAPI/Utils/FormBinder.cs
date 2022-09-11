using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public class FormBinder<TModel>
        where TModel : class, new()
    {

        public TModel Model { get; }

        private FormBinder(TModel model)
        {
            Model = model;
        }

        public static ValueTask<FormBinder<TModel>> BindAsync(HttpContext httpContext, ParameterInfo _)
        {
            var result = new TModel();
            var form = httpContext.Request.Form;
            foreach (var property in typeof(TModel).GetProperties())
            {
                // basic primitive types
                if (property.PropertyType.IsPrimitive)
                {
                    var obj = GetObjectForPrimitive(form[property.Name], property.PropertyType);
                    property.SetValue(result, obj);
                }
                // files
                else if (property.PropertyType == typeof(IFormFile))
                {
                    var file = form.Files.SingleOrDefault(f => f.Name.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase));
                    property.SetValue(result, file);
                }
                // IEnumerable<>
                else if (property.PropertyType.IsAssignableTo(typeof(IEnumerable)) && property.PropertyType.IsGenericType)
                {
                    var values = form[property.Name];
                    if (values != StringValues.Empty)
                    {
                        var type = property.PropertyType.GenericTypeArguments.First();

                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(type);

                        var instance = (IList)Activator.CreateInstance(constructedListType);

                        var obj = values.Select(v => GetObjectForPrimitive(v, type)).ToArray();
                        property.SetValue(result, obj);
                    }
                }
            }

            return ValueTask.FromResult(new FormBinder<TModel>(result));
        }

        private static object GetObjectForPrimitive(string str, Type type)
        {
            if (type == typeof(string))
                return str;
            else if (type == typeof(int))
                return int.Parse(str);
            else if (type == typeof(double))
                return double.Parse(str);
            else if (type == typeof(bool))
                return bool.Parse(str);
            else if (type == typeof(byte))
                return byte.Parse(str);
            else if (type == typeof(DateTime))
                return DateTime.Parse(str);

            return str;
        }
    }

}
