using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                List<Product> comments = new List<Product>();

                foreach (var comment in query)
                {
                    comments.Add(comment);
                }
                productsDb.ItemsSource = comments.ToList();
            }
            using (DatabaseContext siContext = new DatabaseContext())
            {
                var query = (from Product in siContext.Product
                             select Product.Comments).ToList();
                List<CommentDb> comments = new List<CommentDb>();

                foreach (var comment in query)
                {
                    if (comment.Count > 0)
                    {
                        foreach (var com in comment)
                        {
                            com.Comment = Regex.Replace(com.Comment, @"<[^>]+>|&nbsp;", "").Trim();
                            comments.Add(com);
                        }
                    }
                }
                commentData.ItemsSource = comments.ToList();
            }
            
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (DatabaseContext siContext = new DatabaseContext())
            {
                var query = (from Product in siContext.Product
                             select Product.Comments).ToList();
                List<CommentDb> comments = new List<CommentDb>();

                foreach (var comment in query)
                {
                    if (comment.Count > 0)
                    {
                        foreach (var com in comment)
                        {
                            com.Comment = Regex.Replace(com.Comment, @"<[^>]+>|&nbsp;", "").Trim();
                            comments.Add(com);
                        }
                    }
                }
                commentData.ItemsSource = comments.ToList();
            }
        }

        private void productSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (DatabaseContext siContext = new DatabaseContext())
            {
                if (productsDb.SelectedItem is Product)
                {
               
                var item = (Product)productsDb.SelectedItem;
                   var getProduct = from Product in siContext.Product where Product.Model.Equals(item.Model) select Product.Comments;
                    List<CommentDb> comments = new List<CommentDb>();

                    foreach (var comment in getProduct)
                    {
                        if (comment.Count > 0)
                        {
                            foreach (var com in comment)
                            {
                                com.Comment = Regex.Replace(com.Comment, @"<[^>]+>|&nbsp;", "").Trim();
                                comments.Add(com);
                            }
                        }
                    }
                    commentData.ItemsSource = comments.ToList();
                }
                else
                {
                    commentData.ItemsSource = new List<Product>();
                
                }
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
