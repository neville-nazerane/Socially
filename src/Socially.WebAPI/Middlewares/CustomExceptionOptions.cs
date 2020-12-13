﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Socially.WebAPI.EndpointUtils;
using Socially.WebAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.Middlewares
{
    public class CustomExceptionHandler : ExceptionHandlerOptions
    {

        public CustomExceptionHandler()
        {
            ExceptionHandler = InvokeAsync;
        }

        private Task InvokeAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
            return HandleErrorAsync(exceptionHandlerPathFeature?.Error, context);
        }

        private static Task HandleErrorAsync(Exception ex, HttpContext context)
        {
            switch (ex)
            {
                case BadRequestException badReq:
                    return context.BadRequestAsync(badReq.Errors);
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return Task.CompletedTask;
            }
        }

    }
}
