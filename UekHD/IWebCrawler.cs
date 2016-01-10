using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    interface IWebCrawler
    {
        Product getPagesContent(IStatisctics statistic, Product product);
        void fillProduct(Product product);
            
    }


}
