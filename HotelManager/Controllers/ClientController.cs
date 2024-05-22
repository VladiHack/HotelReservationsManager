using HotelManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.XPath;

namespace Hotel_Reservations_Manager.Controllers
{
    public class ClientController:Controller
    {
        private readonly HotelReservationsManagerDbContext _context;

        public ClientController(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Clients.ToList());
        }
        public IActionResult Create()
        {
            ViewBag.role = RoleSupplier.role;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            string msg = "";
            if(client.PhoneNumber.Length!=10)
            {
                msg += "Телефонният номер трябва да е с дължина 10 цифри!\n";
            }
            if(_context.Clients.FirstOrDefault(x=>x.Email==client.Email)!=null)
            {
                msg += "Вече има клиент с такъв имейл!\n";
            }
            if (_context.Clients.FirstOrDefault(x => x.FirstName == client.FirstName&& x.LastName==client.LastName) != null)
            {
                msg += "Вече има клиент с такова име!\n";
            }
            ViewBag.Message = msg;
            List<Client> clients = _context.Clients.ToList();
            client.Id = clients[clients.Count() - 1].Id + 1;
            if (ModelState.IsValid)
            {               
                    if(msg=="")
                {
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                    
            }
        
            return View(client);
        }
        public IActionResult Delete(int id)
        {
            var clients = _context.Clients.FirstOrDefault(x => x.Id == id);
            return View(clients);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'HotelDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var clients = _context.Clients.FirstOrDefault(y => y.Id == id);
            return View(clients);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            string msg = "";
            if (client.PhoneNumber.Length != 10)
            {
                msg += "Телефонният номер трябва да е с дължина 10 цифри!\n";
            }
            if (_context.Clients.FirstOrDefault(x => x.Email == client.Email && x.Id!=client.Id) != null)
            {
                msg += "Вече има клиент с такъв имейл!\n";
            }
            if (_context.Clients.FirstOrDefault(x => x.FirstName == client.FirstName && x.LastName == client.LastName&& x.Id!=client.Id) != null)
            {
                msg += "Вече има клиент с такова име!\n";
            }
            ViewBag.Message = msg;
            int id = client.Id;
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid&&msg=="")
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Clients.FirstOrDefault(x => x.Id == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        public IActionResult Details(int id)
        {
            var clients = _context.Clients.FirstOrDefault(y => y.Id == id);
            return View(clients);
        }
        public IActionResult ShowPreviousReservations(int id)
        {
            List<ReservationsClient> reservationsClients = _context.ReservationsClients.Where(x => x.ClientId == id).ToList();
            List<Reservation> reservations = new List<Reservation>();
            foreach(ReservationsClient reservation in reservationsClients)
            {
                Reservation res = _context.Reservations.FirstOrDefault(p => p.Id == reservation.ReservationId);
                reservations.Add(res);
            }
            return View(reservations);
        }
    }
}
