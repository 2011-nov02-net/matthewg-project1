using System.ComponentModel.DataAnnotations;

namespace Project1.WebApp.Models {
    public class ProductViewModel {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
