using Microsoft.AspNetCore.Http;
using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.EndpointUtils
{
    public static class HttpContextModelExtensions
    {

        public static async Task<bool> TryValidateModelAsync<TModel>(this HttpContext context, 
                                                                     Func<TModel, Task> onValid)
        {
            var validationResults = new List<ValidationResult>();

            var model = await context.Request.ReadFromJsonAsync<TModel>();
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults);

            if (isValid)
            {
                if (onValid is not null) await onValid(model);
                return true;
            }
            else
            {
                var errors = new List<ErrorModel>();

                foreach (var validation in validationResults)
                    foreach (var field in validation.MemberNames)
                    {
                        var errorModel = errors.SingleOrDefault(e => e.Field == field);
                        if (errorModel is null)
                        {
                            errorModel = new ErrorModel { 
                                Field = field
                            };
                        }
                        errorModel.Errors.Add(validation.ErrorMessage);
                    }

                await context.Response.WriteAsJsonAsync(errors);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return false;
            }
        }

        public static Task<bool> TryValidateModelAsync<TModel>(this HttpContext context, Action<TModel> onValid)
            => TryValidateModelAsync(context, (Func<TModel,Task>) (model => {
                onValid(model);
                return Task.CompletedTask;
            }));

        public static async Task BadRequestAsync(this HttpContext context, IEnumerable<ErrorModel> errors)
        {
            await context.Response.WriteAsJsonAsync(errors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

    }
}
