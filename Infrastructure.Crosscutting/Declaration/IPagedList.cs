namespace Infrastructure.Crosscutting.Declaration
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
