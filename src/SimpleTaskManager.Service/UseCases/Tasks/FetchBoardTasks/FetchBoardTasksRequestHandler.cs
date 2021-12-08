using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Service.Dtos;

namespace SimpleTaskManager.Service.UseCases.Tasks.FetchBoardTasks
{
    public class FetchBoardTasksRequestValidator : AbstractValidator<FetchBoardTasksRequest>
    {
        public FetchBoardTasksRequestValidator()
        {
            RuleFor(r => r.BoardId).NotEmpty();
        }
    }

    public class FetchBoardTasksRequest : IRequest<IEnumerable<TaskDto>>
    {
        public FetchBoardTasksRequest(int boardId)
        {
            BoardId = boardId;
        }

        public int BoardId { get; }
    }

    public class FetchBoardTasksRequestHandler : IRequestHandler<FetchBoardTasksRequest, IEnumerable<TaskDto>>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public FetchBoardTasksRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDto>> Handle(FetchBoardTasksRequest request, CancellationToken cancellationToken)
        {
            var tasks = await _context
                .Tasks
                .Where(task => task.BoardId == request.BoardId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
    }
}
