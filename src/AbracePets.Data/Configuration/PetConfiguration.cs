using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbracePets.Domain.Entities;

namespace AbracePets.Data.Configuration
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Foto)
                .IsRequired();

            builder.Property(p => p.Especie)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Sexo)
                .IsRequired();

            builder.Property(p => p.Raca)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.Cor)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasMany(p => p.Eventos)
                .WithOne(p => p.Pet)
                .HasForeignKey(p => p.PetId);

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            builder.ToTable("TB_Pet");
        }
    }
}