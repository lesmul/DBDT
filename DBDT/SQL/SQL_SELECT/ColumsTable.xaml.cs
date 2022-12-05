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
        public ColumsTable()
        {
            InitializeComponent();
        }

        public string columsselect = "";
        private void Click_Close(object sender, RoutedEventArgs e)
        {

          var aa = itContr.SelectedItems.Count;
            for (int i = 0; i < itContr.SelectedItems.Count; i++)
            {
                columsselect += itContr.SelectedItems[i].ToString() + " ,";
            }
            this.Close();
        }
    }
}
