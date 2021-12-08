using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;

namespace SimpleTaskManager.Service.UseCases.Boards.GrantAccess
{
    public class GrantAccessRequestValidator : AbstractValidator<GrantAccessRequest>
    {
        public GrantAccessRequestValidator()
        {
            RuleFor(r => r.BoardId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }

    public class GrantAccessRequest : IRequest
    {
        public GrantAccessRequest(int boardId, int userId)
        {
            BoardId = boardId;
            UserId = userId;
        }

        public int BoardId { get; set; }
        public int UserId { get; set; }
    }

    public class GrantAccessRequestHandler : IRequestHandler<GrantAccessRequest>
    {
        private readonly SimpleTaskManagerContext _context;

        public GrantAccessRequestHandler(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(GrantAccessRequest request, CancellationToken cancellationToken)
        {
            var board = await _context
                .Boards
                .Include(board => board.BoardUsers)
                .SingleOrDefaultAsync(board => board.Id == request.BoardId);
            if (board == null)
            {
                throw new EntityNotFoundException("Board");
            }

            var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException("User");
            }

            if (board.CanBeAccessedBy(user))
            {
                return Unit.Value;
            }

            board.GrantUserAccess(user);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
