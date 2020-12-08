using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Project1.DataModel.Repositories;
using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project1.UnitTests {
    public class RepositoryTests {

        [Fact]
        public void Repo_ValidCustomer_AddsToDb() {
            // arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<DataModel.Models.TrainingProjectContext>().UseSqlite(connection).Options;

            var customer = new Customer("John", "Doe", "asdf@email.com");

            using (var context = new DataModel.Models.TrainingProjectContext(options)) {
                context.Database.EnsureCreated();
                var repo = new StoreRepository(context);

                // act
                repo.AddCustomer(customer);
                repo.Save();
            }

            // assert
            using var context2 = new DataModel.Models.TrainingProjectContext(options);
            DataModel.Models.Customer customerActual = context2.Customers.Single(c => c.Email == "asdf@email.com");
            Assert.Equal(customer.FirstName, customerActual.FirstName);
        }

        [Fact]
        public void Repo_DuplicateCustomer_DbRejects() {
            // arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<DataModel.Models.TrainingProjectContext>().UseSqlite(connection).Options;

            var customer = new Customer("John", "Doe", "asdf@email.com");
            var customer2 = new Customer("John", "Doe", "asdf@email.com");

            using (var context = new DataModel.Models.TrainingProjectContext(options)) {
                context.Database.EnsureCreated();
                var repo = new StoreRepository(context);
                repo.AddCustomer(customer);
                repo.Save();

                // act
                repo.AddCustomer(customer2);
                try {
                    repo.Save();
                } catch (DbUpdateException e) {

                }

                // assert
                var transaction = context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added ||
                                x.State == EntityState.Deleted ||
                                x.State == EntityState.Modified).ToList();
                Assert.Empty(transaction);
            }
            using var context2 = new DataModel.Models.TrainingProjectContext(options);
            Assert.Equal(1, context2.Customers.Count());
        }

        [Fact]
        public void Repo_ValidOrder_AddsToDb() {
            // arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<DataModel.Models.TrainingProjectContext>().UseSqlite(connection).Options;

            var customer = new Customer("John", "Doe", "asdf@email.com");
            var location = new Location("TestLocation", "", "", "", "", "", "");
            Dictionary<int, int> productsList = new Dictionary<int, int>();

            var order = new Order(location, customer, DateTime.Now, productsList);

            using (var context = new DataModel.Models.TrainingProjectContext(options)) {
                context.Database.EnsureCreated();
                var repo = new StoreRepository(context);

                repo.AddCustomer(customer);
                repo.AddLocation(location);
                customer.Id = 1;
                location.Id = 1;
                repo.UpdateLocationStock(location);
                repo.Save();
                

                // act
                repo.AddOrder(order);
                repo.Save();
            }

            // assert
            using var context2 = new DataModel.Models.TrainingProjectContext(options);
            DataModel.Models.Order orderActual = context2.Orders.Single(o => o.Id == 1);
            Assert.Equal(order.Time, orderActual.Date);
        }
    }
}
