namespace VisionCare.Application.Exceptions;

public class BusinessException : Exception
{
    public int StatusCode { get; }

    public BusinessException(string message, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public class ConflictException : BusinessException
{
    public ConflictException(string message)
        : base(message, 409) { }
}

public class UnprocessableEntityException : BusinessException
{
    public UnprocessableEntityException(string message)
        : base(message, 422) { }
}

public class ServiceUnavailableException : BusinessException
{
    public ServiceUnavailableException(string message)
        : base(message, 503) { }
}
