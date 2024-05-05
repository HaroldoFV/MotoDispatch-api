using MotoDispatch.Application.Interfaces;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Rental.CompleteRental;

public class CompleteRental(
    IRentalRepository rentalRepository,
    IRentalPlanRepository rentalPlanRepository,
    IUnitOfWork unitOfWork
) : ICompleteRental
{
    public async Task<RentalCostDetailsOutput> Handle(
        CompleteRentalInput request,
        CancellationToken cancellationToken
    )
    {
        request.ActualReturnDate = request.ActualReturnDate.ToUniversalTime();

        var rental = await rentalRepository.Get(request.RentalId, cancellationToken);
        rental.CompleteRental(request.ActualReturnDate);

        var rentedDays = (request.ActualReturnDate.Date - rental.StartDate.Date).Days + 1;

        var plannedDays = (rental.EndDate.Date - rental.StartDate.Date).Days + 1;

        var totalCost = rental.RentalPlan.CalculateTotalCost(
            rentedDays, plannedDays
        );

        rentalPlanRepository.AttachPlan(rental.RentalPlan);

        await rentalRepository.Update(rental, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return new RentalCostDetailsOutput(
            rental.Id,
            totalCost,
            request.ActualReturnDate
        );
    }
}