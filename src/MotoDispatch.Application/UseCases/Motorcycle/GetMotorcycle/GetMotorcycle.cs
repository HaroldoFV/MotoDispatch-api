using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Domain.Repository;

namespace MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;

public class GetMotorcycle(
    IMotorcycleRepository motorcycleRepository)
    : IGetMotorcycle
{
    public async Task<MotorcycleModelOutput> Handle(
        GetMotorcycleInput request,
        CancellationToken cancellationToken)
    {
        var motorcycle = await motorcycleRepository.Get(request.Id, cancellationToken);
        return MotorcycleModelOutput.FromMotorcycle(motorcycle);
    }
}