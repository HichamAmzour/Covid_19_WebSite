using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_WebSite.Interfaces
{
    interface IUserManger
    {
        bool CreateUser(string UID,string nom, string Mot_Pass, string Email);
        bool CreateUser(Utilisateur user);
        bool UserExists(string UserName);
        bool ModifyUser(string UID_user, Utilisateur User);
        bool ModifyUser(Utilisateur User);
        bool DeleteUser(string UID_user);
        Utilisateur SearchUser(string UID_user);
        List<Utilisateur> ListOfUsers();


    }
}
