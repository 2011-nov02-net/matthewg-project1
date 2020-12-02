using System.Collections.Generic;

namespace Project1.Library.Models {
    public class Location {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string Zip { get; }
        public string Phone { get; set; }
        public Dictionary<int, int> Stock { get; set; }
        public Dictionary<int, decimal> Prices { get; set; }

        public Location() {
            Stock = new Dictionary<int, int>();
            Prices = new Dictionary<int, decimal>();
        }

        public Location(string name, string address, string city, string state, string country, string zip, string phone) {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
            Phone = phone;
            Stock = new Dictionary<int, int>();
            Prices = new Dictionary<int, decimal>();
        }

        /// <summary>
        /// Add some number of items to the store's stock
        /// </summary>
        /// <param name="product">Product object thats stock is to be modified</param>
        /// <param name="qty">Integer value by how much to modify the stock</param>
        /// <returns>True if succeeded in modifying the stock amount. False if there was an attempt to input a negative amount for a non-yet-stocked product</returns>
        public bool AddStock(int productId, int qty) {
            if (Stock.ContainsKey(productId)) {
                if (qty < 0 && System.Math.Abs(qty) > Stock[productId]) {
                    return false;
                }
                Stock[productId] += qty;
                return true;
            } else if (qty > 0) {
                Stock.Add(productId, qty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set some price amount associated with a product
        /// </summary>
        /// <param name="product">Product object thats price is to be modified</param>
        /// <param name="price">decimal floating point number</param>
        /// <returns>True if price was successfully set. False if price is 0 or below</returns>
        public bool AddPrice(int productId, decimal price) {
            if (price <= 0) {
                return false;
            }
            if (Prices.ContainsKey(productId)) {
                Prices[productId] = price;
                return true;
            } else {
                Prices.Add(productId, price);
                return true;
            }
        }

    }
}
