using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UekHD
{
    /// <summary>
    /// Interaction logic for DatabasePresentationWindow.xaml
    /// </summary>
    public partial class DatabasePresentationWindow : Window
    {
        public DatabasePresentationWindow()
        {
            InitializeComponent();
            using (DatabaseContext siContext = new DatabaseContext())
            {
                var query = (from Product in siContext.Product
                            select  Product).ToList();
                //var co = query.Select<CommentDb>();
                /*  var l = from Product.Comments in query select CommentDb;
                  List<String> comments;
                  foreach (CommentDb v in query)
                  {
                      comments.Add(v.Comment);
                  }*/
                List<Product> comments = new List<Product>();

                foreach (var comment in query)
                {
                    comments.Add(comment);
                    /*if (comment.Count>0)
                    {
                        foreach (var com in comment)
                        {
                            comments.Add(com);
                        }
                    }*/
                   // comments.Add(from x in siContext.Comments where x.CommentDbID = comment.CommentDbID);
                }
                //var l = from CommentDb in query select CommentDb.Comment;
                dataGrid.ItemsSource = comments.ToList();
            }

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (DatabaseContext siContext = new DatabaseContext())
            {
                var query = (from Product in siContext.Product
                             select Product.Comments).ToList();
                //var co = query.Select<CommentDb>();
                /*  var l = from Product.Comments in query select CommentDb;
                  List<String> comments;
                  foreach (CommentDb v in query)
                  {
                      comments.Add(v.Comment);
                  }*/
                List<CommentDb> comments = new List<CommentDb>();

                foreach (var comment in query)
                {
                    if (comment.Count > 0)
                    {
                        foreach (var com in comment)
                        {
                            comments.Add(com);
                        }
                    }
                    // comments.Add(from x in siContext.Comments where x.CommentDbID = comment.CommentDbID);
                }
                //var l = from CommentDb in query select CommentDb.Comment;
                dataGrid.ItemsSource = comments.ToList();
            }
        }
    }
}
