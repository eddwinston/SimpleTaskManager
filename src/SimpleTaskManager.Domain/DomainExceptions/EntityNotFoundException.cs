using System;

namespace SimpleTaskManager.Entities.DomainExceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string entityName) : base($"{entityName} not found")
        {
        }
    }
}
