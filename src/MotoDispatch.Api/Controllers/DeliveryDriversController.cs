using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoDispatch.Api.ApiModels.DeliveryDriver;
using MotoDispatch.Api.ApiModels.Response;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Application.UseCases.DeliveryDriver.CreateDeliveryDriver;
using MotoDispatch.Application.UseCases.DeliveryDriver.GetDeliveryDriver;
using MotoDispatch.Application.UseCases.DeliveryDriver.ListDeliveryDrivers;
using MotoDispatch.Application.UseCases.DeliveryDriver.UpdateDeliveryDriver;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryDriversController(
        IMediator mediator)
        : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DeliveryDriverModelOutput>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateDeliveryDriverInput input,
            CancellationToken cancellationToken
        )
        {
            var output = await mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new {output.Id},
                new ApiResponse<DeliveryDriverModelOutput>(output)
            );
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<DeliveryDriverModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateDeliveryDriverApiInput apiInput,
            [FromRoute] Guid id,
            CancellationToken cancellationToken
        )
        {
            var input = new UpdateDeliveryDriverInput(
                id,
                apiInput.Name,
                apiInput.CNHType,
                apiInput.CNPJ,
                apiInput.CNHNumber
            );
            var output = await mediator.Send(input, cancellationToken);
            return Ok(new ApiResponse<DeliveryDriverModelOutput>(output));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<DeliveryDriverModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken
        )
        {
            var output = await mediator.Send(new GetDeliveryDriverInput(id), cancellationToken);
            return Ok(new ApiResponse<DeliveryDriverModelOutput>(output));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListDeliveryDriversOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int? page = null,
            [FromQuery(Name = "per_page")] int? perPage = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] SearchOrder? dir = null
        )
        {
            var input = new ListDeliveryDriversInput();
            if (page is not null) input.Page = page.Value;
            if (perPage is not null) input.PerPage = perPage.Value;
            if (!String.IsNullOrWhiteSpace(search)) input.Search = search;
            if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
            if (dir is not null) input.Dir = dir.Value;

            var output = await mediator.Send(input, cancellationToken);

            return Ok(
                new ApiResponseList<DeliveryDriverModelOutput>(output)
            );
        }

        [HttpPost("{id:guid}/upload-cnh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UploadCNHImage(
            [FromRoute] Guid id,
            [FromForm] UploadDeliveryDriverCnhImageApiInput apiInput,
            CancellationToken cancellationToken)
        {
            var input = apiInput.ToUploadCNHInput(id);
            var output = await mediator.Send(input, cancellationToken);
            return Ok(new ApiResponse<DeliveryDriverModelOutput>(output));
        }
    }
}