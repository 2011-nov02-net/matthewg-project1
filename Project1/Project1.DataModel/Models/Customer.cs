using System.Collections.Generic;

#nullable disable

namespace Project1.DataModel.Models {
    public partial class Customer {
        public Customer() {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Class { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
