using DBDT.Konfiguracja;
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
using static DBDT.SQL.WPF_DODAJ_EXCEL;

namespace DBDT.SQL
{
    /// <summary>
    /// Logika interakcji dla klasy UWPF_ZAPYTANIE_SQL.xaml
    /// </summary>
    public partial class UWPF_EXCEL_SQL : UserControl
    {
        public int szer;
        public int wys;
        DataTable dt = new DataTable();

        public UWPF_EXCEL_SQL()
        {
            InitializeComponent();
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            
            dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_objektu, opis, pole1 from objekty order by id desc");

            LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
        }

        private void mouse_dbl_clikck(object sender, MouseButtonEventArgs e)
        {
            Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;

            //((System.Windows.Controls.Panel)((System.Windows.FrameworkElement)sender).Parent).Children

            WPF_DODAJ_EXCEL FRM = new WPF_DODAJ_EXCEL();

            FRM.id_rec = ((System.Data.DataRowView)selectedItem).Row.ItemArray[0].ToString();

            FRM.TXT_OPIS.Text= ((System.Data.DataRowView)selectedItem).Row.ItemArray[1].ToString(); ;
            FRM.TXT_NAZWA_OBJ.Text = ((System.Data.DataRowView)selectedItem).Row.ItemArray[2].ToString();

            XLSX iXLSX = new XLSX(((System.Data.DataRowView)selectedItem).Row.ItemArray[3].ToString(), "/SQL/", "/IKONY/excel.ico", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green));
            FRM.items.Add(iXLSX);

            FRM.ZaladujLV();

            if (FRM.ShowDialog() == true)
            {
              //  _PUBLIC_SqlLite.DODAJ_REKORD_OBJEKT(FRM.TXT_NAZWA_OBJ.Text.Trim(), FRM.TXT_OPIS.Text.Trim(), FRM.items[0].CalaScieszka, FRM.items[0].TextExcel);
            }

        }
        private void buttonSzukaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_objektu, opis, pole1 from objekty where opis like '%" + txtWhere.Text.Trim() + "%' order by id desc");
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
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_objektu, opis, pole1 from objekty order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
        }

        private void Click_Dodaj(object sender, RoutedEventArgs e)
        {
            WPF_DODAJ_EXCEL FRM = new WPF_DODAJ_EXCEL();

            if (FRM.ShowDialog() == true)
            {
                _PUBLIC_SqlLite.DODAJ_REKORD_OBJEKT(FRM.TXT_NAZWA_OBJ.Text.Trim(), FRM.TXT_OPIS.Text.Trim(), FRM.items[0].CalaScieszka, FRM.items[0].TextExcel);
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_objektu, opis, pole1 from objekty order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }
        }

        private void Click_Usun(object sender, RoutedEventArgs e)
        {
            if (LB_HIST_ZAPYTAN_SQL.SelectedItems.Count == 0) return;

            if (MessageBox.Show("Czy usunąć plik automatyzacji?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                Object selectedItem = LB_HIST_ZAPYTAN_SQL.SelectedItem;
               _PUBLIC_SqlLite.USUN_REKORD_OBJEKT(((System.Data.DataRowView)selectedItem).Row.ItemArray[0].ToString());
                dt = _PUBLIC_SqlLite.SelectQuery("select id, nazwa_objektu, opis, pole1 from objekty order by id desc");
                LB_HIST_ZAPYTAN_SQL.ItemsSource = dt.AsDataView();
            }

            
        }
    }
}
