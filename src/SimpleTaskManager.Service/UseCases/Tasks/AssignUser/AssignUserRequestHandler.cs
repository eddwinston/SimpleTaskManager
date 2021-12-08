using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;

namespace SimpleTaskManager.Service.UseCases.Tasks.AssignUser
{
    public class AssignUserRequestValidator : AbstractValidator<AssignUserRequest>
    {
        public AssignUserRequestValidator()
        {
            RuleFor(r => r.TaskId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }

    public class AssignUserRequest : IRequest
    {
        public AssignUserRequest(int taskId, int userId)
        {
            TaskId = taskId;
            UserId = userId;
        }

        public int TaskId { get; }
        public int UserId { get; }
    }

    public class AssignUserRequestHandler : IRequestHandler<AssignUserRequest>
    {
        private readonly SimpleTaskManagerContext _context;

        public AssignUserRequestHandler(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AssignUserRequest request, CancellationToken cancellationToken)
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

            task.AssignToUser(user);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
