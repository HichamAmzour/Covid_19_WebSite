using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class RoleUserModel
    {
        public List<Role> listeRoles { get; set; }
        public List<Role_Util> listeRolesUtilis { get; set; }
        public List<Utilisateur> listeUtilisateurs { get; set; }

        public Role role { get; set; }
        public string Role_Util { get; set; }
        public Utilisateur utilisateur { get; set; }

        public RoleUserModel()
        {
            listeRoles = new List<Role>();
            listeRolesUtilis = new List<Role_Util>();
            listeUtilisateurs = new List<Utilisateur>();
        }

    }
}