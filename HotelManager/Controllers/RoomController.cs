using HotelManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservations_Manager.Controllers
{
    public class RoomController:Controller
    {

        private readonly HotelReservationsManagerDbContext _context;

        public RoomController(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // var roomContext = _context.Rooms.Include(e => e.TypeNavigation);
            ViewBag.role = RoleSupplier.role;
            return View(_context.Rooms.ToList());

        }
        public IActionResult Create()
        {
            ViewData["TypeID"] = new SelectList(_context.RoomTypes, "Id", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Room room)
        {
            if (_context.Rooms.Count()!=0)
            {
                List<Room> rooms = _context.Rooms.ToList();
                room.Id = rooms[rooms.Count() - 1].Id + 1;
            }
            else
            {
                room.Id = 1;
            }
            ViewData["TypeID"] = new SelectList(_context.RoomTypes, "Id", "Type", room.Type);
            _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

        }
         public IActionResult Edit(int id)
        {
            var room = _context.Rooms.FirstOrDefault(y => y.Id == id);
            ViewData["TypeID"] = new SelectList(_context.RoomTypes, "Id", "Type",room.Type);
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Room room)
        {
            int id = room.Id;
          
               
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            
        }

        public  IActionResult Details(int id)
        {
            var room = _context.Rooms
                .FirstOrDefault(m => m.Id == id);
            return View(room);
        }

        public IActionResult Delete(int id)
        {
            var rooms = _context.Rooms.FirstOrDefault(x => x.Id == id);
            return View(rooms);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'HotelDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
