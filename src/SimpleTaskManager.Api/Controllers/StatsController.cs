using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleTaskManager.Service.UseCases.Stats;

namespace SimpleTaskManager.Api.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics([FromQuery]string name)
        {
            var request = new StatsRequest(name);

            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
