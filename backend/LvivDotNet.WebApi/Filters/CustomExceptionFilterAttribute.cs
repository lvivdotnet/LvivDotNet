using System;
using System.Net;
using LvivDotNet.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LvivDotNet.Filters
{
    /// <summary>
    /// Exception filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
            {
                return;
            }

            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    ((ValidationException)context.Exception).Failures);

                return;
            }

            HttpStatusCode code;
            switch (context.Exception)
            {
                case NotFoundException e:
                    code = HttpStatusCode.NotFound;
                    break;
                case AuthException e:
                    code = HttpStatusCode.Unauthorized;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                error = new[] { context.Exception.Message },
                stackTrace = context.Exception.StackTrace,
            });
        }
    }
}
