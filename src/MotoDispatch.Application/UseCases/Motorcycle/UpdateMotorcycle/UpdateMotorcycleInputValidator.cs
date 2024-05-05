using FluentValidation;

namespace MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

public class UpdateMotorcycleInputValidator
    : AbstractValidator<UpdateMotorcycleInput>
{
    public UpdateMotorcycleInputValidator()
        => RuleFor(x => x.Id).NotEmpty();
}