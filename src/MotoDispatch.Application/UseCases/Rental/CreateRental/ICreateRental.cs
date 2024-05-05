using MediatR;
using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;

namespace MotoDispatch.Application.UseCases.Rental.CreateRental;

public interface ICreateRental
    : IRequestHandler<CreateRentalInput, RentalModelOutput>
{
}