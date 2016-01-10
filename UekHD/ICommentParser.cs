using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    /// <summary>
    /// Parser komentarzy dla danego produktu. 
    /// </summary>
    interface ICommentParser
    {
        /// <summary>
        /// Metoda pobiera dane z pageContent i uzupełnia komentarze do product.
        /// </summary>
        /// <param name="pageContent"></param>
        /// <param name="product"></param>
        void getCommentsContentFromPage(string pageContent, Product product);
    }
}
