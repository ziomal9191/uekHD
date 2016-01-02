using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class StatiscticsObject : IStatisctics
    {
        public StatiscticsObject() { }
        public void addDowlodedPage(string url)
        {
            urlAddresses.Add(url);
        }
        public List<string> getDowloadedPages()
        {
            return urlAddresses;
        }
        public void addCommentSizeRecord(int productCommentSize)
        {
            this.productCommentSize = productCommentSize;
        }
        private List<string> urlAddresses = new List<string>();
        private int productCommentSize = 0;
    }

}
