using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_WebSite.Interfaces
{
    interface IRoleManager
    {
        bool CreateRole(string RoleName);
        bool RoleExists(string RoleName);
        bool RoleModify(string UID, string NewRoleName);
        bool UserHasRole(string RoleName, string UID_User );
        bool AssingRoleToUser(string UID_Role, string UID_User );
        string GetRoleName(string roleId );
        List<Role> GetListOfRoles();
        Role GetRole(string UID);
        bool RemoveRole(string UID);


    }
}
