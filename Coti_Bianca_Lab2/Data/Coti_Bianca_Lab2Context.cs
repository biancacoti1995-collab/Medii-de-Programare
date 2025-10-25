using Coti_Bianca_Lab2.Models;
using Microsoft.EntityFrameworkCore;


namespace Coti_Bianca_Lab2.Data
{
    public class Coti_Bianca_Lab2Context : DbContext
    {
        public Coti_Bianca_Lab2Context(DbContextOptions<Coti_Bianca_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookID, bc.CategoryID });

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookID);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories) 
                .HasForeignKey(bc => bc.CategoryID);
        }
    }
}
