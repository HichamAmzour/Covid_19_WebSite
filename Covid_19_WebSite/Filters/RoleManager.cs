using Covid_19_WebSite.Controllers;
using Covid_19_WebSite.Interfaces;
using Covid_19_WebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Covid_19_WebSite.Filters
{

    public class RoleManager : IRoleManager
    {

        COVID_19_DBEntities db;

        public RoleManager()
        {
            this.db = new COVID_19_DBEntities();
        }

        public bool AssingRoleToUser(string UID_Role, string UID_User)
        {
            UserManager userManager = new UserManager();
            Role role = GetRole(UID_Role);
            Utilisateur user =  userManager.SearchUser(UID_User);
            Role_Util rolUtil = new Role_Util() {
                UID = GestionGlobal.GetUID("RUT"),
                Date=DateTime.Now,
                UID_Role=role.UID,
                UID_Utilisateur=user.UID,
            };

            db.Role_Util.Add(rolUtil);
            int saved = db.SaveChanges();
            return saved > 0;
        }

        public Role GetRoleUtilisateur(string UID_Ru)
        {
            var rolU = db.Role_Util.Find(UID_Ru);
            return db.Roles.Find(rolU.UID_Role);
        }

        public bool CreateRole(string RoleName)
        {
            string UID = GestionGlobal.GetUID("ROL");
            Role role = new Role();
            role.Nom_Role = RoleName;
            role.UID = UID;
            db.Roles.Add(role);
            int x = db.SaveChanges();

            return x>0;
        }

        public List<Role> GetListOfRoles()
        {
            return db.Roles.ToList();
        }

        public Role GetRole(string UID)
        {
            return  db.Roles.Where(r => r.UID == UID).FirstOrDefault();
        }

        public string GetRoleName(string roleId)
        {
            var role =  db.Roles.Where(r => r.UID==roleId).FirstOrDefault();
            return role.Nom_Role ?? null;
        }

        public bool RemoveRole(string UID)
        {
            var role_Utils = db.Role_Util.Where(x => x.UID_Role == UID).ToList();
            if (role_Utils.Count > 0)
            {
                foreach (var item in role_Utils)
                {
                    var ru = db.Role_Util.Remove(item);
                }
                db.SaveChanges();

            }
            Role r =  GetRole(UID);
            db.Roles.Remove(r);
            int deleted = db.SaveChanges();
            return deleted > 0;
        }

        public bool RoleExists(string RoleName)
        {
            var role =  db.Roles.Where(x => x.Nom_Role.ToLower() == RoleName.ToLower()).FirstOrDefault();
            return role != null ? true : false;
        }

        public bool RoleModify(string UID, string NewRoleName)
        {
            Role role=  db.Roles.Where(x => x.UID == UID).FirstOrDefault();
            role.Nom_Role = NewRoleName;
            int saved =  db.SaveChanges();
            return saved > 0;
        }

        public  bool UserHasRole(string RoleName, string UID_User)
        {
            Role_Util roleUser =  db.Role_Util.Where(u => u.UID_Utilisateur == UID_User && u.Role.Nom_Role == RoleName).FirstOrDefault();
            return roleUser != null ? true : false; 
        }
       
    }
}