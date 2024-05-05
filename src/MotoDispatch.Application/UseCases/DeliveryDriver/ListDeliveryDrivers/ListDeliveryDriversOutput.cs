using MotoDispatch.Application.Common;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.ListDeliveryDrivers;

public class ListDeliveryDriversOutput(
    int page,
    int perPage,
    int total,
    IReadOnlyList<DeliveryDriverModelOutput> items)
    : PaginatedListOutput<DeliveryDriverModelOutput>(page, perPage, total, items);