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
                var query = from p in siContext.Product
                            select p;

                dataGrid.ItemsSource = query.ToList();
            }

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* System.Windows.Data.CollectionViewSource categoryViewSource =
             ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoryViewSource")));
             using (var db = new DatabaseContext())
             {

                 categoryViewSource.Source = db.Product.Local;
             }*/
            // dataGrid.DataContext = new LinqServerModeSource();
            using (DatabaseContext siContext = new DatabaseContext())
            {
                var query = from p in siContext.Product
                            select p;

                dataGrid.ItemsSource = query.ToList();
            }
        }
    }
}
