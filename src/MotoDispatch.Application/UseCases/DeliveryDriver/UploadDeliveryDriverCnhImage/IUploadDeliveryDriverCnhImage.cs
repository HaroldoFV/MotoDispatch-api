using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UploadDeliveryDriverCnhImage;

public interface IUploadDeliveryDriverCnhImage
    : IRequestHandler<UploadDeliveryDriverCnhImageInput, DeliveryDriverModelOutput>
{
}