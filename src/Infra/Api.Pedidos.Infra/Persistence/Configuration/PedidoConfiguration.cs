using Api.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Pedidos.Infra.Persistence.Configuration;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ClienteId).IsRequired();

        builder.Property(p => p.DataAbertura)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(p => p.DataAtualizacao)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(p => p.Status).IsRequired();

        builder.HasIndex(p => p.ClienteId);

        // 1:N 
        builder.HasMany(p => p.Itens)
            .WithOne()
            .HasForeignKey(i => i.PedidoId)
            .HasConstraintName("FK_ItemPedido_Pedido")
            .OnDelete(DeleteBehavior.Cascade);
    }
}