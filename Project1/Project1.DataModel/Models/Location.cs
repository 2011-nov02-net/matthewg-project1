using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.DataModel.Models {
    public partial class Location
    {
        public Location()
        {
            LocationInventories = new HashSet<LocationInventory>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<LocationInventory> LocationInventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
