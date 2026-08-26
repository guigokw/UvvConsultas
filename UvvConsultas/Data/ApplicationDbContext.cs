using Microsoft.EntityFrameworkCore;
using UvvConsultas.Models;

namespace UvvConsultas.Data
{
    public class ApplicationDbContext : DbContext
    {
  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // criando as tabelas no banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }


        // configurando o modelo de dados para garantir que o email seja único
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
