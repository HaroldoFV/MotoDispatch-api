using MediatR;
using MotoDispatch.Application.Common;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Application.UseCases.Rental.ListRentals;

public class ListRentalsInput(
    int page = 1,
    int perPage = 15,
    string search = "",
    string sort = "",
    SearchOrder dir = SearchOrder.Asc)
    : PaginatedListInput(page, perPage, search, sort, dir), IRequest<ListRentalsOutput>
{
    public ListRentalsInput()
        : this(1, 15, "", "", SearchOrder.Asc)
    {
    }
}