using Application.Api.Exceptions;

namespace Application.Api.Filters;

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