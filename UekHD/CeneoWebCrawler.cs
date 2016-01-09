using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class CeneoWebCrawler : IWebCrawler
    {
        public CeneoWebCrawler(string downladString)
        {
            m_downloadString = downladString;
        }
        public Product getCommentList(IStatisctics statistic, Product product)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            ILinkProvider provider = new CeneoLinkProvider(m_downloadString);
            using (var db = new DatabaseContext())
            {

                string link = "";
                while ((link = provider.getLink()) != "")
                {
                    statistic.addDowlodedPage(link);
                    string pageContent = client.DownloadString(link);
                    ceneoParser.getCommentsContentFromPage(pageContent, product);
                }
                return product;
            }
            
        }

        private ICommentParser ceneoParser = new CeneoCommentParser();
        private string m_downloadString;

    }
}
