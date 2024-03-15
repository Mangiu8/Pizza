﻿using PizzeriaExpress.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace PizzeriaExpress.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string psw)
        {
            using (var context = new ModelDBContext())
            {
                var user = context.Utenti.FirstOrDefault(u => u.Email == email && u.Psw == psw);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    if (user.isAdmin)
                    {
                        TempData["Login"] = "Benvenuto/a " + user.Nome + " " + user.Cognome + " [Admin]";
                        return RedirectToAction("Index", "GestioneProdotti");
                    }
                    else
                    {
                        TempData["Login"] = "Benvenuto/a " + user.Nome + " " + user.Cognome;
                        return RedirectToAction("Index", "Prodotti");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email o password errati");
                    return View();
                }
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(Utenti utente)
        {
            using (var context = new ModelDBContext())
            {
                context.Utenti.Add(utente);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Prodotti");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            TempData["LogoutMess"] = "Logout effettuato con successo";
            return RedirectToAction("Index");
        }
    }
}
