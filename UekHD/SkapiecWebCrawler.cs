using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class SkapiecWebCrawler: IWebCrawler
    {
        public SkapiecWebCrawler(string downladString)
        {
            m_downloadString = downladString;
        }
        public Product getCommentList(IStatisctics statistic, Product product)
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
                    skapiecParser.getCommentsContentFromPage(pageContent, product);
                }
                return product;
            }

        }

        private ICommentParser skapiecParser = new SkapiecCommentParser();
        private string m_downloadString;
    }
}
