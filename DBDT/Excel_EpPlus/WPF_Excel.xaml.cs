using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace DBDT.Excel_EpPlus
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_Excel : System.Windows.Window
    {

        private DataSet ds_d = new DataSet();
        public DataTable dt_exec = new DataTable();

        private SqlHandler sqlHandler = new SqlHandler();

        public WPF_Excel(DataTable dt)
        {
            InitializeComponent();

            exectGrid.ItemsSource = dt.DefaultView;

            dt_exec = dt;

            //string j_column = "";
            //string m_column = "";

            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    j_column += "" + dt.Columns[i].ColumnName + ", ";
            //    m_column += "@" + dt.Columns[i].ColumnName + ", ";
            //}

            //j_column = j_column.TrimEnd().TrimEnd(',');
            //m_column = m_column.TrimEnd().TrimEnd(',');

            //TXT_INFO_SELECT.Text = "SELECT [wskazna_kolumna] FROM NazwaTabeli(" + j_column + ") VALUES (" + m_column + ");" + "\r\n";
            var dtf = _PUBLIC_SqlLite.SelectQuery("SELECT id, opis FROM funkcje WHERE nazwa_funkcji = '__WPF_Excel' order by id desc");
            if(dtf.Rows.Count >0)
            {
                TXT_INFO_SELECT.Text = dtf.Rows[0]["opis"].ToString()  + "\r\n";
            }
            else 
            {
                TXT_INFO_SELECT.Text = "SELECT NazwaKolumny FROM NazwaTabeli WHERE NazwaKolumny = [wartosc_z_wskazanej_kolumny];" + "\r\n";
            }

        }
        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy ropocząć sprawdzanie wartości", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);

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

                string value = "";

                for (int i = 0; i < exectGrid.SelectedCells.Count; i++)
                {
                    DataGridCellInfo cell = exectGrid.SelectedCells[i];

                    if (cell.Item != null)
                    {
                        value += cell.Column.Header.ToString() + ", " + "\r\n";
                        break;
                    }
                }

                if(dt_exec.Columns.Contains("SprDane") == false)
                dt_exec.Columns.Add("SprDane", typeof(System.Boolean));
    
                string connectionString = @"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
            
                        try
                        {
                            for (int i = 0; i < dt_exec.Rows.Count; i++)
                            {
                                string select = TXT_INFO_SELECT.Text.Replace("[wartosc_z_wskazanej_kolumny]", "'" + value + "';");
                                dt_exec.Rows[i][value] = sqlHandler.CzyJestWpis(select);
                            }
        
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd podczas weryfikacji danych: " + ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show(ex.Message, "Błąd odczytu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            sqlHandler.Disconnect();

            MessageBox.Show("Zakończyłem sprawdzanie!" + "\r\n" + "Ilość błędów: " + ilosc_Bledow, "Koniec pracy...", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            _PUBLIC_SqlLite.DODAJ_REKORD_SQL_FUKCJE("__WPF_Excel", TXT_INFO_SELECT.Text.Trim(), "", "", "", "", "", "", "", "", "", "");
        }
        private void ClickDelSelectRow(object sender, RoutedEventArgs e)
        {
            string value = "";
 
            for (int i = 0; i < exectGrid.SelectedCells.Count; i++)
            {
                DataGridCellInfo cell = exectGrid.SelectedCells[i];

                if (cell.Item != null)
                {
                    value += cell.Column.Header.ToString() + ", " + "\r\n";

                   ((System.Data.DataRowView)cell.Item).Row.Delete();

                }
            }
        }
    }
}
