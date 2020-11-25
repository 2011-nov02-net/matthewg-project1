using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.DataModel.Models {
    public partial class LocationInventory
    {
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}
