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

            var attributes = typeof(Employee).GetProperties().Where(x => x.Name != "Id").Select(x => x.Name);
            var employees = dbContext.Employees;
            var enumerable = attributes as string[] ?? attributes.ToArray();

            foreach (var employee in employees)
            {
                var employeeObject = enumerable.ToDictionary(attribute => attribute,
                    attribute =>
                    {
                        var value = employee.GetType().GetProperty(attribute)?.GetValue(employee)?.ToString();
                        return attribute == "DateOfBirth" ? DateTime.Parse(value).ToString("yyyy-MM-dd") : value;
                    });
                var jsonObject = (JsonSerializer.Serialize(employeeObject));
                dbContext.EmployeeJsonAttributes.Add(new EmployeeJsonAttribute
                {
                    EmployeeId = employee.Id,
                    Attributes = jsonObject
                });
            }

            dbContext.SaveChanges();
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