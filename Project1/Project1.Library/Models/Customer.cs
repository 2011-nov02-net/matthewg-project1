using Project1.Library.Interfaces;
using System.Collections.Generic;

namespace Project1.Library.Models {
    public class Customer : IUser {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Dictionary<int, int> Cart { get; set; }
        public Location CurrentLocation { get; set; }

        public Customer() {
            Cart = new Dictionary<int, int>();
            CurrentLocation = null;
        }

        public Customer(string first_name, string last_name, string email) {
            FirstName = first_name;
            LastName = last_name;
            Email = email;
            Cart = new Dictionary<int, int>();
            CurrentLocation = null;
        }

        /// <summary>
        /// Add a product and quantity to a customer's cart, subtracting it from the store's stock
        /// </summary>
        /// <param name="product">Product object to be added to the cart</param>
        /// <param name="qty">Integer amount to be added</param>
        /// <returns>True if product was successfully added to the cart. False if the quantity is less than 1, or if the store does not contain the product.</returns>
        public bool AddToCart(int productId, int qty) {
            if (qty < 1) {
                return false;
            }
            if (CurrentLocation.Stock.ContainsKey(productId) && CurrentLocation.Stock[productId] >= qty) {
                if (Cart.ContainsKey(productId)) {
                    Cart[productId] += qty;
                } else {
                    Cart.Add(productId, qty);
                }
                CurrentLocation.AddStock(productId, -qty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an amount of some product from a customer's cart, adding it back to the store's stock
        /// </summary>
        /// <param name="product">Product object to be removed from the cart</param>
        /// <param name="qty">Integer amount to be removed</param>
        /// <returns>True if the product was successfully removed. False if the quantity is less than 1, or if the store does not contain the product.</returns>
        public bool RemoveFromCart(int productId, int qty) {
            if (qty < 1) {
                return false;
            }
            if (Cart.ContainsKey(productId)) {
                if (Cart[productId] > qty) {
                    Cart[productId] -= qty;

                } else if (Cart[productId] == qty) {
                    Cart.Remove(productId);
                }
                CurrentLocation.AddStock(productId, qty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all products from cart.
        /// </summary>
        public void EmptyCart() {
            foreach (var productId in Cart.Keys) {
                RemoveFromCart(productId, Cart[productId]);
            }
        }

        /// <summary>
        /// Set a brand new cart for the customer
        /// </summary>
        public void NewCart() {
            Cart = new Dictionary<int, int>();
        }
    }
}
