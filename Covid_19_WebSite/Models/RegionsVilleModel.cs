using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class RegionsVilleModel
    {
        public string UID { get; set; }

        public string Nom { get; set; }

        public List<VillePouRegionModel> Villes { get; set; } = new List<VillePouRegionModel>();
    }
}