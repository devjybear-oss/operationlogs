namespace ChillPay.Core.Exceptions;

public class InvalidParameterException : ArgumentException
{
    public InvalidParameterException()
        : base()
    {
    }

    public InvalidParameterException(string message)
        : base(message)
    {
    }

    public static void ThrowIfNull(object argument, string message)
    {
        if (argument is null)
        {
            throw new InvalidParameterException(message);
        }
    }
}
