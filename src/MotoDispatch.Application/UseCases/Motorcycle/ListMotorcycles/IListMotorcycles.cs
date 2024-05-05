using MediatR;

namespace MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;

public interface IListMotorcycles
    : IRequestHandler<ListMotorcyclesInput, ListMotorcyclesOutput>
{
}