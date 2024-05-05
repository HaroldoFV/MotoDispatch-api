using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoDispatch.Api.ApiModels.Rental;
using MotoDispatch.Api.ApiModels.Response;
using MotoDispatch.Application.UseCases.Rental.CompleteRental;
using MotoDispatch.Application.UseCases.Rental.CreateRental;
using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;
using MotoDispatch.Application.UseCases.Rental.ListRentals;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController(
    IMediator mediator)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RentalModelOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateRentalApiInput input)
    {
        var command = new CreateRentalInput(input.DeliveryDriverId, input.MotorcycleId, input.Days);
        var output = await mediator.Send(command);

        return CreatedAtAction(
            nameof(Create),
            new {id = output.Id},
            new ApiResponse<RentalModelOutput>(output));
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteRental(Guid id, [FromBody] CompleteRentalApiInput input)
    {
        var command = new CompleteRentalInput(id, input.ActualReturnDate);
        var output = await mediator.Send(command);
        return Ok(new ApiResponse<RentalCostDetailsOutput>(output));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListRentalsOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery(Name = "deliveryDriverId")] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListRentalsInput();
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (!String.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var output = await mediator.Send(input, cancellationToken);

        return Ok(
            new ApiResponseList<RentalModelOutput>(output)
        );
    }
}