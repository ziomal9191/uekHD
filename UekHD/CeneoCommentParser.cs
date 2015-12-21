using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System;

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
        }

        private void fillAdvantages(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
         //   pros - cell
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes("//span[@class=\"pros-cell\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)//[class=\"product-reviews js_product-reviews js_reviews-hook\"]
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {

           //         comment.Comment = commentNode.InnerText;
                }

            }
        }

        private void fillStar(CommentDb comment, HtmlAgilityPack.HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes("//span[@class=\"review-score-count\"]");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
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
                    //HtmlAgilityPack.HtmlNodeCollection bodyNodes1 = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"pros-cell\"]");;

                    //HtmlAgilityPack.HtmlNodeCollection bodyNodes2 = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"review-score-count\"]"); ;
                    CommentDb comment = new CommentDb(); //{ Comment = node.InnerText };
                    fillComment(comment, node);
                    product.Comments.Add(comment);

                }
            }
            
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
    }
}
