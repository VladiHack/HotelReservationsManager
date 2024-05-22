using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class ReservationsClient
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public int? ClientId { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
