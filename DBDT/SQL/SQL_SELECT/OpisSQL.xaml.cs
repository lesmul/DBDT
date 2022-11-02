using DBDT.USTAWIENIA_PROGRAMU;
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
    /// Logika interakcji dla klasy OpisSQL.xaml
    /// </summary>
    public partial class OpisSQL : Window
    {
        public OpisSQL()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt = _PUBLIC_SqlLite.SelectQuery("select distinct pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania order by pole1, pole2, pole3, pole4, pole5, pole6");

            var sql1 = from DataRow myRow in dt.Rows where (string)myRow["pole1"] != "" select myRow["pole1"];
            var sql2 = from DataRow myRow in dt.Rows where (string)myRow["pole2"] != "" select myRow["pole2"];
            var sql3 = from DataRow myRow in dt.Rows where (string)myRow["pole3"] != "" select myRow["pole3"];
            var sql4 = from DataRow myRow in dt.Rows where (string)myRow["pole4"] != "" select myRow["pole4"];
            var sql5 = from DataRow myRow in dt.Rows where (string)myRow["pole5"] != "" select myRow["pole5"];
            var sql6 = from DataRow myRow in dt.Rows where (string)myRow["pole6"] != "" select myRow["pole6"];

            CBpoziom1.ItemsSource = sql1.Distinct();
            CBpoziom2.ItemsSource = sql2.Distinct();
            CBpoziom3.ItemsSource = sql3.Distinct();
            CBpoziom4.ItemsSource = sql4.Distinct();
            CBpoziom5.ItemsSource = sql5.Distinct();
            CBpoziom6.ItemsSource = sql6.Distinct();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
