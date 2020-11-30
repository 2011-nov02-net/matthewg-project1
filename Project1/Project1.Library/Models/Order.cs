using Project1.Library.Interfaces;
using System;
using System.Collections.Generic;

namespace Project1.Library.Models {
    public class Order {
        public int Id { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public Dictionary<Product, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public IUser Customer { get; set; }
        public DateTime Time { get; set; }

        public Order() {
            Products = new Dictionary<Product, int>();
        }

        public Order(Location location, IUser customer, DateTime time, Dictionary<Product, int> products) {
            Products = products;
            Location = location;
            Customer = customer;
            Time = time;
            PricePaid = new Dictionary<Product, decimal>();
            foreach (Product p in Products.Keys) {
                PricePaid[p] = location.Prices[p];
            }
        }
    }
}
