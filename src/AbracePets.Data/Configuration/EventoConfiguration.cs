using AbracePets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbracePets.Data.Configuration
{
    internal class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Data)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired();

            builder.Property(e => e.Local)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.PetId)
                .IsRequired();

            builder.ToTable("TB_Evento");
        }
    }
}
