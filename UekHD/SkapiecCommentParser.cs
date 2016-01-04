using HtmlAgilityPack;
using System;
using System.Linq;

namespace UekHD
{
    internal class SkapiecCommentParser : ICommentParser
    {
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

        private void fillProductInfo(Product product)
        {
            fillComments(product);
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

        private bool isCommentExistInProduct(Product product, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//div[@class=\"opinion-container\"]//p");
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

        private void fillUsability(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(".//span[@class=\"btn gray link\"]");
            int votesToYes = 0;
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    if (commentNode.InnerHtml.Contains("ico-hand-down"))
                    {
                    string textToParse = commentNode.InnerHtml;
                    textToParse = textToParse.Replace("<i class=\"ico-hand-down\">&nbsp;</i>Pomocna (", "");
                    textToParse = textToParse.Replace(")", "");
                    votesToYes =  Convert.ToInt32(textToParse);
                    comment.Usability = votesToYes;
                    
                    }
                    else
                    {

                    string textToParse = commentNode.InnerHtml;
                    textToParse = textToParse.Replace("<i class=\"ico-hand-up\">\n                      &nbsp;</i>(", "");
                    textToParse = textToParse.Replace(")", "");
                    comment.UsabilityVotes = votesToYes + Convert.ToInt32(textToParse);
                    
                    }
                   
                }
            }
            bodyNodes = node.SelectNodes("./span[@class=\"btn gray link\"][2]");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                    string ll = commentNode.InnerHtml;
                    //comment.Usability = Convert.ToInt32(commentNode.InnerText);
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
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(containerPath+"span[@class=\"date\"]");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode commentNode in bodyNodes)
                {
                            comment.Date = DateTime.Parse(commentNode.InnerHtml);
                    break;
                }
            }
        }

        private void fillAuthor(CommentDb comment, HtmlNode node)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(containerPath+"span[@class=\"author\"]");//div//span[@class=\"review-time\"]//time");
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
            HtmlNodeCollection bodyNodes = node.SelectNodes(".//div[@class=\"opinion-container\"]//p");//("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
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
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(containerPath + "ul[@class=\"pros-n-cons\"]//li[2]");
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
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = node.SelectNodes(containerPath + "ul[@class=\"pros-n-cons\"]//li[1]");
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
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//ul[@class=\"opinion-list\"]/li");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                {
                    if (!isCommentExistInProduct(product, node))
                    {

                        CommentDb comment = new CommentDb();
                        comment.PortalName = "Skapiec";
                        fillComment(comment, node);
                        product.Comments.Add(comment);
                    }
                }
            }
        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
        string containerPath = ".//div[@class=\"opinion-container\"]//";
    }
}