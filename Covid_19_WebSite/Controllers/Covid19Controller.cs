using Covid_19_WebSite.Filters;
using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace Covid_19_WebSite.Controllers
{

   
    public class Covid19Controller : ApiController
    {
        private API_Admin Data  = new API_Admin();
        // api/Covid19/tous
        // this links angular server port number to this microsoft server
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19")]
        [HttpGet]
        public APIModel Get()
        {
            return Data.API();
        }

        // this gets the regions and them cities
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/regions")]
        [HttpGet]
        public List<RegionsVilleModel> GetRegion()
        {
            RegionModel reg = new RegionModel();
            return reg.GetRegionsVilles();
        }

        // region statistics
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/region/{UID}/{date}")]
        [HttpGet]
        public StatistiquesModel GetRegionStat(string UID, string date)
        {
            // code to get the region statistics with date
            RegionModel reg = new RegionModel();
            return reg.ListeStatistiques(UID,date);
        }

        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/region")]
        [HttpGet]
        public List<StatistiquesModel> GetRegionStat(string UID)
        {
            // code to get the region statistics
            RegionModel reg = new RegionModel();
            return reg.ListeStatistiques(UID);
        }

        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/region/all")]
        [HttpGet]
        public List<StatistiquesModel> GetRegionStatAll(string UID)
        {
            // code to get the region statistics
            RegionModel reg = new RegionModel();
            return reg.GetRegionStatistiquesForDashboard(UID);
        }


        // city statistics
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/ville")]
        [HttpGet]
        public List<StatistiquesModel> GetVilleStat(string UID)
        {
            // code to get city statstics
            VilleModel v = new VilleModel();
            return v.ListeStatistiques(UID);
        }

        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/ville/{UID}/{date}")]
        [HttpGet]
        public StatistiquesModel GetVilleStat(string UID,string date)
        {
            // code to get city statstics with date
            VilleModel v = new VilleModel();
            return v.ListeStatistiques(UID,date);
        }

        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/ville/all")]
        [HttpGet]
        public List<StatistiquesModel> GetVilleStatAll(string UID)
        {
            // code to get the region statistics
            VilleModel v = new VilleModel();
            return v.GetVilleStatistiquesForDashboard(UID);
        }

        // general statistics
        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/general")]
        [HttpGet]
        public StatistiquesModel GetStatistiquesGenerale()
        {
            // code to get all statstics
            Covid_Results covid = new Covid_Results();
            return covid.GetStatistiques();
        }

        [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [Route("api/Covid19/general/all")]
        [HttpGet]
        public List<StatistiquesModel> GetStatistiquesForDashb()
        {
            // code to get all statstics
            Covid_Results covid = new Covid_Results();
            return covid.GetStatistiquesForDashboard();
        }



    }
}
