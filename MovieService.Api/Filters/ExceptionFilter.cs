using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MovieService.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                Error = "An unexpected error occurred.",
                Details = context.Exception.Message
            })
            {
                StatusCode = 500
            };

            // Log the exception (logging implementation not shown)
            // Logger.LogError(context.Exception, "An error occurred while processing the request.");

            context.ExceptionHandled = true;
        }
    }
}