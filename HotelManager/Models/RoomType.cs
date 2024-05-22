using System;
using System.Collections.Generic;

namespace HotelManager.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string? Type { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
