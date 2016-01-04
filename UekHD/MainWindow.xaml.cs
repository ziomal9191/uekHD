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
            using (DatabaseContext siContext = new DatabaseContext())
            {
                foreach (Product p in siContext.Product)
                {
                    XmlSerializer writer = new XmlSerializer(p.GetType());
                    StreamWriter file = new StreamWriter("data.xml");
                    writer.Serialize(file, p);
                    file.Close();
                }
            }
        }
        private HttpCommentGeter httpGeter;
    }
}
