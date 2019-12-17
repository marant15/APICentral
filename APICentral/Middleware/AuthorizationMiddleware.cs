using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICentral.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Services;

namespace APICentral.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private IAuthorizationService _authService;

        public AuthorizationMiddleware(RequestDelegate next, IAuthorizationService authService)
        {
            _next = next;
            _authService = authService;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //if (httpContext.Request.Path.Value.Contains("login") || httpContext.Request.Path.Value.Contains("swagger")||
             //   httpContext.Request.Path.Value.Contains("/swagger/v1/swagger.json") || httpContext.Request.Path.Value.Contains("/favicon.ico"))
            //{
             //   return _next(httpContext);
            //} 
            //else
           // {
            //    string bearerTOken = httpContext.Request.Headers["Authorization"];
            //    string[] typeOfAuth = bearerTOken.Split(" ");
            //if (typeOfAuth[0] == "Bearer")
            //{
            //    if (_authService.ValidateUser(bearerTOken))
            //    {
            //        return _next(httpContext);
            //    }
            //    else
            //    {
            //        throw new UnauthorizedAccessException();
            //    }
            //}
            return _next(httpContext);

            //   }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
