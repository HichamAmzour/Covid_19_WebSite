using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;

namespace Covid_19_WebSite.Models
{
    public class ScrapySharpModel
    {
         HtmlDocument doc = new HtmlDocument();

        public IEnumerable<HtmlNode> lastResult { get; set; }
        public async Task<string> Scrape()
        {
            ScrapingBrowser Browser = new ScrapingBrowser() {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true,
                UserAgent = new FakeUserAgent("Jhone", "Mark"),
                IgnoreCookies = true,
                UseDefaultCookiesParser = true,
            
            };
        //https://covid.kafil.xyz/ 

            Uri url = new Uri(@"https://www.google.com/search?q=maroc+coronavirus+statistiques&oq=maroc+coronavirus+statistiques&aqs=chrome..69i57.11990j0j4&sourceid=chrome&ie=UTF-8");
            WebPage PageResults =await Browser.NavigateToPageAsync(url);
            HtmlNode TitelNode = PageResults.Html.CssSelect("body").First();
            doc = new HtmlDocument();
            doc.LoadHtml(TitelNode.InnerHtml);
            lastResult = doc.DocumentNode.SelectNodes(@"/ body / div[6] / div[2] / div[9] / div[1] / div[2] / div / div[2] / div[2] / div / div / div / div / div[4] / div[5] / div / div / div / div[3] / div[1] / div[2] / div / div / div / div / div");
            lastResult = doc.DocumentNode.Descendants("div").Where(x=> x.GetAttributeValue("id")== "_mZDIXoGqBbCNlwTg3aeoCA16").ToList();
            lastResult = lastResult.Where(x => x.CssSelect("table") !=null);
            
            return TitelNode.InnerText;
           //TitelNode.OuterHtml;
        }
 
        //public void Scrape2()
        //{
        //    var url = @"https://www.hespress.com/regions/469236.html";
        //    var http =new HttpClient();
        //    var html = http.GetStringAsync(url);
            
        //}

    }
}