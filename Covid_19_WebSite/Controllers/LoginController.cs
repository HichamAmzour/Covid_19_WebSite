using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Covid_19_WebSite.Filters;
using Covid_19_WebSite.Models;

namespace Covid_19_WebSite.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        COVID_19_DBEntities db = new COVID_19_DBEntities();
        static bool isAuthanticated=true;
        public object Alert { get; private set; }

        // GET: Login
       
        [HttpGet]
        public ActionResult Index()
        {
            
            bool isNotAllowd;
            bool.TryParse(Request.QueryString["err"],out isNotAllowd);
            if (!isAuthanticated)
            {
                ViewBag.msg = "Le mot de pass ou le nom d'utilisateur est incorrect !!!";
                ViewBag.Class = "alert alert-danger";
            }
            else if (isNotAllowd)
            {
                ViewBag.msg = "Vous éte pas autorisé de consulter cette page !!!";
                ViewBag.Class = "alert alert-danger";
            }
            else
            {
                ViewBag.msg = "";
                ViewBag.Class = "";
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult Connecter(string util,string pass)
        {
            RolesProvider roleManager = new RolesProvider();
            if (ModelState.IsValid)
            {
                //dynamic Alert = new ExpandoObject();
                var U = db.Utilisateurs.Where(x => x.Mot_Pass == pass && x.pseudo == util).FirstOrDefault();
                if ( U != null)
                {
                    isAuthanticated = true;
                    Session["utilisateur"] = U.pseudo;
                    Session["utilisateurUID"] = U.UID;
                    //Microsoft.AspNet.Identity applicationIdentity = new Microsoft.AspNet.Identity();
                    Session["Role"] = roleManager.GetRoleOfUser(U.UID);

                    return RedirectToAction("Index", "Acceuille", U.Email);
                }
                else
                {
                    isAuthanticated = false;   
                    return RedirectToAction("Index","Login");
                }
            }else
            {
                    isAuthanticated = false;
                    return RedirectToAction("Index", "Login",new Utilisateur());
            }
        }
        [HttpGet]
        public ActionResult AjouterUtil()
        {
            return View();
        }

        public ActionResult deconnecter()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
    }
}