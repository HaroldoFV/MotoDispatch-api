using MotoDispatch.Application.Common;
using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;

namespace MotoDispatch.Application.UseCases.Rental.ListRentals;

public class ListRentalsOutput(
    int page,
    int perPage,
    int total,
    IReadOnlyList<RentalModelOutput> items)
    : PaginatedListOutput<RentalModelOutput>(page, perPage, total, items);