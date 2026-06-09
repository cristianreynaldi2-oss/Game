using System;

/// Custom Exception untuk validasi input tidak valid pada game.
public class InvalidInputException : Exception
{
    public InvalidInputException(string message) : base(message) { }
}