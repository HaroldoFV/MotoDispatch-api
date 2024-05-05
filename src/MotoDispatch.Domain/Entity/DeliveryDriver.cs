using DocumentValidator;
using MotoDispatch.Domain.Enum;
using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.Validation;

namespace MotoDispatch.Domain.Entity;

public class DeliveryDriver : AggregateRoot
{
    public string Name { get; private set; }
    public string CNPJ { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CNHNumber { get; private set; }
    public CNHType CNHType { get; private set; }
    public string? CNHImagePath { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DeliveryDriver()
    {
    }

    public DeliveryDriver(
        string name,
        string cnpj,
        DateTime dateOfBirth,
        string cnhNumber,
        CNHType cnhType)
        : base()
    {
        Name = name;
        CNPJ = cnpj;
        DateOfBirth = dateOfBirth;
        CNHNumber = cnhNumber;
        CNHType = cnhType;
        CreatedAt = DateTime.Now.ToUniversalTime();

        Validate();
    }

    public void Update(
        string name,
        CNHType cnhType,
        string? cnpj = null,
        string? cnhNumber = null
    )
    {
        Name = name;
        CNPJ = cnpj ?? CNPJ;
        CNHNumber = cnhNumber ?? CNHNumber;
        CNHType = cnhType;

        Validate();
    }

    public void SetCNHImagePath(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || string.IsNullOrWhiteSpace(imagePath))
        {
            throw new EntityValidationException("Invalid image path.");
        }

        CNHImagePath = imagePath;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.ValidateStringLength(Name, 2, 100, nameof(Name));

        if (!CnpjValidation.Validate(CNPJ))
            throw new EntityValidationException($"Invalid CNPJ {CNPJ}. Please verify.");

        if (!CnhValidation.Validate(CNHNumber))
            throw new EntityValidationException($"Invalid CNH number '{CNHNumber}'. Please verify.");

        if (!System.Enum.IsDefined(typeof(CNHType), CNHType))
        {
            throw new EntityValidationException($"Invalid CNHType value: {CNHType}");
        }
    }
}