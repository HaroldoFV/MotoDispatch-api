namespace MotoDispatch.Application.Exceptions;

public abstract class ApplicationException(string? message)
    : Exception(message);