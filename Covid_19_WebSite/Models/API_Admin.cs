using Covid_19_WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Covid_19_WebSite.Models
{
    public class API_Admin
    {

        COVID_19_DBEntities db = new COVID_19_DBEntities();

        APIModel Data = new APIModel();


        public APIModel API()
        {
            int casC = 0, casM = 0, casR = 0, casT = 0;

            foreach (var reg in db.Regions.ToList())
            {
                RegionModel Region_ = new RegionModel();

                Region_.UID = reg.UID;
                Region_.Nom = reg.Nom;
                foreach (var vil in db.Villes.Where(x => x.UID_Region == reg.UID).ToList())
                {
                    VilleModel Ville_ = new VilleModel();

                    Ville_.UID = vil.UID;
                    Ville_.Nom = vil.Nom;

                    foreach (var e in db.Enumerations.Where(e => e.UID_V == vil.UID).ToList())
                    {
                        StatistiquesModel Statistique_ = new StatistiquesModel()
                        {

                            UID = e.UID,
                            Cas_Confirmer = (int)e.Cas_Confirmer,
                            Cas_Mort = (int)e.Cas_Mort,
                            Cas_Retablis = (int)e.Cas_Retablis,
                            Date_Ins = ConvertDate.ConverDate((DateTime)e.Date_Ins)

                        };
                        //verify if there is some date to show or show 0 
                        if (e.Cas_Confirmer > 0)
                            casC += (int)e.Cas_Confirmer;
                        if (e.Cas_Retablis > 0)
                            casR += (int)e.Cas_Retablis;
                        if (e.Cas_Mort > 0)
                            casM += (int)e.Cas_Mort;
                        if (Statistique_.UID != null)
                            Ville_.Statistiques.Add(Statistique_);
                    }
                    // add villes with stat in region
                    Region_.Villes.Add(Ville_);
                }


                // return  Region statis if exists

                var RegStat = ReturnRegStat(Region_.UID);

                if (RegStat == null)
                {
                    RegionStatistique rgS = new RegionStatistique()
                    {
                        UID = GestionGlobal.GetUID("REGSTAT"),
                        Cas_Confirmer = casC,
                        Cas_Mort = casM,
                        Cas_Retablis = casR,
                        Date_Ins = DateTime.Now,
                        UID_R = Region_.UID
                    };
                    //db.RegionStatistiques.Add(rgS);
                    //db.Entry(rgS).State = System.Data.Entity.EntityState.Added;
                    //db.SaveChanges();
                    Region_.Statistiques = ReturnRegStat(Region_.UID);
                }
                else
                {
                    RegStat = ReturnRegStat(Region_.UID);
                    Region_.Statistiques = RegStat;
                }

                Data.api.Add(Region_);
                casC = casM = casR = casT = 0;
            }
            return Data;
        }

        public List<StatistiquesModel> ReturnRegStat(string UID_R)
        {
            List<StatistiquesModel> ls = new List<StatistiquesModel>();
            DateTime d = DateTime.Now;
            var q = (from s in db.RegionStatistiques
                     where s.UID_R == UID_R
                     orderby s.Date_Ins
                     descending
                     select new { s.UID, s.Cas_Confirmer, s.Cas_Mort, s.Cas_Retablis, s.Date_Ins }).ToList();

            if (q != null)
            {
                foreach (var item in q)
                {
                    StatistiquesModel st = new StatistiquesModel()
                    {
                        UID = item.UID,
                        Cas_Confirmer = (int)item.Cas_Confirmer,
                        Cas_Mort = (int)item.Cas_Mort,
                        Cas_Retablis = (int)item.Cas_Retablis,
                        Date_Ins = ConvertDate.ConverDate((DateTime)item.Date_Ins),
                    };
                    ls.Add(st);

                }
                return ls;
            }

            return null;
        }

    }
}