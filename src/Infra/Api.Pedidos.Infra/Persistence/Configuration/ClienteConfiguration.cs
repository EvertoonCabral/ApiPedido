using Api.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Pedidos.Infra.Persistence.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Telefone)
            .HasMaxLength(30);

        builder.Property(c => c.IsAtivo)
            .HasDefaultValue(true);

        builder.OwnsOne(c => c.Endereco, end =>
        {
            end.Property(e => e.Rua).HasMaxLength(200);
            end.Property(e => e.Numero).HasMaxLength(50);
            end.Property(e => e.Bairro).HasMaxLength(100);
            end.Property(e => e.Cidade).HasMaxLength(100);
            end.Property(e => e.Estado).HasMaxLength(2);
            end.Property(e => e.Cep).HasMaxLength(20);
            
            end.WithOwner();
        });

        builder.HasIndex(c => c.Email).IsUnique();
    }
}