using PizzeriaExpress.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PizzeriaExpress.Controllers
{
    [Authorize]
    public class ProdottiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Prodotti
        public ActionResult Index()
        {
            return View(db.Prodotti.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddToCart(int id, int Quantita)
        {
            var prodotto = db.Prodotti.Find(id);
            if (prodotto != null)
            {
                var cart = Session["cart"] as List<Prodotti> ?? new List<Prodotti>();
                prodotto.Quantita = Quantita;
                if (cart.Any(p => p.IdProdotto == id))
                {
                    var productInCart = cart.First(p => p.IdProdotto == id);
                    productInCart.Quantita += Quantita;
                }
                else
                {
                    cart.Add(prodotto);
                }
                Session["cart"] = cart;
                TempData["AddCart"] = "Prodotto aggiunto al carrello";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Search")]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string productName)
        {
            var prodotti = db.Prodotti.Where(p => p.Nome.Contains(productName)).ToList();
            return View("Index", prodotti);
        }
    }
}
