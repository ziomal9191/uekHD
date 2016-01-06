using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

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

        private void buttonEtl(object sender, RoutedEventArgs e)
        {
            string textToGetComments = TextBox.GetLineText(0);
            IStatisctics statistics = new StatiscticsObject();
            HttpCommentGeter httpGeter = new HttpCommentGeter(textToGetComments,
                                                              statistics);
            httpGeter.loadProductToDataBase();
        }

        private void buttonClearDb(object sender, RoutedEventArgs e)
        {
            DatabaseContext context = new DatabaseContext();
            context.Comments.RemoveRange(context.Comments);
            context.Product.RemoveRange(context.Product);
            context.SaveChanges();
        }

        //Present database
        private void buttonPresentDatabse(object sender, RoutedEventArgs e)
        {
            DatabasePresentationWindow dbWindow = new DatabasePresentationWindow();
            dbWindow.Show();
        }

        private void buttonExtract(object sender, RoutedEventArgs e)
        {

            if (!wasTransform && wasLoad && !wasExtract)
            {

                wasExtract = true;
                string textToGetComments = TextBox.GetLineText(0);
                IStatisctics statistics = new StatiscticsObject();
                httpGeter = new HttpCommentGeter(textToGetComments,
                                                              statistics);
                
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wrong sequence", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Warning);
                //warning window
            }
        }
        private bool wasExtract=false;
        private bool wasTransform=false;
        private bool wasLoad=true;

        private void buttonTransform(object sender, RoutedEventArgs e)
        {
            if (!wasTransform && wasLoad && wasExtract)
            {
                wasTransform = true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wrong sequence", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Warning);
                //warning window
            }
        }

        private void buttonLoad(object sender, RoutedEventArgs e)
        {
            if (wasExtract && wasTransform /*&& !wasLoad*/)
            {
                wasExtract = false;
                wasTransform = false;
                httpGeter.loadProductToDataBase();
               // wasLoad = flase;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Wrong sequence", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Warning);
                //warning window
            }
            
        }


        private void buttonSaveToFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".xml"; // Default file extension
            dialog.Filter = "Xml documents (.xml)|*.xml"; // Filter files by extension
            if (dialog.ShowDialog(this) == true)
            {
                using (DatabaseContext siContext = new DatabaseContext())
                {
                    List<Product> query = (from Product in siContext.Product
                                           select Product).ToList();
                    using (var writer = System.Xml.XmlWriter.Create(dialog.FileName))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("InfoDb");
                        foreach (Product product in query)
                        {
                            writer.WriteStartElement("Product");
                            writer.WriteElementString("ProductId", product.ProductId.ToString());
                            writer.WriteElementString("Type", product.Type);
                            writer.WriteElementString("Brand", product.Brand);
                            writer.WriteElementString("Model", product.Model);
                            foreach (CommentDb comment in product.Comments)
                            {
                                writer.WriteStartElement("CommentBlock");
                                writer.WriteElementString("CommentDbID", comment.CommentDbID.ToString());
                                writer.WriteElementString("Comment", comment.Comment);
                                writer.WriteElementString("Stars", comment.Stars.ToString());
                                writer.WriteElementString("Advantages", comment.Advantages);
                                writer.WriteElementString("Disadvantages", comment.Disadvantages);
                                writer.WriteElementString("Author", comment.Author);
                                writer.WriteElementString("Date", comment.Date.ToString());
                                writer.WriteElementString("Recomend", comment.Recommend.ToString());
                                writer.WriteElementString("Usability", comment.Usability.ToString());
                                writer.WriteElementString("UsabilityVotes", comment.UsabilityVotes.ToString());
                                writer.WriteElementString("PortalName", comment.PortalName);
                                writer.WriteEndElement();

                            }
                            writer.WriteEndElement();
                        }
                    }
                }
            }
        }
        private HttpCommentGeter httpGeter;
    }
}
