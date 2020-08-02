using System;
using System.Globalization;
using System.IO;
using System.Linq;
using EVA_Model.Data;
using EVA_Model.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVA_Model
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
                foreach (var attribute in enumerable)
                {
                    dbContext.EmployeeAttributes.Add(new EmployeeAttribute
                    {
                        EmployeeId = employee.Id,
                        AttributeName = attribute,
                        AttributeValue =  employee.GetType().GetProperty(attribute)?.GetValue(employee)?.ToString()
                    });
                }
                
            }
            dbContext.SaveChanges();
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
            services.AddSingleton(Configuration);
        }
    }
}