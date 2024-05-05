using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

public interface IGetMotorcycle :
    IRequestHandler<GetMotorcycleInput, MotorcycleModelOutput>
{
}