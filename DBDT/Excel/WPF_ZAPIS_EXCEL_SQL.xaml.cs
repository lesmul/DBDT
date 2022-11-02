using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using DBDT.SQL.SQL_SELECT;
using System.Data.SqlClient;
using System.Globalization;
using DataTable = System.Data.DataTable;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls.Primitives;
using DBDT.Excel.DS;
using System.IO;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_ZAPIS_EXCEL_SQL : System.Windows.Window
    {

        private DataSet ds_d = new DataSet();
        public DataTable dt_exec = new DataTable();

        private SqlHandler sqlHandler = new SqlHandler();

        public WPF_ZAPIS_EXCEL_SQL(DataTable dt)
        {
            InitializeComponent();

            exectGrid.ItemsSource = dt.DefaultView;

            dt_exec = dt;

            string j_column = "";
            string m_column = "";

            for (int i = 0;i < dt.Columns.Count;i++)
            {
                j_column += "" + dt.Columns[i].ColumnName + ", ";
                m_column += "@" + dt.Columns[i].ColumnName + ", ";
            }

            j_column = j_column.TrimEnd().TrimEnd(',');
            m_column = m_column.TrimEnd().TrimEnd(',');

            TXT_INFO_INSERT.Text = "INSERT INTO NazwaTabeli(" + j_column + ") VALUES (" + m_column + ");" + "\r\n";

        }
        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy zapisać zmiany", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resoluts == MessageBoxResult.No) return;

            int ilosc_Bledow = 0;

            dt_exec.AcceptChanges();

            try
            {

               var dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Skonfiguruj połączenie do serwera.");
                }

                string connectionString = @"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True";

                // string connectionString = "Your_Connection_String"; // Zastąp odpowiednim ciągiem połączenia do bazy danych MS SQL Server

                //TXT_INFO_INSERT.Text = TXT_INFO_INSERT.Text.ToUpper();

                string nazwa_tabeli = TXT_INFO_INSERT.Text.Substring(TXT_INFO_INSERT.Text.IndexOf("INSERT INTO") + 11, 
                    TXT_INFO_INSERT.Text.IndexOf("(") - TXT_INFO_INSERT.Text.IndexOf("INSERT INTO") - 11);

                string kolumny_insert = TXT_INFO_INSERT.Text.Substring(TXT_INFO_INSERT.Text.IndexOf(nazwa_tabeli.Trim()) + nazwa_tabeli.Trim().Length + 1,
                    TXT_INFO_INSERT.Text.IndexOf("VALUES") - TXT_INFO_INSERT.Text.IndexOf("(") - 3);

                string[] naz_kolumn = kolumny_insert.Split(new char[] { ',' }); 

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = nazwa_tabeli.Trim();//"KodyPocztowe"; // Nazwa tabeli docelowej w bazie danych

                        for (int i = 0; i< naz_kolumn.Length; i++)
                        {
                            bulkCopy.ColumnMappings.Add(naz_kolumn[i].Trim(), naz_kolumn[i].Trim()); // Mapowanie kolumny "KodPocztowy"
                        }

                        //bulkCopy.ColumnMappings.Add("Kolumna1", "KodPocztowy"); // Mapowanie kolumny "KodPocztowy"
                        //bulkCopy.ColumnMappings.Add("Kolumna2", "Adres"); // Mapowanie kolumny "Adres"
                        //bulkCopy.ColumnMappings.Add("Kolumna3", "Miejscowosc"); // Mapowanie kolumny "Miejscowosc"
                        //bulkCopy.ColumnMappings.Add("Kolumna4", "Wojewodztwo"); // Mapowanie kolumny "Wojewodztwo"
                        //bulkCopy.ColumnMappings.Add("Kolumna5", "Powiat"); // Mapowanie kolumny "Powiat"

                        try
                        {
                            bulkCopy.WriteToServer(dt_exec);
                            MessageBox.Show("Dane zostały zapisane do tabeli" + nazwa_tabeli);
                   
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd podczas zapisywania danych: " + ex.Message,"Błąd!!!",MessageBoxButton.OK,MessageBoxImage.Error);
                            ilosc_Bledow++;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
               if (System.Environment.UserName == "Leszek Mularski")
                {
                    MessageBox.Show(ex.Message + "\r\n" + "\r\n" + ex.StackTrace, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }   
            }

            sqlHandler.Disconnect();

            MessageBox.Show("Zakończyłem zapis!" + "\r\n" + "Ilość błędów: " + ilosc_Bledow, "Koniec pracy...", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Button_Click_for(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy zapisać zmiany", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resoluts == MessageBoxResult.No) return;

            int ilosc_Bledow = 0;

            dt_exec.AcceptChanges();

            try
            {

                var dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Skonfiguruj połączenie do serwera.");
                }

                string connectionString = @"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True";

                // string connectionString = "Your_Connection_String"; // Zastąp odpowiednim ciągiem połączenia do bazy danych MS SQL Server

                //TXT_INFO_INSERT.Text = TXT_INFO_INSERT.Text.ToUpper();

                string nazwa_tabeli = TXT_INFO_INSERT.Text.Substring(TXT_INFO_INSERT.Text.IndexOf("INSERT INTO") + 11,
                    TXT_INFO_INSERT.Text.IndexOf("(") - TXT_INFO_INSERT.Text.IndexOf("INSERT INTO") - 11);

                string kolumny_insert = TXT_INFO_INSERT.Text.Substring(TXT_INFO_INSERT.Text.IndexOf(nazwa_tabeli.Trim()) + nazwa_tabeli.Trim().Length + 1,
                    TXT_INFO_INSERT.Text.IndexOf("VALUES") - TXT_INFO_INSERT.Text.IndexOf("(") - 3);

                string[] naz_kolumn = kolumny_insert.Split(new char[] { ',' });

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = nazwa_tabeli.Trim();//"KodyPocztowe"; // Nazwa tabeli docelowej w bazie danych

                        for (int i = 1; i < naz_kolumn.Length; i++)
                        {
                            bulkCopy.ColumnMappings.Add(naz_kolumn[i].Trim(), naz_kolumn[i].Trim()); // Mapowanie kolumny "KodPocztowy"
                        }

                        try
                        {
                            bulkCopy.WriteToServerAsync(dt_exec);
                            MessageBox.Show("Dane zostały zapisane do tabeli" + nazwa_tabeli);
                            ilosc_Bledow++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd podczas zapisywania danych: " + ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (System.Environment.UserName == "Leszek Mularski")
                {
                    MessageBox.Show(ex.Message + "\r\n" + "\r\n" + ex.StackTrace, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            sqlHandler.Disconnect();

            MessageBox.Show("Zakończyłem zapis!" + "\r\n" + "Ilość błędów: " + ilosc_Bledow, "Koniec pracy...", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        

        private void Button_Click_Guid(object sender, RoutedEventArgs e)
        {
          for(int i = 0; i< dt_exec.Rows.Count ; i++)
            {
                dt_exec.Rows[i][0] = Guid.NewGuid(); 
            }
        }
    }
}
