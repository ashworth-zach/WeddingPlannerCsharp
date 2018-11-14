using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using weddingplanner.Models;
using Microsoft.EntityFrameworkCore;
namespace weddingplanner.Controllers
{
    public class HomeController : Controller
    {
        private UserContext dbContext;

        public HomeController(UserContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            lock (Console.Out)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(dbContext.guests.ToList() + "DFSLJFLSDKJFLKDJS");
            }

            return View("index");
        }
        [HttpPost("Register")]
        public IActionResult Register(UserRegistration userReg)
        {
            LoginRegisterViewModel LoginErrors = new LoginRegisterViewModel()
            {
                UserReg = userReg
            };
            if (ModelState.IsValid)
            {
                // take the userReg object and convert it to User, with a hashed pw

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                if (dbContext.user.Any(u => u.email == userReg.email))
                {
                    ModelState.AddModelError("email", "Email already in use!");
                    return View("index", LoginErrors);
                }
                User newUser = new User
                {
                    firstname = userReg.firstname,
                    lastname = userReg.lastname,
                    email = userReg.email,
                };
                newUser.password = Hasher.HashPassword(newUser, userReg.password); // hash pw
                dbContext.user.Add(newUser);
                // save the new user with hashed pw
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                dbContext.SaveChanges();
                return RedirectToAction("Weddings");
            }

            return View("Index", LoginErrors);
        }

        [HttpPost("SubmitLogin")]
        public IActionResult SubmitLogin(UserLogin userLog)
        {
            if (ModelState.IsValid)
            {
                User CheckUser = dbContext.user.FirstOrDefault(x => x.email == userLog.logemail);
                if (CheckUser != null && userLog.logpassword != null)
                {
                    // check if the password matches
                    var Hasher = new PasswordHasher<User>();
                    if (0 != Hasher.VerifyHashedPassword(CheckUser, CheckUser.password, userLog.logpassword))
                    {
                        // if match, set id to session & redirect
                        HttpContext.Session.SetInt32("UserId", CheckUser.UserId);
                        return RedirectToAction("Weddings");
                    }
                }
            }
            LoginRegisterViewModel LoginErrors = new LoginRegisterViewModel()
            {
                UserLog = userLog
            };
            return View("Index", LoginErrors);
        }
        [HttpGet("Weddings")]
        public IActionResult Weddings()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            List<Wedding> AllWeddings = dbContext.wedding.Include(x => x.guests).ToList();
            foreach (var wedding in AllWeddings)
            {
                int total = dbContext.guests.Where(x => x.WeddingId == wedding.WeddingId).Count();
                wedding.total = total;
            }
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);

            ViewBag.UserId = UserId;
            ViewBag.AllGuests = dbContext.guests.ToList();
            ViewBag.AllWeddings = AllWeddings;
            return View("Weddings");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("newwedding")]
        public IActionResult NewWedding()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);

            ViewBag.UserId = UserId;
            return View("newwedding");
        }
        [HttpPost("CreateWedding")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);

            if (ModelState.IsValid)
            {
                wedding.UserId = UserId;
                dbContext.wedding.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("Weddings");
            }
            return View("newwedding");
        }
        [HttpGet("show/{id}")]
        public IActionResult ShowWedding(int id)
        {

            ViewBag.people = dbContext.wedding.Where(x => x.WeddingId == id).Include(wed => wed.guests).ThenInclude(x => x.user).ToList();
            // .ThenInclude(guest => guest.user).Where(w=>w.WeddingId==id).ToList();  .Where(x=>x.WeddingId==id)
            ViewBag.Wedding = dbContext.wedding.FirstOrDefault(x => x.WeddingId == id);
            return View();
        }
        [HttpGet("rsvp/{weddingId}")]
        public IActionResult Rsvp(int weddingId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int User = HttpContext.Session.GetInt32("UserId") ?? default(int);
            // Console.WriteLine(User+ "  " + User.GetType());
            //Check guests to see if the user is already a guest
            if (dbContext.guests.Any(guest => guest.UserId == User && guest.WeddingId == weddingId))
            {
                return RedirectToAction("Weddings");

            }
            User usertoAdd = dbContext.user.FirstOrDefault(x => x.UserId == User);
            Wedding WeddingToAdd = dbContext.wedding.FirstOrDefault(x => x.WeddingId == weddingId);
            Guests NewGuest = new Guests
            {
                UserId = User,
                wedding = WeddingToAdd,
            };
            dbContext.guests.Add(NewGuest);
            dbContext.SaveChanges();
            return RedirectToAction("Weddings");
        }
        [HttpGet("delete/{weddingId}")]
        public IActionResult DeleteWedding(int weddingId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            List<Guests> GuestToDelete = dbContext.guests.Where(x => x.WeddingId == weddingId).ToList();
            foreach (var guest in GuestToDelete)
            {
                dbContext.guests.Remove(guest);
                dbContext.SaveChanges();
            }
            Wedding WeddingtoDelete = dbContext.wedding.FirstOrDefault(x => x.WeddingId == weddingId);
            dbContext.wedding.Remove(WeddingtoDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Weddings");
        }
        [HttpGet("unrsvp/{weddingId}")]
        public IActionResult Unrsvp(int weddingId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int UserId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            Guests RsvpToRemove = dbContext.guests.FirstOrDefault(x => x.WeddingId == weddingId && x.UserId == UserId);
            dbContext.guests.Remove(RsvpToRemove);
            dbContext.SaveChanges();
            return RedirectToAction("Weddings");
        }
    }
}