using MotoDispatch.Application.Exceptions;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Domain.Repository;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

public class CreateMotorcycle(
    IMotorcycleRepository motorcycleRepository,
    IUnitOfWork unitOfWork)
    : ICreateMotorcycle
{
    public async Task<MotorcycleModelOutput> Handle(
        CreateMotorcycleInput input,
        CancellationToken cancellationToken)
    {
        var motorcycle = new DomainEntity.Motorcycle(
            input.Year,
            input.LicensePlate,
            input.Model
        );
        await ValidateLicensePlate(input, cancellationToken);

        await motorcycleRepository.Insert(motorcycle, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return MotorcycleModelOutput.FromMotorcycle(motorcycle);
    }

    private async Task ValidateLicensePlate(CreateMotorcycleInput input, CancellationToken cancellationToken)
    {
        var motorcycleExisted = await motorcycleRepository.GetByLicensePlate(input.LicensePlate, cancellationToken);

        if (motorcycleExisted != null)
            throw new DuplicateEntityException("License plate already exists.");
    }
}