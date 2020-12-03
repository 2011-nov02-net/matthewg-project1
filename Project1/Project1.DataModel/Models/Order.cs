using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.DataModel.Models {
    public partial class Order {
        public Order() {
            OrderContents = new HashSet<OrderContent>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderContent> OrderContents { get; set; }
    }
}
