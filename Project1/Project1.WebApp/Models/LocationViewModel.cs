using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models {
    public class LocationViewModel {

        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string Address { get; set; }


        public string City { get; set; }

        [StringLength(2)]
        
        public string State { get; set; }


        public string Country { get; set; }


        public string Zip { get; set; }

        [Phone]
        public string Phone { get; set; }

        
        public Dictionary<int, int> Stock { get; set; }


        public Dictionary<int, decimal> Prices { get; set; }

    }
}
