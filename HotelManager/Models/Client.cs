using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class Client
    {
        public Client()
        {
            ReservationsClients = new HashSet<ReservationsClient>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Adult { get; set; }

        public virtual ICollection<ReservationsClient> ReservationsClients { get; set; }
    }
}
