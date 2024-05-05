using MotoDispatch.Domain.SeedWork.SearchableRepository;

namespace MotoDispatch.Application.Common;

public abstract class PaginatedListInput(
    int page,
    int perPage,
    string search,
    string sort,
    SearchOrder dir)
{
    public int Page { get; set; } = page;
    public int PerPage { get; set; } = perPage;
    public string Search { get; set; } = search;
    public string Sort { get; set; } = sort;
    public SearchOrder Dir { get; set; } = dir;

    public SearchInput ToSearchInput()
        => new(Page, PerPage, Search, Sort, Dir);
}