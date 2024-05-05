using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Enum;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.CreateDeliveryDriver;

public class CreateDeliveryDriverInput : IRequest<DeliveryDriverModelOutput>
{
    public required string Name { get; set; }
    public required string CNPJ { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string CNHNumber { get; set; }
    public CNHType CNHType { get; set; }
}