using Api.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Pedidos.Infra.Persistence.Configuration;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("PedidoItens");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.PedidoId).IsRequired();

        builder.Property(i => i.ProdutoId).IsRequired();

        builder.Property(i => i.NomeProduto)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.PrecoUnitario)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Quantidade)
            .IsRequired();

        // N:1 
        builder.HasOne(i => i.Produto)
            .WithMany() 
            .HasForeignKey(i => i.ProdutoId)
            .HasConstraintName("FK_ItemPedido_Produto")
            .OnDelete(DeleteBehavior.Restrict);
    }
}