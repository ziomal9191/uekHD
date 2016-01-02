using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    interface ICommentParser
    {
        CommentList getCommentsContentFromPage(string pageContent, Product product);
        string getModelName(string pageContent);
        string getBrandName(string pageContent);
    }
}
