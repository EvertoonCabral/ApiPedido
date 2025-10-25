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

        builder.Property(p => p.ClienteId)
               .IsRequired();

        builder.Property(p => p.DataAbertura)
               .HasColumnType("datetime2")
               .IsRequired();

        builder.Property(p => p.DataAtualizacao)
               .HasColumnType("datetime2")
               .IsRequired();

        builder.Property(p => p.Status)
               .IsRequired();


        builder.HasIndex(p => p.ClienteId);


        builder
            .HasMany(p => p.Produtos)
            .WithMany() // como Produtos não tem navegação de volta
            .UsingEntity<Dictionary<string, object>>(
                "PedidoProduto",
                right => right.HasOne<Produto>()
                              .WithMany()
                              .HasForeignKey("ProdutoId")
                              .HasConstraintName("FK_PedidoProduto_Produto")
                              .OnDelete(DeleteBehavior.Cascade),
                left  => left.HasOne<Pedido>()
                             .WithMany()
                             .HasForeignKey("PedidoId")
                             .HasConstraintName("FK_PedidoProduto_Pedido")
                             .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("PedidoProdutos");
                    join.HasKey("PedidoId", "ProdutoId");
                    join.HasIndex("ProdutoId");
                }
            );
    }
}