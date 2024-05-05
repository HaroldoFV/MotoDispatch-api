using MediatR;

namespace MotoDispatch.Application.UseCases.Motorcycle.DeleteMotorcycle;

public class DeleteMotorcycleInput(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}