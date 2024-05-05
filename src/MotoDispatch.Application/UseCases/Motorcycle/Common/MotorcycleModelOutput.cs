using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.Application.UseCases.Motorcycle.Common;

public class MotorcycleModelOutput(
    Guid id,
    int year,
    string licensePlate,
    string model,
    DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public int Year { get; set; } = year;
    public string LicensePlate { get; set; } = licensePlate;
    public string Model { get; set; } = model;
    public DateTime CreatedAt { get; set; } = createdAt;

    public static MotorcycleModelOutput FromMotorcycle(DomainEntity.Motorcycle motorcycle)
    {
        return new(
            motorcycle.Id,
            motorcycle.Year,
            motorcycle.LicensePlate,
            motorcycle.Model,
            motorcycle.CreatedAt
        );
    }
}