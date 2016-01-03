using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    interface IWebCrawler
    {
       // Product getCommentList(IStatisctics statistic);
        void addProductToDatabase(Product product);
        Product getCommentList(IStatisctics statistic, Product product);
    }
}
