using System.Net;

namespace SchedulerService.Errors;

public class RestException : Exception
{
    public RestException()
    {
    }

    public RestException(HttpStatusCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public HttpStatusCode Code { get; }
    public string Message { get; }
}