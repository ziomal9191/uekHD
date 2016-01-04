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
            product = new Product();
            m_webCrawler = new CeneoWebCrawler("http://www.ceneo.pl/" + productId + "#tab=reviews");
            product = m_webCrawler.getCommentList( statistic, product);
            ILinkToProductFinder productFinder = new SkapiecLinkToProductFinder();
            string foundProduct = productFinder.getLinkToProduct(product);
            if (foundProduct != null)
            {
                m_webCrawler = new SkapiecWebCrawler("http://www.skapiec.pl" + productFinder.getLinkToProduct(product) + "#opinie");
                product = m_webCrawler.getCommentList(statistic, product);
            }

        }
        public void loadProductToDataBase()
        {
            m_webCrawler.addProductToDatabase(product);
        }
        IWebCrawler m_webCrawler;
        private Product product;
    }
}
