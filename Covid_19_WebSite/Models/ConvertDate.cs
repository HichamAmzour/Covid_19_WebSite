using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid_19_WebSite.Models
{
    public class ConvertDate
    {
        public static string ConverDate(DateTime dt)
        {
            string d, m;
            if (dt.Day <= 9)
                d = "0" + dt.Day.ToString();
            else
                d = dt.Day.ToString();

            if (dt.Month <= 9)
                m = "0" + dt.Month.ToString();
            else
                m = dt.Month.ToString();

            return m + "/" + d + "/" + dt.Year;
        }
    }
}