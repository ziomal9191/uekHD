﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    /// <summary>
    /// Klasa dostarcza linki do komentarzy z serwisu skąpiec.pl
    /// </summary>
    class SkapiecLinkProvider : ILinkProvider
    {
        /// <summary>
        /// konstruktor przyjmujący link początkowy
        /// </summary>
        /// <param name="pageLink"></param>
        public SkapiecLinkProvider(string pageLink)
        {
            m_pageLink = pageLink;
        }
        /// <summary>
        /// Zwraca kolejne linki
        /// </summary>
        /// <returns></returns>
        public string getLink()
        {
            if (!wasFirstPage)
            {
                wasFirstPage = true;
                return m_pageLink;
            }
            return commentPagesLinkProvider();
        }
        /// <summary>
        /// Parsuje i pobiera linki kolejnych stron komentarzy
        /// </summary>
        /// <returns></returns>
        private string commentPagesLinkProvider()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string pageContent = client.DownloadString(m_pageLink);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);


            string xpath = "";
            xpath = ".//div[@class=\"partial pager\"]/ul[@class=\"numeric-list\"]//li//a";
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (bodyNodes != null)
            {
                bool wasActive = false;
                foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                {
                    if (!wasActive)
                    {
                        foreach (HtmlAgilityPack.HtmlAttribute attribut in node.Attributes)
                        {

                            if (attribut.Value == "btn m blue selected")
                            {
                                wasActive = true;
                            }
                        }
                    }
                    else
                    {
                        m_pageLink = "http://www.skapiec.pl" + node.GetAttributeValue("href", "");
                        return m_pageLink;
                    }
                }
            }

            return "";
        }
        private bool wasFirstPage = false;
        private string m_pageLink;
    }

}
