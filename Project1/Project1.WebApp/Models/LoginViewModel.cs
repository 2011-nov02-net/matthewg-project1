using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models {
    public class LoginViewModel {
        [RegularExpression("^(.+)(@)(.+)[.](.+)$")]
        [Required]
        public string Email { get; set; }
    }
}
