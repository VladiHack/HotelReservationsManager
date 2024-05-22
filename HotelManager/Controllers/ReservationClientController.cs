using HotelManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HotelReservationManager.Controllers
{
    public class ReservationClientController : Controller
    {
        private readonly HotelReservationsManagerDbContext _context;
        int reservationId = 0;

        public ReservationClientController(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ReservationsClients.ToList());
        }
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "Id", "Email");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationsClient reservation)
        {
            var reservationClients = _context.ReservationsClients.ToList();
            if(reservationClients.Count==0||reservationClients==null)
            {
                reservation.Id = 1;
            }
            else 
            {
                reservation.Id = reservationClients[reservationClients.Count-1].Id+1;
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "Id", "Email", reservation.ClientId);
            _context.ReservationsClients.Add(reservation);
            Reservation res = _context.Reservations.FirstOrDefault(p => p.Id == reservation.ReservationId);
            Client client = _context.Clients.FirstOrDefault(p => p.Id == reservation.ClientId);
            Room room = _context.Rooms.FirstOrDefault(p => p.Id==res.RoomId);
            res.Cost += ReturnCost(reservation, res, client, room) *(res.Departure-res.Arrival).Days;
            _context.Reservations.Update(res);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
       public decimal ReturnCost(ReservationsClient reservation,Reservation res,Client client,Room room)
        {
            decimal cost = 0;
       

            if (client.Adult)
            {
                cost += room.PriceBedAdult;
                if (res.AllInclusive)
                {
                    cost += room.PriceBedAdult * (decimal)0.4;
                }
                if (res.IncludedBreakfast)
                {
                    cost += room.PriceBedAdult * (decimal)0.2;
                }
            }
            else
            {
                cost += room.PriceBedChild;
                if (res.AllInclusive)
                {
                    cost += room.PriceBedChild * (decimal)0.4;
                }
                if (res.IncludedBreakfast)
                {
                    cost += room.PriceBedChild * (decimal)0.2;
                }
            }
            return cost;
        }
        public IActionResult Delete(int id)
        {
            var reservations = _context.ReservationsClients.FirstOrDefault(x => x.Id==id);
            return View(reservations);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'HotelDbContext.ReservationsClients'  is null.");
            }
            var reservation = await _context.ReservationsClients.FindAsync(id);
            if (reservation != null)
            {
                DeleteOperation(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        void DeleteOperation(ReservationsClient reservation)
        {
            Reservation res = _context.Reservations.FirstOrDefault(p => p.Id == reservation.ReservationId);
            Client client = _context.Clients.FirstOrDefault(p => p.Id == reservation.ClientId);
            Room room = _context.Rooms.FirstOrDefault(p => p.Id == res.RoomId);

            res.Cost -= ReturnCost(reservation, res, client, room) * (res.Departure-res.Arrival).Days;
            _context.Remove(reservation);
            _context.Reservations.Update(res);
        }
        void CreateOperation(ReservationsClient reservation)
        {
            _context.ReservationsClients.Add(reservation);
            Reservation res = _context.Reservations.FirstOrDefault(p => p.Id == reservation.ReservationId);
            Client client = _context.Clients.FirstOrDefault(p => p.Id == reservation.ClientId);
            Room room = _context.Rooms.FirstOrDefault(p => p.Id == res.RoomId);
            res.Cost += ReturnCost(reservation, res, client, room);
            _context.Reservations.Update(res);
        }
        public IActionResult Edit(int id)
        {
            var reservation = _context.ReservationsClients.FirstOrDefault(y => y.Id == id);
            ViewData["ClientID"] = new SelectList(_context.Clients, "Id", "Email", reservation.ClientId);
            return View(reservation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationsClient reservation)
        {
            int id = reservation.Id;
            var oldRes = await _context.ReservationsClients.FindAsync(id);
            //Removing old reservation
            DeleteOperation(oldRes);
            //Adding new one with the same id
            CreateOperation(reservation);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var reservation = _context.ReservationsClients
                .FirstOrDefault(m => m.Id == id);
            return View(reservation);
        }

        public IActionResult ShowClients(int id)
        {
            List<Client> clients = new List<Client>();
            var reservationClients=_context.ReservationsClients.Where(p=>p.ReservationId == id).ToList();
            foreach(var reservation in  reservationClients)
            {
                clients.Add(_context.Clients.FirstOrDefault(p => p.Id == reservation.ClientId));
            }
            return View(clients);
        }

    }
}
