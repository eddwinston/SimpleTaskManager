using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleTaskManager.Service.UseCases.Boards.CreateBoard;
using SimpleTaskManager.Service.UseCases.Boards.FetchBoard;
using SimpleTaskManager.Service.UseCases.Boards.GrantAccess;
using SimpleTaskManager.Service.UseCases.Boards.RevokeAccess;
using SimpleTaskManager.Api.Models;

namespace SimpleTaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/boards")]
    public class BoardsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(IMediator mediator, ILogger<BoardsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{boardId}")]
        public async Task<IActionResult> GetBoard(int boardId)
        {
            var request = new FetchBoardRequest(boardId);

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard(NewBoardModel model)
        {
            var request = new CreateBoardRequest(model.Name);

            var result = await _mediator.Send(request);

            return Created($"api/boards/{result.Id}", result);
        }

        [HttpPost]
        [Route("{boardId}/grant")]
        public async Task<IActionResult> GrantAccessToUser([FromRoute] int boardId, [FromBody]UserIdModel model)
        {
            var request = new GrantAccessRequest(boardId, model.UserId);

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("{boardId}/revoke")]
        public async Task<IActionResult> RevokeUserAccess([FromRoute] int boardId, [FromBody] UserIdModel model)
        {
            var request = new RevokeAccessRequest(boardId, model.UserId);

            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
