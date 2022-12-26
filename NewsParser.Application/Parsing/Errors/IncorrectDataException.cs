using System.Net;
using NewsParser.Application.Common.Errors;

namespace NewsParser.Application.Parsing.Errors;

public class IncorrectDataException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "Invalid input data format.";
}