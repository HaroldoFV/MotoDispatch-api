using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UploadDeliveryDriverCnhImage;

public class UploadDeliveryDriverCnhImageInput
    : IRequest<DeliveryDriverModelOutput>
{
    public Guid DeliveryDriverId { get; set; }
    public required string Extension { get; set; }
    public required Stream FileStream { get; set; }
    public required string ContentType { get; set; }
    public required string OriginalFileName { get; set; }
}