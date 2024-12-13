namespace ExpressYourself.Domain.Exceptions;
public class InvalidIpException : Exception
{
    public InvalidIpException() { }

    public InvalidIpException(string message)
        : base(message) { }

    public InvalidIpException(string message, Exception inner)
        : base(message, inner) { }
}

