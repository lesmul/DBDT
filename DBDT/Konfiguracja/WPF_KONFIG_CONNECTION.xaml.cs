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
using Microsoft.Win32;

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

            wart_pol();

            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole2, pole3, pole4, pole5, pole6, pole10, pole11  FROM ParametryPalaczenia WHERE pole9 LIKE 'POLE_KONFIGURACJI_INDEKSOW' order by id desc");

            if (dt.Rows.Count > 0)
            {
                TXT_INDESK_DOPLAT.Text = dt.Rows[0]["pole2"].ToString();
                TXT_INDESK_DUMMY.Text = dt.Rows[0]["pole3"].ToString();
                TXT_INDESK_INNY.Text = dt.Rows[0]["pole4"].ToString();
                TXT_INDESK_DRUK.Text = dt.Rows[0]["pole5"].ToString();
                TXT_INDESK_UZYTKOWNIKA.Text = dt.Rows[0]["pole6"].ToString();

                TXT_SQL_PUSTY_NUMER.Text = dt.Rows[0]["pole10"].ToString();
                TXT_SQL_XML_SERII.Text = dt.Rows[0]["pole11"].ToString();

            }
        }

        private void wart_pol()
        {
            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10, pole8, pole9 FROM ParametryPalaczenia WHERE pole9 LIKE 'TXT_LOKALIZACJA_PLIKOW_%' OR pole9 ='TXT_LOKALIZACJA_PLIKU_MATKI' order by id desc");

            sbyte sb_poziom = 0;
            LBL_LK_1.Content = "Lokalizacja - katalog głowny [#1]:";
            LBL_LK_2.Content = "Lokalizacja - katalog głowny [#2]:";
            LBL_LK_3.Content = "Lokalizacja - katalog głowny [#3]:";
            LBL_LK_4.Content = "Lokalizacja - katalog głowny [#4]:";
            LBL_LK_5.Content = "Lokalizacja - katalog głowny [#5]:";

            if (SC_POZIOM.Value == 1)
            {
                sb_poziom = 5;
                LBL_LK_1.Content = "Lokalizacja - katalog głowny [#6]:";
                LBL_LK_2.Content = "Lokalizacja - katalog głowny [#7]:";
                LBL_LK_3.Content = "Lokalizacja - katalog głowny [#8]:";
                LBL_LK_4.Content = "Lokalizacja - katalog głowny [#9]:";
                LBL_LK_5.Content = "Lokalizacja - katalog głowny [#10]:";
            }
            else if (SC_POZIOM.Value == 2)
            {
                sb_poziom = 10;
                LBL_LK_1.Content = "Lokalizacja - katalog głowny [#11]:";
                LBL_LK_2.Content = "Lokalizacja - katalog głowny [#12]:";
                LBL_LK_3.Content = "Lokalizacja - katalog głowny [#13]:";
                LBL_LK_4.Content = "Lokalizacja - katalog głowny [#14]:";
                LBL_LK_5.Content = "Lokalizacja - katalog głowny [#15]:";
            }
            else if (SC_POZIOM.Value == 3)
            {
                sb_poziom = 15;
                LBL_LK_1.Content = "Lokalizacja - katalog głowny [#16]:";
                LBL_LK_2.Content = "Lokalizacja - katalog głowny [#17]:";
                LBL_LK_3.Content = "Lokalizacja - katalog głowny [#18]:";
                LBL_LK_4.Content = "Lokalizacja - katalog głowny [#19]:";
                LBL_LK_5.Content = "Lokalizacja - katalog głowny [#20]:";
            }

            TXT_LOKALIZACJA_PLIKOW_1.Text = "";
            TXT_LOKALIZACJA_PLIKOW_2.Text = "";
            TXT_LOKALIZACJA_PLIKOW_3.Text = "";
            TXT_LOKALIZACJA_PLIKOW_4.Text = "";
            TXT_LOKALIZACJA_PLIKOW_5.Text = "";
            TXT_LOKALIZACJA_PLIKOW_OPIS_1.Text = "";
            TXT_LOKALIZACJA_PLIKOW_OPIS_2.Text = "";
            TXT_LOKALIZACJA_PLIKOW_OPIS_3.Text = "";
            TXT_LOKALIZACJA_PLIKOW_OPIS_4.Text = "";
            TXT_LOKALIZACJA_PLIKOW_OPIS_5.Text = "";

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_" +  (1 + sb_poziom) + "'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKOW_1.Text = dv[0]["pole10"].ToString();
                    TXT_LOKALIZACJA_PLIKOW_OPIS_1.Text = dv[0]["pole8"].ToString();
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_" +  (2 + sb_poziom) + "'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKOW_2.Text = dv[0]["pole10"].ToString();
                    TXT_LOKALIZACJA_PLIKOW_OPIS_2.Text = dv[0]["pole8"].ToString();
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_" + (3 + sb_poziom) + "'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKOW_3.Text = dv[0]["pole10"].ToString();
                    TXT_LOKALIZACJA_PLIKOW_OPIS_3.Text = dv[0]["pole8"].ToString();
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_" + (4 + sb_poziom) + "'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKOW_4.Text = dv[0]["pole10"].ToString();
                    TXT_LOKALIZACJA_PLIKOW_OPIS_4.Text = dv[0]["pole8"].ToString();
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_" + (5 + sb_poziom) + "'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKOW_5.Text = dv[0]["pole10"].ToString();
                    TXT_LOKALIZACJA_PLIKOW_OPIS_5.Text = dv[0]["pole8"].ToString();
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKU_MATKI'";
                if (dv.Count > 0)
                {
                    TXT_LOKALIZACJA_PLIKU_MATKI.Text = dv[0]["pole10"].ToString();
                }

            }
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
                    dt = _PUBLIC_SqlLite.SelectQuery("select count(*) from ParametryPalaczenia where `nazwa_bazy` <> ''");

                    if (dt.Rows.Count > 10000)
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

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy, serwer || ' - ' || nazwa_bazy as SerwerIBaza FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

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
        private void ZAPISZ_LOK_KATALAGU_Click(object sender, RoutedEventArgs e)
        {
            sbyte sb_poziom = 0;

            if (SC_POZIOM.Value == 1)
            {
                sb_poziom = 5;
            }

            if (SC_POZIOM.Value == 2)
            {
                sb_poziom = 10;
            }

            if (SC_POZIOM.Value == 3)
            {
                sb_poziom = 15;
            }

            switch (((System.Windows.FrameworkElement)sender).Tag.ToString())
            {
                case "T1":
                if (TXT_LOKALIZACJA_PLIKOW_1.Text.Trim() == "")
                {
                    return;
                }

                var dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {
                    _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", TXT_LOKALIZACJA_PLIKOW_OPIS_1.Text.Trim(), TXT_LOKALIZACJA_PLIKOW_1.Text.Trim(), "TXT_LOKALIZACJA_PLIKOW_" + (1 + sb_poziom) , true);
                }
                break;
                case "T2":
                    if (TXT_LOKALIZACJA_PLIKOW_2.Text.Trim() == "")
                    {
                        return;
                    }

                    dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                    if (dr == MessageBoxResult.Yes)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "",TXT_LOKALIZACJA_PLIKOW_OPIS_2.Text.Trim(), TXT_LOKALIZACJA_PLIKOW_2.Text.Trim(), "TXT_LOKALIZACJA_PLIKOW_" + (2 + sb_poziom), true);
                    }
                    break;
                case "T3":
                    if (TXT_LOKALIZACJA_PLIKOW_3.Text.Trim() == "")
                    {
                        return;
                    }

                    dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                    if (dr == MessageBoxResult.Yes)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", TXT_LOKALIZACJA_PLIKOW_OPIS_3.Text.Trim(), TXT_LOKALIZACJA_PLIKOW_3.Text.Trim(), "TXT_LOKALIZACJA_PLIKOW_" + (3 + sb_poziom), true);
                    }
                    break;
                case "T4":
                    if (TXT_LOKALIZACJA_PLIKOW_4.Text.Trim() == "")
                    {
                        return;
                    }

                    dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                    if (dr == MessageBoxResult.Yes)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", TXT_LOKALIZACJA_PLIKOW_OPIS_4.Text.Trim(), TXT_LOKALIZACJA_PLIKOW_4.Text.Trim(), "TXT_LOKALIZACJA_PLIKOW_" + (4 + sb_poziom), true);
                    }
                    break;
                case "T5":
                    if (TXT_LOKALIZACJA_PLIKOW_5.Text.Trim() == "")
                    {
                        return;
                    }

                    dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                    if (dr == MessageBoxResult.Yes)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", TXT_LOKALIZACJA_PLIKOW_OPIS_5.Text.Trim(), TXT_LOKALIZACJA_PLIKOW_5.Text.Trim(), "TXT_LOKALIZACJA_PLIKOW_" + (5 + sb_poziom), true);
                    }
                    break;
                default:
                break;
            }
            
        }
        private void ZAPISZ_LOK_MATKA_Click(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);

            if (dr == MessageBoxResult.Yes)
            {
                _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", "BAZA-ZRODLO", TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim(), "TXT_LOKALIZACJA_PLIKU_MATKI", true);
            }
        }

        private void SC_POZIOM_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            wart_pol();
        }

        private void Open_file_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik bazy jako żródło";
            openFileDialog.Filter = "Plik bazy (_dbdt.db)|*_dbdt.db|Inny (*.db)|*.db";
            openFileDialog.Multiselect= false;//_dbdt.db "Pliki obrazów (*.bmp, *.jpg)|*.bmp;*.jpg|Wszystkie pliki (*.*)|*.*"”
            if (openFileDialog.ShowDialog() == true)
                TXT_LOKALIZACJA_PLIKU_MATKI.Text = openFileDialog.FileName;
        }

        private void button_synchronizuj_Click(object sender, RoutedEventArgs e)
        {
            if (TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim() == "") return;

            DataTable dtm = new DataTable();
            DataTable dtl = new DataTable();

            dtm = _PUBLIC_SqlLite.SelectQuery_DB_MASTER("SELECT nazwa_objektu, opis, pole1, pole7, kto_zmienil, objekt FROM objekty WHERE poleint1 = 1", TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim());

            dtl = _PUBLIC_SqlLite.SelectQuery("SELECT pole7 FROM objekty");

            if (dtm.Rows.Count > 0)
            {
                DataView dataView = new DataView(dtl);
                for (int i = 0;i< dtm.Rows.Count; i++)
                {
                    dataView.RowFilter = "pole7 = '" + dtm.Rows[0]["pole7"].ToString() + "'";

                    if(dataView.Count  == 0)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_OBJEKT_Z_MASTER(dtm.Rows[i]["nazwa_objektu"].ToString(),
                            dtm.Rows[i]["opis"].ToString(),dtm.Rows[i]["pole1"].ToString(),dtm.Rows[i]["pole7"].ToString(), 
                            dtm.Rows[i]["kto_zmienil"].ToString(),dtm.Rows[i]["objekt"].ToString());
                    }
                }
            }

            dtm = _PUBLIC_SqlLite.SelectQuery_DB_MASTER("SELECT * FROM obrobki WHERE poleint2 = 1", TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim());

            dtl = _PUBLIC_SqlLite.SelectQuery("SELECT pole7 FROM obrobki");

            if (dtm.Rows.Count > 0)
            {
                DataView dataView = new DataView(dtl);
                for (int i = 0; i < dtm.Rows.Count; i++)
                {
                    dataView.RowFilter = "pole7 = '" + dtm.Rows[0]["pole7"].ToString() + "'";

                    if (dataView.Count == 0)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_OBROBKI_Z_MASTER(dtm.Rows[i]["nazwa_obrobki"].ToString(),
                            dtm.Rows[i]["pole1"].ToString(), dtm.Rows[i]["pole2"].ToString(),
                            dtm.Rows[i]["pole3"].ToString(), dtm.Rows[i]["pole4"].ToString(), dtm.Rows[i]["pole5"].ToString(),
                            dtm.Rows[i]["pole6"].ToString(), dtm.Rows[i]["pole7"].ToString(), (int)dtm.Rows[i]["poleint1"], (int)dtm.Rows[i]["poleint2"], 
                            dtm.Rows[i]["opis"].ToString(), dtm.Rows[i]["kto_zmienil"].ToString());
                    }
                }
            }

            dtm = _PUBLIC_SqlLite.SelectQuery_DB_MASTER("SELECT nazwa_zapytania, sql, pole1, pole2, pole3, pole4, pole5, pole6, kto_zmienil, pole7 FROM sql_zapytania WHERE poleint1 = 1", TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim());

            dtl = _PUBLIC_SqlLite.SelectQuery("SELECT pole7 FROM sql_zapytania");
            
            if (dtm.Rows.Count > 0)
            {
                DataView dataView = new DataView(dtl);
                for (int i = 0; i < dtm.Rows.Count; i++)
                {
                    dataView.RowFilter = "pole7 = '" + dtm.Rows[0]["pole7"].ToString() + "'";

                    if (dataView.Count == 0)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_SQL_ZAPYTANIA_Z_MASTER(dtm.Rows[i]["nazwa_zapytania"].ToString(),
                            dtm.Rows[i]["sql"].ToString(), dtm.Rows[i]["pole1"].ToString(), dtm.Rows[i]["pole2"].ToString(),
                            dtm.Rows[i]["pole3"].ToString(), dtm.Rows[i]["pole4"].ToString(), dtm.Rows[i]["pole5"].ToString(),
                            dtm.Rows[i]["pole6"].ToString(), dtm.Rows[i]["kto_zmienil"].ToString(), dtm.Rows[i]["pole7"].ToString());
                    }
                }
            }

            dtm = _PUBLIC_SqlLite.SelectQuery_DB_MASTER("SELECT serwer, nazwa_bazy, pole10, pole9, pole8, pole7, kto_zmienil FROM ParametryPalaczenia WHERE poleint1 = 1", TXT_LOKALIZACJA_PLIKU_MATKI.Text.Trim());

            dtl = _PUBLIC_SqlLite.SelectQuery("SELECT pole7 FROM ParametryPalaczenia");

            if (dtm.Rows.Count > 0)
            {
                DataView dataView = new DataView(dtl);
                for (int i = 0; i < dtm.Rows.Count; i++)
                {
                    dataView.RowFilter = "pole7 = '" + dtm.Rows[0]["pole7"].ToString() + "'";

                    if (dataView.Count == 0)
                    {
                        _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA_Z_MASTER(dtm.Rows[i]["serwer"].ToString(),
                            dtm.Rows[i]["nazwa_bazy"].ToString(), dtm.Rows[i]["pole7"].ToString(), dtm.Rows[i]["pole8"].ToString(),
                            dtm.Rows[i]["pole9"].ToString(), dtm.Rows[i]["pole10"].ToString(), dtm.Rows[i]["kto_zmienil"].ToString());
                    }
                }
            }

        }

        private void Save_file_all_click(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("Czy ustawić jako domyślne ustawienia dla źródła?", "Uwaga!!!", MessageBoxButton.YesNo);

            if (dr == MessageBoxResult.Yes)
            {
                _PUBLIC_SqlLite.ZAPISZ_ZMIANY_SQL("UPDATE objekty SET poleint1 = 1; UPDATE obrobki SET poleint2 = 1; UPDATE sql_zapytania SET poleint1 = 1; UPDATE ParametryPalaczenia SET poleint1 = 1 WHERE nazwa_bazy = '';");
            }
            
        }

        private void B_ZAPISZ_KONF_INDEKS_Click(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);

            if (dr == MessageBoxResult.Yes)
            {
                _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA("", "", "", TXT_SQL_PUSTY_NUMER.Text.Trim(), "POLE_KONFIGURACJI_INDEKSOW", true, TXT_INDESK_DOPLAT.Text.Trim(), TXT_INDESK_DUMMY.Text.Trim(),
                    TXT_INDESK_INNY.Text.Trim(),TXT_INDESK_DRUK.Text.Trim(),TXT_INDESK_INNY.Text.Trim(),TXT_SQL_XML_SERII.Text.Trim());
            }
        }
        
    }
}
