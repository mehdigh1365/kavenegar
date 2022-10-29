using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Kavenegar.DataTransitLibrary.Common.Helper.Messages;
using kavenegar.DataTransitLibrary.Application.Core.DataTransit.Command.ImportDataTransit;
using kavenegar.DataTransitLibrary.Application.Core.DataTransit.Command.Create;

namespace kavenegar.DataTransitLibrary.Api.Controllers.DataTransits
{
    [ApiController]
    [Route("[Controller]")]
    public class DataTransitController : Controller
    {
        private readonly IMediator _mediator;
        public DataTransitController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [ProducesResponseType(typeof(ApiMessage), 200)]
        [HttpPost("ImportDataTransitFromExcel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportDataTransit([FromForm] ImportDataTransitToRedisFromExcelCommand createDataTransitToRedisCommand,
           CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createDataTransitToRedisCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return Ok();
        }

        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [ProducesResponseType(typeof(ApiMessage), 200)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateDataTransitCommand createDataTransitCommand,
           CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createDataTransitCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return Ok();
        }
    }
}
