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
                TempData["CartEmpty"] = "Il carrello è vuoto";
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
                    if (productToRemove.Quantita > 1)
                    {
                        productToRemove.Quantita--;
                    }
                    else
                    {
                        cart.Remove(productToRemove);
                    }
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
                    newDetail.Quantita = Convert.ToInt32(product.Quantita);
                    db.Dettagli.Add(newDetail);
                    db.SaveChanges();
                }
                cart.Clear();
            }
            TempData["Ordine"] = "L'ordine è stato inviato correttamente";
            return RedirectToAction("Index", "Prodotti");
        }
    }
}
