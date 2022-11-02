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

namespace DBDT.SQL.Indeksy
{
    /// <summary>
    /// Logika interakcji dla klasy IndeksTable.xaml
    /// </summary>
    public partial class IndeksTable : Window
    {

        public string public_str_copy;

        List<object> itemsSort = new List<object>();
        public List<object> itemsIndeksy = new List<object>();

        public IndeksTable()
        {
            InitializeComponent();
         }

        public string columsselect = "";
        private void Click_Close(object sender, RoutedEventArgs e)
        {

            //var aa = itContr.SelectedItems.Count;

            for (int i = 0; i < itContr.SelectedItems.Count; i++)
            {
                columsselect += itContr.SelectedItems[i].ToString() + "\r\n";
            }

            Clipboard.SetDataObject(columsselect.TrimEnd(Environment.NewLine.ToCharArray()));

            public_str_copy = columsselect.TrimEnd(Environment.NewLine.ToCharArray());

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
            itemsSort = (List<object>)itContr.ItemsSource;
            this.Tag = "OK";
        }

        private void wind_Closed(object sender, EventArgs e)
        {
            itContr.ItemsSource = null;
            this.Tag = "NOK";
            itemsIndeksy.Clear();
        }
    }
}
