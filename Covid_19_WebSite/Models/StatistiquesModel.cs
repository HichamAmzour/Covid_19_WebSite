using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class StatistiquesModel
    {
       
        public string UID { get; set; }
        [Display(Name ="Cas Mort")]
        public int Cas_Mort { get; set; }
        [Display(Name = "Cas Rétablis")]
        public int Cas_Retablis { get; set; }
        [Display(Name = "Cas Confirmer")]
        public int Cas_Confirmer { get; set; }

        public int Total_Cas_Confirmer { get; set; }
        public int Total_Cas_Mort{ get; set; }
        public int Total_Cas_Retablis { get; set; }
      
        [Display(Name = "Date")]
        public string Date_Ins { get; set; }

        public StatistiquesModel()
        {
            UID = "";
            Cas_Mort = Cas_Retablis = Cas_Confirmer=Total_Cas_Confirmer=Total_Cas_Mort=Total_Cas_Retablis = 0;
            Date_Ins = DateTime.Now.ToShortDateString();
        }

    }
}