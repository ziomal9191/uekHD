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
            System.Net.WebClient client = new System.Net.WebClient();
            string downloadString = client.DownloadString("http://www.ceneo.pl/19299330#tab=reviews");
            ceneoParser.getCommentsContentFromPage(downloadString);
        }
        private ICommentParser ceneoParser = new CeneoCommentParser();
    }
}
