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
                {////*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol
                    //                                                                               
                    HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[2]/div/div/div[2]/div[3]/div[2]/ol/li/div/div[1]/p");// //body//div[@id='body']class=\"product - review - body\"");
                    if (bodyNodes != null)
                        using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter("./lala.txt"))
                        {
                            string text="";
                            foreach (HtmlAgilityPack.HtmlNode node in bodyNodes)
                            {

                                //outputFile.WriteLine(pageContent);
                                text += node.InnerText; 
  //                              outputFile.WriteLine(node.ToString());
  //                              outputFile.Flush();
  //                              outputFile.Close();


                            }
                            outputFile.WriteLine(text);
                                                          outputFile.Flush();
                                                          outputFile.Close();
                        }
                }


            }
            return pageContent;
        }

    }
}
