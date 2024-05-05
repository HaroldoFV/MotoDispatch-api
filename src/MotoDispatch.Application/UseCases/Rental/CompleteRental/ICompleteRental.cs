using MediatR;

namespace MotoDispatch.Application.UseCases.Rental.CompleteRental;

public interface ICompleteRental
    : IRequestHandler<CompleteRentalInput, RentalCostDetailsOutput>
{
}