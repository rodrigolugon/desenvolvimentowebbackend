using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // IMPORTANTE
using Microsoft.EntityFrameworkCore;
using Academia.Models;

namespace Academia.Models {
    // Verifique se aqui diz IdentityDbContext e não apenas DbContext
    public class Context : IdentityDbContext {
        public Context(DbContextOptions<Context> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Esta linha é CRUCIAL para o Identity. Se faltar, dá erro.
            base.OnModelCreating(modelBuilder);

            // Suas tabelas e chaves
            modelBuilder.Entity<TreinoExercicio>()
                .HasKey(te => new { te.TreinoID, te.ExercicioID });

            modelBuilder.Entity<Treino>()
                .HasOne(t => t.Personal)
                .WithMany(p => p.Treinos)
                .HasForeignKey(t => t.PersonalID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Personal> Personals { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<TreinoExercicio> TreinoExercicios { get; set; }
    }
}