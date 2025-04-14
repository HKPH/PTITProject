namespace BookStore.Domain.Entities;
public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int PageIndex { get; }
    public int PageSize { get; }

    public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public bool HasNextPage => PageIndex < TotalPages;
    public bool HasPreviousPage => PageIndex > 1;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}