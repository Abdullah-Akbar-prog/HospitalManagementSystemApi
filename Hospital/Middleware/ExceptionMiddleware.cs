using Hospital.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hospital.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (status, title) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, "Resource  not found"),
                    BadRequestException => (HttpStatusCode.BadRequest, "Bad request"),
                    UnauthorizedAppException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                    _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
                };

                if (status == HttpStatusCode.InternalServerError)
                    _logger.LogError(ex, "Unhandled exception");
                var problem = new ProblemDetails
                {
                    Status = (int)status,
                    Title = title,
                    Detail = ex.Message
                };
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }

}