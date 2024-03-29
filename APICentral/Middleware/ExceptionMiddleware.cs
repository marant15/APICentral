﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services;

namespace APICentral.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandlerExceptionAsync(httpContext, e);
            }
        }

        private static Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = getCode(ex.InnerException);
            var errorObj = new
            {
                Code = statusCode,
                Message = ex.Message
            };
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorObj));
        }

        private static int getCode(Exception ex)
        {
            if(ex.GetType() == typeof(ServicesException))
            {
                return ((ServicesException)ex).code;
            }
            return 500;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
