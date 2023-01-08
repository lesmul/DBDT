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
using System.Windows.Shapes;
using static DBDT.Excel.ClipboardHelper;
using System.Collections;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Windows.Documents;

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
        DataSet ds = new DataSet();
        string old_tab_name;

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

            if (ds.Tables.Count == 0)
            {
                ds.Tables.Add(dt_d);
            }

            dv.Table = dt_d;

            //DG_MOJE_USTAWIENIA.ItemsSource = dv;

            //System.Data.DataTable dtcb = new System.Data.DataTable();
            //dtcb = _PUBLIC_SqlLite.SelectQuery("SELECT id, (opis || ' \\ ' || pole1) as opisx FROM objekty order by pole1");

            //CB_NAZ_EXCEL.ItemsSource = dtcb.DefaultView;
            //CB_NAZ_EXCEL.DisplayMemberPath = "opisx";

            //TI_M.Tag = -1;

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    TabItem item = new TabItem();
            //    item.ContentTemplate = TryFindResource("TC_Zakl") as DataTemplate;
            //    item.Header = dt.Rows[i]["Opis"].ToString();
            //    item.Content = TI_M.Content;
            //    item.ToolTip = dt.Rows[i]["Opis"].ToString();
            //    item.Tag = dt.Rows[i]["id"].ToString();
            //    TC_Zakl.Items.Add(item);
            //}

            load_tab();

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

            if (System.IO.File.Exists(dti.Rows[0]["pole11"].ToString()) == false)
            {
                string str_inf = _PUBLIC_SqlLite.ZAPISZ_DO_PLIKU_XSL(dti.Rows[0]["pole11"].ToString(),
                dti.Rows[0]["pole11"].ToString(), dtix.Rows[0]["id"].ToString());

                MessageBox.Show(dti.Rows[0]["pole11"].ToString(), "Plik zapisano!");
            }

            try
            {
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
                catch (Exception exx)
                {

                    MessageBox.Show(exx.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //Save the workbook
                ExcelWorkBook.Save();

                //Close the workbook
                ExcelWorkBook.Close();

                //Quit the excel process
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void B_ZAPISZ_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (TXT_NAZ_ZAKLADKI.Text.Trim() == "")
            {
                MessageBox.Show("Wypełnij pola");
                return;
            }

            if (TXT_SQL.Text.Trim() == "") TXT_SQL.Text = "Brak";

            if (id_s == "-1")
            {
                _PUBLIC_SqlLite.DODAJ_REKORD_SQL_FUKCJE(TXT_NAZ_ZAKLADKI.Text.Trim(), TXT_SQL.Text, CB_NAZ_EXCEL.Text.Trim(), TXT_NAZ_ARKUSZA.Text,
                TXT_KOMORKA_START.Text, CB_UNIKAT.IsChecked.ToString(), TXT_NR_PROJEKTU.Text.Trim(), "", "", "", "", TXT_LOK_PLIK_WYNIKOWY.Text.Trim());
                TXT_NAZ_ZAKLADKI.Text = "";
            }
            else
            {
                _PUBLIC_SqlLite.ZMIEN_REKORD_SQL_FUKCJE(TXT_NAZ_ZAKLADKI.Text.Trim(), TXT_SQL.Text, CB_NAZ_EXCEL.Text.Trim(), TXT_NAZ_ARKUSZA.Text,
                TXT_KOMORKA_START.Text, CB_UNIKAT.IsChecked.ToString(), TXT_NR_PROJEKTU.Text.Trim(), "", "", "", "", TXT_LOK_PLIK_WYNIKOWY.Text.Trim(), id_s);
            }

            if (old_tab_name != TXT_NAZ_ZAKLADKI.Text) load_tab();

        }

        private void uc_loaded(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_USTAWIENIA.Columns.Count > 0)
            {
                DG_MOJE_USTAWIENIA.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                DG_MOJE_USTAWIENIA.Columns[1].Visibility = System.Windows.Visibility.Hidden;
            }

            string ScieszkaProgramu;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(ScieszkaProgramu + "_auto.xml");

            if (fi.Exists == true && dt_d.Rows.Count == 0)
            {
                ds.ReadXml(ScieszkaProgramu + "_auto.xml");

                if (ds.Tables.Count > 0)
                {
                    dt_d = ds.Tables[0];
                    dv.Table = ds.Tables[0];
                }
            }
        }

        private void tc_selection_clear()
        {
            dv.RowFilter = "";
            B_ZAPISZ.Content = "Dodaj zakładkę";
            TXT_NAZ_ZAKLADKI.Text = "";
            TXT_SQL.Text = "";
            CB_NAZ_EXCEL.Text = "";
            TXT_NAZ_ARKUSZA.Text = "";
            TXT_KOMORKA_START.Text = "";
            CB_UNIKAT.IsChecked = false;
            TXT_LOK_PLIK_WYNIKOWY.Text = "";
            TXT_NR_PROJEKTU.Text = "";
            B_Wyslij_Excel.Visibility = System.Windows.Visibility.Hidden;
            B_Usun.Visibility = System.Windows.Visibility.Hidden;
            MI_CRTL_PLUS_V.IsEnabled = false;
            dt_d.Columns["Objekt"].DefaultValue = "";
            DG_MOJE_USTAWIENIA.CanUserAddRows = false;
        }
        private void tc_selection_changed(object sender, SelectionChangedEventArgs e)
        {

            if (TC_Zakl.SelectedItem == null)
            {
                id_s = "-1";
                old_tab_name = "";

                tc_selection_clear();

                return;
            }

            if (e.Source is TabControl)
            {
                if (((FrameworkElement)((System.Windows.Controls.Primitives.Selector)sender).SelectedItem).Tag == null)
                {
                    id_s = "-1";
                    old_tab_name = "";
                }
                else
                {
                    id_s = ((FrameworkElement)((System.Windows.Controls.Primitives.Selector)sender).SelectedItem).Tag.ToString();
                    old_tab_name = ((HeaderedContentControl)((System.Windows.Controls.Primitives.Selector)e.OriginalSource).SelectedItem).Header.ToString();
                }

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
                    tc_selection_clear();

                    return;
                }

                DG_MOJE_USTAWIENIA.CanUserAddRows = true;

                B_ZAPISZ.Content = "Zmień";

                System.Data.DataTable dti = new System.Data.DataTable();

                dti = _PUBLIC_SqlLite.SelectQuery("SELECT * FROM funkcje WHERE id = " + id_s);

                if (dti.Rows.Count == 0) return;

                TXT_NAZ_ZAKLADKI.Text = dti.Rows[0]["nazwa_funkcji"].ToString();
                old_tab_name = TXT_NAZ_ZAKLADKI.Text;
                TXT_SQL.Text = dti.Rows[0]["opis"].ToString();
                CB_NAZ_EXCEL.Text = dti.Rows[0]["pole1"].ToString();
                TXT_NAZ_ARKUSZA.Text = dti.Rows[0]["pole2"].ToString();
                TXT_KOMORKA_START.Text = dti.Rows[0]["pole3"].ToString();

                TXT_NR_PROJEKTU.Text = dti.Rows[0]["pole5"].ToString();

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
                B_Usun.Visibility = System.Windows.Visibility.Visible;
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
                if (boolCSV == true)
                {
                    string linia = rowData[i][0];
                    char separator = ';';
                    string[] strx = linia.Split(separator);

                    DataRow dr = dt_d.NewRow();

                    if (strx.Length > 0) dr["Nazwa"] = strx[0].ToString();
                    if (strx.Length > 1) dr["Wartość"] = strx[1].ToString();
                    if (strx.Length > 2) dr["Tekst"] = strx[2].ToString();

                    dt_d.Rows.Add(dr);
                }
                else
                {

                    DataRow dr = dt_d.NewRow();

                    if (rowData.Count > 0) dr["Nazwa"] = rowData[i][0].ToString();
                    if (rowData.Count > 1) dr["Wartość"] = rowData[i][1].ToString();
                    if (rowData.Count > 2) dr["Tekst"] = rowData[i][2].ToString(); ;

                    dt_d.Rows.Add(dr);
                }

            }

        }

        private void unloaded(object sender, RoutedEventArgs e)
        {
            string ScieszkaProgramu;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }

            ds.WriteXml(ScieszkaProgramu + "_auto.xml");
        }

        private void load_tab()
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, nazwa_funkcji as Opis, pole5 as Nazwa, pole6 FROM funkcje ORDER BY nazwa_funkcji");

            //DataColumn wCol1 = dt_d.Columns.Add("ID", typeof(Int32));
            //wCol1.AllowDBNull = false;
            //wCol1.Unique = true;
            //wCol1.AutoIncrement = true;
            //DataColumn wCol2 = dt_d.Columns.Add("id_obj", typeof(string));
            //wCol2.DefaultValue = "-1";

            //DataColumn wCol3 = dt_d.Columns.Add("Objekt", typeof(string));
            //wCol3.ReadOnly = true;
            //wCol3.DefaultValue = "";

            //dt_d.Columns.Add("Nazwa", typeof(string));
            //dt_d.Columns.Add("Wartość", typeof(string));
            //dt_d.Columns.Add("Tekst", typeof(string));

            //dt_d.TableName = "DaneX";

            //if (ds.Tables.Count == 0)
            //{
            //    ds.Tables.Add(dt_d);
            //}

            //dv.Table = dt_d;

            DG_MOJE_USTAWIENIA.ItemsSource = dv;

            System.Data.DataTable dtcb = new System.Data.DataTable();
            dtcb = _PUBLIC_SqlLite.SelectQuery("SELECT id, (opis || ' \\ ' || pole1) as opisx FROM objekty order by pole1");

            CB_NAZ_EXCEL.ItemsSource = dtcb.DefaultView;
            CB_NAZ_EXCEL.DisplayMemberPath = "opisx";

            TI_M.Tag = -1;

            if (TC_Zakl.Items.Count > 1)
            {
                while (TC_Zakl.Items.Count > 1)
                {
                    TC_Zakl.Items.RemoveAt(1);

                }
            }

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

        private void B_USUN_ZAKL_Click(object sender, RoutedEventArgs e)
        {

            var resolut = MessageBox.Show("Czy usunąć zakładkę?", "Uwaga!!!", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (resolut == MessageBoxResult.No) return;

            if (_PUBLIC_SqlLite.ZAPISZ_ZMIANY_SQL("DELETE FROM funkcje WHERE id = " + id_s) == false)
            {
                MessageBox.Show("Wystapił problem z usunięciem zakładki", "Błąd", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                load_tab();
            };
        }

        private void b_sql_x_click(object sender, RoutedEventArgs e)
        {
            MW_SQL.txtCode.Text = TXT_SQL.Text;
        }

        private void MenuItem_Klonuj(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_USTAWIENIA.SelectedCells.Count == 0)
            {
                return;
            }

            switch (((FrameworkElement)sender).Tag)
            {
                case "WR":
                    zamien_wybrane("WR", false, false);
                    break;
                case "ALU":
                    zamien_wybrane("ALU", false, false);
                    break;
                case "PVC":
                    zamien_wybrane("PVC", false, false);
                    break;
                case "WR_":
                    zamien_wybrane("WR", false, true);
                    break;
                case "ALU_":
                    zamien_wybrane("ALU", false, true);
                    break;
                case "PVC_":
                    zamien_wybrane("PVC", false, true);
                    break;
                case "XX_DOWOLNY":
                    bool bool_zmien = false;
                    if (tbCustomDataRange.Text.IndexOf("_") > -1) bool_zmien = true;
                    zamien_wybrane(tbCustomDataRange.Text, bool_zmien, false);
                    break;
                case "DEL_":
                    zamien_wybrane("_", true, true);
                    break;
            }

        }

        private void zamien_wybrane(string str_x, bool zamien,bool zastap)
        {

            if (str_x.Trim() == "") return;

            IList items = DG_MOJE_USTAWIENIA.SelectedItems;

            foreach (object item in items)
            {

                DataRowView MyRow = (DataRowView)item;

                if (zamien == false)
                {
                    DataRow row = dt_d.NewRow();
                    row["id_obj"] = MyRow["id_obj"].ToString();
                    row["Objekt"] = MyRow["Objekt"].ToString();
                    row["Nazwa"] = MyRow["Nazwa"].ToString();
                    row["Wartość"] = MyRow["Wartość"].ToString();
                    row["Tekst"] = MyRow["Tekst"].ToString();
                    dt_d.Rows.Add(row);
                }

                string value = MyRow["Nazwa"].ToString();
                int ch_s = value.ToString().IndexOf("_");

                if (zamien == true)
                {
                    if (ch_s > -1)
                    {
                        MyRow["Nazwa"] = (str_x.StartsWith("_") ? "" : str_x) + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                    }
                    else
                    {
                        if (value.StartsWith("_"))
                        {
                            MyRow["Nazwa"] = (str_x.StartsWith("_") ? "" : str_x) + value;
                        }
                        else
                        {
                            MyRow["Nazwa"] = (str_x.StartsWith("_") ? "" : str_x + "_") + value;
                        }
                    }
                }
                else
                {
                    if (zastap == true)
                    {
                        if (ch_s > -1)
                        {
                            MyRow["Nazwa"] = (str_x.StartsWith("_") ? "": str_x + "_") + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow["Nazwa"] = str_x + value;
                            }
                            else
                            {
                                MyRow["Nazwa"] = str_x + "_" + value;
                            }
                        }
                    }
                    else
                    {
                        if (value.StartsWith("_"))
                        {
                            MyRow["Nazwa"] = str_x + value;
                        }
                        else
                        {
                            MyRow["Nazwa"] = str_x + "_" + value;
                        }
                    }
                }
            }
        }
    }

    public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static bool boolCSV;

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
                boolCSV = true;

            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
                boolCSV = false;
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
