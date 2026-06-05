public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);

    public int Start =>
        TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;

    public int End =>
        Math.Min(Page * PageSize, TotalCount);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}