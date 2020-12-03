using System.Collections.Generic;

#nullable disable

namespace Project1.DataModel.Models {
    public partial class Product {
        public Product() {
            LocationInventories = new HashSet<LocationInventory>();
            OrderContents = new HashSet<OrderContent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationInventory> LocationInventories { get; set; }
        public virtual ICollection<OrderContent> OrderContents { get; set; }
    }
}
