﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    interface ICommentParser
    {
        string getCommentsContentFromPage(string pageContent);
    }
}
