using System;

public class InvalidDamageException : Exception
{
    public InvalidDamageException(string message)
        : base(message)
    {
    }
}