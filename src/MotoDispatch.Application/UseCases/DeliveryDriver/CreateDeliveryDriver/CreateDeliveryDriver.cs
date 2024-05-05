using MotoDispatch.Application.Exceptions;
using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Repository;
using DomainEntity = MotoDispatch.Domain.Entity;


namespace MotoDispatch.Application.UseCases.DeliveryDriver.CreateDeliveryDriver;

public class CreateDeliveryDriver(
    IDeliveryDriverRepository deliveryDriverRepository,
    IUnitOfWork unitOfWork)
    : ICreateDeliveryDriver
{
    public async Task<DeliveryDriverModelOutput> Handle(
        CreateDeliveryDriverInput request,
        CancellationToken cancellationToken)
    {
        NormalizeDriverInputs(request);

        var deliveryDriver = new DomainEntity.DeliveryDriver(
            request.Name,
            request.CNPJ,
            request.DateOfBirth,
            request.CNHNumber,
            request.CNHType
        );

        await ValidateCNHNumberUniqueness(request, cancellationToken);
        await ValidateCNPJUniqueness(request, cancellationToken);


        await deliveryDriverRepository.Insert(deliveryDriver, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return DeliveryDriverModelOutput.FromDeliveryDriver(deliveryDriver);
    }

    private void NormalizeDriverInputs(CreateDeliveryDriverInput request)
    {
        request.CNHNumber = request.CNHNumber.Trim()
            .Replace(".", "")
            .Replace("-", "");

        request.CNPJ = request.CNPJ.Trim()
            .Replace(".", "")
            .Replace("-", "")
            .Replace("/", "");
    }

    private async Task ValidateCNHNumberUniqueness(CreateDeliveryDriverInput request,
        CancellationToken cancellationToken)
    {
        var deliveryDriverExisted = await GetDeliveryDriverByCNHNumber(request.CNHNumber, cancellationToken);

        if (deliveryDriverExisted != null)
        {
            throw new DuplicateEntityException("CNH Number already exists.");
        }
    }

    private async Task ValidateCNPJUniqueness(CreateDeliveryDriverInput request, CancellationToken cancellationToken)
    {
        var deliveryDriverExisted = await GetDeliveryDriverByCNPJ(request.CNPJ, cancellationToken);

        if (deliveryDriverExisted != null)
        {
            throw new DuplicateEntityException("CNPJ already exists.");
        }
    }

    private async Task<DomainEntity.DeliveryDriver?> GetDeliveryDriverByCNHNumber(string cnhNumber,
        CancellationToken cancellationToken)
    {
        return await deliveryDriverRepository.GetByCNHNumberAsync(cnhNumber, cancellationToken);
    }

    private async Task<DomainEntity.DeliveryDriver?> GetDeliveryDriverByCNPJ(string cnpj,
        CancellationToken cancellationToken)
    {
        return await deliveryDriverRepository.GetByCNPJAsync(cnpj, cancellationToken);
    }
}