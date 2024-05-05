namespace MotoDispatch.Application.Common;

public abstract class PaginatedListOutput<TOutputItem>(
    int page,
    int perPage,
    int total,
    IReadOnlyList<TOutputItem> items)
{
    public int Page { get; set; } = page;
    public int PerPage { get; set; } = perPage;
    public int Total { get; set; } = total;
    public IReadOnlyList<TOutputItem> Items { get; set; } = items;
}