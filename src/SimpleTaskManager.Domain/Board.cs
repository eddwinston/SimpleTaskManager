using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace SimpleTaskManager.Domain
{
    public class Board : ModelBase
    {
        public Board()
        {
            Tasks = new Collection<TaskItem>();
            BoardUsers = new Collection<BoardUser>();
        }

        public Board(string name) : this()
        {
            Name = name;
        }

        public string Name { get; private set; }
        public virtual ICollection<TaskItem> Tasks { get; private set; }
        public virtual ICollection<BoardUser> BoardUsers { get; private set; }

        public void GrantUserAccess(User user)
        {
            if (BoardUsers.Any(bu => bu.User.Email == user.Email))
            {
                return;
            }

            BoardUsers.Add(new BoardUser
            {
                User = user
            });
        }

        public void RevokeUserAccess(User user)
        {
            RevokeUserAccess(user.Id);
        }

        public void RevokeUserAccess(int userId)
        {
            var boardUser = BoardUsers.SingleOrDefault(x => x.UserId == userId);
            BoardUsers.Remove(boardUser);
        }

        public TaskItem CreateNewTask(string title)
        {
            if (Tasks.Any(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new DuplicateNameException("Task with title already exist");
            }

            var task = TaskItem.New(title, this);
            Tasks.Add(task);

            return task;
        }

        public bool CanBeAccessedBy(User user)
        {
            return CanBeAccessedBy(user.Id);
        }

        public bool CanBeAccessedBy(int userId)
        {
            return BoardUsers.Any(user => user.UserId == userId);
        }
    }
}
