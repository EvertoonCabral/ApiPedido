namespace Api.Pedidos.Application.Common.Dtos;

public class PageResponse<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public long TotalItems { get; set; }
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
}