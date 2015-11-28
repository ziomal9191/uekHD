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
            System.Net.WebClient client = new System.Net.WebClient();
            //client.Proxy = null;
            string downloadString = client.DownloadString("http://www.ceneo.pl/" + productId + "#tab=reviews");
            ceneoParser.getCommentsContentFromPage(downloadString);
        }
        private ICommentParser ceneoParser = new CeneoCommentParser();
    }
}
