using System;
using System.Linq;
using EAV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EAV.Data
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
        public DbSet<EmployeeJsonAttribute> EmployeeJsonAttributes { get; set; }
        public DbSet<EmployeeAttribute> EmployeeAttributes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySQL(_configuration.GetConnectionString("DataConnection"))
                .EnableSensitiveDataLogging();
            
            options.ConfigureWarnings(warnings =>
            {
                warnings.Log(CoreEventId.IncludeIgnoredWarning);
                warnings.Log(RelationalEventId.QueryClientEvaluationWarning);
            });
            options.UseLoggerFactory(GetLoggerFactory());
        }
        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddConsole()
                    //.AddFilter(category: DbLoggerCategory.Database.Command.Name, level: LogLevel.Information));
                    .AddFilter(level => true)); // log everything
            return serviceCollection.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        }
    }
    
}