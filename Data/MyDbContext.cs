using System;
using EVA_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace EVA_Model.Data
{
    public class MyDbContext : DbContext
    {
        private readonly IConfigurationRoot _configuration;

        public MyDbContext(IConfigurationRoot configuration)
        {
            this._configuration = configuration;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeEav> EmployeeEav { get; set; }
        public DbSet<EmployeeAttribute> EmployeeAttributes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySQL(_configuration.GetConnectionString("DataConnection"));
        }
    }
    
}