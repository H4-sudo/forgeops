namespace ForgeOps.Api.Shared;

public class BaseForgeOpsPagination<T>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
}
