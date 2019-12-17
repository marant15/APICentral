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

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.ToLower() == "/crm-api/login" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/index.html" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/v1/swagger.json" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/favicon-32x32.png" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/favicon-16x16.png" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/swagger-ui.css" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/swagger-ui-bundle.js" ||
                httpContext.Request.Path.Value.ToLower() == "/swagger/swagger-ui-standalone-preset.js"
                )
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                string bearerTOken = httpContext.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(bearerTOken))
                {
                    string[] typeOfAuth = bearerTOken.Split(" ");
                    if (typeOfAuth[0] == "Bearer")
                    {
                        if (_authService.ValidateUser(bearerTOken))
                        {
                            await _next(httpContext);
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("Unauthorized");
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException("Token null");
                }
            }
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
