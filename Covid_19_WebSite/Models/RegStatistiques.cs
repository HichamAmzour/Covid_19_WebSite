using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class RegStatistiques
    {
        public string UID { get; set; }
        public int Cas_Mort { get; set; }
        public int Cas_Retablis { get; set; }
        public int Cas_Confirmer { get; set; }
        public int Cas_Totale { get; set; }
        public string Date_Ins { get; set; }
    }
}