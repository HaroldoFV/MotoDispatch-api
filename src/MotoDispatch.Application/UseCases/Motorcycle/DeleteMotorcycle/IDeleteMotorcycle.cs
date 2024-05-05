using MediatR;

namespace MotoDispatch.Application.UseCases.Motorcycle.DeleteMotorcycle;

public interface IDeleteMotorcycle
    : IRequestHandler<DeleteMotorcycleInput>
{
}