using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

using WeddingPlanner.Models;
namespace WeddingPlanner.Controllers
{
    public class WeddingEventController : Controller
    {
        private BaseContext _context;
        public WeddingEventController(BaseContext context)
            {
                _context = context;
            }
            [HttpGet]
            [RouteAttribute("Dashboard")]
        public IActionResult Dashboard()
            {
                ViewBag.Errors = new List<string>();
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index", "User");
                }
                User CurrentUser = _context.Users
                .Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .Include(u => u.WeddingsPlanned)
                        .ThenInclude(w => w.GuestsAttending)
                            .ThenInclude(g => g.Guest)
                    .Include(u => u.GuestAtWeddings)
                        .ThenInclude(w => w.Event)
                    .SingleOrDefault();
                ViewBag.AllWeddings = _context.Weddings
                .Where(w=>w.EventDate>System.DateTime.Now)
                    .OrderBy(d=>d.EventDate)
                    .Include(w => w.GuestsAttending)
                        .ThenInclude(g => g.Guest)
                    .Include(u => u.Owner)
                    .ToList();
                ViewBag.User = CurrentUser;
                return View("dashboard");
            }
            [HttpGet]
            [RouteAttribute("ViewEvent")]
        public IActionResult ViewEvent()
            {
                ViewBag.Errors = new List<string>();
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index", "User");
                }
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .SingleOrDefault();
                Wedding CurrentWedding = _context.Weddings.Where(w => w.WeddingId == (int) TempData["WeddingId"])
                    .Include(U => U.GuestsAttending)
                        .ThenInclude(G => G.Guest)
                    .Include(w => w.Owner)
                    .SingleOrDefault();
                ViewBag.Event = CurrentWedding;
                ViewBag.User = CurrentUser;
                ViewBag.Pending = (ViewBag.Event.GuestsAttending.Count - ViewBag.Event.RSVPCount());
                return View("viewEvent");
            }
            [HttpGet]
            [RouteAttribute("ViewEvent/{id}")]
        public IActionResult ViewEvent(int id)
            {
                ViewBag.Errors = new List<string>();
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index", "User");
                }
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .SingleOrDefault();
                Wedding CurrentWedding = _context.Weddings.Where(w => w.WeddingId == id)
                    .Include(U => U.GuestsAttending)
                        .ThenInclude(G => G.Guest)
                    .Include(w => w.Owner)
                    .SingleOrDefault();
                ViewBag.Event = CurrentWedding;
                ViewBag.User = CurrentUser;
                ViewBag.Pending = (ViewBag.Event.GuestsAttending.Count - ViewBag.Event.RSVPCount());
                return View("viewEvent");
            }
            [HttpGet]
            [RouteAttribute("AddEvent")]
        public IActionResult AddEvent()
            {
                List<string> Errors = new List<string>();
                try
                {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>>("Errors");
                    foreach(object error in Results)
                    {
                        Errors.Add(error.ToString());
                    }
                }
                catch { }
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .SingleOrDefault();
                ViewBag.User = CurrentUser;
                ViewBag.Errors = Errors;
                return View("createEvent");
            }
            [HttpPost]
            [RouteAttribute("AddEvent")]
        public IActionResult AddEvent(WeddingValidation NewWedding)
            {
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId")).SingleOrDefault();
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index", "User");
                }
                if (ModelState.IsValid)
                {
                    Wedding CurrentWedding = NewWedding.ToWedding();
                    CurrentWedding.Owner = CurrentUser;
                    _context.Add(CurrentWedding);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                List<string> Errors = new List<string>();
                Dictionary<string, string> Error = new Dictionary<string, string>();
                foreach(string key in ViewData.ModelState.Keys)
                {
                    foreach(ModelError error in ViewData.ModelState[key].Errors)
                    {
                        Errors.Add(error.ErrorMessage);
                    }
                }
                HttpContext.Session.SetObjectAsJson("Errors", Errors);
                return RedirectToAction("AddEvent");
            }
            [HttpPost]
            [RouteAttribute("AddRSVP")]
        public IActionResult AddRSVP(bool Side, int id)
            {
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .SingleOrDefault();
                Wedding CurrentWedding = _context.Weddings.Where(w => w.WeddingId == id)
                    .SingleOrDefault();
                if (_context.WeddingGuests.Where(g => g.GuestId == CurrentUser.UserId).Where(w => w.EventId == CurrentWedding.WeddingId).Count() == 0)
                {
                    WeddingGuest NewGuest = new WeddingGuest(CurrentWedding, CurrentUser, Side);
                    _context.Add(NewGuest);
                    _context.SaveChanges();
                }
                else
                {
                    WeddingGuest NewGuest = _context.WeddingGuests.Where(g => g.GuestId == CurrentUser.UserId).Where(w => w.EventId == CurrentWedding.WeddingId).SingleOrDefault();
                    NewGuest.GuestOfSideA = Side;
                    _context.SaveChanges();
                }
                TempData.Add("WeddingId", CurrentWedding.WeddingId);
                return RedirectToAction("ViewEvent");
            }
            [HttpGet]
            [RouteAttribute("RemoveRSVP/{id}")]
        public IActionResult RemoveRSVP(int id)
            {
                User CurrentUser = _context.Users.Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .SingleOrDefault();
                Wedding CurrentWedding = _context.Weddings.Where(w => w.WeddingId == id)
                    .SingleOrDefault();
                List<WeddingGuest> NewGuest = _context.WeddingGuests.Where(w => w.GuestId == CurrentUser.UserId)
                    .Where(w => w.EventId == id).ToList();
                _context.WeddingGuests.RemoveRange(NewGuest);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            [HttpPost]
            [RouteAttribute("ConfirmRSVP")]
        public IActionResult ConfirmRSVP(int GuestId, int EventId)
        {
            User Guest = _context.Users.Where(u => u.UserId == GuestId)
                .SingleOrDefault();
            Wedding CurrentWedding = _context.Weddings.Where(w => w.WeddingId == EventId)
                .SingleOrDefault();
            WeddingGuest NewGuest = _context.WeddingGuests.Where(w => w.GuestId == Guest.UserId)
                .Where(w => w.EventId == CurrentWedding.WeddingId).SingleOrDefault();
            NewGuest.Pending = false;
            _context.SaveChanges();
            TempData.Add("WeddingId", CurrentWedding.WeddingId);
            return RedirectToAction("ViewEvent");
        }
    }
}