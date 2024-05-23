using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbracePets.Domain.Entities;

namespace AbracePets.Data.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.EmailLogin)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Senha)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(u => u.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.ToTable("TB_Usuario");
    }
}