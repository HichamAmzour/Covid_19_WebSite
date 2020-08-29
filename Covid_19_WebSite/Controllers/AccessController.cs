using Covid_19_WebSite.Filters;
using Covid_19_WebSite.Interfaces;
using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Covid_19_WebSite.Controllers
{
    [AuthorizedAttr(Roles="admin")]
    public class AccessController : Controller
    {
        RoleManager roleManager;
        RoleUserModel roleUserModel;
        UserManager userManager;
        string message="";
        public AccessController()
        {
            roleManager = new RoleManager();
            roleUserModel = new RoleUserModel();
            userManager = new UserManager();
        }

        // GET: Access
        public ActionResult Index(string id="",string roleUtili="")
        {
            ViewBag.modifyRole = false;
            ViewBag.modifyRoleUtil = false;
            message = Request.QueryString["msg"];
            if (message != "" && message!=null)
            {
                ViewBag.style = "";
                ViewBag.msg = message;
            }
            else
            {
                ViewBag.style = "display:none";
                ViewBag.msg = "";

            }

            roleUserModel.listeRoles= roleManager.GetListOfRoles();
            roleUserModel.listeUtilisateurs = userManager.ListOfUsers();
            roleUserModel.listeRolesUtilis = userManager.GetListOfRolesUsers();
            if (id != "")
            {
                ViewBag.modifyRole = true;
                roleUserModel.role = roleManager.GetRole(id);
            }
            if (roleUtili != "")
            {
                roleUserModel.role = roleManager.GetRoleUtilisateur(roleUtili);
                roleUserModel.utilisateur = userManager.GetRoleUtilisateur(roleUtili);
                roleUserModel.Role_Util = roleUtili;
                ViewBag.modifyRoleUtil = true;
            }


            return View(roleUserModel);
        }

        public ActionResult Assign(string UID_User, string UID_Role)
        {
            bool success = roleManager.AssingRoleToUser(UID_Role, UID_User);
            return RedirectToAction("Index",success);

        }

        // GET: Access/Details/5
        public ActionResult Details(string id)
        {
            Role role = roleManager.GetRole(id) ?? new Role();
            return View(role);
        }

        // POST: Access/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Nom_Role)
        {
            message = "Erreur d'ajout de type d'autorisations !!";
            // TODO: Add insert logic here
            bool saved= roleManager.CreateRole(Nom_Role);
            if (saved)
                message= "le type d'autorisation a été ajouter avec success";

            return RedirectToAction("Index", new { msg=message });
           
        }

        // GET: Access/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return RedirectToAction("Index",new { id });
        }

        // POST: Access/Edit/5
        
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(string id, string Nom_Role)
        {
            message = "La modification de cette d'autorisations n'été pas effectuer !!";
            try
            {
                // TODO: Add update logic here
                bool modified=roleManager.RoleModify(id, Nom_Role);
                if (modified)
                    message = "La modification de cette d'autorisations effectuer !!";

                return RedirectToAction("Index", new { msg = message });
            }
            catch
            {
                return View();
            }
        }


        // POST: Access/Delete/5
        public ActionResult Delete(string id)
        {
            message = "La suppression de cette d'autorisation n'été pas effectuer !!";
            try
            {
                // TODO: Add delete logic here
                bool deleted = roleManager.RemoveRole(id);
                if (deleted)
                    message = "La suppression de cette d'autorisation effectuer !!";
                return RedirectToAction("Index", new { msg = message });
            }
            catch
            {
                return View();
            }
        }
    }
}
