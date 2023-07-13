using DBDT.Excel.DS;
using DBDT.USTAWIENIA_PROGRAMU;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static IronPython.Modules._ast;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy UC_EXEC_DATA.xaml
    /// </summary>
    public partial class UC_EXEC_DATA : UserControl
    {

        //string id_s;
        //System.Data.DataTable dt_d = new System.Data.DataTable();
        DataSetXML ds_d = new DataSetXML();
        //DataView dv = new DataView();
        bool boolCSV = false;
        public UC_EXEC_DATA()
        {
            InitializeComponent();

            LBL_INF_2.Text = "Cześć no to zaczynamy :) Dziś mamy: " + DateTime.Now.ToString("dd MMMM") + " do weekenu pozostało: " + (6 - (int)DateTime.Now.DayOfWeek) + " dni...";

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

        private void MenuItem_Click_Paste(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DG_MOJE_DANE.SelectedCells.Count < 1) return;

                List<string[]> rowData = ClipboardHelper.ParseClipboardData();

                int ind_id_kol = DG_MOJE_DANE.CurrentColumn.DisplayIndex;
                int ind_id_row = DG_MOJE_DANE.Items.IndexOf(DG_MOJE_DANE.CurrentItem);

                for (int i = 0; i < rowData.Count; i++)
                {
                    if (boolCSV == true)
                    {
                        string linia = rowData[i][0];
                        char separator = ';';
                        string[] strx = linia.Split(separator);

                        DataRow dr;

                        if (ind_id_row > DG_MOJE_DANE.Items.Count - 2)
                        {
                            dr = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].NewRow();
                        }
                        else
                        {
                            dr = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Rows[ind_id_row];
                        }

                        if (strx.Length > 0)
                        {

                            if (dr[ind_id_kol].GetType() == typeof(int))
                            {
                                if (rowData[i][0].GetType() == typeof(string))
                                {
                                    dr[ind_id_kol] = 0;
                                }
                                else
                                {
                                    dr[ind_id_kol] = Convert.ToInt32(rowData[i][0].ToString().Trim());
                                }
                            }
                            else
                            {
                                if (dr[ind_id_kol].GetType() == rowData[i][0].GetType()) dr[ind_id_kol] = rowData[i][0].ToString().Trim();
                            }

                            dr["Ilość_Znaków"] = strx[0].ToString().Trim().Length;
                        }

                        if (rowData[i].Length > 1 && DG_MOJE_DANE.Columns.Count >= ind_id_kol + 1)
                        {
                            if (dr[ind_id_kol + 1].GetType() == rowData[i][1].GetType()) dr[ind_id_kol + 1] = rowData[i][1].ToString();
                        }
                        if (rowData[i].Length > 2 && DG_MOJE_DANE.Columns.Count >= ind_id_kol + 2)
                        {
                            if (dr[ind_id_kol + 2].GetType() == rowData[i][2].GetType()) dr[ind_id_kol + 2] = rowData[i][2].ToString();
                        }

                        if (ind_id_row > DG_MOJE_DANE.Items.Count - 2) ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Rows.Add(dr);
                        ind_id_row++;
                    }
                    else
                    {

                        DataRow dr;

                        if (ind_id_row > DG_MOJE_DANE.Items.Count - 2)
                        {
                            dr = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].NewRow();
                        }
                        else
                        {
                            dr = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Rows[ind_id_row];
                        }

                        if (rowData.Count > 0)
                        {
                            if (dr[ind_id_kol].GetType() == typeof(int))
                            {
                                if (rowData[i][0].GetType() == typeof(string))
                                {
                                    dr[ind_id_kol] = 0;
                                }
                                else
                                {
                                    dr[ind_id_kol] = Convert.ToInt32(rowData[i][0].ToString().Trim());
                                }
                            }
                            else
                            {
                                if (dr[ind_id_kol].GetType() == rowData[i][0].GetType()) dr[ind_id_kol] = rowData[i][0].ToString().Trim();
                            }

                            dr["Ilość_Znaków"] = rowData[i][0].ToString().Trim().Length;
                        }

                        if (rowData[i].Length > 1 && DG_MOJE_DANE.Columns.Count >= ind_id_kol + 1)
                        {
                            if (dr[ind_id_kol + 1].GetType() == rowData[i][1].GetType()) dr[ind_id_kol + 1] = rowData[i][1].ToString();
                        }
                        if (rowData[i].Length > 2 && DG_MOJE_DANE.Columns.Count >= ind_id_kol + 2)
                        {
                            if (dr[ind_id_kol + 2].GetType() == rowData[i][2].GetType()) dr[ind_id_kol + 2] = rowData[i][2].ToString();
                        }

                        if (ind_id_row > DG_MOJE_DANE.Items.Count - 2) ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Rows.Add(dr);
                        ind_id_row++;
                    }

                }

                LBL_INF_2.Text = "Wklejono dane...";
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message, "Błąd podczas wklejania!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void MenuItem_Click_Cut(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_DANE.SelectedCells.Count < 1) return;

            string str_where = "";

            foreach (DataGridCellInfo itemx in DG_MOJE_DANE.SelectedCells)
            {
                var col = itemx.Column as DataGridColumn;
                var row = itemx.Item as DataRowView;

                if (row != null)
                {

                   str_where += row.Row[col.Header.ToString()].ToString().TrimEnd() + "\r\n";

                    var col_t = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Columns[col.Header.ToString()];
  
                    if (((FrameworkElement)sender).Tag.ToString() != "C")
                    {
                        if (col.DisplayIndex > 2)
                        {
                            if (col_t.DataType == typeof(bool))
                            {
                                row.Row[col.Header.ToString()] = false;
                            }
                            else if (col_t.DataType == typeof(Int32))
                            {
                                row.Row[col.Header.ToString()] = 0;
                            }
                            else
                            {
                                row.Row[col.Header.ToString()] = "";
                            }
                        }
                    }

                }
            }

            str_where = str_where.Substring(0, str_where.Length - 3);

            Clipboard.SetDataObject(str_where);

        }

        private void MenuItem_Click_Del(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_DANE.SelectedCells.Count < 1) return;

            foreach (DataGridCellInfo itemx in DG_MOJE_DANE.SelectedCells)
            {
               // var col = itemx.Column as DataGridColumn;
                var row = itemx.Item as DataRowView;

                if (row != null)
                {

                    row.Delete();

                }
            }
        }
        private void MenuItem_Klonuj(object sender, RoutedEventArgs e)
        {
            if (DG_MOJE_DANE.SelectedCells.Count == 0)
            {
                return;
            }

            switch (((FrameworkElement)sender).Tag)
            {
                case "WR":
                    zamien_wybrane.Zamien_wybrane_txt("WR", false, false, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "ALU":
                    zamien_wybrane.Zamien_wybrane_txt("ALU", false, false, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "PVC":
                    zamien_wybrane.Zamien_wybrane_txt("PVC", false, false, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "WR_":
                    zamien_wybrane.Zamien_wybrane_txt("WR", false, true, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "ALU_":
                    zamien_wybrane.Zamien_wybrane_txt("ALU", false, true, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "PVC_":
                    zamien_wybrane.Zamien_wybrane_txt("PVC", false, true, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "XX_DOWOLNY":
                    bool bool_zmien = false;
                    if (tbCustomDataRange.Text.IndexOf("_") > -1) bool_zmien = true;
                    zamien_wybrane.Zamien_wybrane_txt(tbCustomDataRange.Text, bool_zmien, false, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                case "DEL_":
                    zamien_wybrane.Zamien_wybrane_txt("_", true, true, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    break;
                default:

                    if (((FrameworkElement)sender).Tag.ToString() != "")
                    {
                        zamien_wybrane.Zamien_wybrane_txt(((FrameworkElement)sender).Tag.ToString().Trim().TrimEnd('_'), false, true, DG_MOJE_DANE, ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()]);
                    }

                    break;

            }

            LBL_INF_2.Text = "Dodano nowe dane... [" + ((FrameworkElement)sender).Tag.ToString() + "]";

        }

        private void MI_KONFIG_SQL_Click(object sender, RoutedEventArgs e)
        {
            konfiguracj_click();
        }

        private void konfiguracj_click()
        {

            if (TXT_PROJEKT.Text == "" || CB_NAZ_ZAK.SelectedItem == null)
            {
                MessageBox.Show("Wybierz projekt", "Błąd konfiguracji!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            WPF_KONFIG_SQL FRM = new WPF_KONFIG_SQL(ds_d, TXT_PROJEKT.Text.Trim(), CB_NAZ_ZAK.Text.Trim());

            FRM.str_nazwa = CB_NAZ_ZAK.SelectedItem.ToString();
            FRM.str_numerProjektu = TXT_PROJEKT.Text.Trim();
            FRM.dt_xml = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()];

            var columnsName = (from c in DG_MOJE_DANE.Columns
                               select c).ToList().OrderBy(x => x.Header);

            if (columnsName.Count() > 0)
            {
                foreach (var col in columnsName)
                {
                    FRM.listPolePowiazane.Add(col.Header.ToString());
                }

                FRM.listPolePowiazane.Add("Wartość z dodatkowych opcji");

            }

            LBL_INF_2.Text = "Konfiguruj dane...";

            FRM.ShowDialog();
        }

        private void konfiguracj_zapis_click()
        {
            if (TXT_PROJEKT.Text.Trim() == "")
            {
                MessageBox.Show("Wybierz projekt", "Błąd konfiguracji!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (TXT_PROJEKT.Text.Trim() != "") ds_write_xml.write_ds_xml(TXT_PROJEKT.Text.Trim(), ds_d);

            DataView dv = new DataView(ds_d.Tables["$SYSTEM$"]);

            dv.RowFilter = "Objekt = '" + CB_NAZ_ZAK.SelectedValue.ToString() + "'";

            if (dv.Count == 0)
            {
                MessageBox.Show("Sprawdź konfigurację!", "Błąd konfiguracji!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            WPF_ZAPIS_SQL FRM = new WPF_ZAPIS_SQL();
            FRM.str_nazwa = CB_NAZ_ZAK.SelectedItem.ToString();
            FRM.str_procedura_tlumaczen = dv[0]["SQL_Tlumaczenia"].ToString();
            FRM.str_numerProjektu = TXT_PROJEKT.Text.Trim();
            //FRM.dv = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].DefaultView;
            FRM.tlumacz = (bool)dv[0]["WlaczTlumaczenie"];

            FRM.ShowDialog();
        }

        private void MI_KONFIG_SQL_EXECUTE_Click(object sender, RoutedEventArgs e)
        {

            konfiguracj_zapis_click();

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

                System.IO.FileInfo fid = new System.IO.FileInfo(ScieszkaProgramu + nazwa_plik_xml + ".xsd");


                if (fid.Exists == true)
                {
                    ds_d.ReadXmlSchema(ScieszkaProgramu + nazwa_plik_xml + ".xsd");
                }

                ds_d.ReadXml(ScieszkaProgramu + nazwa_plik_xml + ".xml");

                if (ds_d.Tables.Count > 3)
                {
                    var result = ds_d.Tables.OfType<System.Data.DataTable>().Select(dt => dt.TableName).Where(val => val.StartsWith("$") == false && val.EndsWith("$") == false);

                    CB_NAZ_ZAK.ItemsSource = result;


                    if (ds_d.Tables[3].Rows.Count > 0)
                    {
                        CB_NAZ_ZAK.SelectedIndex = 0;
                    }

                    if (DG_MOJE_DANE.Columns.Count > 3)
                    {
                        DG_MOJE_DANE.Columns[0].Visibility = Visibility.Collapsed;
                        DG_MOJE_DANE.Columns[1].Visibility = Visibility.Collapsed;
                        DG_MOJE_DANE.Columns[2].Visibility = Visibility.Collapsed;
                        // DG_MOJE_DANE.Columns[3].Visibility = Visibility.Collapsed;
                    }

                }

            }
            else
            {
                B_DODAJ_PROJEKT.IsEnabled = false;
            }

        }

        private void B_FIND(object sender, RoutedEventArgs e)
        {
            CB_NAZ_ZAK.Tag = "NEW";
            B_DODAJ_PROJEKT.IsEnabled = false;

            TXT_PROJEKT.Visibility = Visibility.Collapsed;
            CB_NR_PROJEKTU.Visibility = Visibility.Visible;

            CB_NAZ_ZAK.ItemsSource = null;

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
                CB_NR_PROJEKTU.Items.Clear();

                var children = DirectoryStructure.GetDirectoryContents(ScieszkaProgramu, "*.xml");

                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].Name != "_auto.xml")
                    {
                        int int_type = children[i].Name.LastIndexOf(children[i].TypeFile.ToString());
                        CB_NR_PROJEKTU.Items.Add(children[i].Name.Substring(0, int_type).Replace("_", "/"));
                    }
                }
            }

            LBL_INF_2.Text = "Wyszukuję projekt: " + TXT_PROJEKT.Text.Trim();

        }

        private void CB_NR_PROJEKTU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DG_MOJE_DANE.ItemsSource = null;

            B_DODAJ_PROJEKT.IsEnabled = true;

            if (((Selector)e.OriginalSource).SelectedIndex < 0)
            {
                B_DODAJ_PROJEKT.IsEnabled = false;
                return;
            }

            CB_NR_PROJEKTU.Text = ((Selector)e.OriginalSource).SelectedItem.ToString();

            ladu_ds_xml(((Selector)e.OriginalSource).SelectedItem.ToString());

            TXT_PROJEKT.Text = CB_NR_PROJEKTU.Text;

            LBL_INF_2.Text = "Załadowano projekt, ilość wszystkich tabel: " + ds_d.Tables.Count + " użytkownika: " + (ds_d.Tables.Count - 3);

        }
        private void cb_nr_pr_key_up(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Clear)
            {
                ladu_ds_xml("");
                B_DODAJ_PROJEKT.IsEnabled = false;
            }

            if (e.Key == System.Windows.Input.Key.Delete)
            {
                ladu_ds_xml("");
                B_DODAJ_PROJEKT.IsEnabled = false;
            }

            if (e.Key == System.Windows.Input.Key.Back)
            {
                CB_NR_PROJEKTU.Text = "";
                B_DODAJ_PROJEKT.IsEnabled = false;
                ladu_ds_xml("");
            }
        }

        private void CB_NAZ_ZAK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CB_NAZ_ZAK.SelectedValue == null)
            {
                B_DODAJ_PROJEKT.IsEnabled = false;
                return;
            }

            DG_MOJE_DANE.Columns.Clear();
            DG_MOJE_DANE.ItemsSource = null;

            DG_MOJE_DANE.AutoGenerateColumns = false;

            foreach (DataColumn col in ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].Columns)
            {

                string[] strWerJ = col.Namespace.Split(';');
                if (strWerJ.Length > 17)
                {
                    if (strWerJ[17].Trim().StartsWith("{") && strWerJ[17].IndexOf("|") > 0 && strWerJ[17].Trim().EndsWith("}"))
                    {
                        string[] strWerSz = strWerJ[17].Split('|');

                        DataView dvcmb = new DataView(ds_d.Tables[strWerSz[0].Trim().TrimStart('{')]);

                        DataGridComboBoxColumn combo = new DataGridComboBoxColumn();
                        combo.ItemsSource = dvcmb;
                        combo.Header = col.ColumnName;
                        combo.SelectedValuePath = col.ColumnName;
                        combo.SortMemberPath = col.ColumnName;
                        combo.DisplayMemberPath = strWerSz[1].Trim().TrimEnd('}');
                        combo.TextBinding = new Binding(col.ColumnName);
                        DG_MOJE_DANE.Columns.Add(combo);
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
                        DG_MOJE_DANE.Columns.Add(combo);
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
                        DG_MOJE_DANE.Columns.Add(boolc);
                    }
                    else
                    {
                        DataGridTextColumn textc = new DataGridTextColumn();
                        textc.Header = col.ColumnName;
                        textc.SortMemberPath = col.ColumnName;
                        textc.Binding = new Binding(col.ColumnName);
                        textc.Binding.ProvideValue(col);
                        DG_MOJE_DANE.Columns.Add(textc);
                    }
                }

            }

            DG_MOJE_DANE.ItemsSource = ds_d.Tables[CB_NAZ_ZAK.SelectedValue.ToString()].DefaultView;

            LBL_INF_2.Text = "Edytujesz zakładkę: " + ((Selector)e.OriginalSource).SelectedItem;

            if (DG_MOJE_DANE.Columns.Count > 3)
            {
                DG_MOJE_DANE.Columns[0].Visibility = Visibility.Collapsed;
                DG_MOJE_DANE.Columns[1].Visibility = Visibility.Collapsed;
                DG_MOJE_DANE.Columns[2].Visibility = Visibility.Collapsed;
                //DG_MOJE_DANE.Columns[3].Visibility = Visibility.Collapsed;
            }

        }

        private void B_DODAJ_NOWY_CLICK(object sender, RoutedEventArgs e)
        {

            WPF_NOWY_PROJEKT frm = new WPF_NOWY_PROJEKT();
            if (frm.ShowDialog() == false) return;

            ds_write_xml.write_ds_xml(TXT_PROJEKT.Text.Trim(), ds_d);

            ds_write_xml.write_ds_xml(frm.TXT_NOWY_NUMER.Text.Trim(), ds_d);

            CB_NR_PROJEKTU.Items.Clear();

            string ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\xml\";

            var children = DirectoryStructure.GetDirectoryContents(ScieszkaProgramu, "*.xml");

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name != "_auto.xml")
                {
                    int int_type = children[i].Name.LastIndexOf(children[i].TypeFile.ToString());
                    CB_NR_PROJEKTU.Items.Add(children[i].Name.Substring(0, int_type).Replace("_", "/"));
                }
            }

            TXT_PROJEKT.Text = frm.TXT_NOWY_NUMER.Text.Trim();
            CB_NR_PROJEKTU.Text = frm.TXT_NOWY_NUMER.Text.Trim();

            ladu_ds_xml(frm.TXT_NOWY_NUMER.Text.Trim());

            LBL_INF_2.Text = "Dodałeś nowy projekt: " + TXT_PROJEKT.Text.Trim() + " możesz edytować projekt w konfiguratorze";
        }

        private void b_sprawdz_w_konf_click(object sender, RoutedEventArgs e)
        {
            LBL_INF_2.Text = "Dane do projektu: " + TXT_PROJEKT.Text.Trim() + " są gotowe do zapisu!";
            konfiguracj_zapis_click();
        }

        private void save_xml_click(object sender, RoutedEventArgs e)
        {
            if (TXT_PROJEKT.Text.Trim() == "")
            {
                LBL_INF_2.Text = "Brak numeru projektu - zmieniłem domyślny...";
            }

            ds_write_xml.write_ds_xml(TXT_PROJEKT.Text.Trim(), ds_d);
            LBL_INF_2.Text = "Zapisałem plik XML: " + TXT_PROJEKT.Text.Trim();
        }

        private void PrevKeyUp(object sender, KeyEventArgs e)
        {
            try
            {

                try
                {

                    if (((DataGridCell)e.OriginalSource) is DataGridCell) return;
                }
                catch (Exception exc)
                {
                    LBL_INF_2.Text = exc.Message;
                }

                if (((System.Windows.Controls.TextBox)e.OriginalSource) is System.Windows.Controls.TextBox)
                {
                    LBL_INF_ILE_ZNAKOW.Text = "Ilość znaków: " + ((System.Windows.Controls.TextBox)e.OriginalSource).Text.Length.ToString();
                }
                else
                {
                    LBL_INF_ILE_ZNAKOW.Text = "Ilość znaków: " + ((System.Windows.Controls.TextBlock)((System.Windows.Controls.ContentControl)e.OriginalSource).Content).Text.Length.ToString();
                }
            }
            catch (Exception ex)
            {
                LBL_INF_2.Text = ex.Message;
            }

        }

        private void prev_key_up_projekt(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CB_NAZ_ZAK.Tag = "NEW";

                if (TXT_PROJEKT.Text.Trim() == "")
                {
                    MessageBox.Show("Załadowano domyślny projekt", "Podaj numer projektu aby go zapisać!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    ladu_ds_xml("_auto");
                    B_DODAJ_PROJEKT.IsEnabled = false;
                }
                else
                {
                    ladu_ds_xml(TXT_PROJEKT.Text.Trim().Replace("/", "_"));
                    B_DODAJ_PROJEKT.IsEnabled = true;
                }
            }
        }

    }

}
