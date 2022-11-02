using DBDT.DrzewoProcesu.Directory.ViewModels.Base;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static IronPython.Modules._ast;
using static IronPython.Runtime.Profiler;

namespace DBDT.Indeksy
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_TYPY_INDEKSOW.xaml
    /// </summary>
    public partial class WPF_TYPY_INDEKSOW : Window
    {

         int id_indesk = -1;

        DataTable dtr = new DataTable();
        string sql = "SELECT id ,nazwa_obrobki ,opis ,pole1 ,pole2 ,pole3 ,pole4 ,pole5 ,pole6 ,pole7 ,pole8 ,pole9 ,pole10 ,pole11 ,poleint1" +
            ",poleint2, kto_zmienil ,data_utworzenia FROM obrobki WHERE poleint1 = 1 ORDER BY nazwa_obrobki";
        public WPF_TYPY_INDEKSOW()
        {
            //Binding="{Binding Path=id}" 
            InitializeComponent();

            dtr = _PUBLIC_SqlLite.SelectQuery(sql);

            dtr.Columns["data_utworzenia"].DefaultValue = DateTime.Now;
            dtr.Columns["kto_zmienil"].DefaultValue = Environment.UserName;
            dtr.Columns["poleint1"].DefaultValue = 1;
            dtr.Columns["poleint2"].DefaultValue = -1;
            dtr.Columns["nazwa_obrobki"].DefaultValue = "Podaj nazwe!!!";

            dgList.ItemsSource = dtr.DefaultView;

            if(dtr.Rows.Count > 0)
            {
                dgList.SelectedIndex = 0;
                id_indesk = dgList.SelectedIndex;

                string[] strOp = ((DataRowView)dgList.SelectedValue).Row["opis"].ToString().Split(',');
                if (strOp.Length > 3)
                {
                    TXT_NAZWA_TABELI.Text = strOp[0].ToString();
                    TXT_NAZWA_KOLUMNY.Text = strOp[1].ToString();
                    CB_TYP.Text = strOp[2].ToString();
                    TXT_NAZWA_TABELI_Z_KOLORAMI.Text = strOp[3].ToString();
                    TXT_NAZWA_KOLUMNY_TYP.Text = (strOp.Length < 5 ? "" : strOp[4].ToString());
                    TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text = (strOp.Length < 6 ? "" : strOp[5].ToString());

                }
           
                if (Convert.ToInt16(((DataRowView)dgList.SelectedValue).Row["poleint2"]) == 1) RB_1.IsChecked = true;
                if (Convert.ToInt16(((DataRowView)dgList.SelectedValue).Row["poleint2"]) == 2) RB_2.IsChecked = true;
                if (Convert.ToInt16(((DataRowView)dgList.SelectedValue).Row["poleint2"]) == 3) RB_3.IsChecked = true;
                if (Convert.ToInt16(((DataRowView)dgList.SelectedValue).Row["poleint2"]) == 4) RB_4.IsChecked = true;

            }

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {

            if (dgList.SelectedValue == null && id_indesk == -1) 
            {
                return;
            }

            if (dgList.SelectedValue.ToString() == "{NewItemPlaceholder}" )
            {
    
                DataRow dr = dtr.NewRow();
                dr["opis"] = TXT_NAZWA_TABELI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY.Text.Trim() + "," + CB_TYP.Text.Trim()
                    + "," + TXT_NAZWA_TABELI_Z_KOLORAMI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TYP.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text.Trim();
                if (RB_1.IsChecked == true) dr["poleint2"] = 1;
                if (RB_2.IsChecked == true) dr["poleint2"] = 2;
                if (RB_3.IsChecked == true) dr["poleint2"] = 3;
                if (RB_4.IsChecked == true) dr["poleint2"] = 4;
                dtr.Rows.Add(dr);
                id_indesk = dtr.Rows.Count - 1;
            }
            else
            {
                dgList.SelectedIndex = id_indesk;

                if (RB_1.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 1;
                if (RB_2.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 2;
                if (RB_3.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 3;
                if (RB_4.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 4;

                ((DataRowView)dgList.SelectedValue).Row["opis"] = TXT_NAZWA_TABELI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY.Text.Trim() + "," + CB_TYP.Text.Trim() 
                    + "," + TXT_NAZWA_TABELI_Z_KOLORAMI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TYP.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text.Trim();

                id_indesk = dgList.SelectedIndex;

            }

            _PUBLIC_SqlLite.UpdateData(sql, dtr);

            dtr = _PUBLIC_SqlLite.SelectQuery(sql);

            dgList.ItemsSource = dtr.DefaultView;

            dgList.SelectedIndex = id_indesk;

            LBL_INF.Content = "Zapisałem dane!!!";
            //MessageBox.Show("Zapisano do bazy!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TXT_NAZWA_KOLUMNY.Text.Trim() == "" || TXT_NAZWA_TABELI.Text.Trim() == "" || TXT_NAZWA_KOLUMNY_TYP.Text.Trim() == "") return;
            DataTable dt = new DataTable();
            dt = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT DISTINCT TOP 10000 " + TXT_NAZWA_KOLUMNY_TYP.Text + " FROM " + TXT_NAZWA_TABELI.Text + " ORDER BY " + TXT_NAZWA_KOLUMNY_TYP.Text);
            if (dt == null) return;
            CB_TYP.ItemsSource = null;
            CB_TYP.ItemsSource = dt.DefaultView;
            CB_TYP.DisplayMemberPath = TXT_NAZWA_KOLUMNY_TYP.Text;
        }

        private void PrevMouseUp(object sender, MouseButtonEventArgs e)
        {
            id_indesk = dgList.SelectedIndex;

            dgList.SelectedIndex = id_indesk;

            LBL_INF.Content = "Nr wiersza: " + (id_indesk + 1);

            var sel_itm = dgList.SelectedValue;
            if (sel_itm == null) return;
            if (sel_itm.ToString() == "{NewItemPlaceholder}") return;

            var cel_val = ((DataRowView)sel_itm).DataView;

            if (cel_val[id_indesk]["opis"] != null)
            {
                string[] strOp = cel_val[id_indesk]["opis"].ToString().Split(',');

                if (strOp.Length > 3)
                {
                    TXT_NAZWA_TABELI.Text = strOp[0].ToString();
                    TXT_NAZWA_KOLUMNY.Text = strOp[1].ToString();
                    CB_TYP.Text = strOp[2].ToString();
                    TXT_NAZWA_TABELI_Z_KOLORAMI.Text = strOp[3].ToString();
                    TXT_NAZWA_KOLUMNY_TYP.Text = (strOp.Length < 5 ? "" : strOp[4].ToString());
                    TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text = (strOp.Length < 6 ? "" : strOp[5].ToString());

                    if (cel_val[id_indesk]["poleint2"] == null) return;

                    if (Convert.ToInt16(cel_val[id_indesk]["poleint2"]) == 1) RB_1.IsChecked = true;
                    if (Convert.ToInt16(cel_val[id_indesk]["poleint2"]) == 2) RB_2.IsChecked = true;
                    if (Convert.ToInt16(cel_val[id_indesk]["poleint2"]) == 3) RB_3.IsChecked = true;
                    if (Convert.ToInt16(cel_val[id_indesk]["poleint2"]) == 4) RB_4.IsChecked = true;

                }
                else
                {

                    TXT_NAZWA_TABELI.Text = "";
                    TXT_NAZWA_KOLUMNY.Text = "";
                    CB_TYP.Text = "";
                    TXT_NAZWA_TABELI_Z_KOLORAMI.Text = "";
                    TXT_NAZWA_KOLUMNY_TYP.Text = "";
                    TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text = "";

                    RB_1.IsChecked = false;
                    RB_2.IsChecked = false;
                    RB_3.IsChecked = false;
                    RB_4.IsChecked = false;

                }
            }

        }

        private void text_changed(object sender, TextChangedEventArgs e)
        {
            if (dgList.SelectedValue.ToString() == "{NewItemPlaceholder}") return;

            ((DataRowView)dgList.SelectedValue).Row["opis"] = TXT_NAZWA_TABELI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY.Text.Trim() + "," + CB_TYP.Text.Trim()
                    + "," + TXT_NAZWA_TABELI_Z_KOLORAMI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TYP.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text.Trim();

        }

        private void rb_click(object sender, RoutedEventArgs e)
        {
            if (dgList.SelectedValue.ToString() == "{NewItemPlaceholder}") return;

            if (RB_1.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 1;
            if (RB_2.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 2;
            if (RB_3.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 3;
            if (RB_4.IsChecked == true) ((DataRowView)dgList.SelectedValue).Row["poleint2"] = 4;

        }

        private void key_up(object sender, KeyEventArgs e)
        {
            if (dgList.SelectedValue.ToString() == "{NewItemPlaceholder}") return;

            ((DataRowView)dgList.SelectedValue).Row["opis"] = TXT_NAZWA_TABELI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY.Text.Trim() + "," + CB_TYP.Text.Trim()
                    + "," + TXT_NAZWA_TABELI_Z_KOLORAMI.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TYP.Text.Trim() + "," + TXT_NAZWA_KOLUMNY_TABELA_KOLORY.Text.Trim();
        }

        private void btn_Kopiuj_Click(object sender, RoutedEventArgs e)
        {

            if (dgList.SelectedValue == null) return;

            DataRow drNew = ((DataRowView)dgList.SelectedValue).DataView.Table.NewRow();
            drNew.ItemArray = ((DataRowView)dgList.SelectedValue).Row.ItemArray;
            ((DataRowView)dgList.SelectedValue).DataView.Table.Rows.Add(drNew);

        }
    }
}  
