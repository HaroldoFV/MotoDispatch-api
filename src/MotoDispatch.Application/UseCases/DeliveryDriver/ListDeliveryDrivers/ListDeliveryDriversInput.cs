using MediatR;
using MotoDispatch.Application.Common;
using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.ListDeliveryDrivers;

public class ListDeliveryDriversInput(
    int page = 1,
    int perPage = 15,
    string search = "",
    string sort = "",
    SearchOrder dir = SearchOrder.Asc)
    : PaginatedListInput(page, perPage, search, sort, dir), IRequest<ListDeliveryDriversOutput>
{
    public ListDeliveryDriversInput()
        : this(1, 15, "", "", SearchOrder.Asc)
    {
    }
}