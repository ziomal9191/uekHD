using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    interface IStatisctics
    {
         void addDowlodedPage(string url);
         List<string> getDowloadedPages();
         void addAddedComment(string comment);
         int getNewCommentCount();
        string getSummary();
    }
}
