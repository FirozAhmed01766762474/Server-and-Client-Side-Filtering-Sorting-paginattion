using Microsoft.EntityFrameworkCore;
using Pagination.Models;

namespace Pagination.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Server=DESKTOP-21UHJJD\\SQLEXPRESS;Database= MyDatabase;Trusted_Connection=true;");
        }
        public DbSet<Employee> Employee { get; set; }
    }
}
