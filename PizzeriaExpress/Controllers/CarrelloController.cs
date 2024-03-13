using PizzeriaExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PizzeriaExpress.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Carrello

        public ActionResult Index()
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Prodotti");
            }
            return View(cart);
        }

        public ActionResult Delete(int? id)
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null)
            {
                var productToRemove = cart.FirstOrDefault(p => p.IdProdotto == id);
                if (productToRemove != null)
                {
                    cart.Remove(productToRemove);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo)
        {
            ModelDBContext db = new ModelDBContext();
            var userId = db.Utenti.FirstOrDefault(u => u.Email == User.Identity.Name).IdUtente;

            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null && cart.Any())
            {
                Ordini newOrder = new Ordini();
                newOrder.Data = DateTime.Now;
                newOrder.Evaso = false;
                newOrder.IdUtente_FK = userId;
                newOrder.Indirizzo = indirizzo;
                newOrder.Totale = cart.Sum(p => p.Prezzo);
                newOrder.Note = note;

                db.Ordini.Add(newOrder);
                db.SaveChanges();

                foreach (var product in cart)
                {
                    Dettagli newDetail = new Dettagli();
                    newDetail.IdOrdine_FK = newOrder.IdOrdine;
                    newDetail.IdProdotto_FK = product.IdProdotto;
                    newDetail.Quantita = 1;

                    db.Dettagli.Add(newDetail);
                    db.SaveChanges();
                }
                cart.Clear();
            }
            return RedirectToAction("Index", "Prodotti");
        }
    }
}
