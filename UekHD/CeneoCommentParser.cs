using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System;
using HtmlAgilityPack;
using System.Windows;

namespace UekHD
{
    /// <summary>
    /// Klasa parsuje komentarze dla danego produktu
    /// </summary>
    class CeneoCommentParser : ICommentParser
    {
        /// <summary>
        /// Metoda pobiera komentarze z pageContent do product
        /// </summary>
        /// <param name="pageContent"></param>
        /// <param name="product"></param>
        public void getCommentsContentFromPage(string pageContent, Product product)
        {


            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                throw new System.ExecutionEngineException();
                // Handle any parse errors as required
            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {
                    fillProductInfo(product);
                }
            }
        }
        /// <summary>
        ///  Metoda dla danego produktu pobiera informacje o produkcie
        /// </summary>
        /// <param name="product"></param>
        private void fillProductInfo(Product product)
        {
            fillComments(product);
            fillType(product);
            fillBrandAndModel(product);
        }
        /// <summary>
        /// Wypełnia pojedyńczy komentarz comment z danego node
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillComment(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            fillCommentContent(comment, node);
            fillStar(comment, node);
            fillAdvantages(comment, node);
            fillDisadvantages(comment, node);
            fillAuthor(comment, node);
            fillCommentDate(comment, node);
            fillRecommend(comment, node);
            fillUsability(comment, node);
        }
        /// <summary>
        /// Pobiera dane z node i sprawdza czy product się nie powtarza
        /// </summary>
        /// <param name="product"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool isCommentExistInProduct(Product product, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//p[@class=\"product-review-body\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            string commentToParse = "";

            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    commentToParse += commentNode.InnerText;
                }
            }
            foreach (CommentDb commentsInDb in product.Comments)
            {
                if (commentsInDb.Comment == null) { continue; }
                if (commentsInDb.Comment.Equals(commentToParse))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Pobiera z node do comment użyteczność komentarza
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillUsability(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//span[@class=\"product-review-usefulness-stats\"]//span");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    if (!commentNode.Attributes[0].Value.Contains("percent"))
                    {
                        if (commentNode.Attributes[0].Value.Contains("-yes-"))
                        {
                            comment.Usability = Convert.ToInt32(commentNode.InnerText);
                        }
                        else
                        {
                            comment.UsabilityVotes = Convert.ToInt32(commentNode.InnerText);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Uzupełnia czy dany komentarz rekomenduje
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillRecommend(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//em[@class=\"product-recommended\"]");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    if (commentNode.InnerText.Contains("Polecam"))
                    {
                        comment.Recommend = true;
                    }
                    else
                    {
                        comment.Recommend = false;
                    }
                }

            }
        }
        /// <summary>
        /// Uzupełnia czas wystawienia komentarza
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillCommentDate(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//div[@class=\"content-wide-col\"]//div//span[@class=\"review-time\"]//time");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    foreach (HtmlAgilityPack.HtmlAttribute attribut in commentNode.Attributes)
                    {

                        if (attribut.Name == "datetime")
                        {
                            comment.Date = DateTime.Parse(attribut.Value);
                        }
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// Uzupełnia pole autor
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillAuthor(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//div[@class=\"product-reviewer\"]");//div//span[@class=\"review-time\"]//time");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    comment.Author = commentNode.InnerText;
                }
            }
        }
        /// <summary>
        /// Uzupełnia tekst komentarza
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillCommentContent(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//p[@class=\"product-review-body\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    comment.Comment += commentNode.InnerText;
                }
            }

        }
        /// <summary>
        /// Wypełnienie produktu danymi o nazwie modelu i producenta
        /// </summary>
        /// <param name="product"></param>
        private void fillBrandAndModel(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dl//strong");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    string[] brand = nodeType.InnerHtml.Split(' ');
                    product.Brand = brand[0];
                    //get model
                    string model = "";
                    for (int i = 1; i < brand.Length; i++)
                    {
                        model += brand[i] + " ";
                    }
                    product.Model = model;
                }
            }


        }
        /// <summary>
        /// Wypełnienie produktu daną o typie
        /// </summary>
        /// <param name="product"></param>
        private void fillType(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dd//span[last()]//span");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    product.Type = nodeType.InnerText;
                }
            }

        }
        /// <summary>
        /// Uzupełnienie komentarza danymi wad
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillDisadvantages(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//span[@class=\"cons-cell\"]//li");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    comment.Disadvantages += commentNode.InnerText;
                }

            }
        }
        /// <summary>
        /// Uzupełnienie komentarza danymi zalet
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillAdvantages(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//span[@class=\"pros-cell\"]//li");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    comment.Advantages += commentNode.InnerText;
                }
            }
        }
        /// <summary>
        /// Uzupełnienie komentarza danymi ilości gwiazdek
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="node"></param>
        private void fillStar(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//span[@class=\"review-score-count\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    string score = commentNode.InnerText;
                    string[] scoreCount = score.Split('/');
                    comment.Stars = Convert.ToDouble(scoreCount[0]);
                }

            }
        }
        /// <summary>
        /// Uzupełnienie komentarza danymi komentarza
        /// </summary>
        /// <param name="product"></param>
        void fillComments(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//ol[@class=\"product-reviews js_product-reviews js_reviews-hook\"]/li");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                {
                    if (!isCommentExistInProduct(product, node))
                    {

                        CommentDb comment = new CommentDb();
                        comment.PortalName = "Ceneo";
                        fillComment(comment, node);
                        product.Comments.Add(comment);
                    }
                }
            }
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
    }
}
