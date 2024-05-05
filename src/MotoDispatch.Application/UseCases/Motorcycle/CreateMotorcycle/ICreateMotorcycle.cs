using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

public interface ICreateMotorcycle
    : IRequestHandler<CreateMotorcycleInput, MotorcycleModelOutput>
{
}