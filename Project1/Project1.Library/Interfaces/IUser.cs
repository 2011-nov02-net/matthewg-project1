using System.Collections.Generic;

namespace Project1.Library.Interfaces {
    public interface IUser {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        public Dictionary<int, int> Cart { get; set; }

        bool AddToCart(int productId, int qty);
        bool RemoveFromCart(int productId, int qty);
        void EmptyCart();
        void NewCart();
    }
}
