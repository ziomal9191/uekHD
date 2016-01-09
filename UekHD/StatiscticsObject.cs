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
        public void addAddedComment(string comment)
        {
            newComments.Add(comment);
        }
        public int getNewCommentCount()
        { return newComments.Count; }
        string toString()
        {
            string statistics = "Checked url";
            foreach (string url in urlAddresses)
            {
                statistics += url + "\n";
            }
            statistics += "New comment number : " + newComments.Count.ToString();
            return statistics;
        }
        public string getSummary()
        {
            string statistics = "Checked url: \n";
            foreach (string url in urlAddresses)
            {
                statistics += url + "\n";
            }
            statistics += "New comment number : " + newComments.Count.ToString();
            return statistics;
        
        }
        private List<string> urlAddresses = new List<string>();
        private List<string> newComments = new List<string>();
        private int productCommentSize = 0;

    }

}
