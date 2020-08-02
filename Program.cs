using System;
using System.Globalization;
using System.IO;
using System.Linq;
using EAV.Data;
using EAV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFrameworkCore.Extensions;

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

            string[] columnNames = {"FirstName", "LastName", "DateOfBirth"};
            var employees = dbContext.EmployeeAttributes
                .Where(x => 
                            dbContext.EmployeeAttributes
                                .Where(i => i.AttributeName == "DateOfBirth")
                                .Select(eId => eId.EmployeeId).Contains(x.EmployeeId) &&
                            columnNames.Contains(x.AttributeName))
                .GroupBy(x => x.EmployeeId)
                .Select(g => new
                {
                    FirstName = g.Max(f => f.AttributeName == "FirstName" ? f.AttributeValue : ""),
                    LastName = g.Max(f => f.AttributeName == "LastName"? f.AttributeValue : ""),
                    DateOfBirth = g.Max(f => f.AttributeName == "DateOfBirth"? f.AttributeValue : ""),
                    Id = g.Key
                })
                .ToList()
                .Where(x => DateTime.ParseExact(x.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddYears(-25));


            var endDate = DateTimeOffset.Now.AddYears(Convert.ToInt32(-2));
            var normalTypes = dbContext.Employees.Where(x => x.DateOfBirth > endDate).ToList();
            
            foreach (var employee in normalTypes)
            {
                Console.WriteLine($"{employee.FirstName} - {employee.DateOfBirth}");
            }
        }
        
        static Func<DateTime> RandomDayFunc()
        {
            DateTime start = new DateTime(1995, 1, 1); 
            Random gen = new Random(); 
            int range = ((TimeSpan)(DateTime.Today - start)).Days; 
            return () => start.AddDays(gen.Next(range));
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