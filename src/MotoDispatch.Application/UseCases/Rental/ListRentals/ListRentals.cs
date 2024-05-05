using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Rental.ListRentals;

public class ListRentals(IRentalRepository rentalRepository) : IListRentals
{
    public async Task<ListRentalsOutput> Handle(ListRentalsInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await rentalRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        return new ListRentalsOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(RentalModelOutput.FromRental)
                .ToList()
        );
    }
}