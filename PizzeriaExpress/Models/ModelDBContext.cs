using System.Data.Entity;

namespace PizzeriaExpress.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext1")
        {
        }

        public virtual DbSet<Dettagli> Dettagli { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordini>()
                .Property(e => e.Totale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.Dettagli)
                .WithRequired(e => e.Ordini)
                .HasForeignKey(e => e.IdOrdine_FK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Consegna)
                .HasPrecision(0);

            modelBuilder.Entity<Prodotti>()
                .HasMany(e => e.Dettagli)
                .WithRequired(e => e.Prodotti)
                .HasForeignKey(e => e.IdProdotto_FK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Utenti)
                .HasForeignKey(e => e.IdUtente_FK)
                .WillCascadeOnDelete(false);
        }
    }
}
