using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.SeedWork;

namespace MotoDispatch.Domain.Entity;

public class RentalPlan : AggregateRoot
{
    public int Days { get; private set; }
    public decimal DailyRate { get; private set; }
    public decimal PenaltyRate { get; private set; }
    public decimal FixedAdditionalRate { get; private set; }

    private RentalPlan() { }

    public RentalPlan(
        int days,
        decimal dailyRate,
        decimal penaltyRate,
        decimal fixedAdditionalRate = 50.00m
    )
    {
        Days = days;
        DailyRate = dailyRate;
        PenaltyRate = penaltyRate;
        FixedAdditionalRate = fixedAdditionalRate;

        Validate();
    }

    public decimal CalculateTotalCost(int rentedDays, int plannedDays)
    {
        if (rentedDays <= plannedDays)
        {
            return CalculateCostForOnOrBeforeTime(rentedDays, plannedDays);
        }

        return CalculateCostForOverTime(rentedDays, plannedDays);
    }

    private decimal CalculateCostForOnOrBeforeTime(int rentedDays, int plannedDays)
    {
        var baseCost = rentedDays * DailyRate;

        if (rentedDays < plannedDays)
        {
            var unusedDays = plannedDays - rentedDays;
            var penalty = unusedDays * DailyRate * PenaltyRate;
            return baseCost + penalty;
        }

        return baseCost;
    }

    private decimal CalculateCostForOverTime(int rentedDays, int plannedDays)
    {
        var baseCost = plannedDays * DailyRate;
        var additionalDays = rentedDays - plannedDays;
        var additionalCost = additionalDays * FixedAdditionalRate;
        return baseCost + additionalCost;
    }

    private void Validate()
    {
        if (Days <= 0)
        {
            throw new EntityValidationException("Days must be greater than zero.");
        }

        if (DailyRate <= 0)
        {
            throw new EntityValidationException("DailyRate must be greater than zero.");
        }

        if (PenaltyRate < 0)
        {
            throw new EntityValidationException("PenaltyRate must not be negative.");
        }

        if (FixedAdditionalRate < 0)
        {
            throw new EntityValidationException("FixedAdditionalRate must not be negative.");
        }
    }
}