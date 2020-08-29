using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Covid_19_WebSite.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizedAttr : AuthorizeAttribute
    {
        COVID_19_DBEntities db;
        RolesProvider rolesProvider;
        public AuthorizedAttr()
        {
            db = new COVID_19_DBEntities();
            rolesProvider = new RolesProvider();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string [] roleName = (string[]) httpContext.Session["Role"];
            string userName =(string) httpContext.Session["utilisateur"];
            string[] claimedRoles = this.Roles.Split(',');
            bool isAuthorized = false;

            var roles = rolesProvider.GetRolesForUser(userName);
            foreach (string item in roles)
            {
                isAuthorized = claimedRoles.Where(x => x == item).FirstOrDefault()!=null;
            }

            if (!isAuthorized)
            {
                httpContext.Response.Redirect("/Login?err=true");
            }

            return isAuthorized;
        }

    }
}