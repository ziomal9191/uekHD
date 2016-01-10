using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    /// <summary>
    /// klasa pobiera wszystkie linki stron dla wszystkich komentarzy dla danego porduktu z serwisu skapiec.pl
    /// </summary>
    class SkapiecWebCrawler: IWebCrawler
    {
        /// <summary>
        /// Pobiera pierwszy String z komentarzami
        /// </summary>
        /// <param name="downladString"></param>
        public SkapiecWebCrawler(string downladString)
        {
            m_downloadString = downladString;
        }
        /// <summary>
        /// Dla serwisu skapiec.pl pobiera całą zawartość z komentarzami
        /// </summary>
        /// <param name="statistic"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product getPagesContent(IStatisctics statistic, Product product)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            ILinkProvider provider = new SkapiecLinkProvider(m_downloadString);
            using (var db = new DatabaseContext())
            {

                string link = "";
                while ((link = provider.getLink()) != "")
                {
                    statistic.addDowlodedPage(link);
                    string pageContent = client.DownloadString(link);
                    pagesContent.Add(pageContent);
                }
                return product;
            }

        }
        /// <summary>
        /// Wypełnia produkt zawartością komentarzy
        /// </summary>
        /// <param name="product"></param>
        public void fillProduct(Product product)
        {
            foreach(string pageContent in pagesContent)
                skapiecParser.getCommentsContentFromPage(pageContent, product);
        }
        private ICommentParser skapiecParser = new SkapiecCommentParser();
        private string m_downloadString;
        private List<string> pagesContent = new List<string>();
    }
}
