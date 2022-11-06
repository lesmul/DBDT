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

                    _PUBLIC_SqlLite.USUN_REKORDY_PAR_POLACZENIA();

                    _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA(TXT_NAZWA_SERWERA.Text.Trim(), TXT_NAZWA_BAZY.Text.Trim());

                }
            }

            Close();

        }
        private void test_click(object sender, RoutedEventArgs e)
        {
            if (TXT_NAZWA_SERWERA.Text.Trim() == "" || TXT_NAZWA_BAZY.Text.Trim() == "")
            {
                MessageBox.Show("Wypełnij pola - nazwa bazy danych oraz nazwa serwera");
                return;
            }
                _connect_mssql.connetionString = @"Server=SQLServer\\" + TXT_NAZWA_SERWERA.Text.Trim() + ";Database=" + TXT_NAZWA_BAZY.Text.Trim() + ";Trusted_Connection=True;MultipleActiveResultSets=true";
    
            try
            {
                _connect_mssql.cnn = new SqlConnection(_connect_mssql.connetionString);
                _connect_mssql.cnn.Open();
                _connect_mssql.cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
