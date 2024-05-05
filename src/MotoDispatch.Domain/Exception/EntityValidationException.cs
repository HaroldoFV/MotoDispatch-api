namespace MotoDispatch.Domain.Exception;

public class EntityValidationException(string? message) : System.Exception(message)
{
}