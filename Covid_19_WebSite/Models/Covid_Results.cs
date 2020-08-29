using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class Covid_Results
    {
        COVID_19_DBEntities db;
        StatistiquesModel statistiques;
        RegionModel reg;

        public Covid_Results()
        {
            db = new COVID_19_DBEntities();
            reg = new RegionModel();
        }

        public StatistiquesModel GetStatistiques()
        {
            DateTime dt= DateTime.Now;

            int TotalCasConfirmer = (int)db.RegionStatistiques.Sum(r => r.Cas_Confirmer).GetValueOrDefault();
            int TotalCasMort =(int) db.RegionStatistiques.Sum(r => r.Cas_Mort).GetValueOrDefault();
            int TotalCasRetablis =(int) db.RegionStatistiques.Sum(r => r.Cas_Retablis).GetValueOrDefault();

            var stat = db.RegionStatistiques.Where(rs => rs.Date_Ins.Value.Day == dt.Day && rs.Date_Ins.Value.Month==dt.Month && rs.Date_Ins.Value.Year==dt.Year).FirstOrDefault();
            // NOT : you need to add a table for moroc statics
            if (stat != null)
            {
                statistiques = new StatistiquesModel() {
                    UID=stat.UID,
                    Cas_Confirmer=(int)stat.Cas_Confirmer,
                    Cas_Mort= (int)stat.Cas_Mort,
                    Cas_Retablis= (int)stat.Cas_Retablis,
                    Total_Cas_Confirmer=TotalCasConfirmer,
                    Total_Cas_Mort=TotalCasMort,
                    Total_Cas_Retablis=TotalCasRetablis,
                    Date_Ins=DateTime.Now.ToShortDateString()
                };
            }
            else
            {
                statistiques = new StatistiquesModel()
                {
                    Total_Cas_Confirmer = TotalCasConfirmer,
                    Total_Cas_Mort = TotalCasMort,
                    Total_Cas_Retablis = TotalCasRetablis,
                };
            }
           
            return statistiques;
        }

        public List<StatistiquesModel> GetStatistiquesForDashboard()
        {
            List<StatistiquesModel> lss = new List<StatistiquesModel>();
            DateTime dt = db.RegionStatistiques.Min(x => x.Date_Ins).Value;
            DateTime dtMax = db.RegionStatistiques.Max(x => x.Date_Ins).Value;


            while(dt != dtMax)
            {

                List<RegionStatistique> results = db.RegionStatistiques.Where(s=> s.Date_Ins.Value.Day==dt.Day && s.Date_Ins.Value.Month==dt.Month && s.Date_Ins.Value.Year==dt.Year).ToList();
                int TotalCasConfirmer = (int)results.Sum(r => r.Cas_Confirmer).GetValueOrDefault() | 0;
                int TotalCasMort = (int)results.Sum(r => r.Cas_Mort).GetValueOrDefault() | 0;
                int TotalCasRetablis = (int)results.Sum(r => r.Cas_Retablis).GetValueOrDefault() | 0;


                StatistiquesModel st = new StatistiquesModel() {

                    Total_Cas_Confirmer = TotalCasConfirmer,
                    Total_Cas_Mort = TotalCasMort,
                    Total_Cas_Retablis = TotalCasRetablis,
                    Date_Ins = ConvertDate.ConverDate(dt)
                    };

                lss.Add(st);
                
                dt=dt.AddDays(1);
                if (dt == dtMax)
                {
                    break;
                }
            }
            return lss;
        }
    }
}