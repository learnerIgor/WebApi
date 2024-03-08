using Common.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Common.Service
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsHandlerMiddleware(RequestDelegate next) 
        {  
            _next = next; 
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exceptions)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var result = string.Empty;

                switch(exceptions)
                {
                    case BadRequestException badRequestException:
                        statusCode=HttpStatusCode.BadRequest;
                        result = JsonConvert.SerializeObject(badRequestException.Message);
                        break;
                    case NotFoundException notFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        result = JsonConvert.SerializeObject(notFoundException.Message);
                        break;
                    default:
                        result = exceptions.Message;
                        break;
                }

                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}
