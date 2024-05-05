namespace MotoDispatch.Application.Exceptions;

public class DuplicateEntityException(string? message)
    : ApplicationException(message)
{
    public static void ThrowIfTrue(bool condition, string exceptionMessage)
    {
        if (condition)
            throw new DuplicateEntityException(exceptionMessage);
    }
}