using Covid_19_WebSite.Controllers;
using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Covid_19_WebSite.Filters
{
    public class RolesProvider : RoleProvider
    {
        COVID_19_DBEntities db;

        public RolesProvider()
        {
            db = new COVID_19_DBEntities();
        }

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            UserManager userManager = new UserManager();
            Role role = db.Roles.Where(x => x.Nom_Role==roleNames[0]).FirstOrDefault();
            Utilisateur user = db.Utilisateurs.Where(x => x.pseudo == usernames[0]).FirstOrDefault();
            Role_Util rolUtil = new Role_Util()
            {
                UID = GestionGlobal.GetUID("RUT"),
                Date = DateTime.Now,
                Role = role,
                Utilisateur = user,
            };

             db.Role_Util.Add(rolUtil);
             db.SaveChanges();
          
        }

        public string [] GetRoleOfUser(string userUID)
        {
            var rl = db.Role_Util.Where(x => x.UID_Utilisateur == userUID).ToList();
            var rlUser = (from r in rl join rol in db.Roles on r.UID_Role equals rol.UID select rol.Nom_Role).ToArray();
            return rlUser ?? new string[0];
        }

        public override void CreateRole(string roleName)
        {
            string UID = GestionGlobal.GetUID("ROL");
            Role role = new Role();
            role.Nom_Role = roleName;
            role.UID = UID;
            db.Roles.Add(role);
            db.SaveChanges();

        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            var role_Utils = db.Role_Util.Where(x => x.Role.Nom_Role == roleName).ToList();
            if (role_Utils.Count > 0)
            {
                foreach (var item in role_Utils)
                {
                    var ru = db.Role_Util.Remove(item);
                }
                db.SaveChanges();

            }
            Role r = db.Roles.Where(x => x.Nom_Role == roleName).FirstOrDefault(); ;
            db.Roles.Remove(r);
            int deleted = db.SaveChanges();
            return deleted > 0;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return db.Roles.Select(x=> x.Nom_Role).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = (from ru in db.Role_Util
                            join rol in db.Roles on ru.UID_Role equals rol.UID
                            join user in db.Utilisateurs on ru.UID_Utilisateur equals user.UID
                            where user.pseudo==username select rol.Nom_Role).ToArray();
            return roles ?? null;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return db.Role_Util.Where(x=> x.Utilisateur.pseudo==username && roleName==x.Role.Nom_Role).FirstOrDefault() != null;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return db.Roles.Where(x => x.Nom_Role==roleName).FirstOrDefault() != null;
        }
    }
}