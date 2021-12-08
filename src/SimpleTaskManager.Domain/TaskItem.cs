using System;

namespace SimpleTaskManager.Domain
{
    public class TaskItem : ModelBase
    {
        public static TaskItem New(string title, Board board)
        {
            return new TaskItem
            {
                Title = title,
                Board = board,
                BoardId = board.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public string Title { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset CompletedAt { get; private set; }
        public TaskStatus Status { get; private set; }
        public virtual User User { get; private set; }
        public virtual int? UserId { get; private set; }
        public virtual Board Board { get; private set; }
        public virtual int BoardId { get; private set; }

        public TaskItem UpdateTitle(string title)
        {
            Title = title;

            return this;
        }

        public TaskItem UpdateStatus(string statusString)
        {
            var status = Enum.Parse<TaskStatus>(statusString);
            switch (status)
            {
                case TaskStatus.Completed:
                    MarkAsComplete();
                    break;

                case TaskStatus.InProgress:
                    MarkAsInProgress();
                    break;
            }

            return this;
        }

        public void AssignToUser(User user)
        {
            if (!Board.CanBeAccessedBy(user))
            {
                throw new UnauthorizedAccessException("Only users in task board can be assigned to task");
            }

            User = user;
            UserId = user.Id;
        }

        public void UnAssignUser(User user)
        {
            if (UserId != user.Id)
            {
                throw new InvalidOperationException("Invalid user");
            }

            User = null;
            UserId = null;
        }

        public void MarkAsInProgress()
        {
            Status = TaskStatus.InProgress;
        }

        public void MarkAsComplete()
        {
            Status = TaskStatus.Completed;
            User = null;
            UserId = null;
            CompletedAt = DateTimeOffset.UtcNow;
        }
    }
}
