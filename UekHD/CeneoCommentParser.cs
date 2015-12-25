using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System;
using HtmlAgilityPack;

namespace UekHD
{

   
    class CeneoCommentParser : ICommentParser
    {
        public CommentList getCommentsContentFromPage(string pageContent)
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
                    Product product = new Product();
                    fillProductInfo(product);
                    using (var db = new DatabaseContext())
                    {
                        db.Product.Add(product);
                        db.SaveChanges();
                    }

                }


            }
            return listOfComments;
        }

        private void fillProductInfo(Product product)
        {
            fillComments(product);
            fillType(product);
            fillBrand(product);
            fillModel(product);
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
            //product - recommended
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

        private void fillModel(Product product)
        {
        }

        private void fillBrand(Product product)
        {
        }

        private void fillType(Product product)
        {
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
                    CommentDb comment = new CommentDb(); 
                    fillComment(comment, node);
                    product.Comments.Add(comment);
                }
            }
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
    }
}
