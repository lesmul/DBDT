using DBDT.USTAWIENIA_PROGRAMU;
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
using System.Data.SqlClient;
using System.Data;

namespace DBDT.Konfiguracja
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_CONNECTION.xaml
    /// </summary>
    public partial class WPF_KONFIG_CONNECTION : Window
    {
        public WPF_KONFIG_CONNECTION()
        {
            InitializeComponent();
        }

        private void zakoncz_click(object sender, RoutedEventArgs e)
        {
            if (TXT_NAZWA_SERWERA.Text.Trim() != "" && TXT_NAZWA_BAZY.Text.Trim() != "")
            { 
                var dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                if (dr== MessageBoxResult.Yes)
                {

                    _PUBLIC_SqlLite.Existsdb("");
           
                    DataTable dt = new DataTable();
                    dt = _PUBLIC_SqlLite.SelectQuery("select count(*) from ParametryPalaczenia");

                    if (dt.Rows.Count > 500)
                    {
                        _PUBLIC_SqlLite.USUN_REKORDY_PAR_POLACZENIA();
                    }

                    _PUBLIC_SqlLite.USUN_REKORD_PAR_POLACZENIA(TXT_NAZWA_SERWERA.Text.Trim(), TXT_NAZWA_BAZY.Text.Trim());

                    _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA(TXT_NAZWA_SERWERA.Text.Trim(), TXT_NAZWA_BAZY.Text.Trim());

                }
            }

            Close();

        }
        private void test_click(object sender, RoutedEventArgs e)
        {
            if (TXT_NAZWA_SERWERA.Text.Trim() == "" || TXT_NAZWA_BAZY.Text.Trim() == "")
            {
                MessageBox.Show("Wypełnij pola - nazwa bazy danych oraz nazwa serwera","Uwaga!!!");
                return;
            }
                _connect_mssql.connetionString = @"Server=" + TXT_NAZWA_SERWERA.Text.Trim() + ";Database=" + TXT_NAZWA_BAZY.Text.Trim() + ";Trusted_Connection=True";
    
            try
            {
                _connect_mssql.cnn = new SqlConnection(_connect_mssql.connetionString);
                _connect_mssql.cnn.Open();
                _connect_mssql.cnn.Close();

                MessageBox.Show("Połączyłem się z serwerem", "Jest OK :)");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void zapisane_polaczenia(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy, serwer || ' - ' || nazwa_bazy as SerwerIBaza FROM ParametryPalaczenia order by id desc");

            CB_HIST_POL.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                TXT_NAZWA_BAZY.Text = dt.Rows[0][2].ToString();

                TXT_NAZWA_SERWERA.Text = dt.Rows[0][1].ToString();
            }

        }

        private void CB_HIST_POL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             Object selectedItem = CB_HIST_POL.SelectedItem;

            TXT_NAZWA_SERWERA.Text = ((System.Data.DataRowView)selectedItem).Row.ItemArray[1].ToString();
            TXT_NAZWA_BAZY.Text = ((System.Data.DataRowView)selectedItem).Row.ItemArray[2].ToString();
        }
    }
}
