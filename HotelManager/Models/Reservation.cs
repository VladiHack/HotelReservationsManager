using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationsClients = new HashSet<ReservationsClient>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public bool IncludedBreakfast { get; set; }
        public bool AllInclusive { get; set; }
        public decimal Cost { get; set; }
        public int? RoomId { get; set; }

        public virtual Room? Room { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ReservationsClient> ReservationsClients { get; set; }
    }
}
