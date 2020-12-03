using System.ComponentModel.DataAnnotations;

namespace Project1.WebApp.Models {
    public class LoginViewModel {
        [RegularExpression("^(.+)(@)(.+)[.](.+)$")]
        [Required]
        public string Email { get; set; }
    }
}
