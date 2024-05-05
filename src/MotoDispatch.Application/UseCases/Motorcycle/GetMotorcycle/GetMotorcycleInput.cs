using MediatR;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

public class GetMotorcycleInput(Guid id) : IRequest<MotorcycleModelOutput>
{
    public Guid Id { get; set; } = id;
}