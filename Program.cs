using System;
using System.Linq;
using EVA_Model.Data;
using EVA_Model.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EVA_Model
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<MyDbContext>();

            foreach (var i in Enumerable.Range(1, 100))
            {
                var faker = new Bogus.Faker();
                dbContext.Employees.Add(new Employee
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    DateOfBirth = faker.Person.DateOfBirth
                });
            }

            dbContext.SaveChanges();
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>();
        }
    }
}