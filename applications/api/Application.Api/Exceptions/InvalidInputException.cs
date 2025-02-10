namespace Application.Api.Exceptions;

public class InvalidInputException(string message) : Exception(message)
{
}