using System;
using System.Collections.Generic;
using System.Data;
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

namespace DBDT.SQL.SQL_SELECT
{
    /// <summary>
    /// Logika interakcji dla klasy ColumsTable.xaml
    /// </summary>
    public partial class ColumsTable : Window
    {

        List<string> itemsSort = new List<string>();
        public ColumsTable()
        {
            InitializeComponent();
        }

        public string columsselect = "";
        private void Click_Close(object sender, RoutedEventArgs e)
        {

            //var aa = itContr.SelectedItems.Count;

            for (int i = 0; i < itContr.SelectedItems.Count; i++)
            {

                if (itContr.SelectedItems[i].ToString().IndexOf("[") > 0)
                {
                    columsselect += itContr.SelectedItems[i].ToString().Substring(0, itContr.SelectedItems[i].ToString().IndexOf("[")) + ", ";
                }
                else
                {
                    columsselect += itContr.SelectedItems[i].ToString() + ", ";
                }

            }
            this.Close();
        }

        private void text_changed(object sender, TextChangedEventArgs e)
        {

            List<string> strings = itemsSort.ConvertAll(obj => obj?.ToString().ToLower());

            var re = strings.Where(x => x.IndexOf(((TextBox)sender).Text.ToLower()) > -1);

            itContr.ItemsSource = re;
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            itemsSort = (List<string>)itContr.ItemsSource;
        }
    }
}
