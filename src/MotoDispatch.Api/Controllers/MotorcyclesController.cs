using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoDispatch.Api.ApiModels.Motorcycle;
using MotoDispatch.Api.ApiModels.Response;
using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;
using MotoDispatch.Application.UseCases.Motorcycle.DeleteMotorcycle;
using MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;
using MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;
using MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcyclesController(
        IMediator mediator)
        : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MotorcycleModelOutput>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateMotorcycleInput input,
            CancellationToken cancellationToken
        )
        {
            var output = await mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                nameof(Create),
                new {output.Id},
                new ApiResponse<MotorcycleModelOutput>(output)
            );
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<MotorcycleModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateMotorcycleApiInput apiInput,
            [FromRoute] Guid id,
            CancellationToken cancellationToken
        )
        {
            var input = new UpdateMotorcycleInput(
                id,
                apiInput.LicensePlate,
                apiInput.Model,
                apiInput.Year
            );
            var output = await mediator.Send(input, cancellationToken);
            return Ok(new ApiResponse<MotorcycleModelOutput>(output));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<MotorcycleModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken
        )
        {
            var output = await mediator.Send(new GetMotorcycleInput(id), cancellationToken);
            return Ok(new ApiResponse<MotorcycleModelOutput>(output));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken
        )
        {
            await mediator.Send(new DeleteMotorcycleInput(id), cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListMotorcyclesOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken,
            [FromQuery] int? page = null,
            [FromQuery(Name = "per_page")] int? perPage = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] SearchOrder? dir = null
        )
        {
            var input = new ListMotorcyclesInput();
            if (page is not null) input.Page = page.Value;
            if (perPage is not null) input.PerPage = perPage.Value;
            if (!String.IsNullOrWhiteSpace(search)) input.Search = search;
            if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
            if (dir is not null) input.Dir = dir.Value;

            var output = await mediator.Send(input, cancellationToken);

            return Ok(
                new ApiResponseList<MotorcycleModelOutput>(output)
            );
        }
    }
}