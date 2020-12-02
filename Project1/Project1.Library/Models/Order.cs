using Project1.Library.Interfaces;
using System;
using System.Collections.Generic;

namespace Project1.Library.Models {
    public class Order {
        public int Id { get; set; }
        public Dictionary<int, int> Products { get; set; }
        public Dictionary<int, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public IUser Customer { get; set; }
        public DateTime Time { get; set; }

        public Order() {
            Products = new Dictionary<int, int>();
        }

        public Order(Location location, IUser customer, DateTime time, Dictionary<int, int> products) {
            Products = products;
            Location = location;
            Customer = customer;
            Time = time;
            PricePaid = new Dictionary<int, decimal>();
            foreach (var p in Products.Keys) {
                PricePaid[p] = location.Prices[p];
            }
        }
    }
}
