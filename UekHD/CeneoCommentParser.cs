using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

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
                    fillComments(product);
                    using (var db = new DatabaseContext())
                    {
                        db.Product.Add(product);
                        db.SaveChanges();
                    }

                }


            }
            return listOfComments;
        }
        void fillComments(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
            if (bodyNodes != null)
                foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                {
                    //HtmlAgilityPack.HtmlNodeCollection bodyNodes1 = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"pros-cell\"]");;
                    
                    //HtmlAgilityPack.HtmlNodeCollection bodyNodes2 = htmlDoc.DocumentNode.SelectNodes("//*[@class=\"review-score-count\"]"); ;
                    product.Comments.Add(new CommentDb { Comment = node.InnerText });
                    
                }
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
    }
}
