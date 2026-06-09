using System;

public class InputTidakValidException : Exception
{
    public InputTidakValidException(string message) : base(message) { }
}