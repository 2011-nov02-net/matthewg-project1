using System.ComponentModel.DataAnnotations;

namespace Project1.WebApp.Models {
    public class CustomerViewModel {

        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        // [RegularExpression("^(.+)(@)(.+)[.](.+)$", ErrorMessage = "Invalid email format.")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
