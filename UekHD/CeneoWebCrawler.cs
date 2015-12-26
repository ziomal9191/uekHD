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
        public void getCommentList()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            CommentList comments = new CommentList();
            ILinkProvider provider = new LinkProvider(m_downloadString);
            Product product = new Product();
            string link = "";
            while ((link = provider.getLink()) != "")
            { 
                string pageContent = client.DownloadString(link);
                comments.AddRange(ceneoParser.getCommentsContentFromPage(pageContent, product) );
            }
          
        }
        private ICommentParser ceneoParser = new CeneoCommentParser();
        private string m_downloadString;

    }
}
