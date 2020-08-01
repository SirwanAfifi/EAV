using EVA_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace EVA_Model.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=eva.db");
    }
}