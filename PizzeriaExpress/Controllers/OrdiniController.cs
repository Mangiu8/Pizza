using PizzeriaExpress.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
                TempData["Message1"] = "L'ordine è stato evaso";
                ordine.Evaso = true;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                TempData["Message2"] = "Evasione annullata";
                ordine.Evaso = false;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> GetNumeroOrdini()
        {

            int totale = await db.Ordini.Where(o => o.Evaso == true).CountAsync();


            return Json(totale, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> IncassatoPerGiorno(DateTime data)
        {
            decimal incasso = await db.Ordini
                .Where(o => o.Data.Year == data.Year && o.Data.Month == data.Month && o.Data.Day == data.Day)
                .SumAsync(o => o.Totale);
            return Json(incasso, JsonRequestBehavior.AllowGet);
        }

    }
}
