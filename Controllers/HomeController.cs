using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cexam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace cexam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext context;
        public HomeController(MyContext mc)
        {
            context = mc;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Name");
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email exists in database.");
            }
            if(ModelState.IsValid)
            {
                var hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);
                context.Users.Add(user);
                context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Name", user.Name);
                return Redirect("/home");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser Data)
        {
            User userInDb = context.Users.FirstOrDefault(userlog => userlog.Email == Data.LoginEmail);
            if(userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Email not found!");
            } 
            else
            {
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(Data, userInDb.Password, Data.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid password");
                }
            }
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            HttpContext.Session.SetString("Name", userInDb.Name);
            return Redirect("/home");
        }
        [HttpGet("new")]
        public IActionResult makenew()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Redirect("/");
            }
            return View("New");
        }

        [HttpGet("home")]
        public IActionResult home()
        {
        int? userId = HttpContext.Session.GetInt32("UserId");
        string username = HttpContext.Session.GetString("Name");
            if (userId == null)
            {
                return Redirect("/");
            }
            List<List> Parties = context.Lists
            .Include(p => p.Creator)
            .Include(p => p.Attendees)
            .OrderBy(p => p.ActDate).ToList();
            for(int i = 0; i < Parties.Count; i++)
            {
                if(Parties[i].ActDate < DateTime.Now)
                {
                    Parties.Remove(Parties[i]);
                }
            }
            ViewBag.Lists = Parties;
            ViewBag.UserId = userId;
            ViewBag.Name = username;
            return View("home");            
        }

        [HttpPost("create")]
        public IActionResult create(List p)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                p.CreatorId = (int) UserId;
                context.Lists.Add(p);
                context.SaveChanges(); 
                return Redirect("/home");
            }
            else{
                return View("New", p);
            }
        }

        [HttpGet("activity/{PartyId}")]
        public IActionResult ShowActivity(int PartyId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("/");
            }
            ViewBag.activity = context.Lists
                .Include(acti => acti.Creator)
                .Include(acti => acti.Attendees)
                .ThenInclude(acti => acti.Joiner)
                .FirstOrDefault(acti => acti.PartyId == PartyId);
            return View("Eventdata");
        }

        [HttpGet("join/{PartyId}")]
        public IActionResult Join(int partyId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("/");
            }
            
            Join i = new Join()
            {
                UserId = (int)UserId,
                PartyId = partyId
            };
            context.Joins.Add(i);
            context.SaveChanges();
            
            return Redirect("/home");
            
        }

        [HttpGet("delete/{PartyId}")]
        public IActionResult Delete(int PartyId)
        {
            List p = context.Lists.FirstOrDefault(remove => remove.PartyId == PartyId);
            context.Lists.Remove(p);
            context.SaveChanges();
            return Redirect("/home");
        }

        [HttpGet("leave/{PartyId}")]
        public IActionResult Leave(int partyId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("/");
            }
            Join join =context.Joins
                .Where(j=> j.PartyId == partyId)
                .FirstOrDefault(j => j.UserId == (int) UserId);
            context.Joins.Remove(join);
            context.SaveChanges();
            return Redirect("/home");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Name");
            return Redirect("/");
        }
    }
}