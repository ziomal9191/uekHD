using MinimumEditDistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace UekHD
{
    /// <summary>
    /// klasa znajduje za pomocą metody Levenshtein link do produktu na stronie skapiec.pl na podstawie nazw produktu
    /// </summary>
    class SkapiecLinkToProductFinder : ILinkToProductFinder
    {
        /// <summary>
        /// Pobiera link do produktu ze strony skapiec.pl na podstawie danych model i producent
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public string getLinkToProduct(Product product)
        {
            string productName = product.Brand +" "+ product.Model;
            productName = productName.TrimEnd();
            productName =productName.Replace(' ', '+');
            productName = productName.ToLower();
            int distance = Levenshtein.CalculateDistance(productName, "", 1);
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            Uri uriAddres = new Uri("http://www.skapiec.pl/szukaj/w_calym_serwisie/" + productName);
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0");
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.Headers.Add("Accept-Language", "pl,en-US;q=0.7,en;q=0.3");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("Cookie", "PHPSESSID=e82ovdm91g5vobf0700n6k6dk6; skapiec_track=MTE5MTc0NzIyNg%3D%3D; YII_CSRF_TOKEN=8cc33c83714d40df25451e3b10a93f8e675eeae4; _ga=GA1.2.288452357.1451829433; __utmx=197911341.0T-zQrfuTne0iqXdL--tYQ$73259467-63:.DHy8J0MdR82UaXE7-wwR2w$73259467-66:; __utmxx=197911341.0T-zQrfuTne0iqXdL--tYQ$73259467-63:1451829432:15552000.DHy8J0MdR82UaXE7-wwR2w$73259467-66:1451829719:15552000; __utma=197911341.288452357.1451829433.1451829435.1451829435.1; __utmb=197911341.6.9.1451829720200; __utmc=197911341; __utmz=197911341.1451829435.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __cktest=123; groki_uuid=f73327cb-113d-4372-8657-75d9eb124fd2; groki_usid=318888af-58b9-40b8-93ec-281d1d92dd73; __utmv=197911341.|2=UID=Brak=1; __gfp_64b=.XiwTpeFNPvRQK5GN82_ZmEnZo8ft2afgYYTqbCJuTT.07; ea_uuid=201601031457157309300828; SkaPaginationSearchPagination=20");
            var responseStream = new System.IO.Compression.GZipStream(client.OpenRead(uriAddres), System.IO.Compression.CompressionMode.Decompress);
            var reader = new System.IO.StreamReader(responseStream);
            string pageContent = reader.ReadToEnd();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            HtmlAgilityPack.HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"partial products js results\"]/div[@class=\"partial box-row js groki click\"]/div[@class =\"box\"] | //div[@class=\"partial products js results\"]/div[@class=\"partial box-row js groki click\"]/div[@class =\"box mono-offer\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            Tuple<string, string, double> bestFit = new Tuple<string, string, double>("","", Double.MaxValue);
            if (nodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode node in nodes)
                {
                    HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//a[1]");
                    foreach (HtmlAgilityPack.HtmlNode nodeA in bodyNodes)
                    {
                        string tet = nodeA.InnerHtml.ToLower();
                        if(bestFit.Item3 > Levenshtein.CalculateDistance(productName, tet, 1))
                        {
                            
                            string page = nodeA.GetAttributeValue("href", "");
                            int levenshtein = Levenshtein.CalculateDistance(productName, tet, 1);
                            bestFit = new Tuple<string, string, double>(tet, page, levenshtein);
                        }

                        break;
                    }
                }
            }
            if (bestFit.Item3 < 20)
                return bestFit.Item2;
            else
                return null;

        }


    }
}
