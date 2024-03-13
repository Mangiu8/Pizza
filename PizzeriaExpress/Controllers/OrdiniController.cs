using PizzeriaExpress.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PizzeriaExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdiniController : Controller
    {
        private ModelDBContext db = new ModelDBContext();


        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Utenti);
            return View(ordini.ToList());
        }
        public ActionResult isEvaso(int id)
        {
            Ordini ordine = db.Ordini.Find(id);
            if (ordine.Evaso == false)
            {
                ordine.Evaso = true;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ordine.Evaso = false;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
