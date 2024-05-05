using Microsoft.AspNetCore.Mvc;
using MotoDispatch.Application.UseCases.DeliveryDriver.UploadDeliveryDriverCnhImage;
using MotoDispatch.Domain.Exception;

namespace MotoDispatch.Api.ApiModels.DeliveryDriver;

public class UploadDeliveryDriverCnhImageApiInput
{
    [FromForm(Name = "cnh_file")] public required IFormFile CNHFile { get; set; }

    public UploadDeliveryDriverCnhImageInput ToUploadCNHInput(Guid deliveryDriverId)
    {
        ValidateCNHFileFormat();

        return new UploadDeliveryDriverCnhImageInput
        {
            DeliveryDriverId = deliveryDriverId,
            FileStream = CNHFile.OpenReadStream(),
            ContentType = CNHFile.ContentType,
            Extension = Path.GetExtension(CNHFile.FileName).TrimStart('.'),
            OriginalFileName = CNHFile.FileName
        };
    }

    private void ValidateCNHFileFormat()
    {
        var allowedExtensions = new HashSet<string> {"png", "bmp"};
        var fileExtension = Path.GetExtension(CNHFile.FileName).ToLowerInvariant().TrimStart('.');

        if (!allowedExtensions.Contains(fileExtension))
            throw new EntityValidationException("Invalid file format. Only PNG and BMP files are allowed.");
    }
}