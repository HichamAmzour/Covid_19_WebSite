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
    public class Role_UtilController : Controller
    {
        private COVID_19_DBEntities db = new COVID_19_DBEntities();

        // GET: Role_Util/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index","Access", new{roleUtili=id });
        }

        // POST: Role_Util/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string UID_Role, string UID_User,string Role_Util)
        {
            string message = "La modification de cette d'autorisation n'été pas effectuer !!";
            if (ModelState.IsValid)
            {
                Role_Util role_Util = db.Role_Util.Find(Role_Util);
                role_Util.UID_Role = UID_Role;
                role_Util.UID_Utilisateur = UID_User;
                role_Util.Date = DateTime.Now;
                db.Entry(role_Util).State = EntityState.Modified;
                int x=db.SaveChanges();
                if (x > 0)
                    message = "La modification de cette d'autorisation effectuer !!";
                return RedirectToAction("Index", "Access", new { msg = message });
            }
           
            return RedirectToAction("Index","Access",new { msg=message });
        }

        
        // POST: Role_Util/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            string message = "La suppression à n'été pas effectuer !!";

            Role_Util role_Util = db.Role_Util.Find(id);
            db.Role_Util.Remove(role_Util);
            int x= db.SaveChanges();
            if (x > 0)
                message = "La suppression effectuer !!";
            return RedirectToAction("Index", "Access", new { msg = message });
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
