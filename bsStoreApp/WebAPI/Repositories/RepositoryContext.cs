using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) 
        {
            
        }
        public DbSet<Book> Books { get; set; }
    }
}
