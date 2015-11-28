using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UekHD
{

    class HttpCommentGeter
    {
        public HttpCommentGeter(string productId)
        {
            //19299330
           IWebCrawler m_webCrawler = new CeneoWebCrawler("http://www.ceneo.pl/" + productId + "#tab=reviews");
           m_webCrawler.getCommentList();

        }
        
    }
}
