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
using System.Windows.Media;
using DBDT.Konfiguracja;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls.Primitives;
using DBDT.Excel.DS;
using static IronPython.Modules._ast;
using static System.Net.Mime.MediaTypeNames;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy UC_Kolory.xaml
    /// </summary>
    /// 
    public partial class UC_Konfig : UserControl
    {

        DataSetXML ds_d = new DataSetXML();

        string akt_tab_name;

        bool pokaz_ukryte_columny = false;

        public UC_Konfig(bool boolTrybAdmin = false)
        {
            InitializeComponent();

            pokaz_ukryte_columny = boolTrybAdmin;

            DG_MOJE_USTAWIENIA.ItemsSource = ds_d.Tables["$Konfigurator zakładak$"].DefaultView;

            System.Data.DataTable dtcb = new System.Data.DataTable();
            dtcb = _PUBLIC_SqlLite.SelectQuery("SELECT id, (opis || ' \\ ' || pole1) as opisx FROM objekty order by pole1");

            CB_NAZ_EXCEL.ItemsSource = dtcb.DefaultView;
            CB_NAZ_EXCEL.DisplayMemberPath = "opisx";

            akt_tab_name = "$Konfigurator zakładak$";

            ladu_ds_xml("");

            System.Data.DataTable dt = new System.Data.DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole2, pole3, pole4, pole5, pole6 FROM ParametryPalaczenia WHERE pole9 LIKE 'POLE_KONFIGURACJI_INDEKSOW' order by id desc");

            if (dt.Rows.Count > 0)
            {
                MI_INDEKS_DOPLATA.Tag = dt.Rows[0]["pole2"].ToString();
                MI_INDEKS_DUMMY.Tag = dt.Rows[0]["pole3"].ToString();
                MI_INDEKS_INNY.Tag = dt.Rows[0]["pole4"].ToString();
                MI_INDEKS_DRUK.Tag = dt.Rows[0]["pole5"].ToString();
                MI_INDEKS_UZYTKOWNIKA.Tag = dt.Rows[0]["pole6"].ToString();

                MI_INDEKS_DOPLATA.Header += dt.Rows[0]["pole2"].ToString();
                MI_INDEKS_DUMMY.Header += dt.Rows[0]["pole3"].ToString();
                MI_INDEKS_INNY.Header += dt.Rows[0]["pole4"].ToString();
                MI_INDEKS_DRUK.Header += dt.Rows[0]["pole5"].ToString();
                MI_INDEKS_UZYTKOWNIKA.Header += dt.Rows[0]["pole6"].ToString();
            }

        }

        private void click_wyslij_zmiany_excel(object sender, System.Windows.RoutedEventArgs e)
        {

            DataView dataView = new DataView(ds_d.Tables["$SYSTEM$"]);
            dataView.RowFilter = "Objekt = '" + akt_tab_name + "'";

            if (dataView.Count == 0)
            {
                MessageBox.Show("Brak wyników w tabeli funkcje!", "Popraw konfiguracje");
                return;
            }

            string[] str_obj = dataView[0]["Nazwa_Kom_Excel"].ToString().Split('\\');

            if (str_obj.Length < 2)
            {
                MessageBox.Show("Skofiguruj połącznie do EXCEL'a", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dataView.Count == 0)
            {
                MessageBox.Show("Brak wyników!", "Popraw konfiguracje");
                return;
            }

            if (System.IO.File.Exists(dataView[0]["LokalizPlikExcel"].ToString()) == false)
            {
                string str_inf = _PUBLIC_SqlLite.ZAPISZ_DO_PLIKU_XSL(dataView[0]["LokalizPlikExcel"].ToString(),
                dataView[0]["LokalizPlikExcel"].ToString(), dataView[0]["id"].ToString());

                MessageBox.Show(dataView[0]["LokalizPlikExcel"].ToString(), "Plik zapisano!");
            }

            try
            {

                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

                //Create workbook Instance and open the workbook from the below location
                Workbook ExcelWorkBook = ExcelApp.Workbooks.Open(dataView[0]["LokalizPlikExcel"].ToString());

                if (dataView[0]["Nazwa_Arkusz"].ToString() == "")
                {
                    MessageBox.Show("Brak danych w polu nazwa arkusza", "Popraw dane", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (dataView[0]["KomurkaStart"].ToString() == "")
                {
                    MessageBox.Show("Brak powiązanych komórek np A1;B1", "Popraw dane", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                try
                {
                    Worksheet sheet = (Worksheet)ExcelApp.Worksheets[dataView[0]["Nazwa_Arkusz"].ToString()];
                    sheet.Select(Type.Missing);
                    //ExcelApp.Cells[4, 1] = dti.Rows[0]["pole2"].ToString();
                    //ExcelApp.Range["A1"].Value = dti.Rows[0]["pole2"].ToString();
                    //dv.RowFilter = "Objekt = '" + CB_NR_PROJEKTU.Text.Trim() + "'";
                    string[] kolX = dataView[0]["KomurkaStart"].ToString().Split(';');
                     
                    DataView dv = new DataView(ds_d.Tables[akt_tab_name]);

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

                    if (dataView[0]["Nazwa_Kom_Excel_NrProj"].ToString().Trim() != "" && dataView[0]["Nazwa_Arkusz_NrProj"].ToString().Trim() != "")
                    {
                        kolX = dataView[0]["Nazwa_Kom_Excel_NrProj"].ToString().Split(';');
                        var id_r = ExcelApp.Range[kolX[0]].Row;
                        var id_c = ExcelApp.Range[kolX[0]].Column;

                        sheet = (Worksheet)ExcelApp.Worksheets[dataView[0]["Nazwa_Arkusz_NrProj"].ToString()];
                        sheet.Select(Type.Missing);
                        ExcelApp.Cells[id_r, id_c] = CB_NR_PROJEKTU.Text.Trim();
                    }

                }
                catch (Exception exx)
                {

                    MessageBox.Show(exx.Message, "Sprawdź nazwę skoroszytu, lub nazwy komórek", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (CB_NR_PROJEKTU.SelectedValue == null && B_ZAPISZ.Content.ToString() != "Dodaj zakładkę") return;

            bool zaladuj_pon_zakl = false;

            if (pokaz_ukryte_columny == true)
            {
                pokaz_ukryte_columny = false;
                load_tab();
                load_item_tab();
            }

            if (TXT_NAZ_ZAKLADKI.Text.Trim() == "" || TXT_NAZ_ZAKLADKI.Text.Trim() == "$Konfigurator zakładak$")
            {
                MessageBox.Show("Nazwa zakładki wymagana!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                TXT_NAZ_ZAKLADKI.Focus();
                TXT_NAZ_ZAKLADKI.Text = "";
                return;
            }

            akt_tab_name = TXT_NAZ_ZAKLADKI.Text.Trim();

            //TXT_POLE_NR_PROJEKT.Text = CB_NR_PROJEKTU.SelectedValue.ToString();

            if (ds_d.Tables.Contains(TXT_NAZ_ZAKLADKI.Text.Trim()) == false)
            {
                System.Data.DataTable table = new System.Data.DataTable();

                table = ds_d.Tables["$Konfigurator zakładak$"].Copy();
                table.TableName = TXT_NAZ_ZAKLADKI.Text.Trim();
                table.Rows.Clear();

                table.Columns["Objekt"].DefaultValue = TXT_NAZ_ZAKLADKI.Text.Trim();
      
                ds_d.Tables.Add(table);

                zaladuj_pon_zakl = true;
            }

            if (TXT_SQL.Text.Trim() == "") TXT_SQL.Text = "Brak";

            if (B_ZAPISZ.Content.ToString() == "Dodaj zakładkę")
            {
                DataRow dr = ds_d.Tables["$SYSTEM$"].NewRow();

                dr["Objekt"] = TXT_NAZ_ZAKLADKI.Text.Trim();

                TXT_SQL.Text = TXT_SQL.Text.Trim();
                TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');
                TXT_SQL.Text = TXT_SQL.Text.Trim();
                TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');

                dr["SQL_Tlumaczenia"] = TXT_SQL.Text;
                dr["Nazwa_Kom_Excel"] = CB_NAZ_EXCEL.Text;
                dr["Nazwa_Arkusz"] = TXT_NAZ_ARKUSZA.Text;
                dr["KomurkaStart"] = TXT_KOMORKA_START.Text;

                if (CB_NR_PROJEKTU.SelectedValue == null)
                {
                    dr["NumerProjektu"] = CB_NR_PROJEKTU.Text.Trim();

                    if (CB_NR_PROJEKTU.Text.Trim() == "")
                    {
                        MessageBox.Show("Numer projektu wymagany!!", "Uwaga!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        CB_NR_PROJEKTU.Focus();
                        return;
                    }
                }
                else
                {
                    dr["NumerProjektu"] = CB_NR_PROJEKTU.SelectedValue.ToString();
                }
                
                dr["Nazwa_Arkusz_NrProj"] = TXT_ARKUSZ_POLE_NR_PROJEKT.Text;
                dr["Nazwa_Kom_Excel_NrProj"] = TXT_POLE_NR_PROJEKT.Text;
                dr["WlaczTlumaczenie"] = CB_UNIKAT.IsChecked;
                dr["LokalizPlikExcel"] = TXT_LOK_PLIK_WYNIKOWY.Text;

                ds_d.Tables["$SYSTEM$"].Rows.Add(dr);
            }
            else
            {
                DataView dv = new DataView(ds_d.Tables["$SYSTEM$"]);
                dv.RowFilter = "Objekt = '" + TXT_NAZ_ZAKLADKI.Text.Trim() + "'";
                if (dv.Count == 0)
                {
                    DataRow dr = ds_d.Tables["$SYSTEM$"].NewRow();

                    dr["Objekt"] = TXT_NAZ_ZAKLADKI.Text.Trim();

                    TXT_SQL.Text = TXT_SQL.Text.Trim();
                    TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');
                    TXT_SQL.Text = TXT_SQL.Text.Trim();
                    TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');

                    dr["SQL_Tlumaczenia"] = TXT_SQL.Text;
                    dr["Nazwa_Kom_Excel"] = CB_NAZ_EXCEL.Text;
                    dr["Nazwa_Arkusz"] = TXT_NAZ_ARKUSZA.Text;
                    dr["KomurkaStart"] = TXT_KOMORKA_START.Text;
                    dr["NumerProjektu"] = CB_NR_PROJEKTU.SelectedValue.ToString();
                    dr["Nazwa_Arkusz_NrProj"] = TXT_ARKUSZ_POLE_NR_PROJEKT.Text;
                    dr["Nazwa_Kom_Excel_NrProj"] = TXT_POLE_NR_PROJEKT.Text;
                    dr["WlaczTlumaczenie"] = CB_UNIKAT.IsChecked;
                    dr["LokalizPlikExcel"] = TXT_LOK_PLIK_WYNIKOWY.Text;

                    ds_d.Tables["$SYSTEM$"].Rows.Add(dr);
                }
                else
                {
                    dv[0]["Objekt"] = TXT_NAZ_ZAKLADKI.Text.Trim();

                    TXT_SQL.Text = TXT_SQL.Text.Trim();
                    TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');
                    TXT_SQL.Text = TXT_SQL.Text.Trim();
                    TXT_SQL.Text = TXT_SQL.Text.TrimEnd(';');

                    dv[0]["SQL_Tlumaczenia"] = TXT_SQL.Text;
                    dv[0]["Nazwa_Kom_Excel"] = CB_NAZ_EXCEL.Text;
                    dv[0]["Nazwa_Arkusz"] = TXT_NAZ_ARKUSZA.Text;
                    dv[0]["KomurkaStart"] = TXT_KOMORKA_START.Text;
                    dv[0]["NumerProjektu"] = CB_NR_PROJEKTU.SelectedValue.ToString();
                    dv[0]["Nazwa_Arkusz_NrProj"] = TXT_ARKUSZ_POLE_NR_PROJEKT.Text;
                    dv[0]["Nazwa_Kom_Excel_NrProj"] = TXT_POLE_NR_PROJEKT.Text;
                    dv[0]["WlaczTlumaczenie"] = CB_UNIKAT.IsChecked;
                    dv[0]["LokalizPlikExcel"] = TXT_LOK_PLIK_WYNIKOWY.Text;

                    if (dv.Count > 1)
                    {
                        dv[1].Delete();
                    }

                }
            }

            ds_write_xml.write_ds_xml(CB_NR_PROJEKTU.Text.Trim(), ds_d);

            if (zaladuj_pon_zakl == true) load_tab();

        }

        private void uc_loaded(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_USTAWIENIA.Columns.Count > 0 && pokaz_ukryte_columny == false)
            {
                DG_MOJE_USTAWIENIA.Columns[0].Visibility = Visibility.Hidden;
                DG_MOJE_USTAWIENIA.Columns[1].Visibility = Visibility.Hidden;
            }

            string ScieszkaProgramu;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\xml\";

            DirectoryInfo info = new DirectoryInfo(ScieszkaProgramu);
            if (info.Exists == false)
            {
                info.Create();
            }
            else
            {
               load_xml_files();
            }

        }
        private void tc_selection_clear()
        {
            //dv.RowFilter = "";
            B_ZAPISZ.Content = "Dodaj zakładkę";
            B_USUN_PROJEKT.IsEnabled = false;
            TXT_NAZ_ZAKLADKI.Text = "";
            TXT_SQL.Text = "";
            CB_NAZ_EXCEL.Text = "";
            TXT_NAZ_ARKUSZA.Text = "";
            TXT_KOMORKA_START.Text = "";
            TXT_POLE_NR_PROJEKT.Text = "";
            CB_UNIKAT.IsChecked = false;
            TXT_LOK_PLIK_WYNIKOWY.Text = "";
            TXT_ARKUSZ_POLE_NR_PROJEKT.Text = "";
            //CB_NR_PROJEKTU.Text = "";
            B_Wyslij_Excel.Visibility = System.Windows.Visibility.Hidden;
            //B_Usun.Visibility = System.Windows.Visibility.Hidden;
            MI_CKONF_KOLUM_DEFAULT.Visibility = System.Windows.Visibility.Visible;
            MI_CKONF_KOLUM.Visibility = System.Windows.Visibility.Collapsed;
            MI_CRTL_PLUS_V.IsEnabled = false;
            // DG_MOJE_USTAWIENIA.CanUserAddRows = false;
            //dv.AllowNew = false;
            B_Usun.Visibility = System.Windows.Visibility.Hidden;

        }
        private void tc_selection_changed(object sender, SelectionChangedEventArgs e)
        {

            if (TC_Zakl.SelectedItem == null)
            {
                akt_tab_name = "";

                tc_selection_clear();

                return;
            }

            if (e.Source is TabControl)
            {

                if (((FrameworkElement)((Selector)sender).SelectedItem).Tag == null)
                {
                    akt_tab_name = "";

                    tc_selection_clear();

                    return;

                }

                akt_tab_name = ((HeaderedContentControl)((Selector)sender).SelectedValue).Header.ToString();

                if (akt_tab_name == "$Konfigurator zakładak$")
                {
                    DG_MOJE_USTAWIENIA.CanUserAddRows = false;

                    TXT_NAZ_ZAKLADKI.Text = "";
                    tc_selection_clear();
                    load_item_tab();

                    if (akt_tab_name != "$Konfigurator zakładak$") DG_MOJE_USTAWIENIA.CanUserAddRows = true;

                    return;
                }

                DG_MOJE_USTAWIENIA.CanUserAddRows = true;

                load_item_tab();

            }

        }

        private void b_open_xls_click(object sender, RoutedEventArgs e)
        {

            DataView dataView = new DataView(ds_d.Tables["$SYSTEM$"]);
            dataView.RowFilter = "Objekt = '" + akt_tab_name + "'";

            if (dataView[0]["LokalizPlikExcel"].ToString() == "") return;

            if (dataView.Count == 0)
            {
                MessageBox.Show("Brak wyników w tabeli funkcje!", "Popraw konfiguracje");
                return;
            }

            try
            {
                Process.Start(dataView[0]["LokalizPlikExcel"].ToString());
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

                    DataRow dr = ds_d.Tables[akt_tab_name].NewRow();

                    if (strx.Length > 0)
                    {
                        dr[4] = strx[0].ToString().Trim();
                        dr["Ilość_Znaków"] = strx[0].ToString().Trim().Length;
                    }

                    if (strx.Length > 1) dr[5] = strx[1].ToString();
                    if (strx.Length > 2) dr[6] = strx[2].ToString();

                    ds_d.Tables[akt_tab_name].Rows.Add(dr);
                }
                else
                {

                    DataRow dr = ds_d.Tables[akt_tab_name].NewRow();

                    if (rowData.Count > 0)
                    {
                        dr[4] = rowData[i][0].ToString().Trim();
                        dr["Ilość_Znaków"] = rowData[i][0].ToString().Trim().Length;
                    }
                    if (rowData[i].Length > 1) dr[5] = rowData[i][1].ToString();
                    if (rowData[i].Length > 2) dr[6] = rowData[i][2].ToString(); ;

                    ds_d.Tables[akt_tab_name].Rows.Add(dr);
                }

             }

        }

        private void unloaded(object sender, RoutedEventArgs e)
        {
            ds_write_xml.write_ds_xml(CB_NR_PROJEKTU.Text.Trim().Replace("/", "_"), ds_d);
        }

        private void load_item_tab()
        {

            if (akt_tab_name.StartsWith("$") && akt_tab_name.EndsWith("$") && pokaz_ukryte_columny == false) return;

            if (ds_d.Tables.Contains(akt_tab_name) == false)
            {
                string old_txt = CB_NR_PROJEKTU.Text;
                CB_NR_PROJEKTU.Items.Clear(); 

                load_xml_files();

                CB_NR_PROJEKTU.Text = old_txt;

                LBL_INF.Content = ".....Załadowano projekt, ilość wszystkich tabel: " + ds_d.Tables.Count + " / " + CB_NR_PROJEKTU.Text;

            }

            TXT_NAZ_ZAKLADKI.Text = akt_tab_name;

            DG_MOJE_USTAWIENIA.CanUserAddRows = true;

            DataView dv = new DataView(ds_d.Tables["$SYSTEM$"]);

            dv.RowFilter = "Objekt = '" + akt_tab_name +  "'";

            DG_MOJE_USTAWIENIA.Columns.Clear();
            DG_MOJE_USTAWIENIA.ItemsSource = null;

            DG_MOJE_USTAWIENIA.AutoGenerateColumns = false;

            foreach (DataColumn col in ds_d.Tables[akt_tab_name].Columns)
            {

                string[] strWerJ = col.Namespace.Split(';');
                if (strWerJ.Length > 17)
                {
                    if (strWerJ[17].Trim().StartsWith("{") && strWerJ[17].IndexOf("|")>0 && strWerJ[17].Trim().EndsWith("}"))
                    {
                        string[] strWerSz = strWerJ[17].Split('|');

                        DataView dvcmb = new DataView(ds_d.Tables[strWerSz[0].Trim().TrimStart('{')]);

                        DataGridComboBoxColumn combo = new DataGridComboBoxColumn();
                        combo.ItemsSource = dvcmb;
                        combo.Header = col.ColumnName;
                        combo.SelectedValuePath = col.ColumnName;
                        combo.SortMemberPath = col.ColumnName;
                        combo.DisplayMemberPath= strWerSz[1].Trim().TrimEnd('}');
                        combo.TextBinding = new Binding(col.ColumnName);
                        DG_MOJE_USTAWIENIA.Columns.Add(combo);
                    }
                    else
                    {
                        string[] strWerSz = strWerJ[17].Split('|');

                        DataGridComboBoxColumn combo = new DataGridComboBoxColumn();
                        combo.ItemsSource = strWerSz;
                        combo.Header = col.ColumnName;
                        combo.SelectedValuePath = col.ColumnName;
                        combo.SortMemberPath = col.ColumnName;
                        combo.TextBinding = new Binding(col.ColumnName);
                        DG_MOJE_USTAWIENIA.Columns.Add(combo);
                    }

                }
                else
                {


                    if (col.DataType == typeof(bool))
                    {
                        DataGridCheckBoxColumn boolc = new DataGridCheckBoxColumn();
                        boolc.Header = col.ColumnName;
                        boolc.SortMemberPath = col.ColumnName;
                        boolc.Binding = new Binding(col.ColumnName);
                        boolc.Binding.ProvideValue(col);
                        DG_MOJE_USTAWIENIA.Columns.Add(boolc);
                    }
                    else
                    {
                        DataGridTextColumn textc = new DataGridTextColumn();
                        textc.Header = col.ColumnName;
                        textc.SortMemberPath = col.ColumnName;
                        textc.Binding = new Binding(col.ColumnName);
                        textc.Binding.ProvideValue(col);
                        DG_MOJE_USTAWIENIA.Columns.Add(textc);
                    }
                }

            }

           // DG_MOJE_USTAWIENIA.AutoGenerateColumns = true;
            DG_MOJE_USTAWIENIA.ItemsSource = ds_d.Tables[akt_tab_name].DefaultView;

            if (DG_MOJE_USTAWIENIA.Columns.Count > 0 && pokaz_ukryte_columny == false)
            {
                DG_MOJE_USTAWIENIA.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                DG_MOJE_USTAWIENIA.Columns[1].Visibility = System.Windows.Visibility.Hidden;
            }

            B_ZAPISZ.Content = "Zmień";
            B_USUN_PROJEKT.IsEnabled = true;

            if (dv.Count == 0)
            {
                DG_MOJE_USTAWIENIA.CanUserAddRows = true;
                return;
            }

            TXT_NAZ_ZAKLADKI.Text = dv[0]["Objekt"].ToString();

            TXT_SQL.Text = dv[0]["SQL_Tlumaczenia"].ToString();
            CB_NAZ_EXCEL.Text = dv[0]["Nazwa_Kom_Excel"].ToString();
            TXT_NAZ_ARKUSZA.Text = dv[0]["Nazwa_Arkusz"].ToString();
            TXT_KOMORKA_START.Text = dv[0]["KomurkaStart"].ToString();

            //CB_NR_PROJEKTU.Text = dv[0]["NumerProjektu"].ToString();
            TXT_POLE_NR_PROJEKT.Text = dv[0]["Nazwa_Kom_Excel_NrProj"].ToString();
            TXT_ARKUSZ_POLE_NR_PROJEKT.Text = dv[0]["Nazwa_Arkusz_NrProj"].ToString();

            CB_UNIKAT.IsChecked = (bool)dv[0]["WlaczTlumaczenie"];

            TXT_LOK_PLIK_WYNIKOWY.Text = dv[0]["LokalizPlikExcel"].ToString();

            B_Wyslij_Excel.Visibility = Visibility.Visible;
            B_Usun.Visibility = Visibility.Visible;
            MI_CKONF_KOLUM_DEFAULT.Visibility = Visibility.Collapsed;
            MI_CKONF_KOLUM.Visibility = Visibility.Visible;
            MI_CRTL_PLUS_V.IsEnabled = true;

        }

        private void load_tab()
        {

            TI_M.Tag = -1;

            if (TC_Zakl.Items.Count > 1)
            {
                while (TC_Zakl.Items.Count > 1)
                {
                    TC_Zakl.Items.RemoveAt(1);

                }
            }

            for (int i = 0; i < ds_d.Tables.Count; i++)
            {
                if ((ds_d.Tables[i].TableName.StartsWith("$") == false && ds_d.Tables[i].TableName.EndsWith("$") == false)
                    || pokaz_ukryte_columny == true)
                {
                    TabItem item = new TabItem();
                    item.BeginInit();
                    item.ContentTemplate = TryFindResource("TC_Zakl") as DataTemplate;
                    item.Header = ds_d.Tables[i].TableName;
                    item.Content = TI_M.Content;

                    DG_MOJE_USTAWIENIA.CanUserAddRows = true;
                    DG_MOJE_USTAWIENIA.IsReadOnly = false;

                    item.Tag = i;
                    item.EndInit();
                    TC_Zakl.Items.Add(item);
                }
                else if (ds_d.Tables.Count == 3 && ds_d.Tables[i].TableName == "$Konfigurator zakładak$")
                {
                    TabItem item = new TabItem();
                    item.BeginInit();
                    item.ContentTemplate = TryFindResource("TC_Zakl") as DataTemplate;
                    item.Header = ds_d.Tables[i].TableName;
                    item.Content = TI_M.Content;

                    DG_MOJE_USTAWIENIA.CanUserAddRows = true;
                    DG_MOJE_USTAWIENIA.IsReadOnly = false;

                    B_ZAPISZ.Content = "Dodaj zakładkę";
                    B_USUN_PROJEKT.IsEnabled = false;
                    B_Usun.Visibility= Visibility.Collapsed;
                    B_Wyslij_Excel.Visibility= Visibility.Collapsed;

                    item.Tag = i;
                    item.EndInit();
                    TC_Zakl.Items.Add(item);
                }
            }

            if (TC_Zakl.Items.Count > 1)
            {
                TC_Zakl.Items.RemoveAt(0);
            }

            load_item_tab();

        }

        private void load_xml_files()
        {
           string ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\xml\";

            var children = DirectoryStructure.GetDirectoryContents(ScieszkaProgramu, "*.xml");

            CB_NR_PROJEKTU.Items.Clear();
            CB_NR_PROJEKTU.Text = "";

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name != "_auto.xml")
                {
                    int int_type = children[i].Name.LastIndexOf(children[i].TypeFile.ToString());
                    CB_NR_PROJEKTU.Items.Add(children[i].Name.Substring(0, int_type).Replace("_", "/"));
                }
            }
        }

        private void B_USUN_ZAKL_Click(object sender, RoutedEventArgs e)
        {

            var resolut = MessageBox.Show("Czy usunąć zakładkę?", "Uwaga!!!", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (resolut == MessageBoxResult.No) return;

            if (akt_tab_name != "$Konfigurator zakładak$")
            {
                ds_d.Tables.Remove(akt_tab_name);

                load_tab();

            }

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
                    zamien_wybrane.Zamien_wybrane_txt("WR", false, false, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "ALU":
                    zamien_wybrane.Zamien_wybrane_txt("ALU", false, false, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "PVC":
                    zamien_wybrane.Zamien_wybrane_txt("PVC", false, false, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "WR_":
                    zamien_wybrane.Zamien_wybrane_txt("WR", false, true, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "ALU_":
                    zamien_wybrane.Zamien_wybrane_txt("ALU", false, true, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "PVC_":
                    zamien_wybrane.Zamien_wybrane_txt("PVC", false, true, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "XX_DOWOLNY":
                    bool bool_zmien = false;
                    if (tbCustomDataRange.Text.IndexOf("_") > -1) bool_zmien = true;
                    zamien_wybrane.Zamien_wybrane_txt(tbCustomDataRange.Text, bool_zmien, false, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                case "DEL_":
                    zamien_wybrane.Zamien_wybrane_txt("_", true, true, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    break;
                default:

                    if (((FrameworkElement)sender).Tag.ToString() != "")
                    {
                        zamien_wybrane.Zamien_wybrane_txt(((FrameworkElement)sender).Tag.ToString().Trim().TrimEnd('_') , false, true, DG_MOJE_USTAWIENIA, ds_d.Tables[akt_tab_name]);
                    }

                    break;

            }
        }

        private void CB_NR_PROJEKTU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LBL_INF.Content = ".....Wyszukuję projekt";

            if (((Selector)e.OriginalSource).SelectedIndex < 0)
            {
                B_ZAPISZ.Content = "Dodaj zakładkę";
                CB_NR_PROJEKTU.Items.Clear();
                CB_NR_PROJEKTU.Text = "";
                akt_tab_name = "";
                ds_d.Tables.Clear();
                ds_d = null;
                ds_d = new DataSetXML();

                tc_selection_clear();

                load_tab();

                load_xml_files();

                return;
            }
                
            CB_NR_PROJEKTU.Text = ((Selector)e.OriginalSource).SelectedItem.ToString();
            ladu_ds_xml(((Selector)e.OriginalSource).SelectedItem.ToString());
            load_tab();

            LBL_INF.Content = ".....Załadowano projekt, ilość wszystkich tabel: " + ds_d.Tables.Count;
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

                ds_d.Tables.Clear(); 

                ds_d = null;

                ds_d = new DataSetXML();

                FileInfo fid = new FileInfo(ScieszkaProgramu + nazwa_plik_xml + ".xsd");

                if (fid.Exists == true)
                {
                    ds_d.ReadXmlSchema(ScieszkaProgramu + nazwa_plik_xml + ".xsd");
                }

                try
                {
                    ds_d.ReadXml(ScieszkaProgramu + nazwa_plik_xml + ".xml");
                }
                catch (Exception exx)
                {

                    MessageBox.Show(exx.Message, "Nie udało się poprawnie załadować dane!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (ds_d.Tables.Count > 3)
                {
                    if (pokaz_ukryte_columny == false || (pokaz_ukryte_columny == true && ds_d.Tables[3].TableName.StartsWith("$") && ds_d.Tables[3].TableName.EndsWith("$")))
                    akt_tab_name = ds_d.Tables[3].TableName;

                    TI_M.Header = akt_tab_name;
                }

                load_item_tab();
                
                DG_MOJE_USTAWIENIA.CanUserAddRows = true;
                DG_MOJE_USTAWIENIA.IsReadOnly = false;

            }

        }
        private void cb_nr_pr_key_up(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Clear) ladu_ds_xml("");
            if (e.Key == System.Windows.Input.Key.Delete) ladu_ds_xml("");
            if (e.Key == System.Windows.Input.Key.Back)
            {
                CB_NR_PROJEKTU.Text = "";
                ladu_ds_xml("");
            }
        }

        private void pmd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LBL_INF.Content = ".....Wybierz projekt";
        }
        private void MI_KONFIG_SQL_Click(object sender, RoutedEventArgs e)
        {
            if (B_Wyslij_Excel.Visibility == Visibility.Hidden) return;
            WPF_KONFIG_SQL FRM = new WPF_KONFIG_SQL(ds_d, CB_NR_PROJEKTU.Text.Trim(), TXT_NAZ_ZAKLADKI.Text.Trim());
            //FRM.int_id = id_s;
            //FRM.int_id = CB_NR_PROJEKTU.Text.Trim();
            FRM.str_nazwa = TXT_NAZ_ZAKLADKI.Text.Trim();
            //FRM.str_procedura_tlumaczen = TXT_SQL.Text.Trim();
            FRM.str_numerProjektu = CB_NR_PROJEKTU.Text.Trim();
            FRM.dt_xml = ds_d.Tables[akt_tab_name];

            var columnsName = (from c in DG_MOJE_USTAWIENIA.Columns
                               select c).ToList().OrderBy(x => x.Header);

            if (columnsName.Count() > 0)
            {
                foreach (var col in columnsName)
                {
                    FRM.listPolePowiazane.Add(col.Header.ToString());
                }

                FRM.listPolePowiazane.Add("Wartość z dodatkowych opcji");

            }

            var resolut =  FRM.ShowDialog();

            if (resolut == true) 
            {
                ladu_ds_xml(CB_NR_PROJEKTU.Text.Trim());
            }

        }

        private void MI_KONFIG_SQL_EXECUTE_Click(object sender, RoutedEventArgs e)
        {
            if (B_Wyslij_Excel.Visibility == Visibility.Hidden) return;

            if (CB_NR_PROJEKTU.Text.Trim() != "") ds_write_xml.write_ds_xml(CB_NR_PROJEKTU.Text.Trim().Replace("/", "_"), ds_d);
 
            WPF_ZAPIS_SQL FRM = new WPF_ZAPIS_SQL();
            FRM.str_nazwa = TXT_NAZ_ZAKLADKI.Text;
            FRM.str_procedura_tlumaczen = TXT_SQL.Text.Trim();
            FRM.str_numerProjektu = CB_NR_PROJEKTU.Text.Trim();
            //FRM.dv = dt_d.DefaultView;
            //FRM.int_id = id_s;
            FRM.int_id = CB_NR_PROJEKTU.Text.Trim();
            FRM.tlumacz = (bool)CB_UNIKAT.IsChecked;
            FRM.ShowDialog();

        }
        private void MenuItem_KONF_KOL(object sender, RoutedEventArgs e)
        {

            if (akt_tab_name == "") return;

            column_dis_sort();

            var columnsSort = (from c in DG_MOJE_USTAWIENIA.Columns
                               select c).ToList().OrderBy(x => x.DisplayIndex);

            if (((FrameworkElement)e.OriginalSource).Tag.ToString() == "0")
            {
                string str_copy = "";

                foreach (var item in columnsSort)
                {
                    str_copy += "'@" + item.Header.ToString() + "', ";
                }
                TXT_SQL.Text += "\r\n" + str_copy.Trim().TrimEnd(',') + "; ";

                return;
            }

            foreach (var item in columnsSort)
            {
                ds_d.Tables[akt_tab_name].Columns[item.Header.ToString()].SetOrdinal(item.DisplayIndex);
            }

            ds_d.Tables[akt_tab_name].AcceptChanges();

            WPF_EDYCJA_KOLUMN frm = new WPF_EDYCJA_KOLUMN(ds_d.Tables[akt_tab_name]);

            if (frm.ShowDialog() == true)
            {
                pokaz_ukryte_columny = (bool)frm.CB_POKAZ_UKRYTE_KOLUMY.IsChecked;

                if (frm.BoolUsunDane == true)
                {
                    ds_d.Tables[akt_tab_name].Rows.Clear();
                }

                if (ds_d.Tables[akt_tab_name] != null)
                {

                    if (((FrameworkElement)sender).Tag.ToString() == "DEFAULT")
                    {
                        //  dt_d = null;
                        // dt_d = new System.Data.DataTable();
                        ds_d.Tables[akt_tab_name].Reset();

                        foreach (DataRow row in frm.dt.Rows)
                        {
                            DataColumn dtc = new DataColumn();

                            dtc.ColumnName = row.ItemArray[1].ToString();
                            dtc.DataType = Type.GetType(row.ItemArray[2].ToString());
                            dtc.AllowDBNull = row.ItemArray[3].ToString() == "False" ? false : true;

                            if(dtc.ColumnName.ToLower() == "id")
                            {
                                dtc.AutoIncrement= true;
                            }

                            if (row.ItemArray[2].ToString() == "System.String")
                            {
                                dtc.DefaultValue = row.ItemArray[4].ToString();
                                dtc.MaxLength = (int)row.ItemArray[5];
                            }
                            else if (row.ItemArray[2].ToString() == "System.Boolean")
                            {
                                dtc.DefaultValue = row.ItemArray[4].ToString().ToLower() == "true" ? true : false;
                            }
                            else if (row.ItemArray[2].ToString() == "System.Int32")
                            {
                                var isNumber = row.ItemArray[4].ToString().All(Char.IsNumber);
                                if(isNumber == true)
                                {
                                    if (dtc.ColumnName.ToLower() != "id") dtc.DefaultValue = Convert.ToInt32(row.ItemArray[4]);
                                }
                               
                                dtc.MaxLength = (int)row.ItemArray[5];
                            }
                            else
                            {
                                dtc.MaxLength = (int)row.ItemArray[5];
                            }

                            var colsx = $"{string.Join("|", row.ItemArray.Where(val => val != DBNull.Value).ToArray())}\r\n";
                            var cols = row.ItemArray.Select(i => "" + i).ToArray();
                            string str_ns = "";
                            for (int i = 6; i < cols.Length; i++)
                            {
                                str_ns += cols[i].ToString() + ";";
                            }
                            dtc.Namespace = str_ns.TrimEnd(';');

                            ds_d.Tables[akt_tab_name].Columns.Add(dtc);
                        }

                        //ds_d.Tables[akt_tab_name].TableName = "$Konfigurator zakładak$";

                        ds_d.Tables[akt_tab_name].AcceptChanges();

                        ds_write_xml.write_ds_xml(CB_NR_PROJEKTU.Text.Trim().Replace("/", "_"), ds_d);

                    }
                    else
                    {
                        if (ds_d.Tables[akt_tab_name].Rows.Count > 0)
                        {
                            MessageBox.Show("W przypadku kiedy tabela posiada dane, zmieniona nazwy kolumny spowoduje jej dodanie a nie zmianę nazwy, nie można też usunąć kolumny.", "Ważna informacja!", MessageBoxButton.OK, MessageBoxImage.Information);

                            DataColumnCollection columns = ds_d.Tables[akt_tab_name].Columns;

                            foreach (DataRow row in frm.dt.Rows)
                            {

                                if (columns.Contains((string)row.ItemArray[1]) == false)
                                {
                                    DataColumn dtc = new DataColumn();

                                    dtc.ColumnName = row.ItemArray[1].ToString();
                                    dtc.DataType = Type.GetType(row.ItemArray[2].ToString());
                                    dtc.AllowDBNull = row.ItemArray[3].ToString() == "False" ? false : true;

                                    if (row.ItemArray[2].ToString() == "System.String")
                                    {
                                        dtc.DefaultValue = row.ItemArray[4].ToString();
                                        dtc.MaxLength = (int)row.ItemArray[5];
                                    }
                                    else if (row.ItemArray[2].ToString() == "System.Boolean")
                                    {
                                        dtc.DefaultValue = row.ItemArray[4].ToString().ToLower() == "true" ? true : false;
                                    }
                                    else
                                    {
                                        dtc.MaxLength = (int)row.ItemArray[5];
                                    }

                                    var colsx = $"{string.Join("|", row.ItemArray.Where(val => val != DBNull.Value).ToArray())}\r\n";
                                    var cols = row.ItemArray.Select(i => "" + i).ToArray();
                                    string str_ns = "";
                                    for (int i = 6; i < cols.Length; i++)
                                    {
                                        str_ns += cols[i].ToString() + ";";
                                    }
                                    dtc.Namespace = str_ns.TrimEnd(';');

                                    ds_d.Tables[akt_tab_name].Columns.Add(dtc);
                                }

                            }
                        }
                        else
                        {
                            ds_d.Tables[akt_tab_name].Reset();

                            foreach (DataRow row in frm.dt.Rows)
                            {
                                DataColumn dtc = new DataColumn();

                                dtc.ColumnName = row.ItemArray[1].ToString();
                                dtc.DataType = Type.GetType(row.ItemArray[2].ToString());
                                dtc.AllowDBNull = row.ItemArray[3].ToString() == "False" ? false : true;

                                if (row.ItemArray[2].ToString() == "System.String")
                                {
                                    dtc.DefaultValue = row.ItemArray[4].ToString();
                                    dtc.MaxLength = (int)row.ItemArray[5];
                                }
                                else if (row.ItemArray[2].ToString() == "System.Boolean")
                                {
                                    dtc.DefaultValue = row.ItemArray[4].ToString().ToLower() == "true" ? true : false;
                                }
                                else
                                {
                                    dtc.MaxLength = (int)row.ItemArray[5];
                                }

                                var colsx = $"{string.Join("|", row.ItemArray.Where(val => val != DBNull.Value).ToArray())}\r\n";
                                var cols = row.ItemArray.Select(i => "" + i).ToArray();
                                string str_ns = "";
                                for (int i = 6; i < cols.Length; i++)
                                {
                                    str_ns += cols[i].ToString() + ";";
                                }
                                dtc.Namespace = str_ns.TrimEnd(';');

                                ds_d.Tables[akt_tab_name].Columns.Add(dtc);
                            }
             
                        }

                        ds_write_xml.write_ds_xml(CB_NR_PROJEKTU.Text.Trim().Replace("/", "_"), ds_d);

                    }
                }

                DG_MOJE_USTAWIENIA.ItemsSource = null;

                DG_MOJE_USTAWIENIA.ItemsSource = ds_d.Tables[akt_tab_name].DefaultView;

                if (DG_MOJE_USTAWIENIA.Columns.Count > 0 && pokaz_ukryte_columny == false)
                {
                    DG_MOJE_USTAWIENIA.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                    DG_MOJE_USTAWIENIA.Columns[1].Visibility = System.Windows.Visibility.Hidden;
                }

                if(pokaz_ukryte_columny == true)
                {
                    load_tab();
                }

                if (ds_d.Tables[akt_tab_name].Columns.Contains("ID"))
                {
                    ds_d.Tables[akt_tab_name].Columns["ID"].AllowDBNull = false;
                    ds_d.Tables[akt_tab_name].Columns["ID"].Unique = true;
                    ds_d.Tables[akt_tab_name].Columns["ID"].AutoIncrement = true;
                }

            }
        }

        private void MyList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.DisplayIndex < 3 && pokaz_ukryte_columny == false)
            {
                e.Cancel = true;
            }
        }

        private void MyList_BeginningEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.DisplayIndex < 3 && pokaz_ukryte_columny == false)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
                if (e.EditingElement.GetType() == typeof(ComboBox))
                {
                    DataRowView MyRow = (DataRowView)DG_MOJE_USTAWIENIA.Items[((Selector)sender).SelectedIndex];
                    MyRow[e.Column.Header.ToString()] = ((ComboBox)e.EditingElement).SelectionBoxItem;
                }
            }
        }

        private void contex_menu_opening(object sender, ContextMenuEventArgs e)
        {
            if (CB_NR_PROJEKTU.Text.Trim() != "")
            {
                MI_KONFIG_SQL.IsEnabled = true;
                MI_KONFIG_EXECUTE_SQL.IsEnabled = true;
            }
            else
            {
                MI_KONFIG_SQL.IsEnabled = false;
                MI_KONFIG_EXECUTE_SQL.IsEnabled = false;
            }
        }

        private void column_dis_ind_changed(object sender, DataGridColumnEventArgs e)
        {

            if (DG_MOJE_USTAWIENIA.Columns.Count > 0)
            {

                if (pokaz_ukryte_columny == true) return;

                if ((string)e.Column.Header != (string)e.Column.SortMemberPath) return;

                if (((string)e.Column.SortMemberPath == "Objekt" || (string)e.Column.SortMemberPath == "ID" ||
                   (string)e.Column.SortMemberPath == "Ilość_Znaków") || e.Column.DisplayIndex < 3)
                {

                    var columnsNameIdFroz = (from c in DG_MOJE_USTAWIENIA.Columns
                                             where (string)c.Header == "ID" || (string)c.Header == "Objekt"
                                             || (string)c.Header == "Ilość_Znaków"
                                             select c).ToList();

                    if (columnsNameIdFroz.Count < 3) return;

                    columnsNameIdFroz[0].DisplayIndex = 0;
                    columnsNameIdFroz[1].DisplayIndex = 1;
                    columnsNameIdFroz[2].DisplayIndex = 2;
                    //columnsNameIdFroz[3].DisplayIndex = 3;


                    e.Column.SortMemberPath = "x";
                    e.Column.DisplayIndex = 3;

                    var columnsNameId = (from c in DG_MOJE_USTAWIENIA.Columns
                                         where (string)c.Header != "ID" && (string)c.Header != "Objekt" && (string)c.Header != "Ilość_Znaków"
                                         select c).ToList();

                    for (int i = 0; i < columnsNameId.Count(); i++)
                    {
                        DG_MOJE_USTAWIENIA.Columns[i + 4].DisplayIndex = i + 3;
                    }

                }

            }

        }

        private void column_dis_sort()
        {
            var columnsNameIdFroz = (from c in DG_MOJE_USTAWIENIA.Columns
                                     where (string)c.Header == "ID" || (string)c.Header == "Objekt" || (string)c.Header == "Ilość_Znaków"
                                     select c).ToList();

            if (columnsNameIdFroz.Count < 4) return;

            if (ds_d.Tables.Contains(akt_tab_name) == false)
            {
                MessageBox.Show("Brak tabeli w obiekcie <DS>", "Błąd", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (akt_tab_name != "$Konfigurator zakładak$") return;
            ds_d.Tables[akt_tab_name].Columns["ID"].SetOrdinal(0);
            ds_d.Tables[akt_tab_name].Columns["Objekt"].SetOrdinal(1);
            ds_d.Tables[akt_tab_name].Columns["Ilość_Znaków"].SetOrdinal(2);

            var columnsNameId = (from c in DG_MOJE_USTAWIENIA.Columns
                                 where (string)c.Header != "ID" && (string)c.Header != "Objekt" && (string)c.Header != "Ilość_Znaków"
                                 select c).ToList();

            for (int i = 0; i < columnsNameId.Count(); i++)
            {
                ds_d.Tables[akt_tab_name].Columns[i + 3].SetOrdinal(i + 3);
            }

            ds_d.Tables[akt_tab_name].AcceptChanges();

        }

        private void PrevKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {

            if (e.Key == System.Windows.Input.Key.Delete) return;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (((DataGrid)sender).CurrentItem.ToString() != "{NewItemPlaceholder}")
                {
                    DataRowView MyRowX = (DataRowView)DG_MOJE_USTAWIENIA.Items[((System.Windows.Controls.Primitives.Selector)sender).SelectedIndex];
                    MyRowX.EndEdit();
                }
                else
                {
                    if (ds_d.Tables[akt_tab_name].Columns.Contains("Ilość_Znaków") == false) return;

                    DataRow dr = ds_d.Tables[akt_tab_name].NewRow();
           
                    dr["Objekt"] = TXT_NAZ_ZAKLADKI.Text.Trim();
                    dr["Ilość_Znaków"] = 0;

                    if (dr["Id"] is System.DBNull)
                    {
                        dr["Id"] = dr.Table.Rows.Count;
                    }

                    ds_d.Tables[akt_tab_name].Rows.Add(dr);
                }

                return;
            }
            if (e.Key == System.Windows.Input.Key.PageDown) return;
            if (e.Key == System.Windows.Input.Key.Down) return;

            if (ds_d.Tables[akt_tab_name].Columns.Contains("Ilość_Znaków") == false) return;

            DataRowView MyRow = (DataRowView)DG_MOJE_USTAWIENIA.Items[((Selector)sender).SelectedIndex];

            if (((System.Windows.Controls.TextBox)e.OriginalSource) is System.Windows.Controls.TextBox)
            {
                MyRow["Ilość_Znaków"] = ((System.Windows.Controls.TextBox)e.OriginalSource).Text.Length;
            }
            else
            {
                MyRow["Ilość_Znaków"] = ((TextBlock)((ContentControl)e.OriginalSource).Content).Text.Length;
            }

            if (MyRow["Id"] is System.DBNull)
            {
                MyRow["Id"]= MyRow.DataView.Table.Rows.Count;
            }

            MyRow.EndEdit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void TXT_NAZ_ZAKLADKI_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (TXT_NAZ_ZAKLADKI.IsFocused)
            {
                if (ds_d.Tables.Contains(TXT_NAZ_ZAKLADKI.Text) == false)
                {
                    B_ZAPISZ.Content = "Dodaj zakładkę";
                    B_USUN_PROJEKT.IsEnabled = false;
                }
                else
                {
                    B_ZAPISZ.Content = "Zmień";
                    B_USUN_PROJEKT.IsEnabled = true;
                }
            }
        }

        private void B_USUN_PROJEKT_Click(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy usunąć projekt?","Uwaga!!!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (resoluts == MessageBoxResult.No) return;

            string ScieszkaProgramu;
            string file_name = CB_NR_PROJEKTU.Text.Trim();

            if (file_name.Trim() == "") return;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\xml\";

            file_name = file_name.Trim().Replace("/", "_");

            File.Delete(ScieszkaProgramu + file_name + ".xml");

            B_ZAPISZ.Content = "Dodaj zakładkę";
            CB_NR_PROJEKTU.Items.Clear();
            CB_NR_PROJEKTU.Text = "";
            TXT_NAZ_ZAKLADKI.Text = "";

            akt_tab_name = "";

            ds_d = new DataSetXML();

            load_tab();

            tc_selection_clear();
 
            DG_MOJE_USTAWIENIA.ItemsSource = null;

            load_xml_files();
        }
    }

}
