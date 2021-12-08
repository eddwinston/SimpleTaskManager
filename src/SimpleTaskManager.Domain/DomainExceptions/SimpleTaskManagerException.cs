using System;

namespace SimpleTaskManager.Entities.DomainExceptions
{
    public class SimpleTaskManagerException : Exception
    {
        public SimpleTaskManagerException()
        {
        }

        public SimpleTaskManagerException(string message) : base(message)
        {
        }
    }
}
