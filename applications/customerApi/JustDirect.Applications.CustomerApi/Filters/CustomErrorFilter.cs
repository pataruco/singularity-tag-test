using JustDirect.Applications.CustomerApi.Exceptions;

namespace JustDirect.Applications.CustomerApi.Filters;

public class CustomErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is InvalidInputException)
        {
            return error.WithMessage(error.Exception.Message);
        }

        return error;
    }
}