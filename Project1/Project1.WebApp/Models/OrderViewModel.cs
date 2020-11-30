using Project1.Library.Interfaces;
using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models {
    public class OrderViewModel {

        public int Id { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public Dictionary<Product, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public IUser Customer { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }
    }
}
