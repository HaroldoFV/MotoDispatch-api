namespace MotoDispatch.Api.ApiModels.Rental;

public class CreateRentalApiInput
{
    public Guid DeliveryDriverId { get; set; }
    public Guid MotorcycleId { get; set; }
    public int Days { get; set; }
}