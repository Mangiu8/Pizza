using PizzeriaExpress.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PizzeriaExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GestioneProdottiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();


        public ActionResult Index()
        {
            return View(db.Prodotti.ToList());
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProdotto,Nome,Foto,Foto2,Foto3,Prezzo,Consegna,Ingredienti")] Prodotti prodotti)
        {
            if (ModelState.IsValid)
            {
                TempData["CreateMessage"] = "Prodotto " + prodotti.Nome + " creato con successo!";
                db.Prodotti.Add(prodotti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prodotti);
        }


        public ActionResult Edit(int? id)
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

        // POST: GestioneProdotti/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProdotto,Nome,Foto,Foto2,Foto3,Prezzo,Consegna,Ingredienti")] Prodotti prodotti)
        {
            if (ModelState.IsValid)
            {
                TempData["EditMessage"] = "Prodotto " + prodotti.Nome + " modificato con successo!";
                db.Entry(prodotti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prodotti);
        }

        // GET: GestioneProdotti/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: GestioneProdotti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["DeleteMessage"] = "Prodotto eliminato con successo!";
            Prodotti prodotti = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotti);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
