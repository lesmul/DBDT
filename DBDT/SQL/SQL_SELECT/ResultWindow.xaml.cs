using System.Windows;
using System.Data;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;
using System.Windows.Documents;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Text;
using DBDT.USTAWIENIA_PROGRAMU;

namespace DBDT.SQL.SQL_SELECT
{
    public partial class ResultWindow : Window
    {
        private static string Nazwa_Tabeli = "";
        private static string[] str_like ;
        private readonly DataView dv = new DataView();

        string copy_data_update = "";
        string copy_data_where ="";
        public ResultWindow(DataTable resultTable, string TableName, string[] like = null)
        {
            InitializeComponent();
            resultTable.TableName = "t_find";
            resultGrid.ItemsSource = resultTable.DefaultView;
            dv.Table = resultTable;
            Title = string.Format("Dane z {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + resultTable.Rows.Count + "]";
            this.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            
            if (TableName != null )
            {
                if (TableName.ToLower().IndexOf(" where") > 0)
                {
                    Nazwa_Tabeli = TableName.Substring(0, TableName.ToLower().IndexOf(" where"));
                }
                else
                {
                    Nazwa_Tabeli = TableName;
                }
            }

            str_like = like;

            LBL_INFO.Content = "Załadowane dane - [" + Nazwa_Tabeli + "]";

            CB_FILTR.ItemsSource = resultTable.Columns;
            CB_FILTR.DisplayMemberPath = "ColumnName";

        }

        void resultGrid_Update_U_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                copy_data_update = "UPDATE " + Nazwa_Tabeli.Trim() + " SET ";
                int intdindex = -1;
                int intdindexst = -1;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                    }
                    else
                    {
                        goto pomin_null;
                    }

                    if (intdindexst == -1)
                    {
                        intdindexst = cell.Column.DisplayIndex;

                    }

                    if (intdindex == intdindexst)
                    {
                        //copy_data += "\r\n";
                        copy_data_update += " ";
                    }

                    intdindex = cell.Column.DisplayIndex;
                    Regex rgx2 = new Regex("\t|\\s+");
                    string result = rgx2.Replace(value, " ");
                    copy_data_update += cell.Column.Header.ToString() + " = '" + result.Trim() + "'" + "\r\n" + ", ";

