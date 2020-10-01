using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using AngleSharp.Text;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test URL
            //https://www.ebay.com/sch/i.html?_from=R40&_nkw=google+pixel+4a&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&LH_BIN=1&_samilow=&_samihi=&_sadis=15&_stpos=&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=50&_fosrp=1

            Console.WriteLine("Please Enter a Url");

            string url = Console.ReadLine();           

            GetHtml(url);

            Console.ReadLine();
        }

        private static void GetHtml(string url)
        {
            //get HTML document from HTTP
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //Represents a complete HTML document
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            //Selects a list of nodes matching the HtmlAgilityPack.HtmlNode.XPath expression
            var htmlName = doc.DocumentNode.SelectNodes("//a[@class = 'vip']");


            //Get all descendant nodes with matching name
            var htmlPrice = doc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("lvprice prc")).ToList();

            //Selects a list of nodes matching the HtmlAgilityPack.HtmlNode.XPath expression
            var htmlCondition = doc.DocumentNode.SelectNodes("//div[@class = 'lvsubtitle']");

            List<string> listTile = new List<string>();
            List<string> listPrice = new List<string>();
            List<string> listCondition = new List<string>();

            foreach (var item in htmlName)
            {
                listTile.Add(item.InnerText);
            }

            foreach (var item in htmlPrice)
            {
                listPrice.Add(item.InnerText);
            }

            foreach (var item in htmlCondition)
            {
                listCondition.Add(item.InnerText);
            }

            for (int i = 0; i < listTile.Count; i++)
            {
                Console.WriteLine(listTile[i] + "\n" + listCondition[i].Trim() + "\n " + Regex.Match(listPrice[i].Trim('\r', '\n', '\t'), @"\d+.\d+"));
                Console.WriteLine(new string('-', 100));
            }

            // Check if a next page link is present 

            string nextPageUrl = "";

            //Get all descendant nodes with matching name
            var nextPage = doc.DocumentNode.Descendants("td")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("pagn-next")).FirstOrDefault();

            //counts the number of all pages
            var pages = doc.DocumentNode.SelectSingleNode("//td[@class = 'pages']").Descendants("a").Count();

            //Get current page number
            var currentPage = doc.DocumentNode.SelectSingleNode("//a[@class = 'pg  curr']").InnerText;

            if (nextPage != null && currentPage != pages.ToString()) 
            {
                //Get next page Url
                nextPageUrl = nextPage.SelectSingleNode("//a[@class = 'gspr next']").GetAttributeValue("href", "");
            }
            if (!String.IsNullOrEmpty(nextPageUrl))
            {
                GetHtml(nextPageUrl);
            }

        }
    }
}
