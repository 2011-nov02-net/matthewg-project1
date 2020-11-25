using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Interfaces {
    public interface IUser {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; }
    }
}
