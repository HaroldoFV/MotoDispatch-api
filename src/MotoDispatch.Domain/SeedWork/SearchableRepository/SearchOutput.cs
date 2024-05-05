namespace MotoDispatch.Domain.SeedWork.SearchableRepository;

public class SearchOutput<TAggregate>(
    int currentPage,
    int perPage,
    int total,
    IReadOnlyList<TAggregate> items)
    where TAggregate : AggregateRoot
{
    public int CurrentPage { get; set; } = currentPage;
    public int PerPage { get; set; } = perPage;
    public int Total { get; set; } = total;
    public IReadOnlyList<TAggregate> Items { get; set; } = items;
}