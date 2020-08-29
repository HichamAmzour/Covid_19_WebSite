using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class VilleModel
    {
        COVID_19_DBEntities db;
        public string UID { get; set; }
        public string Nom { get; set; }
        public List<StatistiquesModel> Statistiques { get; set; }
        public VilleModel()
        {
            Statistiques = new List<StatistiquesModel>();
            db = new COVID_19_DBEntities();
        }


        public List<StatistiquesModel> ListeStatistiques(string UID_V)
        {
            Ville ville = db.Villes.Where(v => v.UID == UID_V).FirstOrDefault();

            var resuls = db.Enumerations.Where(e => e.Ville.UID == ville.UID).OrderByDescending(o=> o.Date_Ins).ToList();

            if (resuls.Count > 0)
            {
                foreach (var stat in resuls)
                {
                    StatistiquesModel lst = new StatistiquesModel() {
                        UID = stat.UID,
                        Cas_Confirmer = (int)stat.Cas_Confirmer | 0,
                        Cas_Mort = (int)stat.Cas_Mort | 0,
                        Cas_Retablis = (int)stat.Cas_Retablis | 0,
                        Total_Cas_Confirmer = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Confirmer) | 0,
                        Total_Cas_Mort = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Mort) | 0,
                        Total_Cas_Retablis = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Retablis) | 0,
                        Date_Ins = ConvertDate.ConverDate((DateTime)stat.Date_Ins)
                     };
                    Statistiques.Add(lst);
                }
            }
            else
            {
                Statistiques.Clear();
                Statistiques.Add(new StatistiquesModel());
            }

            return Statistiques;
        }
        public StatistiquesModel ListeStatistiques(string UID_V,string date)
        {
            StatistiquesModel stat;
            DateTime dt = DateTime.Parse(date);
            Ville ville = db.Villes.Where(v => v.UID == UID_V).FirstOrDefault();
            var resuls = db.Enumerations.Where(e => e.Ville.UID == ville.UID && e.Date_Ins==dt).FirstOrDefault();
           
            if(resuls!=null)
            {
               stat = new StatistiquesModel()
                {
                    UID = resuls.UID,
                    Cas_Confirmer = (int)resuls.Cas_Confirmer | 0,
                    Cas_Mort = (int)resuls.Cas_Mort | 0,
                    Cas_Retablis = (int)resuls.Cas_Retablis | 0,
                    Total_Cas_Confirmer = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Confirmer) | 0,
                    Total_Cas_Mort = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Mort) | 0,
                    Total_Cas_Retablis = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Retablis) | 0,
                    Date_Ins = ConvertDate.ConverDate((DateTime)resuls.Date_Ins)
                };
            }
            else
            {
                stat = new StatistiquesModel() {
                    Total_Cas_Confirmer = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Confirmer).GetValueOrDefault() | 0,
                    Total_Cas_Mort = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Mort).GetValueOrDefault() | 0,
                    Total_Cas_Retablis = (int)db.Enumerations.Where(e => e.UID_V == UID_V && e.Date_Ins.Value <= dt).Sum(r => r.Cas_Retablis).GetValueOrDefault() | 0,
                    Date_Ins = ConvertDate.ConverDate(DateTime.Parse(date))
                };
            }
           
            return stat;
        }
        public List<StatistiquesModel> GetVilleStatistiquesForDashboard(string UID)
        {
            List<StatistiquesModel> lss = new List<StatistiquesModel>();
            DateTime dt = db.Enumerations.Min(x => x.Date_Ins).Value;
            DateTime dtMax = db.Enumerations.Max(x => x.Date_Ins).Value;


            while(dt != dtMax)
            {

                var results = db.Enumerations.Where(s => s.Date_Ins.Value.Day == dt.Day && s.Date_Ins.Value.Month == dt.Month && s.Date_Ins.Value.Year == dt.Year && s.UID_V == UID).ToList();
                int TotalCasConfirmer = (int)results.Sum(r => r.Cas_Confirmer).GetValueOrDefault() | 0;
                int TotalCasMort = (int)results.Sum(r => r.Cas_Mort).GetValueOrDefault() | 0;
                int TotalCasRetablis = (int)results.Sum(r => r.Cas_Retablis).GetValueOrDefault() | 0;


                StatistiquesModel st = new StatistiquesModel()
                {

                    Total_Cas_Confirmer = TotalCasConfirmer,
                    Total_Cas_Mort = TotalCasMort,
                    Total_Cas_Retablis = TotalCasRetablis,
                    Date_Ins = ConvertDate.ConverDate(dt)
                };

                lss.Add(st);

                dt = dt.AddDays(1);
                if (dt == dtMax)
                {
                    break;
                }

            }
            return lss;
        }

    }
}