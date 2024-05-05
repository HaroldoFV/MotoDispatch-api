namespace MotoDispatch.Api.ApiModels.Motorcycle;

public class UpdateMotorcycleApiInput(
    string licensePlate,
    string? model = null,
    int? year = null)
{
    public int? Year { get; set; } = year;
    public string LicensePlate { get; set; } = licensePlate;
    public string? Model { get; set; } = model;
}