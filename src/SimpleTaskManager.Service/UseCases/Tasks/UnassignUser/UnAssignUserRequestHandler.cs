using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;

namespace SimpleTaskManager.Service.UseCases.Tasks.AssignUser
{
    public class UnAssignUserRequestValidator : AbstractValidator<UnAssignUserRequest>
    {
        public UnAssignUserRequestValidator()
        {
            RuleFor(r => r.TaskId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }

    public class UnAssignUserRequest : IRequest
    {
        public UnAssignUserRequest(int taskId, int userId)
        {
            TaskId = taskId;
            UserId = userId;
        }

        public int TaskId { get; }
        public int UserId { get; }
    }

    public class UnAssignUserRequestHandler : IRequestHandler<UnAssignUserRequest>
    {
        private readonly SimpleTaskManagerContext _context;

        public UnAssignUserRequestHandler(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UnAssignUserRequest request, CancellationToken cancellationToken)
        {
            var task = await _context
                .Tasks
                .Include(t => t.Board)
                .ThenInclude(b => b.BoardUsers)
                .SingleOrDefaultAsync(x => x.Id == request.TaskId);
            if (task == null)
            {
                throw new EntityNotFoundException("Task");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException("User");
            }

            task.UnAssignUser(user);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
