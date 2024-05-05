using MotoDispatch.Application.Exceptions;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

public class UpdateMotorcycle(
    IMotorcycleRepository motorcycleRepository,
    IUnitOfWork unitOfWork)
    : IUpdateMotorcycle
{
    public async Task<MotorcycleModelOutput> Handle(UpdateMotorcycleInput request, CancellationToken cancellationToken)
    {
        var motorcycle = await motorcycleRepository.Get(request.Id, cancellationToken);

        await ValidateLicensePlate(request, cancellationToken);
        motorcycle.Update(request.LicensePlate, request.Year, request.Model);

        await motorcycleRepository.Update(motorcycle, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        return MotorcycleModelOutput.FromMotorcycle(motorcycle);
    }

    private async Task ValidateLicensePlate(UpdateMotorcycleInput input, CancellationToken cancellationToken)
    {
        var motorcycleExisted = await motorcycleRepository.GetByLicensePlate(input.LicensePlate, cancellationToken);

        if (motorcycleExisted != null && motorcycleExisted.Id != input.Id)
            throw new DuplicateEntityException("Duplicate license plate.");
    }
}