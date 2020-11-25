using System;
using System.Collections.Generic;

namespace Project1.Library.Models {
    public class Order {
        public int Id { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public Dictionary<Product, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public Customer Customer { get; set; }
        public DateTime Time { get; set; }

        public Order() {
            Products = new Dictionary<Product, int>();
        }

        public Order(Location location, Customer customer, DateTime time) {
            Products = customer.Cart;
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
