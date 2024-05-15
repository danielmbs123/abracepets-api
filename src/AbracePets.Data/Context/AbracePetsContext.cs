using Microsoft.EntityFrameworkCore;
using AbracePets.Domain.Entities;
using System.Reflection;

namespace AbracePets.Data.Context
{
    public class AbracePetsContext : DbContext
    {
        public DbSet<Pet> PetSet { get; set; }
        public DbSet<Usuario> UsuariosSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string conexao = "server=mysql.tccnapratica.com.br;port=3306;database=tccnapratica04;uid=tccnapratica04;password=2bHK94";
            optionsBuilder.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
