using Microsoft.EntityFrameworkCore;

namespace Bookish.Models;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BorrowRelationModel> Relations { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=LibraryDB;User Id=sa;Password=AndSpiHsiLow27;Encrypt=False");
    }
}