
using CozaStore.Errors;
using System.Net;
using System.Text.Json;

namespace CozaStore.Middleware
{
    public class ExceptionMiddleware 
    {
        public RequestDelegate Next { get; set; }
        public ILogger<ExceptionMiddleware> Logger { get; set; }
        public IHostEnvironment Env { get; set; }

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment evn)
        {
            Next = next;
            Logger = logger;
            Env = evn;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = Env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                    : new AppException(context.Response.StatusCode, ex.Message, "Internal server error");
               
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(response, options);

                context.Response.Redirect($"/Buggy/ServerError?statusCode={response.StatusCode}&message={Uri.EscapeDataString(response.Message)}&details={Uri.EscapeDataString(response.Details)}");
            }
        }
    }
}
