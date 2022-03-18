using Microsoft.AspNetCore.Http;
using Miritush.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Miritush.API.Exceptions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorHandler = new DTO.ErrorMessageResult();             

                switch (error)
                {
                    case BadHttpRequestException e:
                        // custom application error
                        errorHandler.Message = e.Message;
                        errorHandler.StackTrace = e.StackTrace;
                        response.StatusCode = (int)e.StatusCode;
                        break;
                    case NotFoundException e:
                        // not found error
                        errorHandler.StatusCode = (int)e.Code;
                        errorHandler.Message = e.Message;
                        errorHandler.StackTrace = e.StackTrace;
                        response.StatusCode = (int)e.Code;
                        break;
                    case UnauthorizedException e:
                        errorHandler.StatusCode = (int)e.Code;
                        errorHandler.Message = e.Message;
                        errorHandler.StackTrace = e.StackTrace;
                        response.StatusCode = (int)e.Code;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(errorHandler);
                await response.WriteAsync(result);
            }
        }
    }
}
