using System;

namespace SimpleTaskManager.Entities.DomainExceptions
{
    public class InputValidationException : Exception
    {
        public InputValidationException()
        {
        }

        public InputValidationException(string message) : base(message)
        {
        }
    }
}
