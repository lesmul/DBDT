using DBDT.SQL.SQL_SELECT;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.MDI;

namespace DBDT.SQL
{
    /// <summary>
    /// Logika interakcji dla klasy UWPF_ZAPYTANIE_SQL.xaml
    /// </summary>
    public partial class UWPF_ZAPYTANIE_SQL : UserControl
    {
        public int szer;
        public int wys;
        DataTable dt = new DataTable();

        private SqlHandler sqlHandler;

        public UWPF_ZAPYTANIE_SQL()
        {
            InitializeComponent();
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            
            dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania order by id desc");

            LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
        }

        private void mouse_dbl_clikck(object sender, MouseButtonEventArgs e)
        {
            Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;

            string curItem = ((System.Data.DataRowView)selectedItem).Row.ItemArray[2].ToString();
            string curItemID = ((System.Data.DataRowView)selectedItem).Row.ItemArray[0].ToString();

             MainWindowSQL sp = new MainWindowSQL();

            sp.txtCode.Text = curItem;
            sp.txtCode.Tag = ((DataRowView)selectedItem).Row.ItemArray[1].ToString();
            sp.id_rec = Convert.ToInt32(curItemID);

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
   
            var parentWindow = Window.GetWindow(this.Parent);

            ((MainWindow)parentWindow).Container.Children.Add(new MdiChild { Content = sp, Name = "FindSQLWindow", Title = "Zapytanie SQL " + ((DBDT.MainWindow)parentWindow).ooo++, WindowState = WindowState.Maximized, Width = ((DBDT.MainWindow)parentWindow).SHT_W, Height = ((DBDT.MainWindow)parentWindow).SHT_H });
            //parentWindow.Close(); // zakończ program
       
        }
        private void buttonSzukaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania where nazwa_zapytania like '%" + txtWhere.Text.Trim() + "%' order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtChanged(object sender, TextChangedEventArgs e)
        {
            if (txtWhere.Text.Trim() == "")
            {
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
        }

        private void Click_Zmien(object sender, RoutedEventArgs e)
        {
            sqlHandler = new SqlHandler();

            Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;
            string[] opiszw = sqlHandler.SQL_Title(((DataRowView)selectedItem).Row.ItemArray[1].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[3].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[4].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[5].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[6].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[7].ToString(),
                ((DataRowView)selectedItem).Row.ItemArray[8].ToString());

            if (opiszw != null)
            {
                _PUBLIC_SqlLite.ZMIEN_OPIS_REKORD_SQL_ZAPYTANIA(opiszw[0], opiszw[1], opiszw[2], opiszw[3], opiszw[4], opiszw[5], opiszw[6],
                    ((DataRowView)selectedItem).Row.ItemArray[0].ToString());
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
        }
        private void Click_Usun(object sender, RoutedEventArgs e)
        {
            if (LB_HIST_ZAPYTAN_SQL.SelectedItems.Count == 0) return;

            if (MessageBox.Show("Czy usunąć zapisaną procedurę SQL?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;
                _PUBLIC_SqlLite.USUN_REKORD_SQL_ZAPYTANIA(((System.Data.DataRowView)selectedItem).Row.ItemArray[0].ToString());
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6 from sql_zapytania order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }

        }
    }
}
