using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextModelExtensions
    {

        public static async Task<(bool isValid, TModel model)> TryValidateModelAsync<TModel>(this HttpContext context, ICollection<ValidationResult> validationResults)
        {
            var model = await JsonSerializer.DeserializeAsync<TModel>(context.Request.Body);
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults);
            return (isValid, model);
        }

    }
}
