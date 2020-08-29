using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Controllers
{
    public class GestionGlobal
    {

        public static string GetUID(string prifix)
        {
            return prifix + '-' + Guid.NewGuid().ToString().ToUpper();
        }
    }
}