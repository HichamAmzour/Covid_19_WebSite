using Covid_19_WebSite.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Covid_19_WebSite.Controllers
{
    [AuthorizedAttr(Roles = "admin,assisstant")]
    public class AcceuilleController : Controller
    {
        // GET: Acceuille
       
        public ActionResult Index()
        {
            bool isAuth = User.Identity.IsAuthenticated;
            return View();
        }
    }
}
