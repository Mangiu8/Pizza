using PizzeriaExpress.Models;
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
    }
}
