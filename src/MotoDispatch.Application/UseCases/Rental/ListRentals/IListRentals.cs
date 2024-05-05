using MediatR;

namespace MotoDispatch.Application.UseCases.Rental.ListRentals;

public interface IListRentals
    : IRequestHandler<ListRentalsInput, ListRentalsOutput>
{
}