﻿using Project1.Library.Interfaces;
using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.WebApp.Models {
    public class OrderViewModel {

        public OrderViewModel() {
            Products = new Dictionary<int, int>();
            PricePaid = new Dictionary<int, decimal>();
        }

        public int Id { get; set; }
        public Dictionary<int, int> Products { get; set; }
        public Dictionary<int, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public IUser Customer { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }
    }
}
