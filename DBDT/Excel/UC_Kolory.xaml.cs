using System.Data;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using DBDT.SQL.SQL_SELECT;
using System;
using System.Collections.Generic;
using DBDT.USTAWIENIA_PROGRAMU;
using System.Windows;
using System.Xml.Linq;
using netDxf.Entities;
using DBDT.DrzewoProcesu.Directory.ViewModels;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;
using System.IO;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy UC_Kolory.xaml
    /// </summary>
    /// 
    public partial class UC_Kolory : UserControl
    {

        string id_s;
        System.Data.DataTable dt_d = new System.Data.DataTable();
        DataView dv = new DataView();

        public UC_Kolory()
        {
            InitializeComponent();

            System.Data.DataTable dt = new System.Data.DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, nazwa_funkcji as Opis, pole5 as Nazwa, pole6 FROM funkcje ORDER BY nazwa_funkcji");

            DataColumn wCol1 = dt_d.Columns.Add("ID", typeof(Int32));
            wCol1.AllowDBNull = false;
            wCol1.Unique = true;
            wCol1.AutoIncrement = true;
            DataColumn wCol2 = dt_d.Columns.Add("id_obj", typeof(string));
            wCol2.DefaultValue = "-1";

            DataColumn wCol3 = dt_d.Columns.Add("Objekt", typeof(string));
            wCol3.ReadOnly = true;
            wCol3.DefaultValue = "";

            dt_d.Columns.Add("Nazwa", typeof(string));
            dt_d.Columns.Add("Wartość", typeof(string));
            dt_d.Columns.Add("Tekst", typeof(string));

            dt_d.TableName = "DaneX";

            dv.Table = dt_d;

            DG_MOJE_USTAWIENIA.ItemsSource = dv;

            System.Data.DataTable dtcb = new System.Data.DataTable();
            dtcb = _PUBLIC_SqlLite.SelectQuery("SELECT id, (opis || ' \\ ' || pole1) as opisx FROM objekty order by pole1");

            CB_NAZ_EXCEL.ItemsSource = dtcb.DefaultView;
            CB_NAZ_EXCEL.DisplayMemberPath = "opisx";

            TI_M.Tag = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TabItem item = new TabItem();
                item.ContentTemplate = TryFindResource("TC_Zakl") as DataTemplate;
                item.Header = dt.Rows[i]["Opis"].ToString();
                item.Content = TI_M.Content;
                item.ToolTip = dt.Rows[i]["Opis"].ToString();
                item.Tag = dt.Rows[i]["id"].ToString();
                TC_Zakl.Items.Add(item);
            }

        }

        private void click_wyslij_zmiany_excel(object sender, System.Windows.RoutedEventArgs e)
        {

            System.Data.DataTable dti = new System.Data.DataTable();

            dti = _PUBLIC_SqlLite.SelectQuery("SELECT * FROM funkcje WHERE id = " + id_s);

            if (dti.Rows.Count == 0) 
            {
                MessageBox.Show("Brak wyników w tabeli funkcje!", "Popraw konfiguracje");
                return;
            }

            string[] str_obj = dti.Rows[0]["pole1"].ToString().Split('\\');

            System.Data.DataTable dtix = new System.Data.DataTable();

            dtix = _PUBLIC_SqlLite.SelectQuery("SELECT id, nazwa_objektu, opis, objekt, kto_zmienil, data_utworzenia, pole1 FROM objekty " +
                "WHERE pole1 = '" + str_obj[1].Trim() + "' AND opis ='" + str_obj[0].Trim() + "'");

            if (dtix.Rows.Count == 0) 
            {
                MessageBox.Show("Brak wyników!", "Popraw konfiguracje");
                return;
            }

            if (System.IO.File.Exists(dti.Rows[0]["pole11"].ToString())==false)
            {
                string str_inf = _PUBLIC_SqlLite.ZAPISZ_DO_PLIKU_XSL(dti.Rows[0]["pole11"].ToString(),
                dti.Rows[0]["pole11"].ToString(), dtix.Rows[0]["id"].ToString());
            }

            //Create Excel Application Instance

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            //Create workbook Instance and open the workbook from the below location
            Workbook ExcelWorkBook = ExcelApp.Workbooks.Open(dti.Rows[0]["pole11"].ToString());

            try
            {
                Worksheet sheet = (Worksheet)ExcelApp.Worksheets[dti.Rows[0]["pole2"].ToString()];
                sheet.Select(Type.Missing);
                //ExcelApp.Cells[4, 1] = dti.Rows[0]["pole2"].ToString();
                //ExcelApp.Range["A1"].Value = dti.Rows[0]["pole2"].ToString();

                string[] kolX = dti.Rows[0]["pole3"].ToString().Split(';');

                for (int j = 0; j < kolX.Length; j++)
                {
                    var id_r = ExcelApp.Range[kolX[j]].Row;
                    var id_c = ExcelApp.Range[kolX[j]].Column;

                    for (int i = 0; i < dv.Count; i++)
                    {
                        if (j + 2 < dv.Table.Columns.Count)
                        {
                            ExcelApp.Cells[id_r, id_c] = dv[i][j + 3].ToString();
                            id_r++;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Save the workbook
            ExcelWorkBook.Save();

            //Close the workbook
            ExcelWorkBook.Close();

            //Quit the excel process
            ExcelApp.Quit();
        }

        private void B_ZAPISZ_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (TXT_NAZ_ZAKLADKI.Text.Trim() == "")
            {
                MessageBox.Show("Wypełnij pola");
                return;
            }

            if (id_s == "-1")
            {
                _PUBLIC_SqlLite.DODAJ_REKORD_SQL_FUKCJE(TXT_NAZ_ZAKLADKI.Text.Trim(), TXT_SQL.Text, CB_NAZ_EXCEL.Text.Trim(), TXT_NAZ_ARKUSZA.Text,
                TXT_KOMORKA_START.Text, CB_UNIKAT.IsChecked.ToString(), "", "", "", "", "", TXT_LOK_PLIK_WYNIKOWY.Text.Trim());
                TXT_NAZ_ZAKLADKI.Text = "";
            }
            else
            {
                _PUBLIC_SqlLite.ZMIEN_REKORD_SQL_FUKCJE(TXT_NAZ_ZAKLADKI.Text.Trim(), TXT_SQL.Text, CB_NAZ_EXCEL.Text.Trim(), TXT_NAZ_ARKUSZA.Text,
                TXT_KOMORKA_START.Text, CB_UNIKAT.IsChecked.ToString(), "", "", "", "", "", TXT_LOK_PLIK_WYNIKOWY.Text.Trim(), id_s);
            }

        }

        private void uc_loaded(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_USTAWIENIA.Columns.Count > 0)
            {
                DG_MOJE_USTAWIENIA.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                DG_MOJE_USTAWIENIA.Columns[1].Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void tc_selection_changed(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {

                id_s = ((System.Windows.FrameworkElement)((System.Windows.Controls.Primitives.Selector)sender).SelectedItem).Tag.ToString();

                if (id_s == "-1")
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = "id_obj = '" + id_s + "'";
                }

                DG_MOJE_USTAWIENIA.ItemsSource = dv;

                dt_d.AcceptChanges(); 

                dt_d.Columns["id_obj"].DefaultValue = id_s;

                if (id_s == "-1")
                {
                    dv.RowFilter = "";
                    B_ZAPISZ.Content = "Zapisz";
                    TXT_NAZ_ZAKLADKI.Text = "";
                    TXT_SQL.Text = "";
                    CB_NAZ_EXCEL.Text = "";
                    TXT_NAZ_ARKUSZA.Text = "";
                    TXT_KOMORKA_START.Text = "";
                    CB_UNIKAT.IsChecked = false;
                    TXT_LOK_PLIK_WYNIKOWY.Text = "";
                    B_Wyslij_Excel.Visibility = System.Windows.Visibility.Hidden;
                    MI_CRTL_PLUS_V.IsEnabled = false;
                    dt_d.Columns["Objekt"].DefaultValue = "";
                    DG_MOJE_USTAWIENIA.CanUserAddRows = false;
                    return;
                }

                DG_MOJE_USTAWIENIA.CanUserAddRows = true;

                B_ZAPISZ.Content = "Zmień";

                System.Data.DataTable dti = new System.Data.DataTable();

                dti = _PUBLIC_SqlLite.SelectQuery("SELECT * FROM funkcje WHERE id = " + id_s);

                if (dti.Rows.Count == 0) return;

                TXT_NAZ_ZAKLADKI.Text = dti.Rows[0]["nazwa_funkcji"].ToString();
                TXT_SQL.Text = dti.Rows[0]["opis"].ToString();
                CB_NAZ_EXCEL.Text = dti.Rows[0]["pole1"].ToString();
                TXT_NAZ_ARKUSZA.Text = dti.Rows[0]["pole2"].ToString();
                TXT_KOMORKA_START.Text = dti.Rows[0]["pole3"].ToString();

                dt_d.Columns["Objekt"].DefaultValue = dti.Rows[0]["nazwa_funkcji"].ToString();

                if (dti.Rows[0]["pole4"].ToString() == "True")
                {
                    CB_UNIKAT.IsChecked = true;
                }
                else
                {
                    CB_UNIKAT.IsChecked = false;
                }

                TXT_LOK_PLIK_WYNIKOWY.Text = dti.Rows[0]["pole11"].ToString();

                B_Wyslij_Excel.Visibility = System.Windows.Visibility.Visible;
                MI_CRTL_PLUS_V.IsEnabled = true;

            }

        }

        private void b_open_xls_click(object sender, RoutedEventArgs e)
        {
            System.Data.DataTable dti = new System.Data.DataTable();

            dti = _PUBLIC_SqlLite.SelectQuery("SELECT * FROM funkcje WHERE id = " + id_s);

            if (dti.Rows.Count == 0)
            {
                MessageBox.Show("Brak wyników w tabeli funkcje!", "Popraw konfiguracje");
                return;
            }

            if (dti.Rows[0]["pole11"].ToString() == "") return;

            try
            {
                Process.Start(dti.Rows[0]["pole11"].ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_Paste(object sender, RoutedEventArgs e)
        {
            List<string[]> rowData = ClipboardHelper.ParseClipboardData();

            for (int i = 0; i < rowData.Count; i++)
            {
                string linia = rowData[i][0];
                string[] strx = linia.Split(';');

                DataRow dr = dt_d.NewRow();

                if(strx.Length > 0) dr["Nazwa"] = strx[0].ToString();
                if (strx.Length > 1) dr["Wartość"] = strx[1].ToString();
                if (strx.Length > 2) dr["Tekst"] = strx[2].ToString();

                dt_d.Rows.Add(dr);
            }

        }
    }

    public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = null;
            object clipboardRawData = null;
            ParseFormat parseFormat = null;

            // get the data and set the parsing method based on the format
            // currently works with CSV and Text DataFormats            
            IDataObject dataObj = System.Windows.Clipboard.GetDataObject();
            if ((clipboardRawData = dataObj.GetData(DataFormats.CommaSeparatedValue)) != null)
            {
                parseFormat = ParseCsvFormat;
            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
            }

            if (parseFormat != null)
            {
                string rawDataStr = clipboardRawData as string;

                if (rawDataStr == null && clipboardRawData is MemoryStream)
                {
                    // cannot convert to a string so try a MemoryStream
                    MemoryStream ms = clipboardRawData as MemoryStream;
                    StreamReader sr = new StreamReader(ms);
                    rawDataStr = sr.ReadToEnd();
                }

                Debug.Assert(rawDataStr != null, string.Format("clipboardRawData: {0}, could not be converted to a string or memorystream.", clipboardRawData));

                string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (rows != null && rows.Length > 0)
                {
                    clipboardData = new List<string[]>();
                    foreach (string row in rows)
                    {
                        clipboardData.Add(parseFormat(row));
                    }
                }
                else
                {
                    Debug.WriteLine("unable to parse row data.  possibly null or contains zero rows.");
                }
            }

            return clipboardData;
        }

        public static string[] ParseCsvFormat(string value)
        {
            return ParseCsvOrTextFormat(value, true);
        }

        public static string[] ParseTextFormat(string value)
        {
            return ParseCsvOrTextFormat(value, false);
        }

        private static string[] ParseCsvOrTextFormat(string value, bool isCSV)
        {
            List<string> outputList = new List<string>();

            char separator = isCSV ? ',' : '\t';
            int startIndex = 0;
            int endIndex = 0;

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == separator)
                {
                    outputList.Add(value.Substring(startIndex, endIndex - startIndex));

                    startIndex = endIndex + 1;
                    endIndex = startIndex;
                }
                else if (ch == '\"' && isCSV)
                {
                    // skip until the ending quotes
                    i++;
                    if (i >= value.Length)
                    {
                        throw new FormatException(string.Format("value: {0} had a format exception", value));
                    }
                    char tempCh = value[i];
                    while (tempCh != '\"' && i < value.Length)
                        i++;

                    endIndex = i;
                }
                else if (i + 1 == value.Length)
                {
                    // add the last value
                    outputList.Add(value.Substring(startIndex));
                    break;
                }
                else
                {
                    endIndex++;
                }
            }

            return outputList.ToArray();
        }
    }
}
