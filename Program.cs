using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using EAV.Data;
using EAV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFrameworkCore.Extensions;
using System.Text.Json.Serialization;


namespace EAV
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<MyDbContext>();

            var employees = dbContext.Employees.FromSqlRaw(
                @"SELECT EmployeeId AS Id, Attributes ->> '$.FirstName' AS FirstName, 
Attributes ->> '$.LastName' AS LastName,
Attributes ->> '$.DateOfBirth' AS DateOfBirth FROM efcoresample.EmployeeJsonAttributes
      WHERE Attributes ->> '$.DateOfBirth' > DATE_SUB(CURRENT_DATE(), INTERVAL {0} YEAR)", 25);

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.DateOfBirth);
            }
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>();
            
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            services.AddSingleton<IConfigurationRoot>(Configuration);
        }
    }
}