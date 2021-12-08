using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Service.Dtos;

namespace SimpleTaskManager.Service.UseCases.Boards.CreateBoard
{
    public class CreateBoardRequestValidator : AbstractValidator<CreateBoardRequest>
    {
        public CreateBoardRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }

    public class CreateBoardRequest : IRequest<BoardDto>
    {
        public CreateBoardRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class CreateBoardRequestHandler : IRequestHandler<CreateBoardRequest, BoardDto>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public CreateBoardRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(CreateBoardRequest request, CancellationToken cancellationToken)
        {
            var board = _mapper.Map<Board>(request);

            var boardNameExist = _context.Boards.Any(b => b.Name == request.Name);
            if (boardNameExist)
            {
                throw new DuplicateNameException("Board name should be unique");
            }

            _context.Boards.Add(board);

            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDto>(board);
        }
    }
}
