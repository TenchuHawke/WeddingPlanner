using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using WeddingPlanner.Models;
namespace WeddingPlanner.Controllers
{
    public class UserController : Controller
    {
        private BaseContext _context;
        public UserController(BaseContext context)
            {
                _context = context;
            }
            [HttpGet]
            [Route("")]
        public IActionResult Index()
            {
                return RedirectToAction("Landing");
            }
            [HttpGet]
            [Route("Landing")]
        public IActionResult Landing()
            {
                List<string> Errors = new List<string>();
                try
                {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>>("Errors");
                    foreach(string error in Results)
                    {
                        Errors.Add(error.ToString());
                    }
                    HttpContext.Session.Remove("Errors");
                }
                catch { }
                ViewBag.Errors = Errors;
                return View("Index");
            }
            [HttpPost]
            [Route("Landing")]
        public IActionResult Landing(string Username)
            {
                List<string> Errors = new List<string>();
                if ((Username == null) || (Username.Length < 2))
                {
                    if (Username == null)
                    {
                        Errors.Add("Username cannot be blank");
                    }
                    else
                    {
                        Errors.Add("Username must contain at least 2 characters.");
                    }
                }
                else
                {
                    User CurrentUser = _context.Users.Where(u => u.Name.ToLower() == Username.ToLower())
                        .SingleOrDefault();
                    if (CurrentUser == null)
                    {
                        CurrentUser = new User();
                        CurrentUser.Name = Username;
                        CurrentUser.Registered = false;
                        _context.Add(CurrentUser);
                        _context.SaveChanges();
                        CurrentUser = _context.Users
                            .Where(u => u.Name == Username)
                            .SingleOrDefault();
                        HttpContext.Session.SetInt32("UserId", CurrentUser.UserId);
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", CurrentUser.UserId);
                        if (CurrentUser.Registered)
                        {
                            return RedirectToAction("Login");
                        }
                    }
                    return RedirectToAction("Choice");
                }
                HttpContext.Session.SetObjectAsJson("Errors", Errors);
                return RedirectToAction("Landing");
            }
            [HttpGetAttribute]
            [RouteAttribute("Choice")]
        public IActionResult Choice()
            {
                List<string> Errors = new List<string>();
                try
                {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>>("Errors");
                    foreach(string error in Results)
                    {
                        Errors.Add(error.ToString());
                    }
                    HttpContext.Session.Remove("Errors");
                }
                catch { }
                ViewBag.Errors = Errors;
                User CurrentUser = _context.Users
                    .Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .Single();
                ViewBag.User = CurrentUser;
                return View();
            }
            [HttpPostAttribute]
            [RouteAttribute("Choice")]
        public IActionResult Choice(string Register)
            {
                if (Register == "true")
                {
                    return RedirectToAction("Register");
                }
                else
                {
                    return RedirectToAction("Dashboard", "WeddingEvent");
                }
            }
            [HttpGet]
            [Route("Login")]
        public IActionResult Login()
            {
                List<string> Errors = new List<string>();
                try
                {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>>("Errors");
                    foreach(string error in Results)
                    {
                        Errors.Add(error.ToString());
                    }
                    HttpContext.Session.Remove("Errors");
                }
                catch { }
                User CurrentUser = _context.Users
                    .Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .Single();
                ViewBag.User = CurrentUser;
                ViewBag.Errors = Errors;
                return View("login");
            }
            [HttpPost]
            [Route("Login")]
        public IActionResult Login(string Password)
            {
                List<string> Errors = new List<string>();
                if (Password == null)
                {
                    Errors.Add("Password can not be blank.");
                }
                if (Errors.Count == 0)
                {
                    User CurrentUser = _context.Users
                        .Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                        .Single();
                    if (CurrentUser.Password == Password)
                    {
                        return RedirectToAction("Dashboard", "WeddingEvent");
                    }
                    Errors.Add("Invalid Email / Password Combination.");
                }
                HttpContext.Session.SetObjectAsJson("Errors", Errors);
                return RedirectToAction("Login");
            }
            [HttpGet]
            [Route("Register")]
        public IActionResult Register()
            {
                List<string> Errors = new List<string>();
                try
                {
                    List<string> Results = HttpContext.Session.GetObjectFromJson<List<string>>("Errors");
                    foreach(object error in Results)
                    {
                        Errors.Add(error.ToString());
                    }
                    HttpContext.Session.Remove("Errors");
                }
                catch { }
                User CurrentUser = _context.Users
                    .Where(u => u.UserId == (int) HttpContext.Session.GetInt32("UserId"))
                    .Single();
                ViewBag.User = CurrentUser;
                ViewBag.Errors = Errors;
                return View("reg");
            }
            [HttpPost]
            [Route("Register")]
        public IActionResult Register(UserValidation user)
            {
                List<string> Errors = new List<string>();
                if (ModelState.IsValid)
                {
                    User Results = _context.Users.Where(u => u.Name == user.Name).SingleOrDefault();
                    Results.Email = user.Email;
                    Results.Password = user.Password;
                    Results.Registered = true;
                    Results.UpdatedAt = System.DateTime.Now;
                    _context.SaveChanges();
                    Results = _context.Users.Where(u => u.Name == user.Name).SingleOrDefault();
                    HttpContext.Session.SetInt32("UserId", Results.UserId);
                    return RedirectToAction("Dashboard", "WeddingEvent");
                }
                else
                {
                    Dictionary<string, string> Error = new Dictionary<string, string>();
                    foreach(string key in ViewData.ModelState.Keys)
                    {
                        foreach(ModelError error in ViewData.ModelState[key].Errors)
                        {
                            Errors.Add(error.ErrorMessage);
                        }
                    }
                    HttpContext.Session.Remove("Errors");
                }
                HttpContext.Session.SetObjectAsJson("Errors", Errors);
                return RedirectToAction("Register");
            }
            [HttpGet]
            [RouteAttribute("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Landing");
        }
    }
}