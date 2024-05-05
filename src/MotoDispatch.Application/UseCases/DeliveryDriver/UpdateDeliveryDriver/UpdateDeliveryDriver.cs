using MotoDispatch.Application.Interfaces;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.Repository;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UpdateDeliveryDriver;

public class UpdateDeliveryDriver(
    IDeliveryDriverRepository deliveryDriverRepository,
    IUnitOfWork unitOfWork)
    : IUpdateDeliveryDriver
{
    public async Task<DeliveryDriverModelOutput> Handle(
        UpdateDeliveryDriverInput request,
        CancellationToken cancellationToken)
    {
        var deliveryDriver = await deliveryDriverRepository.Get(request.Id, cancellationToken);

        NormalizeDriverInputs(request);

        await ValidateCNHNumberUniqueness(request, cancellationToken);
        await ValidateCNPJUniqueness(request, cancellationToken);

        deliveryDriver.Update(
            request.Name,
            request.CNHType,
            request.CNPJ,
            request.CNHNumber
        );
        await deliveryDriverRepository.Update(deliveryDriver, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return DeliveryDriverModelOutput.FromDeliveryDriver(deliveryDriver);
    }


    #region Private Methods

    private void NormalizeDriverInputs(UpdateDeliveryDriverInput request)
    {
        request.CNHNumber = request.CNHNumber?.Trim()
            .Replace(".", "")
            .Replace("-", "");

        request.CNPJ = request.CNPJ?.Trim()
            .Replace(".", "")
            .Replace("-", "")
            .Replace("/", "");
    }

    private async Task ValidateCNHNumberUniqueness(UpdateDeliveryDriverInput request,
        CancellationToken cancellationToken)
    {
        var deliveryDriverExisted = await GetDeliveryDriverByCNHNumber(request.CNHNumber, cancellationToken);

        if (deliveryDriverExisted != null && deliveryDriverExisted.Id != request.Id)
        {
            throw new EntityValidationException("CNHNumber already exists with a different delivery driver.");
        }
    }

    private async Task ValidateCNPJUniqueness(UpdateDeliveryDriverInput request, CancellationToken cancellationToken)
    {
        var deliveryDriverExisted = await GetDeliveryDriverByCNPJ(request.CNPJ, cancellationToken);

        if (deliveryDriverExisted != null && deliveryDriverExisted.Id != request.Id)
        {
            throw new EntityValidationException("CNPJ already exists with a different delivery driver.");
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

    #endregion
}