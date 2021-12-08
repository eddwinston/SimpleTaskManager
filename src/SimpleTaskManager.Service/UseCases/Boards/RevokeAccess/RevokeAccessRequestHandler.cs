using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;

namespace SimpleTaskManager.Service.UseCases.Boards.RevokeAccess
{
    public class RevokeAccessRequestValidator : AbstractValidator<RevokeAccessRequest>
    {
        public RevokeAccessRequestValidator()
        {
            RuleFor(r => r.BoardId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }

    public class RevokeAccessRequest : IRequest
    {
        public RevokeAccessRequest(int boardId, int userId)
        {
            BoardId = boardId;
            UserId = userId;
        }

        public int BoardId { get; }
        public int UserId { get; }
    }

    public class RevokeAccessRequestHandler : IRequestHandler<RevokeAccessRequest>
    {
        private readonly SimpleTaskManagerContext _context;

        public RevokeAccessRequestHandler(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RevokeAccessRequest request, CancellationToken cancellationToken)
        {
            var board = await _context
                .Boards
                .Include(board => board.BoardUsers)
                .SingleOrDefaultAsync(board => board.Id == request.BoardId);
            if (board == null)
            {
                throw new Exception("Board not found");
            }

            if (!board.CanBeAccessedBy(request.UserId))
            {
                return Unit.Value;
            }

            board.RevokeUserAccess(request.UserId);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
