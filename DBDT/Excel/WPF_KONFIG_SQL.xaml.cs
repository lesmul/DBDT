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
using System.Collections;
using System.Security.Cryptography;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_KONFIG_SQL : System.Windows.Window
    {
        // System.Data.DataTable dt = new System.Data.DataTable();
        //  System.Data.DataSet dsp = new System.Data.DataSet();
        DataView dataView = new DataView();

        ObservableCollection<string> listTypyDanych = null;
        ObservableCollection<string> listTypyParametrow = null;
        public ObservableCollection<string> listPolePowiazane = null;

        public string str_nazwa = "";
        string str_procedura_tlumaczen = "";
        public string str_numerProjektu = "";
        public System.Data.DataTable dt_xml = new System.Data.DataTable();

        public bool zapisOK = false;

        DataSet dt_s = new DataSet();
        string nazwa_pliku_xml;
        string naz_zakl;
        public WPF_KONFIG_SQL(DataSet dt_st, string str_nazwa_pliku_xml, string nazwa_zakladki)
        {
            InitializeComponent();

            dt_s = dt_st;
            //dt = dt_s.Tables["$SYSTEM_CONFIG$"];
            dataView = new DataView(dt_s.Tables["$SYSTEM_CONFIG$"]);

            naz_zakl = nazwa_zakladki;

            //dt_s.Tables["$SYSTEM_CONFIG$"].Columns("NazwaZakladki") = nazwa_zakladki;
            // dt_s.Tables["$SYSTEM_CONFIG$"].Columns[dt_s.Tables["$SYSTEM_CONFIG$"].Columns.Count -1].DefaultValue = nazwa_zakladki;

            nazwa_pliku_xml = str_nazwa_pliku_xml;

            listTypyDanych = new ObservableCollection<string>();
            listTypyParametrow = new ObservableCollection<string>();
            listPolePowiazane = new ObservableCollection<string>();

            listTypyDanych.Add("@");
            listTypyDanych.Add("adParamInput");
            listTypyDanych.Add("adParamOutput");
            gtypy_kolumn.ItemsSource = listTypyDanych;

            listTypyParametrow.Add("");
            listTypyParametrow.Add("adVarChar");
            listTypyParametrow.Add("adInteger");
            listTypyParametrow.Add("Nie tłumacz");
            listTypyParametrow.Add("Tłumacz");
            listTypyParametrow.Add("Tłumacz bez nr projektu");
            listTypyParametrow.Add("Kod języka");
            listTypyParametrow.Add("Pusty wiersz");
            gtypy_danych.ItemsSource = listTypyParametrow;

            listPolePowiazane.Add("");

            gpole_powiazane.ItemsSource = listPolePowiazane;

        }
        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            //$SYSTEM_CONFIG$


            dt_s.Tables["$SYSTEM_CONFIG$"].Columns["NazwaZakladki"].DefaultValue = naz_zakl;


            dataView.RowFilter = "NazwaZakladki = '" + str_nazwa + "'";

            dataView.Sort = "TypParametru, LP";

            resultGrid.CanUserSortColumns = false;

            resultGrid.ItemsSource = dataView;

            if (dataView.Count > 0)
            {
                TXT_SQL.Text = dataView[0]["PoleSQL"].ToString();
            }

            LB_INF_SQL.Content = "Ilość rekordów: " + dataView.Count;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            dataView.Table.AcceptChanges();

            string jest_ok = "";

        spr2:

            if (TXT_SQL.Text.EndsWith(Environment.NewLine))
            {
                TXT_SQL.Text = TXT_SQL.Text.Substring(0, TXT_SQL.Text.Length - Environment.NewLine.Length).TrimEnd(); ;
                goto spr2;
            }
            if (TXT_SQL.Text.EndsWith("¶") == false)
            {
                TXT_SQL.Text += "¶";
                
            }
            else
            {
                TXT_SQL.Text = TXT_SQL.Text.TrimEnd('¶').TrimEnd();
                goto spr2;
            }

                string[] sprawdz = TXT_SQL.Text.Trim().Split('¶');

            if (sprawdz.Length > 0)
            {
                TXT_SQL.Text = "";
                for (int i = 0; i < sprawdz.Length - 1; i++)
                {
                    TXT_SQL.Text += sprawdz[i].ToString().Trim().TrimEnd(',') + "¶" + "\r\n";
                }
            }

            for (int i = 0; i < dataView.Count; i++)
            {
                string sql = "";

                sql = TXT_SQL.Text.Trim();

                if (i == 0)
                {
                    TXT_SQL.Text = sql;
                }

                if (dataView[i]["LP"] == null) dataView[i]["LP"] = 0;
                if (dataView[i]["LP"].ToString() == "") dataView[i]["LP"] = 0;

                dataView[i]["PoleSQL"] = sql;
                dataView[i]["NazwaZakladki"] = str_nazwa;

                if (dataView[i]["id"].ToString() == "")
                {
                    //if (dt.Rows[i]["LP"] == null) dt.Rows[i]["LP"] = 0;
                    //if (dt.Rows[i]["LP"].ToString() == "") dt.Rows[i]["LP"] = 0;

                    if (dataView[i]["TypParametru"].ToString().Trim() == "")
                    {
                        MessageBox.Show("Typ parametru wymagany!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }

                    if (dataView[i]["NazwaParametru"].ToString().Trim() == "" && dataView[i]["TypParametru"].ToString() != "¶")
                    {
                        if (dataView[i]["TypDanych"].ToString() != "Kod języka" && dataView[i]["TypDanych"].ToString() != "Pusty wiersz")
                        {
                            MessageBox.Show("Nazwa parametru wymagana!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        }
                    }

                    if (dataView[i]["TypDanych"].ToString() == "Kod języka" || dataView[i]["TypDanych"].ToString() == "Pusty wiersz")
                    {
                        dataView[i]["PolePowiazane"] = "";
                    }


                    //if (_PUBLIC_SqlLite.DODAJ_ZMIEN_REKORD_SQL_OPERACJE(sql, str_nazwa, int_id, dt.Rows[i]["pole1"].ToString(), dt.Rows[i]["pole2"].ToString(), dt.Rows[i]["pole3"].ToString()
                    //, dt.Rows[i]["pole4"].ToString(), dt.Rows[i]["pole5"].ToString(), "-1", str_nazwa, (int)dt.Rows[i]["poleint2"]) == false) jest_ok += "Poz = " + (i + 1).ToString() + " ";

                }

                 //else
                //{
                //    //if (_PUBLIC_SqlLite.DODAJ_ZMIEN_REKORD_SQL_OPERACJE(sql, str_nazwa, int_id, dt.Rows[i]["pole1"].ToString(), dt.Rows[i]["pole2"].ToString(), dt.Rows[i]["pole3"].ToString()
                //    //, dt.Rows[i]["pole4"].ToString(), dt.Rows[i]["pole5"].ToString(), dt.Rows[i]["id"].ToString(), str_nazwa, (int)dt.Rows[i]["poleint2"]) == false) jest_ok += "Poz = " + (i + 1).ToString() + " ";
                //}
            }

            if (jest_ok == "")
            {
                //dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, opis, nazwa_obrobki, poleint1, poleint2, pole1, pole2, pole3, pole4, pole5 FROM obrobki " +
                // "WHERE poleint1 = " + int_id + " AND nazwa_obrobki = '" + str_nazwa + "' ORDER BY pole1, poleint2");

                //resultGrid.ItemsSource = dt.AsDataView();

                //if (dt.Rows.Count > 0)
                //{
                //    TXT_SQL.Text = dt.Rows[0]["PoleSQL"].ToString();
                //}

                LB_INF_SQL.Content = "Ilość rekordów: " + dataView.Count;

                TXT_SQL.Focus();
                TXT_SQL.CaretIndex = TXT_SQL.Text.Length;

                ds_write_xml.write_ds_xml(nazwa_pliku_xml, (DS.DataSetXML)dt_s);

                zapisOK = true;

            }
            else
            {
                MessageBox.Show("Sprawdź dane w: " + jest_ok, "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public class Info
        {
            public string TYPY_KOLUMN { get; set; }
            public string TYPY_PARAMETROW { get; set; }
            public string POWIAZANE_POLEW { get; set; }
        }

        private void cell_editing_end(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.EditingElement is System.Windows.Controls.TextBox) return;

            if (((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedItem == null) return;

            if (((System.Data.DataRowView)e.Row.Item).Row.ItemArray[0].ToString() == "") return;

            if (((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedItem.ToString() == "@" &&
                ((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedValuePath.ToString() == "TypParametru")
            {
                DataView dv = new DataView(dataView.Table);
                dv.RowFilter = "id=" + ((System.Data.DataRowView)e.Row.Item).Row.ItemArray[0].ToString();
                if (dv.Count > 0)
                {
                    dv.BeginInit();
                    dv[0]["TypDanych"] = "";
                    dv.EndInit();
                }
                //  MessageBox.Show("Wartość w polu typu danych musi być pusta", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedItem.ToString() != "@" &&
                ((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedItem.ToString() != "¶" &&
                ((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedValuePath.ToString() == "TypParametru")
            {
                DataView dv = new DataView(dataView.Table);
                dv.RowFilter = "id=" + ((System.Data.DataRowView)e.Row.Item).Row.ItemArray[0].ToString();
                if (dv.Count > 0)
                {
                    dv.BeginInit();
                    dv[0]["TypDanych"] = "adVarChar";
                    dv.EndInit();
                }
            }

        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {

            bool tlumacz = false;
            if (((FrameworkElement)e.OriginalSource).Tag.ToString() != "0")
            {
                tlumacz = true;
            }

            if (dt_xml.Rows.Count > 0)
            {
                TXT_SQL_TEST.Text = Execute_sql_txt.data_exec_function(dataView.Table, dt_xml, TXT_SQL.Text.Trim(), dt_xml.Rows[0]["id"].ToString(),
                    str_procedura_tlumaczen, tlumacz, str_numerProjektu);
                TXT_SQL_TEST.Text += "\r\n" + Execute_sql_txt.data_exec_procedura(dataView.Table, dt_xml, TXT_SQL.Text, dt_xml.Rows[0]["id"].ToString(),
                    str_procedura_tlumaczen, tlumacz, str_numerProjektu);
            }
            else
            {
                TXT_SQL_TEST.Text = Execute_sql_txt.data_exec_function(dataView.Table, dt_xml, TXT_SQL.Text.Trim(), "-1", str_procedura_tlumaczen, tlumacz, str_numerProjektu);
                if (dt_xml.Rows.Count > 0) TXT_SQL_TEST.Text += "\r\n" + Execute_sql_txt.data_exec_procedura(dataView.Table, dt_xml, TXT_SQL.Text, "-1",
                    str_procedura_tlumaczen, tlumacz, str_numerProjektu);
                MessageBox.Show("Test wymaga przynajmnie 1 rekordu danych!", "Brak danych!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void prev_key_up(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Delete) return;
            try
            {

                var currentRowIndex = resultGrid.Items.IndexOf(resultGrid.CurrentItem);

                DataRowView MyRow = (DataRowView)resultGrid.Items[currentRowIndex];

                _PUBLIC_SqlLite.USUN_REKORD_SQL_OPERACJE(MyRow["id"].ToString());

                MyRow.BeginEdit();
                MyRow.Delete();
                MyRow.EndEdit();
                dataView.Table.AcceptChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_KONF_KOL(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0)
            {
                return;
            }

            string dod_zn = "";

            if (((System.Windows.FrameworkElement)sender).Tag.ToString() != "DEFAULT")
            {
                dod_zn = "@";
            }

            IList items = resultGrid.Items;

            foreach (object item in items)
            {
                if (item.ToString() != "{NewItemPlaceholder}")
                {
                    DataRowView MyRow = (DataRowView)item;
                    TXT_SQL.Text += " " + dod_zn + MyRow[5].ToString() + ", ";
                }
            }

            TXT_SQL.Text = TXT_SQL.Text.TrimEnd().TrimEnd(',');
        }

        private void Click_NN(object sender, RoutedEventArgs e)
        {
            TXT_SQL.Text += "¶" + "\r\n";
        }

        private void Button_Example_Click(object sender, RoutedEventArgs e)
        {
            TXT_SQL_TEST.Text = "EXECUTE SetNazwaFunkcji @kolumna_1,  @kolumna_2,  @kolumna_3,  @kolumna_4,  @test_x1¶\r\nCommandText = \"dbo.up_dbdt_AddNewBaseNazwa\" kolumna_1, kolumna_2¶ -- procedura wykorzystuje paramtry (CommandText = zapis wymagany)";
        }

        private void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           this.DialogResult = zapisOK;
        }
    }
}
