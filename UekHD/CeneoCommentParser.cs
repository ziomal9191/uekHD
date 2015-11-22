using System.Linq;

namespace UekHD
{
    class CeneoCommentParser : ICommentParser
    {
        public string getCommentsContentFromPage(string pageContent)
        {

            //System.IO.File.Create("lala.txt");


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
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

                    HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//p*");// class=\"product - review - body\"");
                    if (bodyNodes != null)
                        using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter("./lala.txt"))
                        {

                            foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                            {

                                //outputFile.WriteLine(pageContent);
                                outputFile.WriteLine(node.InnerHtml);
                                outputFile.Flush();
                                outputFile.Close();


                            }
                        }
                }


            }
            return pageContent;
        }

    }
}
