using MotoDispatch.Application.Interfaces;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Motorcycle.DeleteMotorcycle;

public class DeleteMotorcycle(
    IMotorcycleRepository motorcycleRepository,
    IRentalRepository rentalRepository,
    IUnitOfWork unitOfWork)
    : IDeleteMotorcycle
{
    public async Task Handle(DeleteMotorcycleInput request, CancellationToken cancellationToken)
    {
        var rentalExisted = await rentalRepository.GetRentalByMotorcycle(
            request.Id,
            cancellationToken);

        if (rentalExisted != null)
            throw new ApplicationException(
                "This motorcycle cannot be deleted because it is associated with at least one rental.");

        var category = await motorcycleRepository.Get(request.Id, cancellationToken);


        await motorcycleRepository.Delete(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }
}