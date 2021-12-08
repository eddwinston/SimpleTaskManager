using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleTaskManager.Domain
{
    public class User : ModelBase
    {
        public static User New(string name, string email) => new User(name, email);

        public User()
        {
            BoardUsers = new Collection<BoardUser>();
            Tasks = new Collection<TaskItem>();
        }

        private T Collection<T>()
        {
            throw new NotImplementedException();
        }

        public User(string name, string email) : this()
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public virtual ICollection<BoardUser> BoardUsers { get; private set; }
        public virtual ICollection<TaskItem> Tasks { get; private set; }
    }
}
