﻿using Project1.Library.Models;
using System.Collections.Generic;

namespace Project1.Library.Interfaces {
    public interface IStoreRepository {
        List<Location> GetLocations();
        IEnumerable<IUser> GetCustomers();
        IEnumerable<IUser> GetCustomersByName(string firstName, string lastName);
        List<Order> GetOrders();
        List<Product> GetProducts();
        Location GetLocationById(int id);
        IUser GetCustomerById(int id);
        IUser GetCustomerByEmail(string s);
        Order GetOrderById(int id);
        Product GetProductById(int id);
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void RemoveLocation(Location location);
        void AddCustomer(Customer customer);
        void UpdateCustomer(IUser customer, string firstName, string lastName, string email);
        void RemoveCustomer(IUser customer);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void RemoveOrder(Order order);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(Product product);
        void UpdateLocationStock(Location location);
        List<Order> GetCustomerOrders(IUser customer);
        List<Order> GetLocationOrders(Location location);
        void Save();
    }
}
