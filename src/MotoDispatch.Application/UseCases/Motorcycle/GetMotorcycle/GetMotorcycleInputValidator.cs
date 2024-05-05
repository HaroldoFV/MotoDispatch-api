using FluentValidation;

namespace MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

public class GetMotorcycleInputValidator
    : AbstractValidator<GetMotorcycleInput>
{
    public GetMotorcycleInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}