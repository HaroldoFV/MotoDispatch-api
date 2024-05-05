using FluentValidation;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.GetDeliveryDriver;

public class GetDeliveryDriverInputValidator
    : AbstractValidator<GetDeliveryDriverInput>
{
    public GetDeliveryDriverInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}