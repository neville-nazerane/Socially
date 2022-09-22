using Microsoft.AspNetCore.Http;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public static class HttpContextModelExtensions
    {

        public static async Task<bool> TryValidateModelAsync<TModel>(this HttpContext context,
                                                                     Func<TModel, CancellationToken, Task> onValid,
                                                                     CancellationToken cancellationToken = default)
        {
            var validationResults = new List<ValidationResult>();

            var model = await context.Request.ReadFromJsonAsync<TModel>(cancellationToken);
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults);

            if (isValid)
            {
                if (onValid is not null) await onValid(model, cancellationToken);
                return true;
            }
            else
            {
                var errors = new List<ErrorModel>();

                foreach (var validation in validationResults)
                {
                    foreach (var field in validation.MemberNames)
                    {
                        var errorModel = errors.SingleOrDefault(e => e.Field == field);
                        if (errorModel is null)
                        {
                            errorModel = new ErrorModel
                            {
                                Field = field
                            };
                            errors.Add(errorModel);
                        }
                        errorModel.Errors.Add(validation.ErrorMessage);
                    }
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(errors, cancellationToken);
                return false;
            }
        }

        public static Task<bool> TryValidateModelAsync<TModel>(this HttpContext context,
                                                                    Func<TModel, Task> onValid,
                                                                    CancellationToken cancellationToken = default)
            => context.TryValidateModelAsync((Func<TModel, CancellationToken, Task>)((m, c)
                    => onValid is not null ? onValid(m) : Task.CompletedTask),
                    cancellationToken);

        public static Task<bool> TryValidateModelAsync<TModel>(this HttpContext context, Action<TModel> onValid)
            => context.TryValidateModelAsync((Func<TModel, Task>)(model =>
            {
                onValid(model);
                return Task.CompletedTask;
            }));

        public static async Task BadRequestAsync(this HttpContext context, IEnumerable<ErrorModel> errors)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(errors);
        }

    }
}
