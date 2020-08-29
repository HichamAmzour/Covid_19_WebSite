using Covid_19_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Covid_19_WebSite.Controllers
{
    public class ScrapController : Controller
    {
        // GET: Scrap
        public async Task<ActionResult> Index()
        {
            ScrapySharpModel s = new ScrapySharpModel();
           // var x = await s.Scrape();
            return View(s=null);
        }
    }
}