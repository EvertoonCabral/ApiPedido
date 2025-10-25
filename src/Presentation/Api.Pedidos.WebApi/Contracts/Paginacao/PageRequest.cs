namespace Api.Pedidos.WebApi.Contracts.Paginacao;

public class PageRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 50;

    public void Normalize(int maxPageSize = 100)
    {
        if (Page <= 0) Page = 1;
        if (PageSize <= 0) PageSize = 20;
        if (PageSize > maxPageSize) PageSize = maxPageSize;
    }
}