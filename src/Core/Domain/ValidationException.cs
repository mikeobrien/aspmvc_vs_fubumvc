using System;

namespace Core.Domain
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) {}
    }
}
