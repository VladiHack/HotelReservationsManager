using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class User
    {
        public User()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int ID { get; set; }
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EGN { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public bool Active { get; set; }
        public DateTime? LeftPosition { get; set; }
        public bool IsAdmin { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
