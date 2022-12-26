using System.Net;
using NewsParser.Application.Common.Errors;

namespace NewsParser.Application.Authentication.Common.Errors;

public class InvalidPasswordException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "Invalid password.";
}