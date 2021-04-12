using System.Data.Entity;

namespace Lab4
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Book> Books { get; set; }
    }
}
