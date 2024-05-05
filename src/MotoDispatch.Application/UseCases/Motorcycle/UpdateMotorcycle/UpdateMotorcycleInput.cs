using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

public class UpdateMotorcycleInput(
    Guid id,
    string licensePlate,
    string? model = null,
    int? year = null)
    : IRequest<MotorcycleModelOutput>
{
    public Guid Id { get; set; } = id;
    public int? Year { get; set; } = year;
    public string LicensePlate { get; set; } = licensePlate;
    public string? Model { get; set; } = model;
}