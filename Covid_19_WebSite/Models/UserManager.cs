using Covid_19_WebSite.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class UserManager : IUserManger
    {
        COVID_19_DBEntities db;

        public UserManager()
        {
            this.db = new COVID_19_DBEntities();
        }

        public bool CreateUser(string UID, string nom, string Mot_Pass, string Email)
        {
            int saved = 0;
            Utilisateur user = new Utilisateur()
            {
                UID = UID,
                pseudo = nom,
                Mot_Pass = Mot_Pass,
                Email = Email
            };
            if (!UserExists(nom)) {
                db.Utilisateurs.Add(user);
                saved= db.SaveChanges() ;
            }
            return saved > 0;
        }
        public Utilisateur GetRoleUtilisateur(string UID_Ru)
        {
            var rolU = db.Role_Util.Find(UID_Ru);
            return db.Utilisateurs.Find(rolU.UID_Utilisateur);
        }
        public bool CreateUser(Utilisateur user)
        {
            db.Utilisateurs.Add(user);
            int x = db.SaveChanges();
            return x > 0;
        }

        public bool DeleteUser(string UID_user)
        {
            int removed = 0;
            var user1 = db.Utilisateurs.Find(UID_user);
            db.Utilisateurs.Remove(user1);
            removed = db.SaveChanges();

            return removed>0;
        }

        public List<Utilisateur> ListOfUsers()
        {
            return db.Utilisateurs.ToList();
        }

        public bool ModifyUser(string UID_user, Utilisateur User)
        {
            Utilisateur user1 = SearchUser(UID_user);
            user1.Email = User.Email;
            user1.pseudo = User.pseudo;
            user1.Mot_Pass = User.Mot_Pass;
            
            db.Entry(user1).State = EntityState.Modified;
            int modified = db.SaveChanges();
            return modified > 0;
        }

        public bool ModifyUser(Utilisateur User)
        {
            db.Entry(User).State = EntityState.Modified;
            int modified = db.SaveChanges();
            return modified > 0;
        }

        public Utilisateur SearchUser(string UID_user)
        {
            Utilisateur user1 = db.Utilisateurs.Where(u => u.UID == UID_user).FirstOrDefault();
            return user1 != null ? user1 : null;
        }

        public bool UserExists(string UserName)
        {
            Utilisateur user1 = db.Utilisateurs.Where(u => u.pseudo.ToLower().Trim() == UserName.ToLower().Trim()).FirstOrDefault();
            return user1 != null ? true : false;
        }

        public List<Role_Util> GetListOfRolesUsers()
        {
            return db.Role_Util.ToList();
        }

    }
}