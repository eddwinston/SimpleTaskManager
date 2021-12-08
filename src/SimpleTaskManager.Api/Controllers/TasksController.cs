using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleTaskManager.Service.UseCases.Tasks.AssignUser;
using SimpleTaskManager.Service.UseCases.Tasks.CreateTask;
using SimpleTaskManager.Service.UseCases.Tasks.FetchBoardTasks;
using SimpleTaskManager.Api.Models;

namespace SimpleTaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTask(NewTaskModel model)
        {
            var request = new CreateTaskRequest(model.BoardId, model.Title);

            var result = await _mediator.Send(request);

            return Created($"api/tasks/{result.Id}", result);
        }

        [HttpGet]
        [Route("board/{boardId}")]
        public async Task<IActionResult> GetBoardTasks([FromRoute]int boardId)
        {
            var request = new FetchBoardTasksRequest(boardId);

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("{taskId}/assign")]
        public async Task<IActionResult> AssignUser([FromRoute] int taskId, [FromBody] UserIdModel userIdModel)
        {
            var request = new AssignUserRequest(taskId, userIdModel.UserId);

            await _mediator.Send(request);

            return Ok();
        }

        [HttpPost]
        [Route("{taskId}/unassign")]
        public async Task<IActionResult> UnAssignUser([FromRoute] int taskId, [FromBody] UserIdModel userIdModel)
        {
            var request = new UnAssignUserRequest(taskId, userIdModel.UserId);

            await _mediator.Send(request);

            return Ok();
        }
    }
}
