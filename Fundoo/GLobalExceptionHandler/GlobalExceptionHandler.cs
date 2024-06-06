using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.CustomExecption;
using System.ComponentModel.DataAnnotations;

namespace Fundoo.GLobalExceptionHandler
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;


        public GlobalExceptionHandler(RequestDelegate next)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                UserException => StatusCodes.Status404NotFound,
                //UserException => StatusCodes.Status500InternalServerError,
                //UserException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ResponseML
            {
                Success = false,
                Message = exception.Message,
                Data = null
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
