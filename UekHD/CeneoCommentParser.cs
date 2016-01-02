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


    class CeneoCommentParser : ICommentParser
    {
        public CommentList getCommentsContentFromPage(string pageContent, Product product)
        {


            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            CommentList listOfComments = new CommentList();
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                throw new System.ExecutionEngineException();
                // Handle any parse errors as required

            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {
                    //Parsing start
                    //Product product = new Product();
                    fillProductInfo(product);
                }
            }
            return listOfComments;
        }

        private void fillProductInfo(Product product)
        {
            fillComments(product);
            fillType(product);
            fillBrandAndModel(product);
            fillAdditionalComment(product);
        }

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
        //    }
        }

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
            foreach(CommentDb commentsInDb in product.Comments)
            {
                if (commentsInDb.Comment == null) { continue; }
                if (commentsInDb.Comment.Equals(commentToParse))
                {
                    return true;
                }
            }

            return false;
        }

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
        private void fillAdditionalComment(Product product)
        {

        }

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
                    fillComment(comment, node);
                    product.Comments.Add(comment);
                }
            }
            }
        }
        public string getModelName(string pageContent)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);

            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dl//strong");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    string[] brand = nodeType.InnerHtml.Split(' ');
                    string model = "";
                    for (int i = 1; i < brand.Length; i++)
                    {
                        model += brand[i] + " ";
                    }
                    return model;
                }
            }
            return "";
        }
        public string getBrandName(string pageContent)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            // HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dl//strong");

            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dl//strong");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    string[] brand = nodeType.InnerHtml.Split(' ');
                    return brand[0];
                }
            }
            return "";
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
    }
}
