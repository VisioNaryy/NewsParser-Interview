using System.Net;
using NewsParser.Application.Common.Errors;

namespace NewsParser.Application.Authentication.Common.Errors;

public class InvalidEmailException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "Specified email doesn't exist.";
}