using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Covid_19_WebSite.Filters;
using Covid_19_WebSite.Models;
using Newtonsoft.Json;

namespace Covid_19_WebSite.Controllers
{
    [AuthorizedAttr(Roles = "admin,assisstant")]
    public class EnumerationsController : Controller
    {
        private COVID_19_DBEntities db = new COVID_19_DBEntities();

        // GET: Enumerations
        public ActionResult Index(string ville="",string region="",string date="")
        {

            string q = $@"SELECT DISTINCT E.* FROM Enumeration E 
                            JOIN Ville v ON v.UID=E.UID_V 
                            JOIN REGION r ON r.UID=v.UID_Region 
                            WHERE Date_ins like '%{date}%' and Lower(v.Nom) like '%{ville.ToLower()}%' and lower(r.Nom) like '%{region.ToLower()}%'";

            var en = db.Enumerations.SqlQuery(q);

            return View(en.ToList().OrderByDescending(x=>x.Date_Ins));
            
        }

        // GET: Enumerations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumeration enumeration = db.Enumerations.Find(id);
            if (enumeration == null)
            {
                return HttpNotFound();
            }
            return View(enumeration);
        }

        // GET: Enumerations/Create
        public ActionResult Create()
        {
            ViewData.Add("ListRegions",  new SelectList(db.Regions, "UID", "Nom"));
            return View();
        }

        // POST: Enumerations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,Cas_Mort,Cas_Retablis,Cas_Confirmer,Cas_Total,Date_Ins,UID_V")] Enumeration enumeration)
        {
            RegionModel reg = new RegionModel();
            if (ModelState.IsValid)
            {
                enumeration.UID = GestionGlobal.GetUID("ENU");
                db.Enumerations.Add(enumeration);

                db.SaveChanges();

                // add statstics of region into regionStatiqtics Table
                reg.AjouterStatistique(enumeration);

                return RedirectToAction("Index");
            }
            ViewData.Add("ListRegions", new SelectList(db.Regions, "UID", "Nom"));

            return View(enumeration);
        }

        // GET: Enumerations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumeration enumeration = db.Enumerations.Find(id);
            if (enumeration == null)
            {
                return HttpNotFound();
            }
            return View(enumeration);
        }

        // POST: Enumerations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,Cas_Mort,Cas_Retablis,Cas_Confirmer,Cas_Total,Date_Ins,UID_V")] Enumeration enumeration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enumeration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enumeration);
        }

        // GET: Enumerations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumeration enumeration = db.Enumerations.Find(id);
            if (enumeration == null)
            {
                return HttpNotFound();
            }
            return View(enumeration);
        }

        // POST: Enumerations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Enumeration enumeration = db.Enumerations.Find(id);
            db.Enumerations.Remove(enumeration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Villes_Region(string UID_R)
        {
            var lsVilles = (from v in db.Villes where v.UID_Region == UID_R select new { v.Nom, v.UID }).ToList();
            //Json(new { listVille = lsVilles }, JsonRequestBehavior.AllowGet);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var json = JsonConvert.SerializeObject(lsVilles, Formatting.Indented);
            //jss.Serialize(lsVilles.ToList());
            ViewBag.villes = lsVilles;
            return Json(json, JsonRequestBehavior.AllowGet);
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
