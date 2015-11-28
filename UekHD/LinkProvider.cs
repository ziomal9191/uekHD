using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class LinkProvider : ILinkProvider
    {
        public LinkProvider(string pageLink)
        {
            m_pageLink = pageLink;
        }
        public string getLink()
        {
            if (!wasFirstPage)
            {
                wasFirstPage = true;
                return m_pageLink;
            }

            //*[@id="body"]/div[2]/div/div/div[2]/div[3]/div[1]/ul
            //if(m_pageNumber< 2)
            //{
            //    m_pageNumber++;
            //    return m_pageLink;
            //}
            return commentPagesLinkProvider();
        }
        private string commentPagesLinkProvider()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string pageContent = client.DownloadString(m_pageLink);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);

            
            string xpath = "";
            if (m_pageNumber == 0)
            {
                m_pageNumber++;
                xpath = "//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/div[2]/ul//li";
            }
            else
            {
                xpath = "//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[1]/ul//li";
            }
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (bodyNodes != null)
            {
                // using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter("./lala1.txt", true, System.Text.Encoding.UTF8))
                // {
                //var encoding = new System.Text.UTF8Encoding();
                bool wasActive = false;
                foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                {
                    if (!wasActive)
                    {
                        foreach (HtmlAgilityPack.HtmlAttribute attribut in node.Attributes)
                        {

                            if (attribut.Value == "active")
                            {
                                wasActive = true;
                            }
                        }
                    }
                    else
                    {
                        HtmlAgilityPack.HtmlNodeCollection links = node.ChildNodes;
                        foreach (HtmlAgilityPack.HtmlNode nodeLink in links)
                        {
                            foreach (HtmlAgilityPack.HtmlAttribute attribut in nodeLink.Attributes)
                            {
                                m_pageLink = "http://www.ceneo.pl" + attribut.Value;
                                return "http://www.ceneo.pl"+ attribut.Value;

                            }
                        }
                    }
                }
                //}
            }

            return "";
        }
        private bool wasFirstPage = false;
        private int m_pageNumber = 0;
        private string m_pageLink;
    }
}
