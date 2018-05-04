using Microsoft.EntityFrameworkCore;

namespace EntityIsNotInStateAdded
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=entity_not_added;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attributes>()
                        .Property<int>("PersonId");

            modelBuilder.Entity<Attributes>()
                        .HasKey("PersonId");

            modelBuilder.Entity<Attributes>()
                        .HasOne(_ => _.Person)
                        .WithOne()
                        .IsRequired();

            modelBuilder.Entity<Attributes>()
                        .Property(_ => _.Attribs)
                        .HasColumnName("Attributes");
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Attributes> Attributes { get; set; }
    }
}