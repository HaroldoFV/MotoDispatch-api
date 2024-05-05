using MotoDispatch.Domain.Enum;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.Common;

public class DeliveryDriverModelOutput(
    Guid id,
    string name,
    string cnpj,
    DateTime dateOfBirth,
    string cnhNumber,
    CNHType cnhType,
    string? cnhImagePath,
    DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string CNPJ { get; set; } = cnpj;
    public DateTime DateOfBirth { get; set; } = dateOfBirth;
    public string CNHNumber { get; set; } = cnhNumber;
    public CNHType CNHType { get; set; } = cnhType;
    public string? CNHImagePath { get; set; } = cnhImagePath;
    public DateTime CreatedAt { get; set; } = createdAt;


    public static DeliveryDriverModelOutput FromDeliveryDriver(DomainEntity.DeliveryDriver deliveryDriver)
    {
        return new(
            deliveryDriver.Id,
            deliveryDriver.Name,
            deliveryDriver.CNPJ,
            deliveryDriver.DateOfBirth,
            deliveryDriver.CNHNumber,
            deliveryDriver.CNHType,
            deliveryDriver.CNHImagePath,
            deliveryDriver.CreatedAt
        );
    }
}