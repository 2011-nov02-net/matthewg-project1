using System.ComponentModel.DataAnnotations;

namespace Project1.WebApp.Models {
    public class LoginViewModel {
        // [RegularExpression("^(.+)(@)(.+)[.](.+)$")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
