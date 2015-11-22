﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class CeneoCommentParser : ICommentParser
    {
        public string getCommentsContentFromPage(string pageContent)
        {

            //System.IO.File.Create("lala.txt");
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter("./lala.txt"))
            {

                outputFile.WriteLine(pageContent);
                outputFile.Flush();
                outputFile.Close();

                
            }
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(pageContent);

           
            return pageContent;
        }
        
    }
}
