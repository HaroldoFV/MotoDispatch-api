namespace MotoDispatch.Application.UseCases.Rental.CompleteRental;

public class RentalCostDetailsOutput(Guid rentalId, decimal totalCost, DateTime actualEndDate)
{
    public Guid RentalId { get; set; } = rentalId;
    public decimal TotalCost { get; set; } = totalCost;
    public DateTime ActualEndDate { get; set; } = actualEndDate;
}