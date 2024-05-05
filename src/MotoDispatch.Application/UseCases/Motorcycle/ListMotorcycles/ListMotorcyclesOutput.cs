using MotoDispatch.Application.Common;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;

public class ListMotorcyclesOutput(
    int page,
    int perPage,
    int total,
    IReadOnlyList<MotorcycleModelOutput> items)
    : PaginatedListOutput<MotorcycleModelOutput>(page, perPage, total, items);