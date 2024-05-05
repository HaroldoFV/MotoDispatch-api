namespace MotoDispatch.Application.UseCases.Rental.CreateRental;

public class RentalPlanDto
{
    public RentalPlanDto(
        int days,
        decimal dailyRate,
        decimal penaltyRate,
        decimal additionalDailyRate
    )
    {
        Days = days;
        DailyRate = dailyRate;
        PenaltyRate = penaltyRate;
        AdditionalDailyRate = additionalDailyRate;
    }

    public int Days { get; set; }
    public decimal DailyRate { get; set; }
    public decimal PenaltyRate { get; set; } 
    public decimal AdditionalDailyRate { get; set; }
}