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

        public UWPF_ZAPYTANIE_SQL()
        {
            InitializeComponent();
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            
            dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql from sql_zapytania order by id desc");

            LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
        }

        private void mouse_dbl_clikck(object sender, MouseButtonEventArgs e)
        {
            Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;

            string curItem = ((System.Data.DataRowView)selectedItem).Row.ItemArray[2].ToString();
            string curItemID = ((System.Data.DataRowView)selectedItem).Row.ItemArray[0].ToString();

            //((System.Windows.Controls.Panel)((System.Windows.FrameworkElement)sender).Parent).Children

            MainWindowSQL sp = new MainWindowSQL();

            sp.txtCode.Text = curItem;
            sp.txtCode.Tag = ((System.Data.DataRowView)selectedItem).Row.ItemArray[1].ToString(); ;

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            ((System.Windows.Controls.Panel)((System.Windows.FrameworkElement)sender).Parent).Children.Add(new MdiChild { Content = sp, Title = "Dodaj nowe zaptanie SQL - " + curItemID, WindowState = WindowState.Maximized, Width = szer, Height = wys });

        }
        private void buttonSzukaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql from sql_zapytania where nazwa_zapytania like '%" + txtWhere.Text.Trim() + "%' order by id desc");
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
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_zapytania, sql from sql_zapytania order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
        }
    }
}
