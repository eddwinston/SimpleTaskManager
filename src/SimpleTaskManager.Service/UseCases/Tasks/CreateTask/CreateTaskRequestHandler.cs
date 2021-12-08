using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using AutoMapper;
using SimpleTaskManager.Service.Dtos;
using FluentValidation;

namespace SimpleTaskManager.Service.UseCases.Tasks.CreateTask
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(r => r.BoardId).NotEmpty();
            RuleFor(r => r.Title).NotEmpty();
        }
    }

    public class CreateTaskRequest : IRequest<TaskDto>
    {
        public CreateTaskRequest(int boardId, string title)
        {
            BoardId = boardId;
            Title = title;
        }

        public int BoardId { get; }
        public string Title { get; }
    }

    public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, TaskDto>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public CreateTaskRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var board = await _context
                .Boards
                .Include(x => x.Tasks)
                .SingleOrDefaultAsync(x => x.Id == request.BoardId);

            var task = board.CreateNewTask(request.Title);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }
    }
}
