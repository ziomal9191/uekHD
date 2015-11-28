using System.Linq;

namespace UekHD
{
    
    class CeneoCommentParser : ICommentParser
    {
        public CommentList getCommentsContentFromPage(string pageContent)
        {

            //System.IO.File.Create("lala.txt");


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            string text = "";
            CommentList listOfComments = new CommentList();
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                throw new System.ExecutionEngineException();
                // Handle any parse errors as required

            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {////*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol
                    //                                                                               
                    HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
                    if (bodyNodes != null)
                        using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter("./lala.txt", true, System.Text.Encoding.UTF8))
                        {
                            var encoding = new System.Text.UTF8Encoding();
                            foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                            {
                                listOfComments.Add(new CeneoProductComment(node.InnerText));
                                text += node.InnerText;
                               
                            }
                           
                            outputFile.WriteLine(text);
                            outputFile.Flush();
                            outputFile.Close();
                        }
                }


            }
            return listOfComments;
        }

    }
}
