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
    public partial class WPF_ZAPIS_SQL : System.Windows.Window
    {
        private DataView dv = new DataView();
        //private DataTable dt = new DataTable();
        private DataSet ds_d = new DataSet();
        private DataTable dt_exec = new DataTable();
        public string int_id;
        public bool tlumacz;
        public string str_nazwa = "";
        public string str_procedura_tlumaczen = "";
        public string str_numerProjektu = "";

        private SqlHandler sqlHandler = new SqlHandler();

        public WPF_ZAPIS_SQL()
        {
            InitializeComponent();

            //DataView dv = new DataView(dt_x);

            //dv.RowFilter = filtr;

            if (dt_exec.Columns.Count == 0)
            {
                DataColumn wCol1 = dt_exec.Columns.Add("id", typeof(Int32));
                wCol1.AllowDBNull = false;
                wCol1.Unique = true;
                wCol1.AutoIncrement = true;
                wCol1.AutoIncrementSeed = 1;

                dt_exec.Columns.Add("Nazwa", typeof(string));
                dt_exec.Columns.Add("Wartość", typeof(string));
                dt_exec.Columns.Add("Funkcja", typeof(string));
                dt_exec.Columns.Add("Procedura", typeof(string));
            }

        }
        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            ladu_ds_xml(str_numerProjektu);

            load_data_exec();

            TXT_INFO.Text = "Błedy -> w przypadku wystąpienia kolumny z przedrostkiem @ - błąd konfiguracji!";

        }

        private void ladu_ds_xml(string nazwa_plik_xml)
        {
            if (nazwa_plik_xml == "") nazwa_plik_xml = "_auto";

            string ScieszkaProgramu;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\xml\";

            if (nazwa_plik_xml.Trim() != "") nazwa_plik_xml = nazwa_plik_xml.Trim().Replace("/", "_");

            FileInfo fi = new System.IO.FileInfo(ScieszkaProgramu + nazwa_plik_xml + ".xml");

            if (fi.Exists == true)
            {

                ds_d = null;

                ds_d = new DataSetXML();

                System.IO.FileInfo fid = new System.IO.FileInfo(ScieszkaProgramu + nazwa_plik_xml + ".xsd");

                if (fid.Exists == true)
                {
                    ds_d.ReadXmlSchema(ScieszkaProgramu + nazwa_plik_xml + ".xsd");
                }

                ds_d.ReadXml(ScieszkaProgramu + nazwa_plik_xml + ".xml");

                dv.Table = ds_d.Tables[str_nazwa];

            }

        }

        private void load_data_exec()
        {

            exectGrid.Columns[1].Header = dv.Table.Columns[4].ColumnName;
            exectGrid.Columns[2].Header = dv.Table.Columns[5].ColumnName;

            for (int i = 0; i < dv.Count; i++)
            {
                DataRow dr = dt_exec.NewRow();

                dr["Nazwa"] = dv[i][4].ToString();
                dr["Wartość"] = dv[i][5].ToString();
                
                if (ds_d.Tables[str_nazwa].Rows.Count > 0)
                {

                    //dr["Funkcja"] = , dv[i]["id"].ToString()); // int_id
                    dr["Funkcja"] = Execute_sql_txt.data_exec_function(ds_d.Tables["$SYSTEM_CONFIG$"], dv.Table, ds_d.Tables["$SYSTEM_CONFIG$"].Rows[0]["PoleSQL"].ToString(), dv[i]["id"].ToString(),
                        str_procedura_tlumaczen, tlumacz, str_numerProjektu);
                    dr["Procedura"] = Execute_sql_txt.data_exec_procedura(ds_d.Tables["$SYSTEM_CONFIG$"], dv.Table, ds_d.Tables["$SYSTEM_CONFIG$"].Rows[0]["PoleSQL"].ToString(), dv[i]["id"].ToString(),
                        str_procedura_tlumaczen, tlumacz, str_numerProjektu);

                }
                else
                {
                    dr["Funkcja"] = "";
                    dr["Procedura"] = "";
                }

                dt_exec.Rows.Add(dr);
            }

            exectGrid.ItemsSource = dt_exec.AsDataView();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy zapisać zmiany", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resoluts == MessageBoxResult.No) return;

            int ilosc_Wpr_zmian = 0;
            int ilosc_Bledow = 0;

            try
            {
                TXT_INFO.Text = "";

                SqlError[] errors;

                System.Data.DataTable dt = new System.Data.DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Skonfiguruj połączenie do serwera.");
                }

                sqlHandler.Connect(@"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True");

                for (int i = 0; i < dt_exec.Rows.Count; i++)
                {
                    if ((string)dt_exec.Rows[i]["Funkcja"] != "")
                    {
                        DataTable result = sqlHandler.Execute((string)dt_exec.Rows[i]["Funkcja"], out errors, false);
                        ilosc_Wpr_zmian += 1;

                        if (errors.Length > 0) TXT_INFO.Text += errors[errors.Length - 1].ToString() + " / [F] LP=" + (i + 1) + "\r\n";
                        ilosc_Bledow += errors.Length;

                        if(errors.Length == 0)
                        {
                            DataGridCell cell = GetCell(i, 3, exectGrid);
                            cell.Background = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            DataGridCell cell = GetCell(i, 3, exectGrid);
                            cell.Background = new SolidColorBrush(Colors.Red);
                        }

                    }
                    if ((string)dt_exec.Rows[i]["Procedura"] != "")
                    {

                        DataTable result = sqlHandler.Execute((string)dt_exec.Rows[i]["Procedura"], out errors, true, Execute_sql_txt.data_procedure_ext((string)dt_exec.Rows[i]["Procedura"]));
                        ilosc_Wpr_zmian += 1;

                        if (errors.Length > 0) TXT_INFO.Text += errors[errors.Length - 1].ToString() + " / [P] LP=" + (i + 1) + "\r\n";

                        ilosc_Bledow += errors.Length;

                        if (errors.Length == 0)
                        {
                            DataGridCell cell = GetCell(i, 4, exectGrid);
                            cell.Background = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            DataGridCell cell = GetCell(i, 4, exectGrid);
                            cell.Background = new SolidColorBrush(Colors.Red);
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

            TXT_INFO.Text += "Ilość prób zapisu = " + ilosc_Wpr_zmian.ToString();

            MessageBox.Show("Zakończyłem zapis!" + "\r\n" + "Ilość błędów: " + ilosc_Bledow, "Koniec pracy...", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        public DataGridCell GetCell(int rowIndex, int columnIndex, DataGrid dg)
        {
            DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            DataGridCellsPresenter p = GetVisualChild<DataGridCellsPresenter>(row);
            DataGridCell cell = p.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
            return cell;
        }

        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
