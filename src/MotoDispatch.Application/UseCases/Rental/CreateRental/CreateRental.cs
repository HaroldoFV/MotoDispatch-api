using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;
using MotoDispatch.Domain.Enum;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Rental.CreateRental;

public class CreateRental(
    IRentalRepository rentalRepository,
    IDeliveryDriverRepository deliveryDriverRepository,
    IMotorcycleRepository motorcycleRepository,
    IRentalPlanRepository rentalPlanRepository,
    IUnitOfWork unitOfWork)
    : ICreateRental
{
    public async Task<RentalModelOutput> Handle(CreateRentalInput request,
        CancellationToken cancellationToken)
    {
        var driver = await deliveryDriverRepository.Get(request.DeliveryDriverId, cancellationToken);
        await motorcycleRepository.Get(request.MotorcycleId, cancellationToken);

        if (driver.CNHType != CNHType.A)
            throw new ApplicationException("Only drivers with CNH type A can rent.");

        var deliveryDriverExisted = await rentalRepository.GetExistingRentalByMotorcycle(
            request.MotorcycleId,
            cancellationToken);

        if (deliveryDriverExisted != null)
            throw new ApplicationException("An existing rental already exists for this motorcycle.");

        var plan = await rentalPlanRepository.GetPlanAsync(request.Days, cancellationToken);

        if (plan == null)
            throw new ApplicationException("No valid rental plan found for the specified number of days.");

        rentalPlanRepository.AttachPlan(plan);

        var rental = new Domain.Entity.Rental(
            request.MotorcycleId,
            request.DeliveryDriverId,
            plan.Id,
            plan
        );

        await rentalRepository.Insert(rental, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return RentalModelOutput.FromRental(rental);
    }
}