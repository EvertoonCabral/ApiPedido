using Api.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Pedidos.Infra.Configuration;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Descricao)
            .HasMaxLength(1000);

        builder.Property(p => p.Preco)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PrecoVenda)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.IsAtivo)
            .HasDefaultValue(true);

        builder.Property(p => p.DataCadastro)
            .HasColumnType("datetime2");

        builder.Property(p => p.DataAtualizacao)
            .HasColumnType("datetime2");

        builder.HasIndex(p => new { p.Nome, p.IsAtivo });
    }
}