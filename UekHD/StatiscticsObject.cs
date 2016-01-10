using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    /// <summary>
    /// Klasa tworząca statystyki wykonanych operacji
    /// </summary>
    class StatiscticsObject : IStatisctics
    {
        public StatiscticsObject() { }
        /// <summary>
        /// dodaje odwiedzoną stronę do statystyk
        /// </summary>
        /// <param name="url"></param>
        public void addDowlodedPage(string url)
        {
            urlAddresses.Add(url);
        }
        public List<string> getDowloadedPages()
        {
            return urlAddresses;
        }
        /// <summary>
        /// Dodanie komentarza
        /// </summary>
        /// <param name="comment"></param>
        public void addAddedComment(string comment)
        {
            newComments.Add(comment);
        }
        /// <summary>
        /// Ilość nowo dodanych komentarzy do db
        /// </summary>
        /// <returns></returns>
        public int getNewCommentCount()
        { return newComments.Count; }

        /// <summary>
        /// Tworzy String podsumowujący statystyki
        /// </summary>
        /// <returns></returns>
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
