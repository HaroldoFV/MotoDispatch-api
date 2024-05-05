using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.ListDeliveryDrivers;

public class ListDeliveryDrivers(IDeliveryDriverRepository deliveryDriverRepository) : IListDeliveryDrivers
{
    public async Task<ListDeliveryDriversOutput> Handle(ListDeliveryDriversInput request,
        CancellationToken cancellationToken)
    {
        var searchOutput = await deliveryDriverRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        return new ListDeliveryDriversOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(DeliveryDriverModelOutput.FromDeliveryDriver)
                .ToList()
        );
    }
}