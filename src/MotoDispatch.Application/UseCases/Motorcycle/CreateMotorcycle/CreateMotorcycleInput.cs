using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

public class CreateMotorcycleInput(int year, string licensePlate, string model)
    : IRequest<MotorcycleModelOutput>
{
    public int Year { get; set; } = year;
    public string LicensePlate { get; set; } = licensePlate;
    public string Model { get; set; } = model;
}