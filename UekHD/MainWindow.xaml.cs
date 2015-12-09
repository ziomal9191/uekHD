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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UekHD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Text box changed ");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string textToGetComments = TextBox.GetLineText(0);
            HttpCommentGeter httpGeter = new HttpCommentGeter(textToGetComments);

        }
        //Clearbutton
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DatabaseContext context = new DatabaseContext();
            context.Comments.RemoveRange(context.Comments);
            context.SaveChanges();
           // IQueryable<CommentDb> productsQuery = from CommentDb in context.Comments;
        }
    }
}
