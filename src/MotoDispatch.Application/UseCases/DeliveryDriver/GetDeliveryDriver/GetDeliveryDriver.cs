using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.GetDeliveryDriver;

public class GetDeliveryDriver(
    IDeliveryDriverRepository deliveryDriverRepository)
    : IGetDeliveryDriver
{
    public async Task<DeliveryDriverModelOutput> Handle(
        GetDeliveryDriverInput request,
        CancellationToken cancellationToken)
    {
        var deliveryDriver = await deliveryDriverRepository.Get(request.Id, cancellationToken);
        return DeliveryDriverModelOutput.FromDeliveryDriver(deliveryDriver);
    }
}