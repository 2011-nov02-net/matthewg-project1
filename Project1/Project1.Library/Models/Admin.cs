using Project1.Library.Interfaces;

namespace Project1.Library.Models {
    public class Admin : IUser {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Admin() {
        }
    }
}
