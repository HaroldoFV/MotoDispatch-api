using MediatR;

namespace MotoDispatch.Application.UseCases.Rental.CompleteRental;

public class CompleteRentalInput(
    Guid rentalId,
    DateTime actualReturnDate
) : IRequest<RentalCostDetailsOutput>
{
    public Guid RentalId { get; set; } = rentalId;
    public DateTime ActualReturnDate { get; set; } = actualReturnDate;
}