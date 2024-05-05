using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Enum;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UpdateDeliveryDriver;

public class UpdateDeliveryDriverInput(
    Guid id,
    string name,
    CNHType cnhType,
    string? cnpj = null,
    string? cnhNumber = null
)
    : IRequest<DeliveryDriverModelOutput>
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string? CNPJ { get; set; } = cnpj;
    public string? CNHNumber { get; set; } = cnhNumber;
    public CNHType CNHType { get; set; } = cnhType;
}