using System.Text.RegularExpressions;
using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.Validation;

namespace MotoDispatch.Domain.Entity;

public class Motorcycle : AggregateRoot
{
    public int Year { get; private set; }
    public string LicensePlate { get; private set; }
    public string Model { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Motorcycle(int year, string licensePlate, string model)
        : base()
    {
        Year = year;
        LicensePlate = licensePlate;
        Model = model;
        CreatedAt = DateTime.Now.ToUniversalTime();

        Validate();
    }

    public void UpdateLicensePlate(string licensePlate)
    {
        LicensePlate = licensePlate;
        Validate();
    }

    public void Update(string licensePlate, int? year = null, string? model = null)
    {
        LicensePlate = licensePlate;
        Year = year ?? Year;
        Model = model ?? Model;

        Validate();
    }


    #region Private Methods

    private void Validate()
    {
        DomainValidation.ValidateStringLength(Model, 2, 50, nameof(Model));
        ValidateLicensePlate(LicensePlate, nameof(LicensePlate));
        ValidateYear(Year, nameof(Year));
    }

    private static void ValidateLicensePlate(string licensePlate, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new EntityValidationException($"{nameof(licensePlate)} should not be empty or null");

        Regex oldPlateRegex = new Regex("^[A-Z]{3}-[0-9]{4}$");
        Regex newPlateRegex = new Regex("^[A-Z]{3}[0-9][A-Z][0-9]{2}$");

        if (!oldPlateRegex.IsMatch(licensePlate) && !newPlateRegex.IsMatch(licensePlate))
        {
            throw new EntityValidationException($"{fieldName} must be in the format AAA-9999 or ABC1D23.");
        }
    }

    private static void ValidateYear(int year, string fieldName)
    {
        if (year < 1900)
            throw new EntityValidationException($"{fieldName} should be greater than 1900.");

        if (year > DateTime.Now.Year)
            throw new EntityValidationException($"{fieldName} should be less than or equal to current year.");
    }

    #endregion
}