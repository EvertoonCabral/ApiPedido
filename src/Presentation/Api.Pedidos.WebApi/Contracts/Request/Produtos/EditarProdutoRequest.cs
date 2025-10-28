public class EditarProdutoRequest
{
    public string Nome { get; set; } = default!;
    public string Descricao { get; set; } = default!;
    public decimal Preco { get; set; }
    public decimal PrecoVenda { get; set; }
}