                pomin_null:;

                }
                copy_data_update = copy_data_update.Substring(0, copy_data_update.Length - 2);

            if (copy_memory.IsChecked == true)  Clipboard.SetDataObject(copy_data_update + copy_data_where);

            if (new_window.IsChecked == true && copy_data_update != "" && copy_data_where != "")
            {
                MainWindowSQL sp = new MainWindowSQL();
                sp.txtCode.Text = copy_data_update + copy_data_where;
                sp.B_EXIT.Visibility = Visibility.Hidden;

                Window nw = new Window();
                nw.Title = "Zmiany w tabeli: " + Nazwa_Tabeli;
                nw.Content = sp;
                nw.Show();
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Ustawiono warunk UPDATE";

        }

        void resultGrid_Update_W_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                copy_data_where = " ";
                int intdindex = -1;
                int intdindexst = -1;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                    }
                    else
                    {
                        goto pomin_null;
                    }

                    if (intdindexst == -1)
                    {
                        intdindexst = cell.Column.DisplayIndex;

                        if (copy_data_where.IndexOf("where") > -1)
                        {
                            copy_data_where = copy_data_where.Substring(0, copy_data_where.IndexOf("where"));
                        }
                    }

                    if (intdindex == -1)
                    {
                        if (copy_data_where.IndexOf("where") < 0)
                        {
                            copy_data_where += " where ";
                        }
                    }

                    if (intdindex == intdindexst)
                    {
                        //copy_data += "\r\n";
                        copy_data_where += " ";
                    }

                    intdindex = cell.Column.DisplayIndex;
                    Regex rgx2 = new Regex("\t|\\s+");
                    string result = rgx2.Replace(value, " ");
                    copy_data_where += cell.Column.Header.ToString() + " = '" + result.Trim() + "'" + "\r\n" + " and ";

                pomin_null:;

                }
                copy_data_where = copy_data_where.Substring(0, copy_data_where.Length - 4);

                if (copy_memory.IsChecked == true) Clipboard.SetDataObject(copy_data_update + copy_data_where);

                if (new_window.IsChecked == true && copy_data_update !="" && copy_data_where !="")
                {
                    MainWindowSQL sp = new MainWindowSQL();
                    sp.txtCode.Text = copy_data_update + copy_data_where;
                    sp.B_EXIT.Visibility = Visibility.Hidden;

                    Window nw = new Window();
                    nw.Title = "Zmiany w tabeli: " + Nazwa_Tabeli;
                    nw.Content = sp;
                    nw.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Ustawiono warunk WHERE";
        }
        void resultGrid_select_Click(object sender, RoutedEventArgs e)
        {

            if (resultGrid.SelectedCells.Count == 0) return;

            //System.Data.DataRowView F_R = (DataRowView)resultGrid.SelectedCells[0].Item;

            try
            {

                string copy_data = "select * from " + Nazwa_Tabeli.Trim() + " ";
                int intdindex = -1;
                int intdindexst = -1;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                    }
                    else
                    {
                        goto pomin_null;
                    }

                    if (intdindexst == -1)
                    {
                        intdindexst = cell.Column.DisplayIndex;

                        if (copy_data.IndexOf("where") > -1)
                        {
                            copy_data = copy_data.Substring(0, copy_data.IndexOf("where"));
                        }
                    }

                    if (intdindex == -1)
                    {
                        if (copy_data.IndexOf("where") < 0)
                        {
                            copy_data += " where ";
                        }
                    }

                    if (intdindex == intdindexst)
                    {
                        //copy_data += "\r\n";
                        copy_data += " ";
                    }

                    intdindex = cell.Column.DisplayIndex;
                    Regex rgx2 = new Regex("\t|\\s+");
                    string result = rgx2.Replace(value, " ");
                    copy_data += cell.Column.Header.ToString() + " = '" + result.Trim() + "'" + "\r\n" + " and ";

                pomin_null:;

                }
                copy_data = copy_data.Substring(0, copy_data.Length - 4);
                //Clipboard.SetText(copy_data.Trim());
                //string sql = copy_data.ToLower().Trim();
                //sql = sql.Replace("\nset", " ");
                //sql = sql.Replace("\n", " ");
                //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
                

                if (copy_memory.IsChecked == true) Clipboard.SetDataObject(copy_data); ;

                if (new_window.IsChecked == true && copy_data != "")
                {
                    MainWindowSQL sp = new MainWindowSQL();
                    sp.txtCode.Text = copy_data;
                    sp.B_EXIT.Visibility = Visibility.Hidden;

                    Window nw = new Window();
                    nw.Title = "Zmiany w tabeli: " + Nazwa_Tabeli;
                    nw.Content = sp;
                    nw.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano warunki select / where...";
        }

        void resultGrid_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            //System.Data.DataRowView F_R = (DataRowView)resultGrid.SelectedCells[0].Item;

            try
            {

                string copy_data = "";
                int intdindex = -1;
                int intdindexst = -1;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                    }
                    else
                    {
                        goto pomin_null;
                    }

                    if (intdindexst == -1)
                    {
                        intdindexst = cell.Column.DisplayIndex;

                        if (copy_data.IndexOf("where") > -1)
                        {
                            copy_data = copy_data.Substring(0, copy_data.IndexOf("where"));
                        }
                    }

                    if (intdindex == -1)
                    {
                        if (copy_data.IndexOf("where") < 0)
                        {
                            copy_data += " where ";
                        }
                    }

                    if (intdindex == intdindexst)
                    {
                        //copy_data += "\r\n";
                        copy_data += " ";
                    }

                    intdindex = cell.Column.DisplayIndex;
                    Regex rgx2 = new Regex("\t|\\s+");
                    string result = rgx2.Replace(value, " ");
                    copy_data += cell.Column.Header.ToString() + " = '" + result.Trim() + "'" + "\r\n" + " and ";

                pomin_null:;

                }
                copy_data = copy_data.Substring(0, copy_data.Length - 4);
                //Clipboard.SetText(copy_data.Trim());
                //string sql = copy_data.ToLower().Trim();
                //sql = sql.Replace("\nset", " ");
                //sql = sql.Replace("\n", " ");
                //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
                Clipboard.SetDataObject(copy_data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano warunki where ......";
        }

        private void click_clear(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject("");
            copy_data_update = "";
            copy_data_where = "";

            LBL_INFO.Content = "Schowek wyczyszczono...";
        }

        
        void ClickCopy_bez_pl(Object sender, RoutedEventArgs args)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                string copy_data = "";

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim();
                    }
                    else
                    {
                        goto pomin_null;
                    }
                    if (i == resultGrid.SelectedCells.Count)
                    {
                        string str_cls = value.Trim();
                        str_cls = str_cls.Replace("\nset", " ");
                        str_cls = str_cls.Replace("\n", " ");
                        str_cls = Regex.Replace(str_cls, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
                        str_cls = str_cls.Replace("Ś", "S");
                        str_cls = str_cls.Replace("ś", "s");
                        str_cls = str_cls.Replace("Ć", "c");
                        str_cls = str_cls.Replace("ć", "c");
                        str_cls = str_cls.Replace("Ń", "N");
                        str_cls = str_cls.Replace("ń", "n");
                        str_cls = str_cls.Replace("Ł", "L");
                        str_cls = str_cls.Replace("ł", "l");
                        str_cls = str_cls.Replace("Ż", "Z");
                        str_cls = str_cls.Replace("ż", "z");
                        str_cls = str_cls.Replace("Ź", "Z");
                        str_cls = str_cls.Replace("ź", "z");
                        str_cls = str_cls.Replace("Ó", "O");
                        str_cls = str_cls.Replace("ó", "o");
                        str_cls = str_cls.Replace("Ą", "A");
                        str_cls = str_cls.Replace("ą", "a");
                        str_cls = str_cls.Replace("Ę", "E");
                        str_cls = str_cls.Replace("ę", "e");

                        copy_data += str_cls;
                    }
                    else
                    {
                        string str_cls = value.Trim();
                        str_cls = str_cls.Replace("\nset", " ");
                        str_cls = str_cls.Replace("\n", " ");
                        str_cls = Regex.Replace(str_cls, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
                        str_cls = str_cls.Replace("Ś", "S");
                        str_cls = str_cls.Replace("ś", "s");
                        str_cls = str_cls.Replace("Ć", "c");
                        str_cls = str_cls.Replace("ć", "c");
                        str_cls = str_cls.Replace("Ń", "N");
                        str_cls = str_cls.Replace("ń", "n");
                        str_cls = str_cls.Replace("Ł", "L");
                        str_cls = str_cls.Replace("ł", "l");
                        str_cls = str_cls.Replace("Ż", "Z");
                        str_cls = str_cls.Replace("ż", "z");
                        str_cls = str_cls.Replace("Ź", "Z");
                        str_cls = str_cls.Replace("ź", "z");
                        str_cls = str_cls.Replace("Ó", "O");
                        str_cls = str_cls.Replace("ó", "o");
                        str_cls = str_cls.Replace("Ą", "A");
                        str_cls = str_cls.Replace("ą", "a");
                        str_cls = str_cls.Replace("Ę", "E");
                        str_cls = str_cls.Replace("ę", "e");

                        copy_data += str_cls + "\r\n";
                    }

                pomin_null:;

                }

                Clipboard.SetDataObject(copy_data.Substring(0, copy_data.Length - 2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano dane bez polskich znaków...";
        }

        void ClickCopyOryg(Object sender, RoutedEventArgs args)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                string copy_data = "";

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim();
                    }
                    else
                    {
                        goto pomin_null;
                    }
                    if (i == resultGrid.SelectedCells.Count)
                    {
                        string str_cls = value;
                        copy_data += str_cls;
                    }
                    else
                    {
                        string str_cls = value;
                        copy_data += str_cls + "\r\n";
                    }

                pomin_null:;

                }

                Clipboard.SetDataObject(copy_data.Substring(0, copy_data.Length - 2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano oryginalne dane...";
        }

        void ClickCopy(Object sender, RoutedEventArgs args) 
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                string copy_data = "";

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim();
                    }
                    else
                    {
                        goto pomin_null;
                    }
                    if(i == resultGrid.SelectedCells.Count)
                    {
                        string str_cls = value.Trim();
                        str_cls = str_cls.Replace("\nset", " ");
                        str_cls = str_cls.Replace("\n", " ");
                        str_cls = Regex.Replace(str_cls, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);

                        copy_data += str_cls;
                    }
                    else
                    {
                        string str_cls = value.Trim();
                        str_cls = str_cls.Replace("\nset", " ");
                        str_cls = str_cls.Replace("\n", " ");
                        str_cls = Regex.Replace(str_cls, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);

                        copy_data += str_cls + "\r\n";
                    }
                     
                pomin_null:;

                }

                Clipboard.SetDataObject(copy_data.Substring(0, copy_data.Length - 2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano dane bez spacji...";
        }

        private void Columns_select_Click(object sender, RoutedEventArgs e)
        {
            string value = "where ";

            for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
            {
                DataGridCellInfo cell = resultGrid.SelectedCells[i];
                if (cell.Item != null)
                {
                    value += cell.Column.Header.ToString() + ", " + "\r\n";
                }
            }

            Clipboard.SetDataObject(value.Substring(0, value.Length - 4));

            LBL_INFO.Content = "Skopiowano nazwy zaznaczonych kolumn...";
        }

        private void ColumnsOR_select_Click(object sender, RoutedEventArgs e)
        {
            if (str_like == null)
            {
                string value = "IF [";
                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value += "OPTION(" + '\u0022' + cell.Column.Header.ToString() + '\u0022' + "," + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + '\u0022' + ") OR ";
                    }
                }
                value = value.Substring(0, value.Length - 4);
                value += "] THEN";
                value += "\r\n";
                value += "   % - warunek  ";
                value += "\r\n";
                value += "ENDIF ";

                Clipboard.SetDataObject(value.Substring(0, value.Length - 1));
            }
            else
            {

                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count > 0)
                {
                    
                    DataTable dt_x = new DataTable();
                    var sqlHandler = new SqlHandler();

                    sqlHandler.Connect(@"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True");

                    if (Nazwa_Tabeli.Trim() !="")
                    {
                        dt_x = sqlHandler.Wartosc_nazwy_pola_scrypt("select top 1 " + str_like[0].ToString() + " from " + Nazwa_Tabeli + " where " + str_like[0].ToString() 
                            + " = '" + str_like[1].ToString() + "'");
                        if (dt_x.Rows.Count > 0)
                        {
                            str_like[1] = dt_x.Rows[0][0].ToString().Trim() ;
                        }
                    }
                }

                string value = "IF [";
                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value += "OPTION(" + '\u0022' + str_like[1].ToString() + '\u0022' + "," + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + '\u0022' + ") OR ";
                    }
                }
                value = value.Substring(0, value.Length - 4);
                value += "] THEN";
                value += "\r\n";
                value += "   % - warunek  ";
                value += "\r\n";
                value += "ENDIF ";

                Clipboard.SetDataObject(value.Substring(0, value.Length - 1));

            }

            LBL_INFO.Content = "Skopiowano dane do skryptu OR...";

        }

        private void ColumnsAND_select_Click(object sender, RoutedEventArgs e)
        {
            if (str_like == null)
            {
                string value = "IF [";
                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value += "OPTION(" + '\u0022' + cell.Column.Header.ToString() + '\u0022' + "," + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + '\u0022' + ") AND ";
                    }
                }
                value = value.Substring(0, value.Length - 5);
                value += "] THEN";
                value += "\r\n";
                value += "   % - warunek  ";
                value += "\r\n";
                value += "ENDIF ";

                Clipboard.SetDataObject(value.Substring(0, value.Length - 1));

            }
            else
            {

                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count > 0)
                {

                    DataTable dt_x = new DataTable();
                    var sqlHandler = new SqlHandler();

                    sqlHandler.Connect(@"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True");

                    if (Nazwa_Tabeli.Trim() != "")
                    {
                        dt_x = sqlHandler.Wartosc_nazwy_pola_scrypt("select top 1 " + str_like[0].ToString() + " from " + Nazwa_Tabeli + " where " + str_like[0].ToString()
                            + " = '" + str_like[1].ToString() + "'");
                        if (dt_x.Rows.Count > 0)
                        {
                            str_like[1] = dt_x.Rows[0][0].ToString().Trim();
                        }
                    }
                }

                string value = "IF [";
                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    if (cell.Item != null)
                    {
                        value += "OPTION(" + '\u0022' + str_like[1].ToString() + '\u0022' + "," + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + '\u0022' + ") AND ";
                    }
                }
                value = value.Substring(0, value.Length - 5);
                value += "] THEN";
                value += "\r\n";
                value += "   % - warunek  ";
                value += "\r\n";
                value += "ENDIF ";

                Clipboard.SetDataObject(value.Substring(0, value.Length - 1));
            }

            LBL_INFO.Content = "Skopiowano dane do skryptu AND...";

        }

        private void text_changed(object sender, TextChangedEventArgs e)
        {
           if( CB_FILTR.Text != "")
            {
                try
                {
                    if (TXT_FILTR.Text.Trim() == "")
                    {
                        dv.RowFilter = "";
                    }
                    else
                    {
                        if (like.IsChecked == true)
                        {
                            dv.RowFilter = CB_FILTR.Text.Trim() + " like '%" + TXT_FILTR.Text.Trim() + "%'";
                        }
                        if(ruwnum.IsChecked == true)
                        {
                            dv.RowFilter = CB_FILTR.Text.Trim() + " = '" + TXT_FILTR.Text.Trim() + "'";
                        }
                    }
                    
                    resultGrid.ItemsSource = dv;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Popraw filtr", MessageBoxButton.OK, MessageBoxImage.Error);
                    TXT_FILTR.Text = "";
                    ruwnum.IsChecked = true;
                }
            }
        }
    }

}
