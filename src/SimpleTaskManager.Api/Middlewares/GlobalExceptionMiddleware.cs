using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SimpleTaskManager.Entities.DomainExceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleTaskManager.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case InputValidationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case SimpleTaskManagerException _:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;

                case EntityNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = exception.Message }));
        }
    }
}
