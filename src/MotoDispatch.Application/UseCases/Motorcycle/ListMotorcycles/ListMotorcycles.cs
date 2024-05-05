using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;

public class ListMotorcycles(IMotorcycleRepository motorcycleRepository) : IListMotorcycles
{
    public async Task<ListMotorcyclesOutput> Handle(ListMotorcyclesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await motorcycleRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        return new ListMotorcyclesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(MotorcycleModelOutput.FromMotorcycle)
                .ToList()
        );
    }
}