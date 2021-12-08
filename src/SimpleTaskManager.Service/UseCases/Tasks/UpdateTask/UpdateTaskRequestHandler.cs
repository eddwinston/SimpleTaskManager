using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;
using SimpleTaskManager.Service.Dtos;

namespace SimpleTaskManager.Service.UseCases.Tasks.UpdateTask
{
    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Title).NotEmpty();
            RuleFor(r => r.Status).NotEmpty();
            RuleFor(r => r.BoardId).NotEmpty();
        }
    }

    public class UpdateTaskRequest : IRequest<TaskDto>
    {
        public UpdateTaskRequest(int id, string title, string status, int boardId)
        {
            Id = id;
            Title = title;
            Status = status;
            BoardId = boardId;
        }

        public int Id { get; }
        public string Title { get; }
        public string Status { get; }
        public int BoardId { get; }
    }

    public class UpdateTaskRequestHandler : IRequestHandler<UpdateTaskRequest, TaskDto>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public UpdateTaskRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (task == null)
            {
                throw new EntityNotFoundException("Task");
            }

            task.UpdateTitle(request.Title).UpdateStatus(request.Status);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }
    }
}
