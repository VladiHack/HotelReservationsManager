using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class Room
    {
        public Room()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        public bool Occupied { get; set; }
        public decimal PriceBedAdult { get; set; }
        public decimal PriceBedChild { get; set; }
        public string RoomNumber { get; set; } = null!;
        public int? Capacity { get; set; }

        public virtual RoomType TypeNavigation { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
