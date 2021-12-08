using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleTaskManager.Service.UseCases.Users.CreateUser;
using SimpleTaskManager.Api.Models;

namespace SimpleTaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]NewUserModel user)
        {
            var request = new CreateUserRequest(user.Name, user.Email);

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("batchCreate")]
        public async Task<IActionResult> CreateUser([FromBody] IEnumerable<NewUserModel> users)
        {
            var batch = users.Select(user => new CreateUserRequest(user.Name, user.Email));
            var request = new CreateUserBatchRequest(batch);

            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
