using MotoDispatch.Domain.Enum;

namespace MotoDispatch.Api.ApiModels.DeliveryDriver;

public class UpdateDeliveryDriverApiInput
{
    public required string Name { get; set; }
    public string? CNPJ { get; set; }
    public string? CNHNumber { get; set; }
    public CNHType CNHType { get; set; }
}