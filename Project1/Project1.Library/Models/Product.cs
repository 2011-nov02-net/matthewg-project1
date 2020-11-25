namespace Project1.Library.Models {
    public class Product {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public Product() { }

        public Product(string name) {
            DisplayName = name;
        }
    }
}
