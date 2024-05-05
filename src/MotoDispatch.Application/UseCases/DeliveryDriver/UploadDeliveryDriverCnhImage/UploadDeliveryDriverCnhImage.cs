using MotoDispatch.Application.Common;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UploadDeliveryDriverCnhImage;

public class UploadDeliveryDriverCnhImage(
    IDeliveryDriverRepository deliveryDriverRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService)
    : IUploadDeliveryDriverCnhImage
{
    public async Task<DeliveryDriverModelOutput> Handle(UploadDeliveryDriverCnhImageInput request,
        CancellationToken cancellationToken)
    {
        var allowedExtensions = new List<string> {"png", "bmp"};

        if (!allowedExtensions.Contains(request.Extension.ToLower()))
        {
            throw new ApplicationException("Invalid file format. Only PNG and BMP files are allowed.");
        }

        var deliveryDriver = await deliveryDriverRepository.Get(request.DeliveryDriverId, cancellationToken);

        var fileName = StorageFileName.Create(
            request.DeliveryDriverId,
            request.OriginalFileName,
            request.Extension);

        var uploadedFilePath = await storageService.Upload(
            fileName,
            request.FileStream,
            request.ContentType,
            cancellationToken
        );

        deliveryDriver.SetCNHImagePath(uploadedFilePath);

        await deliveryDriverRepository.Update(deliveryDriver, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return DeliveryDriverModelOutput.FromDeliveryDriver(deliveryDriver);
    }
}