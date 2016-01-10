using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    /// <summary>
    /// klasa pobiera wszystkie linki stron dla wszystkich komentarzy dla danego porduktu z serwisu ceneo.pl
    /// </summary>
    class CeneoWebCrawler : IWebCrawler
    {
        /// <summary>
        /// Pobiera pierwszy String z komentarzami
        /// </summary>
        /// <param name="downladString"></param>
        public CeneoWebCrawler(string downladString)
        {
            m_downloadString = downladString;
        }
        /// <summary>
        /// Wypełnia produkt zawartością komentarzy
        /// </summary>
        /// <param name="product"></param>
        public void fillProduct(Product product)
        {
            foreach(string pageContent in pagesContent)
                ceneoParser.getCommentsContentFromPage(pageContent, product);
        }
        /// <summary>
        /// Dla serwisu ceneo.pl pobiera całą zawartość z komentarzami
        /// </summary>
        /// <param name="statistic"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product getPagesContent(IStatisctics statistic, Product product)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            ILinkProvider provider = new CeneoLinkProvider(m_downloadString);
                string link = "";
                while ((link = provider.getLink()) != "")
                {
                    statistic.addDowlodedPage(link);
                    string pageContent = client.DownloadString(link);
                    pagesContent.Add(pageContent);
                }
                return product;
            
        }
        
        private ICommentParser ceneoParser = new CeneoCommentParser();
        private string m_downloadString;
        private List<string> pagesContent = new List<string>();
    }

}