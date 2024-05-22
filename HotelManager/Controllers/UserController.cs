
using HotelManager.Models;
using HotelManager.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace Hotel_Reservations_Manager.Controllers
{
    public class UserController:Controller
    {
        private readonly HotelReservationsManagerDbContext _context;

        public UserController(HotelReservationsManagerDbContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            string msg = "";
            if (_context.Users.Count() == 0)
            {
                user.IsAdmin = true;
            }
            else
            {
                user.IsAdmin = false;
            }
            if(user.LeftPosition==null)
            {
                user.Active= true;
            }
            else
            {
                user.Active = false;
            }
            List<User> users = _context.Users.ToList();
            user.ID = users[users.Count() - 1].ID + 1;

            msg = UserValidator.ReturnErrorsCreate(users, user);
            ViewBag.Message = msg;

           if (msg=="")
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginEntity partUser)
        {
            string msg = $"Няма такъв потребител!";
            User user = _context.Users.FirstOrDefault(e => e.Email == partUser.Email && e.Password == partUser.Password);
            if(user!=null)
            {
                if(user.IsAdmin)
                {
                    RoleSupplier.role = "Admin";
                }
                else
                {
                    RoleSupplier.role = "User";
                }
                
                if(user.Active==false)
                {
                    RoleSupplier.role = "Not Active";
                }
                msg = "Успешно влизане! Вашата роля е "+RoleSupplier.role;
            }
            ViewBag.Message = msg;
            return View();
        }
        public IActionResult Logout()
        {
            RoleSupplier.role = "Client";
            return View();
        }
       

        public IActionResult Edit(int id) 
        {
            var users=_context.Users.AsNoTracking().FirstOrDefault(y => y.ID == id);
            return View(users);
        }
        public IActionResult Delete(int id)
        {
            var users = _context.Users.FirstOrDefault(x => x.ID == id);
            return View(users);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HotelDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                foreach(var reservation in _context.Reservations.ToList())
                {
                    if(reservation.UserId==user.ID)
                    {
                        _context.Remove(reservation);
                    }
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            User oldUser = _context.Users.AsNoTracking().FirstOrDefault(p => p.ID == user.ID);
            List<User> users = _context.Users.AsNoTracking().ToList();
            
            string msg = "";
            msg = UserValidator.ReturnErrorsEdit(users, oldUser, oldUser.ID); 
            ViewBag.Message = msg;
            if (msg == "")
            {
                int id = user.ID;
                if(user.LeftPosition==null)
                {
                    user.Active=true;
                }
                else
                {
                    user.Active = false;
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));



            
        }

        public  IActionResult Details(int id)
        {
            var users = _context.Users.FirstOrDefault(y => y.ID == id);
            return View(users);
        }
      
    }
}
