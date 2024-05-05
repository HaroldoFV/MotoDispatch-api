using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

public interface IUpdateMotorcycle
    : IRequestHandler<UpdateMotorcycleInput, MotorcycleModelOutput>
{
}