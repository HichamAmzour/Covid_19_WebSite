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
    [AuthorizedAttr(Roles = "admin")]
    public class UtilisateurController : Controller
    {
        private COVID_19_DBEntities db;
        private UserManager userManager;
        public UtilisateurController()
        {
            this.db = new COVID_19_DBEntities();
            userManager = new UserManager();
        }

        // GET: Utilisateur
        public ActionResult Index(string user="", string email="")
        {
            string q = $@"SELECT * FROM Utilisateur WHERE Lower(pseudo) like '%{user.ToLower()}%' and Lower(email) like '%{email.ToLower()}%'";

            var lsUtil = db.Utilisateurs.SqlQuery(q);
            var msg = Request.QueryString["success"];
            if ( msg!= null)
            {
                ViewBag.msg = bool.Parse(msg) ? msg : "";
                ViewBag.alert = bool.Parse(msg) ? "alert alert-success" : "alert alert-danger";
                ViewBag.style = bool.Parse(msg) ? "" : "display:none";
            }
            else
            {
                ViewBag.style = "display:none";
            }

            return View(lsUtil.OrderByDescending(x => x.pseudo).Reverse().ToList());

        }

        // GET: Utilisateur/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = userManager.SearchUser(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateur/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Utilisateur/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pseudo,Mot_Pass,Email")] Utilisateur utilisateur)
        {
            utilisateur.UID = GestionGlobal.GetUID("UTI");
            if (ModelState.IsValid)
            {
                userManager.CreateUser(utilisateur);
                return RedirectToAction("Index");
            }

            return View(utilisateur);
        }

        // GET: Utilisateur/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = userManager.SearchUser(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateur/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,pseudo,Mot_Pass,Email")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                userManager.ModifyUser(utilisateur);
                return RedirectToAction("Index");
            }
            return View(utilisateur);
        }

        // GET: Utilisateur/Delete/5
        // NOT : Problem de suppression  ..
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur =userManager.SearchUser(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateur/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
           bool isGood= userManager.DeleteUser(id);

            return RedirectToAction("Index",new { success=isGood });
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
