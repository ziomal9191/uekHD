using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UekHD
{

    class HttpCommentGeter
    {
        public HttpCommentGeter(string productId, IStatisctics statistic)
        {
            //19299330
           m_webCrawler = new CeneoWebCrawler("http://www.ceneo.pl/" + productId + "#tab=reviews");
           product = m_webCrawler.getCommentList( statistic);
           //m_webCrawler.addProductToDatabase()
        }
        public void loadProductToDataBase()
        {
            m_webCrawler.addProductToDatabase(product);
        }
        IWebCrawler m_webCrawler;
        private Product product;
    }
}
