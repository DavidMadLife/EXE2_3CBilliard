// Exceptions/EmailAlreadyExistsException.cs
using System;

namespace EXE201_3CBilliard_Service.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
        {
        }

        public EmailAlreadyExistsException(string message)
            : base(message)
        {
        }

        public EmailAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
