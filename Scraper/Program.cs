using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.ebay.com/sch/i.html?_from=R40&_nkw=google+pixel+4a&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&LH_BIN=1&_samilow=&_samihi=&_sadis=15&_stpos=&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=50&_fosrp=1";
            GetHtml(url);
        }

        private static async void GetHtml(string url)
        {

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            var htmlList = doc.DocumentNode.SelectNodes("//a[@class = 'vip']");


            var htmlPrice = doc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("lvprice prc")).ToList();


            var htmlCondition = doc.DocumentNode.SelectNodes("//div[@class = 'lvsubtitle']");

            List<string> listTile = new List<string>();
            List<string> listPrice = new List<string>();
            List<string> listCondition = new List<string>();


            foreach (var item in htmlList)
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
                     
            Console.ReadLine();
        }

    }
}
