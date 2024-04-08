using System.Net;
using System.Text.Json;
using SchedulerService.Errors;

namespace SchedulerService.Middleware;

public class ErrorHandling
{
    private readonly ILogger<ErrorHandling> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandling> logger)
    {
        string message = null;

        switch (ex)
        {
            case RestException re:
                logger.LogError(ex, "REST ERROR");
                message = re.Message;
                context.Response.StatusCode = (int)re.Code;
                break;
            case Exception e:
                logger.LogError(ex, "SERVER ERROR");
                message = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";

        if (message != null)
        {
            var result = JsonSerializer.Serialize(new
            {
                message
            });

            await context.Response.WriteAsync(result);
        }
    }
}