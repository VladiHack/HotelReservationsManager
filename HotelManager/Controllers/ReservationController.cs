using HotelManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Hotel_Reservations_Manager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HotelReservationsManagerDbContext _context;

        public ReservationController(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            DateTime today = DateTime.Now;
            foreach(var reservation in _context.Reservations.ToList())
            {
                if(reservation.Departure<=today)
                {
                    //We free the room when it is departure time
                   Room room= _context.Rooms.FirstOrDefault(p=>p.Id==reservation.RoomId);
                    room.Occupied = false;
                    _context.Update(room);
                }
            }
            
            return View(_context.Reservations.ToList());

        }
        public IActionResult Create()
        {

            ViewData["UserID"] = new SelectList(_context.Users.Where(e=>e.Active==true), "ID", "Username");
            ViewData["RoomID"] = new SelectList(_context.Rooms.Where(e => e.Occupied == false), "Id", "RoomNumber");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            string msg = "";
            if (reservation.Arrival> reservation.Departure)
            {
                msg += "Не може да си тръгнете, преди да сте пристигнали (:\n";
            }
            ViewBag.Message = msg;
            if(msg=="")
            {
                List<Reservation> reservations = _context.Reservations.ToList();
                if (reservations.Count == 0 || reservations == null) { reservation.Id = 1; }
                else
                {
                    reservation.Id = reservations[reservations.Count() - 1].Id + 1;
                }
                if (reservation.Arrival < reservation.Departure)
                {
                    Room room = _context.Rooms.FirstOrDefault(e => e.Id == reservation.RoomId);
                    room.Occupied = true;
                    _context.Update(room);
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                }

            }

            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Edit(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(y => y.Id == id);
            ViewData["UserID"] = new SelectList(_context.Users.Where(e => e.Active == true), "ID", "Username", reservation.UserId);
            ViewData["RoomID"] = new SelectList(_context.Rooms.Where(e => e.Occupied == false), "Id", "RoomNumber",reservation.RoomId);
            return View(reservation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            Reservation oldRes = _context.Reservations.AsNoTracking().FirstOrDefault(p => p.Id == reservation.Id);
            string msg = "";
            if (reservation.Arrival > reservation.Departure)
            {
                msg += "Не може да си тръгнете, преди да сте пристигнали (:\n";
            }
            int oldResDays = (oldRes.Departure - oldRes.Arrival).Days;
            int newDays = (reservation.Departure - reservation.Arrival).Days;
            ViewBag.Message = msg;
            if(msg=="")
            {
                 
                int id = reservation.Id;

                oldRes.Cost /= oldResDays;
                oldRes.Cost*=newDays;
                reservation.Cost= oldRes.Cost;
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }

                return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var reservation = _context.Reservations
                .FirstOrDefault(m => m.Id == id);
            return View(reservation);
        }

        public IActionResult Delete(int id)
        {
            var reservations = _context.Reservations.FirstOrDefault(x => x.Id == id);
            return View(reservations);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'HotelDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                foreach (var reservationClient in _context.ReservationsClients.ToList())
                {
                    if (reservation.Id ==reservationClient.ReservationId)
                    {
                        _context.Remove(reservationClient);
                    }
                }
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
