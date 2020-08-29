using System;
using System.Collections.Generic;
using Covid_19_WebSite.Models;
using System.Linq;
using System.Web;
using Covid_19_WebSite.Controllers;

namespace Covid_19_WebSite.Models
{
    public class RegionModel
    {
        COVID_19_DBEntities db;
        public string UID { get; set; }
        public string Nom { get; set; }
        
        public List<StatistiquesModel> Statistiques { get; set; } 
        public List<VilleModel> Villes { get; set; } 

        public RegionModel()
        {
            this.Statistiques =new List<StatistiquesModel>();
            this.Villes = new List<VilleModel>();
            db = new COVID_19_DBEntities();
        }

        public List<StatistiquesModel> ListeStatistiques(string UID_R)
        {
            Region region = db.Regions.Where(r => r.UID == UID_R).FirstOrDefault();
            var resuls = db.RegionStatistiques.Where(s => s.UID_R==UID_R).OrderByDescending(o => o.Date_Ins).ToList();
            if(resuls.Count>0)
            {
                foreach (var stat in resuls)
                {
                    StatistiquesModel lst = new StatistiquesModel()
                    {
                        UID = stat.UID,
                        Cas_Confirmer = (int)stat.Cas_Confirmer | 0,
                        Cas_Mort = (int)stat.Cas_Mort | 0,
                        Cas_Retablis = (int)stat.Cas_Retablis | 0,
                        Total_Cas_Confirmer = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Confirmer) | 0,
                        Total_Cas_Mort = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Mort) | 0,
                        Total_Cas_Retablis = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= stat.Date_Ins.Value).Sum(r => r.Cas_Retablis) | 0,
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
            return Statistiques.OrderByDescending(x=> x.Date_Ins).ToList();
        }

        public StatistiquesModel ListeStatistiques(string UID_R,string date)
        {
            StatistiquesModel stat;
            DateTime dt = DateTime.Parse(date);
            Region region = db.Regions.Where(r => r.UID == UID_R).FirstOrDefault();
            var resuls = db.RegionStatistiques.Where(s => s.UID_R == UID_R && s.Date_Ins==dt).FirstOrDefault();
            if (resuls != null)
            {
                stat= new StatistiquesModel()
                    {
                        UID = resuls.UID,
                        Cas_Confirmer = (int)resuls.Cas_Confirmer | 0,
                        Cas_Mort = (int)resuls.Cas_Mort | 0,
                        Cas_Retablis = (int)resuls.Cas_Retablis | 0,
                        Total_Cas_Confirmer= (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value<=dt).Sum(r => r.Cas_Confirmer) | 0,
                        Total_Cas_Mort= (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= dt).Sum(r => r.Cas_Mort) | 0,
                        Total_Cas_Retablis= (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= dt).Sum(r => r.Cas_Retablis) | 0,
                        Date_Ins = ConvertDate.ConverDate((DateTime)resuls.Date_Ins)
                    };
            }
            else
            {
                stat = new StatistiquesModel() {
                    Total_Cas_Confirmer = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= dt).Sum(r => r.Cas_Confirmer).GetValueOrDefault() | 0,
                    Total_Cas_Mort = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= dt).Sum(r => r.Cas_Mort).GetValueOrDefault() | 0,
                    Total_Cas_Retablis = (int)db.RegionStatistiques.Where(r => r.UID_R == UID_R && r.Date_Ins.Value <= dt).Sum(r => r.Cas_Retablis).GetValueOrDefault() | 0,
                    Date_Ins = ConvertDate.ConverDate(DateTime.Parse(date))

                };
            }
            return stat;
        }

        public List<RegionsVilleModel> GetRegionsVilles()
        {
            List<RegionsVilleModel> rgV = new List<RegionsVilleModel>();
            foreach (var item in db.Regions)
            {
                RegionsVilleModel reg = new RegionsVilleModel();
                reg.UID = item.UID;
                reg.Nom = item.Nom;

                foreach (var vil in item.Villes)
                {
                    reg.Villes.Add(new VillePouRegionModel() { Nom = vil.Nom, UID = vil.UID });
                }
                rgV.Add(reg);
            }

            return rgV;
        }

        public bool AjouterStatistique(Enumeration enume)
        {
            int x = 0;
            Region reg = db.Regions.Where(i=> i.Villes.Where(y=> y.UID==enume.UID_V).FirstOrDefault().UID==enume.UID_V).FirstOrDefault();
            var regStat = StatExists(reg.UID);

            if (regStat!=null)
            {
                regStat.Cas_Confirmer += enume.Cas_Confirmer;
                regStat.Cas_Mort += enume.Cas_Mort;
                regStat.Cas_Retablis += enume.Cas_Retablis;
                db.Entry(regStat).State = System.Data.Entity.EntityState.Modified;
                x = db.SaveChanges();
            }
            else
            {
                RegionStatistique regionStatistique = new RegionStatistique() {
                    Cas_Confirmer=enume.Cas_Confirmer,
                    Cas_Mort=enume.Cas_Mort,
                    Cas_Retablis=enume.Cas_Retablis,
                    Date_Ins=enume.Date_Ins,
                    UID=GestionGlobal.GetUID("REGSTAT"),
                    Region=reg
                };
                db.Entry(regionStatistique).State = System.Data.Entity.EntityState.Added;
                x = db.SaveChanges();
            }
            return x > 0;
        }

        public RegionStatistique StatExists(string UID_R)
        {
            DateTime dt = DateTime.Now;
            var result = db.RegionStatistiques.Where(r=> r.UID_R==UID_R && r.Date_Ins.Value.Day==dt.Day && r.Date_Ins.Value.Month==dt.Month && r.Date_Ins.Value.Year == dt.Year).FirstOrDefault();
            return result ?? null;
        }

        public List<StatistiquesModel> GetRegionStatistiquesForDashboard(string UID)
        {
            List<StatistiquesModel> lss = new List<StatistiquesModel>();
            DateTime dt = db.RegionStatistiques.Min(x => x.Date_Ins).Value;
            DateTime dtMax = db.RegionStatistiques.Max(x => x.Date_Ins).Value;


            while(dt != dtMax)
            {

                List<RegionStatistique> results = db.RegionStatistiques.Where(s => s.Date_Ins.Value.Day == dt.Day && s.Date_Ins.Value.Month == dt.Month && s.Date_Ins.Value.Year == dt.Year && s.UID_R == UID).ToList();
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