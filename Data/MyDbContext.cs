using EVA_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace EVA_Model.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeEav> EmployeeEav { get; set; }
        public DbSet<EmployeeAttribute> EmployeeAttributes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=eva.db");
    }
}