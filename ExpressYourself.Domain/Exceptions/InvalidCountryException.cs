namespace ExpressYourself.Domain.Exceptions;

public class InvalidCountryException : Exception
{
    public InvalidCountryException() { }

    public InvalidCountryException(string message)
        : base(message) { }

    public InvalidCountryException(string message, Exception inner)
        : base(message, inner) { }
}

