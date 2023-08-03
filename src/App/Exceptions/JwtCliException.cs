namespace App.Exceptions;

public class JwtCliException : Exception
{
    public JwtCliException(string message) : base(message)
    {
    }
}