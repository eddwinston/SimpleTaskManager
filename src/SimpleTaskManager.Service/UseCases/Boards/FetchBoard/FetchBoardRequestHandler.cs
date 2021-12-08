using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Service.Dtos;

namespace SimpleTaskManager.Service.UseCases.Boards.FetchBoard
{
    public class FetchBoardRequestValidator : AbstractValidator<FetchBoardRequest>
    {
        public FetchBoardRequestValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
        }
    }

    public class FetchBoardRequest : IRequest<BoardDto>
    {
        public FetchBoardRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class FetchBoardRequestHandler : IRequestHandler<FetchBoardRequest, BoardDto>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public FetchBoardRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(FetchBoardRequest request, CancellationToken cancellationToken)
        {
            var board = await _context
                .Boards
                .Include(b => b.Tasks)
                .Include(b => b.BoardUsers)
                    .ThenInclude(bu => bu.User)
                .SingleOrDefaultAsync(b => b.Id == request.Id);

            return _mapper.Map<BoardDto>(board);
        }
    }
}
