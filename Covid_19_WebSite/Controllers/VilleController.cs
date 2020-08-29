using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Covid_19_WebSite.Filters;
using Covid_19_WebSite.Models;

namespace Covid_19_WebSite.Controllers
{

    [AuthorizedAttr(Roles = "admin,assisstant")]
    public class VilleController : Controller
    {
        private COVID_19_DBEntities db = new COVID_19_DBEntities();

        // GET: Ville
        public ActionResult Index(string value = "")
        {
            if (value == "")
                return View(db.Villes.OrderBy(x=> x.Nom).ToList());
            else
            {
                var vi = db.Villes.Where(v => v.Nom.ToLower().Contains(value.ToLower())).OrderBy(x => x.Nom).ToList();
                return View(vi);
            }
        }

        // GET: Ville/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ville ville = db.Villes.Find(id);
            if (ville == null)
            {
                return HttpNotFound();
            }
            return View(ville);
        }

        // GET: Ville/Create
        public ActionResult Create()
        {
            ViewBag.UID_Region = new SelectList(db.Regions, "UID", "Nom");
            return View();
        }

        // POST: Ville/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,Nom,UID_Region")] Ville ville)
        {
            ville.UID = GestionGlobal.GetUID("VIL");
            if (ModelState.IsValid)
            {
                db.Villes.Add(ville);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.UID_Region = new SelectList(db.Regions, "UID", "Nom", ville.UID_Region);
            return View(ville);
        }

        // GET: Ville/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ville ville = db.Villes.Find(id);
            if (ville == null)
            {
                return HttpNotFound();
            }
            ViewBag.UID_Region = new SelectList(db.Regions, "UID", "Nom", ville.UID_Region);
            return View(ville);
        }

        // POST: Ville/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,Nom,UID_Region")] Ville ville)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ville).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UID_Region = new SelectList(db.Regions, "UID", "Nom", ville.UID_Region);
            return View(ville);
        }

        // GET: Ville/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ville ville = db.Villes.Find(id);
            if (ville == null)
            {
                return HttpNotFound();
            }
            return View(ville);
        }

        // POST: Ville/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Ville ville = db.Villes.Find(id);
            db.Villes.Remove(ville);
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
    }
}
