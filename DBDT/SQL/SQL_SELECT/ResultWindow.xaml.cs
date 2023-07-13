using DBDT.Excel;
using DBDT.SQL.Indeksy;
using Microsoft.Scripting.Utils;
using System;
using System.Collections;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DBDT.SQL.SQL_SELECT
{
    public partial class ResultWindow : Window
    {
        private static string Nazwa_Tabeli = "";
        private static string str_option = "";
        private static string[] str_like;
        private readonly DataView dv = new DataView();
        DataSet ds = new DataSet();

        string copy_data_update = "";
        string copy_data_where = "";
        double dbl_row_count;

        IndeksTable spc = new IndeksTable();
        public ResultWindow(DataTable resultTable, string TableName, string[] like = null, string t_find_name = "t_find",
            DataSet dsl = null, string strInf2 = "")
        {
            InitializeComponent();

            spc.itContr.ItemsSource = null;

            resultTable.TableName = t_find_name;
            resultGrid.ItemsSource = resultTable.DefaultView;
            dv.Table = resultTable;
            dbl_row_count = resultTable.Rows.Count;
            Title = string.Format("Dane z {0} z {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + resultTable.Rows.Count + "]";
            this.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            if (dsl != null)
            {
                FullTabele.Visibility = Visibility.Visible;
                //FullTabele.ItemsSource = dsl.Tables;
                //FullTabele.DisplayMemberPath= "TableName";
                //FullTabele.SelectedIndex=0;

                for (int i = 0; i < dsl.Tables.Count; i++)
                {
                    if (dsl.Tables[i].Rows.Count > 0)
                    {
                        FullTabele.Items.Add(dsl.Tables[i].TableName);
                    }
                }

                FullTabele.SelectedIndex = 0;

                FullTabele.ToolTip = "Załadowane table [" + FullTabele.Items.Count + " z " + dsl.Tables.Count.ToString() + "]";

                ds = dsl;
            }

            if (TableName != null)
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

            if (like == null)
            {
                str_option = "";
            }
            else
            {
                str_option = str_like[0];
            }

            if (tbCustomDataRangeOption.Text.Trim() == "")
            {
                if (str_option.Length > 30)
                {
                    tbCustomDataRangeOption.Text = str_option.Substring(0, 30);
                }
                else
                {
                    tbCustomDataRangeOption.Text = str_option;
                }
            }

            LBL_INFO.Content = "Załadowane dane - [" + Nazwa_Tabeli + "]";

            CB_FILTR.ItemsSource = resultTable.Columns;
            CB_FILTR.DisplayMemberPath = "ColumnName";

            if (Nazwa_Tabeli.Length > 50)
            {
                tbCustomDataRange.Text = Nazwa_Tabeli.Substring(0, 50);
            }
            else
            {
                tbCustomDataRange.Text = Nazwa_Tabeli;
            }

            if (strInf2 != "") LBL_INFO_2.Content = strInf2;

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

                if (copy_memory.IsChecked == true) Clipboard.SetDataObject(copy_data_update + copy_data_where);

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

            LBL_INFO.Content = "Ustawiono warunek UPDATE";

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

            LBL_INFO.Content = "Ustawiono warunek WHERE";
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

                bool bool_new_row = false;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];

                    var col = cell.Column as DataGridColumn;

                    if (i < resultGrid.SelectedCells.Count - 1)
                    {
                        DataGridCellInfo cellN = resultGrid.SelectedCells[i + 1];

                        if (cellN.Column.DisplayIndex > col.DisplayIndex)
                        {
                            bool_new_row = false;
                        }
                        else
                        {
                            bool_new_row = true;
                        }

                    }

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

                        if (bool_new_row)
                        {
                            copy_data += str_cls + "\r\n";
                        }
                        else
                        {
                            copy_data += str_cls + "\t";
                        }
                    }

                pomin_null:;

                }

                copy_data = copy_data.TrimEnd('\t');
                copy_data = copy_data.TrimEnd(Environment.NewLine.ToCharArray());

                Clipboard.SetDataObject(copy_data);
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

                bool bool_new_row = false;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {
                    string value = "";
                    DataGridCellInfo cell = resultGrid.SelectedCells[i];

                    var col = cell.Column as DataGridColumn;

                    if (i < resultGrid.SelectedCells.Count - 1)
                    {
                        DataGridCellInfo cellN = resultGrid.SelectedCells[i + 1];

                        if (cellN.Column.DisplayIndex > col.DisplayIndex)
                        {
                            bool_new_row = false;
                        }
                        else
                        {
                            bool_new_row = true;
                        }

                    }

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

                        if (bool_new_row)
                        {
                            copy_data += str_cls + "\r\n";
                        }
                        else
                        {
                            copy_data += str_cls + "\t";
                        }
                    }

                pomin_null:;

                }

                copy_data = copy_data.TrimEnd('\t');
                copy_data = copy_data.TrimEnd(Environment.NewLine.ToCharArray());

                Clipboard.SetDataObject(copy_data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano oryginalne dane...";
        }

        void ClickCopy(Object sender, RoutedEventArgs args)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {

                string copy_data = "";

                bool bool_new_row = false;

                for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                {

                    string value = "";

                    DataGridCellInfo cell = resultGrid.SelectedCells[i];

                    var col = cell.Column as DataGridColumn;

                    if (i < resultGrid.SelectedCells.Count - 1)
                    {
                        DataGridCellInfo cellN = resultGrid.SelectedCells[i + 1];

                        if (cellN.Column.DisplayIndex > col.DisplayIndex)
                        {
                            bool_new_row = false;
                        }
                        else
                        {
                            bool_new_row = true;
                        }

                    }

                    // var row = cell.Item as DataRowView;

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

                        copy_data += str_cls;
                    }
                    else
                    {
                        string str_cls = value.Trim();
                        str_cls = str_cls.Replace("\nset", " ");
                        str_cls = str_cls.Replace("\n", " ");
                        str_cls = Regex.Replace(str_cls, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);

                        if (bool_new_row)
                        {
                            copy_data += str_cls + "\r\n";
                        }
                        else
                        {
                            copy_data += str_cls + "\t";
                        }

                    }

                pomin_null:;

                }

                copy_data = copy_data.TrimEnd('\t');
                copy_data = copy_data.TrimEnd(Environment.NewLine.ToCharArray());

                Clipboard.SetDataObject(copy_data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd !!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Skopiowano dane bez spacji...";
        }

        private void Columns_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (((FrameworkElement)sender).Tag.ToString() == "SELECT")
                {
                    string value = "";

                    for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
                    {
                        DataGridCellInfo cell = resultGrid.SelectedCells[i];
                        if (cell.Item != null)
                        {
                            value += cell.Column.Header.ToString() + ", " + "\r\n";
                        }
                    }


                    LBL_INFO.Content = "Dane tylko z zaznaczonych kolum";
                }
                else
                {
                    string value = "";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Columns_select_option_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {
                DataGridCellInfo cell = resultGrid.SelectedCells[0];

                if (cell.Item != null)
                {
                    tbCustomDataRangeOption.Text = ((System.Windows.FrameworkElement)sender).Tag.ToString().Trim();
                    str_option = tbCustomDataRangeOption.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ColumnsOR_select_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int licz_copy = 0;

                //bool boolWstawEnter = false;

                string valuex = "IF [";

                if (((FrameworkElement)sender).Tag.ToString() == "NEW") valuex = "";

                foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                {
                    var col = item.Column as DataGridColumn;
                    var row = item.Item as DataRowView;

                    if (row != null)
                    {
                        if (((FrameworkElement)sender).Tag.ToString() == "NEW")
                        {
                            valuex += "IF [OPTION(" + '\u0022' + str_option + '\u0022' + "," + '\u0022' + row.Row[col.Header.ToString()].ToString().Trim() + '\u0022' + ")] THEN" + "\r\n" + "    % - warunek  " + "\r\n" + "ENDIF " + "\r\n";
                        }
                        else
                        {
                            valuex += "OPTION(" + '\u0022' + str_option + '\u0022' + "," + '\u0022' + row.Row[col.Header.ToString()].ToString().Trim() + '\u0022' + ")" + ((FrameworkElement)sender).Tag.ToString() + " OR ";
                        }

                        licz_copy++;

                        //if (licz_copy % 2 == 0)
                        //{

                        //    if (boolWstawEnter == false)
                        //    {
                        //        boolWstawEnter = true;
                        //    }
                        //    else
                        //    {
                        //        boolWstawEnter = false;
                                valuex += "\r\n";
                        //    }
                        //}

                    }

                }

                valuex = valuex.TrimEnd(Environment.NewLine.ToCharArray());

                if (((FrameworkElement)sender).Tag.ToString() != "NEW")
                {
                    valuex = valuex.Substring(0, valuex.Length - 4);
                    valuex += "] THEN";
                    valuex += "\r\n";
                    valuex += "    % - warunek  ";
                    valuex += "\r\n";
                    valuex += "ENDIF ";
                }


                Clipboard.SetDataObject(valuex.Substring(0, valuex.Length - 1));


                if (tbCustomDataRangeOption.Text.Trim() == "")
                {
                    if (str_option.Length > 30)
                    {
                        tbCustomDataRangeOption.Text = str_option.Substring(0, 30);
                    }
                    else
                    {
                        tbCustomDataRangeOption.Text = str_option;
                    }
                }

                LBL_INFO.Content = "Skopiowano: " + licz_copy.ToString() + " -> dane do skryptu OR...";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ColumnsAND_select_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int licz_copy = 0;

                if (((FrameworkElement)sender).Tag.ToString() == "SETOPTION")
                {
                    string valuex = "";

                    ArrayList arrayList_SET = new ArrayList();
                    ArrayList arrayList_GET = new ArrayList();
                    ArrayList arrayList_GET_Indeks = new ArrayList();

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {
                        var col = item.Column as DataGridColumn;

                        if (arrayList_GET_Indeks.Count == 0)
                        {
                            arrayList_GET_Indeks.Add(col.DisplayIndex + ":" + col.Header.ToString());
                        }
                        else
                        {
                            if (arrayList_GET_Indeks.Contains(col.DisplayIndex + ":" + col.Header.ToString()) == false) arrayList_GET_Indeks.Add(col.DisplayIndex + ":" + col.Header.ToString());
                        }
                    }

                    arrayList_GET_Indeks.Sort();

                    //  ((System.Data.DataRowView)item.Item).Row.ItemArray

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {

                        var row = item.Item as DataRowView;

                        if (row.Row.ItemArray.Length > 0)
                        {
                            string str_col_name = arrayList_GET_Indeks[0].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);

                            arrayList_SET.Add(row.Row[str_col_name].ToString().Trim());
                        }

                    }

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {

                        var row = item.Item as DataRowView;

                        if (arrayList_GET_Indeks.Count > 1)
                        {
                            string str_col_name = arrayList_GET_Indeks[1].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);
                            arrayList_GET.Add(row.Row[str_col_name]);
                        }
                        else if (row.Row.ItemArray.Length == 1)
                        {
                            string str_col_name = arrayList_GET_Indeks[0].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);
                            arrayList_GET.Add(row.Row[str_col_name]);
                        }
                        else
                        {
                            LBL_INFO.Content = "Brak danych aby poprawnie wygenerować skrypt SETOPTION...";
                            return;
                        }

                    }

                    if (arrayList_SET.Count != arrayList_GET.Count)
                    {
                        LBL_INFO.Content = "Wybierz poprawne dane nie wygenerowano skryptu SETOPTION...";
                        return;
                    }

                    for (int i = 0; i < arrayList_SET.Count; i += 2)
                    {
                        valuex += "SETOPTION(" + '\u0022' + arrayList_SET[i].ToString().TrimEnd() + '\u0022' + "," + '\u0022' + arrayList_GET[i].ToString().TrimEnd() + '\u0022' + ");"
                            + "\r\n";
                    }

                    Clipboard.SetDataObject(valuex.TrimEnd(Environment.NewLine.ToCharArray()));

                    LBL_INFO.Content = "Skopiowano: " + licz_copy.ToString() + " -> dane do skryptu SETOPTION...";

                    return;

                }
                else if (((FrameworkElement)sender).Tag.ToString() == "IF" || ((FrameworkElement)sender).Tag.ToString() == "IFTYLKO")
                {

                    string valuex = "";

                    ArrayList arrayList_SET = new ArrayList();
                    ArrayList arrayList_GET = new ArrayList();
                    ArrayList arrayList_GET_Indeks = new ArrayList();

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {
                        var col = item.Column as DataGridColumn;

                        if (arrayList_GET_Indeks.Count == 0)
                        {
                            arrayList_GET_Indeks.Add(col.DisplayIndex + ":" + col.Header.ToString());
                        }
                        else
                        {
                            if (arrayList_GET_Indeks.Contains(col.DisplayIndex + ":" + col.Header.ToString()) == false) arrayList_GET_Indeks.Add(col.DisplayIndex + ":" + col.Header.ToString());
                        }
                    }

                    arrayList_GET_Indeks.Sort();

                    //  ((System.Data.DataRowView)item.Item).Row.ItemArray

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {

                        var row = item.Item as DataRowView;

                        if (row.Row.ItemArray.Length > 0)
                        {
                            string str_col_name = arrayList_GET_Indeks[0].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);

                            arrayList_SET.Add(row.Row[str_col_name].ToString().Trim());
                        }

                    }

                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {

                        var row = item.Item as DataRowView;

                        if (arrayList_GET_Indeks.Count > 1)
                        {
                            string str_col_name = arrayList_GET_Indeks[1].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);
                            arrayList_GET.Add(row.Row[str_col_name]);
                        }
                        else if (row.Row.ItemArray.Length == 1)
                        {
                            string str_col_name = arrayList_GET_Indeks[0].ToString().Substring(arrayList_GET_Indeks[0].ToString().IndexOf(":") + 1);
                            arrayList_GET.Add(row.Row[str_col_name]);
                        }
                        else
                        {
                            LBL_INFO.Content = "Brak danych aby poprawnie wygenerować skrypt SETOPTION...";
                            return;
                        }

                    }

                    if (arrayList_SET.Count != arrayList_GET.Count)
                    {
                        LBL_INFO.Content = "Wybierz poprawne dane nie wygenerowano skryptu SETOPTION...";
                        return;
                    }

                    for (int i = 0; i < arrayList_SET.Count; i += 2)
                    {
                        if (((FrameworkElement)sender).Tag.ToString() == "IFTYLKO")
                        {
                            valuex += "IF[OPTION(" + '\u0022' + arrayList_SET[i].ToString().TrimEnd() + '\u0022' + "," + '\u0022' + arrayList_GET[i].ToString().TrimEnd() + '\u0022' + ")]THEN" + "\r\n";
                        }
                        else
                        {
                            valuex += "IF[OPTION(" + '\u0022' + arrayList_SET[i].ToString().TrimEnd() + '\u0022' + "," + '\u0022' + arrayList_GET[i].ToString().TrimEnd() + '\u0022' + ")]THEN"
                                     + "\r\n" + "    " + "\r\n" + "ENDIF"
                                     + "\r\n";
                        }
                    }

                    Clipboard.SetDataObject(valuex.TrimEnd(Environment.NewLine.ToCharArray()));

                    LBL_INFO.Content = "Skopiowano: " + licz_copy.ToString() + " -> dane do skryptu IF...";

                    return;

                }
                else
                {
                    bool boolWstawEnter = false;
                    string valuex = "IF [";
                    foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                    {
                        var col = item.Column as DataGridColumn;
                        var row = item.Item as DataRowView;

                        if (row != null)
                        {
                            valuex += "OPTION(" + '\u0022' + str_option + '\u0022' + "," + '\u0022' + row.Row[col.Header.ToString()].ToString().Trim() + '\u0022' + ")"
                                + ((FrameworkElement)sender).Tag.ToString() + " AND ";
                            licz_copy++;

                            //if (licz_copy % 2 == 0)
                            //{

                            //    if (boolWstawEnter == false)
                            //    {
                            //        boolWstawEnter = true;
                            //    }
                            //    else
                            //    {
                            //        boolWstawEnter = false;
                                    valuex += "\r\n";
                            //    }
                            //}
                        }

                    }

                    valuex = valuex.TrimEnd(Environment.NewLine.ToCharArray());

                    valuex = valuex.Substring(0, valuex.Length - 5);
                    valuex += "] THEN";
                    valuex += "\r\n";
                    valuex += "    % - warunek  ";
                    valuex += "\r\n";
                    valuex += "ENDIF ";

                    Clipboard.SetDataObject(valuex.Substring(0, valuex.Length - 1));

                }

                if (tbCustomDataRangeOption.Text.Trim() == "")
                {
                    if (str_option.Length > 50)
                    {
                        tbCustomDataRangeOption.Text = str_option.Substring(0, 50);
                    }
                    else
                    {
                        tbCustomDataRangeOption.Text = str_option;
                    }

                    LBL_INFO.Content = "Skopiowano: " + licz_copy.ToString() + " -> dane do skryptu AND...";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void text_changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CB_FILTR.Text != "")
                {
                    TXT_FILTR.Background = System.Windows.Media.Brushes.White;
                    TXT_FILTR_COPY.Text = "";

                    try
                    {
                        if (dv.Table.Columns.Contains(CB_FILTR.Text) == false)
                        {
                            CB_FILTR.Text = "";
                            LBL_INFO_2.Content = "Błąd - spróbuj jeszcze raz...";
                            return;
                        }

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
                            if (Notlike.IsChecked == true)
                            {
                                dv.RowFilter = CB_FILTR.Text.Trim() + " not like '%" + TXT_FILTR.Text.Trim() + "%'";
                            }
                            if (Notlike2.IsChecked == true)
                            {
                                dv.RowFilter = CB_FILTR.Text.Trim() + " like '" + TXT_FILTR.Text.Trim() + "'";
                            }
                            if (ruwnum.IsChecked == true)
                            {
                                var col_dt = ((DataColumn)((Selector)CB_FILTR).SelectedValue).DataType;

                                string zna = "'";

                                switch (col_dt.ToString())
                                {
                                    case "System.String":
                                        TXT_FILTR.Text = Convert.ToString(TXT_FILTR.Text).ToString();
                                        break;
                                    case "System.DateTime":
                                        if (TXT_FILTR.Text.Length < 10) return;
                                        TXT_FILTR.Text = Convert.ToDateTime(TXT_FILTR.Text).ToString();
                                        break;
                                    case "System.Guid":
                                        TXT_FILTR.Text = Convert.ToString(TXT_FILTR.Text).ToString();
                                        break;
                                    default:
                                        zna = "";
                                        TXT_FILTR.Text = Convert.ToDouble(TXT_FILTR.Text).ToString();
                                        break;
                                }

                                dv.RowFilter = CB_FILTR.Text + " = " + zna + TXT_FILTR.Text.Trim() + zna;
                            }
                        }

                        resultGrid.ItemsSource = dv;

                        Title = string.Format("Dane z {0} z {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + dv.Count + " z " + dbl_row_count + "]";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Popraw filtr", MessageBoxButton.OK, MessageBoxImage.Error);
                        TXT_FILTR.Text = "";
                        ruwnum.IsChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CB_FILTR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((DataColumn)((Selector)sender).SelectedValue == null) return;

                var col_dt = ((DataColumn)((Selector)sender).SelectedValue).DataType;

                switch (col_dt.ToString())
                {
                    case "System.String":
                        ruwnum.IsEnabled = true;
                        like.IsEnabled = true;
                        like.IsChecked = true;
                        Notlike.IsEnabled = like.IsEnabled;
                        Notlike2.IsEnabled = like.IsEnabled;
                        break;
                    case "System.Guid":
                        ruwnum.IsChecked = true;
                        ruwnum.IsEnabled = true;
                        like.IsEnabled = false;
                        Notlike.IsEnabled = like.IsEnabled;
                        Notlike2.IsEnabled = like.IsEnabled;
                        break;
                    default:
                        ruwnum.IsChecked = true;
                        ruwnum.IsEnabled = true;
                        like.IsEnabled = false;
                        Notlike.IsEnabled = like.IsEnabled;
                        Notlike2.IsEnabled = like.IsEnabled;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void selectde_cells(object sender, SelectedCellsChangedEventArgs e)
        {
            LBL_INFO_2.Content = "Ilość zaznaczonych komórek: " + resultGrid.SelectedCells.Count.ToString();
        }

        private void mouse_down(object sender, MouseButtonEventArgs e)
        {
            LBL_INFO_2.Content = "Ilość zaznaczonych komórek: " + resultGrid.SelectedCells.Count.ToString();
        }

        private void zmien_nazwe_skrypt_porown(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)sender).Tag.ToString() == "NAZ_TAB")
                {
                    Nazwa_Tabeli = tbCustomDataRange.Text.Trim();
                }
                else
                {
                    str_option = tbCustomDataRangeOption.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cont_menu_opening(object sender, ContextMenuEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            try
            {
                DataGridCellInfo cell = resultGrid.SelectedCells[0];

                if (cell.Item != null)
                {
                    MI_DOWOLNY_OPTION_AUTO_K.Header = "Ustaw wartość OPTION na: " + '\u0022' + cell.Column.Header.ToString().Trim() + '\u0022';
                    MI_DOWOLNY_OPTION_AUTO_K.Tag = cell.Column.Header.ToString().Trim();

                    var str_txt = (TextBlock)cell.Column.GetCellContent(cell.Item);

                    if (str_txt != null)
                    {
                        if (((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim().Length > 25)
                        {
                            MI_DOWOLNY_OPTION_AUTO_W.Header = "Ustaw wartość OPTION na: " + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim().Substring(0, 25).TrimEnd() + '\u0022';
                            MI_DOWOLNY_OPTION_AUTO_W.Tag = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim().Substring(0, 25).TrimEnd();
                        }
                        else
                        {
                            MI_DOWOLNY_OPTION_AUTO_W.Header = "Ustaw wartość OPTION na: " + '\u0022' + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + '\u0022';
                            MI_DOWOLNY_OPTION_AUTO_W.Tag = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim();
                        }

                        var col_t = dv.Table.Columns[cell.Column.Header.ToString()];

                        if (col_t.DataType == typeof(string))
                        {
                            MI_UPDATE_WIDOCZNE_AND.Header = "UPDATE widoczne WHERE " + cell.Column.Header.ToString().Trim()
                                + " = '" + (str_txt.Text.Length > 25 ? str_txt.Text.Substring(0, 25) + "..." : str_txt.Text.TrimEnd()) + "' AND [" + resultGrid.SelectedCells.Count + "]";
                            MI_UPDATE_WIDOCZNE_OR.Header = "UPDATE widoczne WHERE " + cell.Column.Header.ToString().Trim() + " = '"
                                + (str_txt.Text.Length > 25 ? str_txt.Text.Substring(0, 25) + "..." : str_txt.Text.TrimEnd()) + "' OR [" + resultGrid.SelectedCells.Count + "]";
                        }
                        else
                        {
                            MI_UPDATE_WIDOCZNE_AND.Header = "UPDATE widoczne WHERE " + cell.Column.Header.ToString().Trim() + " = "
                                + (str_txt.Text.Length > 25 ? str_txt.Text.Substring(0, 25) + "..." : str_txt.Text.TrimEnd()) + " AND [" + resultGrid.SelectedCells.Count + "]";
                            MI_UPDATE_WIDOCZNE_OR.Header = "UPDATE widoczne WHERE " + cell.Column.Header.ToString().Trim() + " = "
                                + (str_txt.Text.Length > 25 ? str_txt.Text.Substring(0, 25) + "..." : str_txt.Text.TrimEnd()) + " OR [" + resultGrid.SelectedCells.Count + "]";
                        }

                    }

                    if (tbCustomDataRangeOption.Text.Trim() == "")
                    {
                        tbCustomDataRangeOption.Text = MI_DOWOLNY_OPTION_AUTO_W.Tag.ToString().Trim();
                        str_option = tbCustomDataRangeOption.Text;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void click_seterror(object sender, RoutedEventArgs e)
        {
            DataGridCellInfo cell = resultGrid.SelectedCells[0];
            if (cell.Item != null)
            {
                if (resultGrid.SelectedCells.Count == 1)
                {
                    Clipboard.SetDataObject("SETERROREX([" + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + "],\u0022NE\u0022);%");
                    LBL_INFO_2.Content = "Ustawiono wartość SETERROREX";
                }
                else if (resultGrid.SelectedCells.Count == 2)
                {
                    DataGridCellInfo cell2 = resultGrid.SelectedCells[1];
                    Clipboard.SetDataObject("SETERROREX([" + ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.Trim() + "],\u0022NE\u0022);%" +
                        ((TextBlock)cell2.Column.GetCellContent(cell2.Item)).Text.Trim());
                    LBL_INFO_2.Content = "Ustawiono wartość SETERROR";
                }
            }
        }

        private void generuj_insert_click(object sender, RoutedEventArgs e)
        {

            try
            {

                StringBuilder sb = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();

                string[] columnNames = dv.Table.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                string strc_const = "INSERT INTO " + Nazwa_Tabeli + " (" + sb.ToString().Substring(0, sb.ToString().Length - 2) + ") VALUES ";

                foreach (DataRowView row in dv)
                {
                    string[] fields = row.Row.ItemArray.Select(field => field.ToString().TrimEnd()).
                                        ToArray();

                    string valmid = "";

                    for (int i = 0; i < fields.Length; i++)
                    {

                        if (DBNull.Value.Equals(row.Row[i]))
                        {
                            valmid += "NULL, ";
                        }
                        else
                        {
                            if (dv.Table.Columns[i].DataType.ToString() == "System.String")
                            {
                                valmid += "'" + fields[i].ToString().TrimEnd() + "', ";
                            }
                            else if (dv.Table.Columns[i].DataType.ToString() == "System.Int32" || dv.Table.Columns[i].DataType.ToString() == "System.Decimal")
                            {
                                valmid += "" + fields[i].ToString().Replace(",", ".") + ", ";
                            }
                            else if (dv.Table.Columns[i].DataType.ToString() == "System.DateTime")
                            {
                                valmid += "" + "CAST('" + fields[i].ToString().TrimEnd() + "' AS DateTime), ";
                            }
                            else if (dv.Table.Columns[i].DataType.ToString() == "System.Guid")
                            {
                                if (dv.Table.Columns[i].ColumnName.Trim().ToLower() == "rowid")
                                {
                                    valmid += "'" + Guid.NewGuid() + "', ";
                                }
                                else
                                {
                                    valmid += "'" + fields[i].ToString() + "', ";
                                }
                            }
                            else
                            {
                                valmid += "'" + fields[i].ToString() + "', ";
                            }
                        }

                    }
                    sbVal.AppendLine(strc_const + "(" + valmid.TrimEnd().TrimEnd(',') + ");");
                    // sbVal.AppendLine(strc_const + "('" + string.Join("','", fields) + "');"); 
                }

                Window window = new Window
                {
                    Title = "Nowe zaptanie SQL",
                    Content = new MainWindowSQL()
                };
                ((MainWindowSQL)window.Content).B_EXIT.Visibility = Visibility.Collapsed;
                ((MainWindowSQL)window.Content).B_ZAPISZ_DO_BAZY.Visibility = Visibility.Collapsed;

                ((MainWindowSQL)window.Content).txtCode.Text = sbVal.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                ((MainWindowSQL)window.Content).txtCode.IsReadOnly = false;

                window.Show();
            }
            catch (Exception ex)
            {
                if (System.Environment.UserName == "Leszek Mularski")
                {
                    MessageBox.Show(ex.StackTrace, "Sprawdź nazwę skoroszytu, lub nazwy komórek", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Sprawdź nazwę skoroszytu, lub nazwy komórek", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void FullTabele_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((System.Windows.Controls.Primitives.Selector)sender).SelectedIndex == -1) return;
            if (ds.Tables.Count == 0) return;

            string tablName = ((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString();

            resultGrid.ItemsSource = ds.Tables[tablName].DefaultView;

            Nazwa_Tabeli = "";

            str_like = null;

            if (like == null)
            {
                str_option = "";
            }

            if (tbCustomDataRangeOption.Text.Trim() == "")
            {
                if (str_option.Length > 30)
                {
                    tbCustomDataRangeOption.Text = str_option.Substring(0, 30);
                }
                else
                {
                    tbCustomDataRangeOption.Text = str_option;
                }
            }

            LBL_INFO.Content = "Załadowane dane - [" + Nazwa_Tabeli + "]";

            CB_FILTR.ItemsSource = null;

            CB_FILTR.ItemsSource = ds.Tables[tablName].Columns;
            CB_FILTR.DisplayMemberPath = "ColumnName";

            if (Nazwa_Tabeli.Length > 50)
            {
                tbCustomDataRange.Text = Nazwa_Tabeli.Substring(0, 50);
            }
            else
            {
                tbCustomDataRange.Text = Nazwa_Tabeli;
            }

            Title = string.Format("Dane z {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + ds.Tables[tablName].Rows.Count + "]";

            resultGrid.IsReadOnly = true;
        }

        private void mouse_double_click(object sender, MouseButtonEventArgs e)
        {
            resultGrid.IsReadOnly = false;
        }

        private void B_INTELIGENTNE_WKLEJ_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).Tag.ToString() == "=")
            {
                ruwnum.IsChecked = true;
            }
            else if (ruwnum.IsChecked == true)
            {
                like.IsChecked = true;
            }

            var clipData = Clipboard.GetData(DataFormats.Text);

            if (clipData == null)
            {
                CB_FILTR.Text = "";
                LBL_INFO_2.Content = "Błąd - spróbuj jeszcze raz...";
                TXT_FILTR_COPY.Text = "";
                return;
            }

            string[] stringSeparators = new string[] { "\r\n" };
            string[] strdata = clipData.ToString().Split(stringSeparators, StringSplitOptions.None);

            if (clipData != null)
            {
                if (dv.Table.Columns.Contains(CB_FILTR.Text) == false)
                {
                    CB_FILTR.Text = "";
                    LBL_INFO_2.Content = "Błąd - spróbuj jeszcze raz...";
                    TXT_FILTR_COPY.Text = "";
                    return;
                }

                string strF = "";

                if (like.IsChecked == true || Notlike.IsChecked == true || Notlike2.IsChecked == true)
                {
                    string strlikeStart = " like '%";
                    string strlikeStop = "%' OR ";

                    if (Notlike.IsChecked == true)
                    {
                        strlikeStart = " not like '%";
                    }

                    if (Notlike2.IsChecked == true)
                    {
                        strlikeStart = " not like '";
                        strlikeStop = "' OR ";
                    }

                    for (int i = 0; i < strdata.Length - 1; i++)
                    {
                        if (strdata[i].ToString().Trim() != "")
                            strF += CB_FILTR.Text.Trim() + strlikeStart + strdata[i].Trim() + strlikeStop;
                    }

                }

                if (ruwnum.IsChecked == true)
                {
                    var col_dt = ((DataColumn)((Selector)CB_FILTR).SelectedValue).DataType;

                    switch (col_dt.ToString())
                    {
                        case "System.String":

                            for (int i = 0; i < strdata.Length - 1; i++)
                            {
                                strF += CB_FILTR.Text.Trim() + " = '" + strdata[i].Trim() + "' OR ";
                            }

                            break;
                        case "System.DateTime":

                            if (strdata.Length < 10) break;

                            for (int i = 0; i < strdata.Length - 1; i++)
                            {
                                strF += CB_FILTR.Text.Trim() + " = '" + Convert.ToDateTime(strdata[i]).ToString() + "' OR ";
                            }

                            //strF = strF.Substring(0, strF.Length - 5);

                            break;
                        default:
                            for (int i = 0; i < strdata.Length - 1; i++)
                            {
                                strF += CB_FILTR.Text.Trim() + " = '" + Convert.ToDouble(strdata[i]).ToString().Trim() + "' OR ";
                            }
                            break;
                    }

                }

                strF = strF.Substring(0, strF.Length - 4);

                TXT_FILTR_COPY.Text = strF;

                try
                {
                    dv.RowFilter = strF;

                    TXT_FILTR.Background = System.Windows.Media.Brushes.Yellow;

                    LBL_INFO.Content = "Załadowano dane - inteligentny filtr.... :)";

                    resultGrid.ItemsSource = dv;

                    Title = string.Format("Dane filtr z {0} z {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + dv.Count + " z " + dbl_row_count + "]";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Popraw filtr", MessageBoxButton.OK, MessageBoxImage.Error);
                    TXT_FILTR.Text = "";
                    ruwnum.IsChecked = true;
                }

            }
        }

        private void resultGrid_Update_Widoczne_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataGridCellInfo cell = new DataGridCellInfo();

                if (resultGrid.SelectedCells.Count > 1)
                {
                    cell = resultGrid.SelectedCells[resultGrid.SelectedCells.Count - 1];
                }
                else
                {
                    cell = resultGrid.SelectedCells[0];
                }

                if (cell == null) return;

                var str_txt = (TextBlock)cell.Column.GetCellContent(cell.Item);

                if (str_txt == null) return;

                string str_where = "";

                bool boolWW = false;
                if (((FrameworkElement)sender).Tag.ToString() == "DELETE")
                {
                    boolWW = true;
                }

                string str_col_name = "";

                string strAndOr = ((FrameworkElement)e.OriginalSource).Tag.ToString();

                if (strAndOr == "") { boolWW = true; strAndOr = "OR"; };

                DataView dvU = new DataView(dv.Table);

                foreach (DataGridCellInfo item in resultGrid.SelectedCells)
                {
                    var col = item.Column as DataGridColumn;
                    var row = item.Item as DataRowView;

                    if (row != null)
                    {
                        if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.String")
                        {
                            str_where += col.Header.ToString() + " = '" + row.Row[col.Header.ToString()].ToString().Trim() + "' " + strAndOr + " ";
                        }
                        else if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.Int32" || dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.Decimal")
                        {
                            str_where += col.Header.ToString() + " = " + row.Row[col.Header.ToString()].ToString().Replace(",", ".") + " " + strAndOr + " ";
                        }
                        else if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.DateTime")
                        {
                            str_where += col.Header.ToString() + " = " + "CAST('" + row.Row[col.Header.ToString()].ToString() + "' AS DateTime) " + strAndOr + " ";
                        }
                        else
                        {
                            str_where += col.Header.ToString() + " = '" + row.Row[col.Header.ToString()].ToString().TrimEnd() + "' " + strAndOr + " ";
                        }

                        str_col_name = col.Header.ToString();
                    }
                }

                str_where = str_where.Substring(0, str_where.Length - strAndOr.Length - 2);

                if (str_where.IndexOf("CAST") > 0)
                {
                    dvU.RowFilter = str_where.Replace("CAST(", "").Replace("AS DateTime)", "");
                }
                else
                {
                    if (((FrameworkElement)sender).Tag.ToString() == "DELETE")
                    {
                        dvU.RowFilter = str_where.Replace("DELETE", "OR");
                    }
                    else
                    {
                        dvU.RowFilter = str_where;
                    }

                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sbVal = new StringBuilder();

                string[] columnNames = dv.Table.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).ToArray();

                //   sb.AppendLine(string.Join(",", columnNames));

                string strc_const = "UPDATE " + Nazwa_Tabeli.Trim() + " SET ";

                foreach (DataRowView row in dvU)
                {

                    string[] fields = row.Row.ItemArray.Select(field => field.ToString().TrimEnd()).ToArray();

                    string valmid = "";

                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (cell.Column.Header.ToString().Trim() != dvU.Table.Columns[i].ColumnName)
                        {

                            if (DBNull.Value.Equals(row.Row[i]))
                            {
                                valmid += dvU.Table.Columns[i].ColumnName + " = NULL, ";
                            }
                            else
                            {
                                if (dvU.Table.Columns[i].DataType.ToString() == "System.String")
                                {
                                    valmid += dvU.Table.Columns[i].ColumnName + " = '" + fields[i].ToString().Trim() + "', ";
                                }
                                else if (dvU.Table.Columns[i].DataType.ToString() == "System.Int32" || dvU.Table.Columns[i].DataType.ToString() == "System.Decimal")
                                {
                                    valmid += dvU.Table.Columns[i].ColumnName + " = " + fields[i].ToString().Replace(",", ".") + ", ";
                                }
                                else if (dvU.Table.Columns[i].DataType.ToString() == "System.DateTime")
                                {
                                    valmid += dvU.Table.Columns[i].ColumnName + " = " + "CAST('" + fields[i].ToString() + "' AS DateTime), ";
                                }
                                else
                                {
                                    valmid += dvU.Table.Columns[i].ColumnName + " = '" + fields[i].ToString().TrimEnd() + "', ";
                                }
                            }
                        }

                    }

                    if (boolWW == true)
                    {
                        if (((FrameworkElement)sender).Tag.ToString() == "DELETE")
                        {
                            sbVal.AppendLine("DELETE " + Nazwa_Tabeli.Trim() + " WHERE " + str_col_name + " = '" + row.Row[str_col_name].ToString().TrimEnd() + "';");
                        }
                        else
                        {
                            if (row.Row.Table.Columns[str_col_name].DataType.ToString() == "System.String")
                            {
                                sbVal.AppendLine(strc_const + "" + valmid.TrimEnd().TrimEnd(',') + " WHERE " + str_col_name + " = '" + row.Row[str_col_name].ToString().TrimEnd() + "';");
                            }
                            else if (row.Row.Table.Columns[str_col_name].DataType.ToString() == "System.Int32" || row.Row.Table.Columns[str_col_name].DataType.ToString() == "System.Decimal")
                            {
                                sbVal.AppendLine(strc_const + "" + valmid.TrimEnd().TrimEnd(',') + " WHERE " + str_col_name + " = " + row.Row[str_col_name].ToString().TrimEnd().Replace(",", ".") + ";");
                            }
                            else if (row.Row.Table.Columns[str_col_name].DataType.ToString() == "System.DateTime")
                            {
                                sbVal.AppendLine(strc_const + "" + valmid.TrimEnd().TrimEnd(',') + " WHERE " + str_col_name + " = CAST('" + row.Row[str_col_name].ToString().TrimEnd() + "' AS DateTime);");
                            }
                            else
                            {
                                sbVal.AppendLine(strc_const + "" + valmid.TrimEnd().TrimEnd(',') + " WHERE " + str_col_name + " = '" + row.Row[str_col_name].ToString().TrimEnd() + "';");
                            }
                        }

                    }

                    if (boolWW == false)
                        sbVal.AppendLine(strc_const + "" + valmid.TrimEnd().TrimEnd(',') + " WHERE " + str_where.Trim() + ";");

                    // sbVal.AppendLine(strc_const + "('" + string.Join("','", fields) + "');"); 
                }

                Window window = new Window
                {
                    Title = "Nowe zaptanie SQL",
                    Content = new MainWindowSQL()
                };
                ((MainWindowSQL)window.Content).B_EXIT.Visibility = Visibility.Collapsed;
                ((MainWindowSQL)window.Content).B_ZAPISZ_DO_BAZY.Visibility = Visibility.Collapsed;

                ((MainWindowSQL)window.Content).txtCode.Text = sbVal.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                ((MainWindowSQL)window.Content).txtCode.IsReadOnly = false;

                window.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void resultGrid_szybki_filtr_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            string strVal = "";
            string strCol = "";

            try
            {

                DataGridCellInfo cell = resultGrid.SelectedCells[0];
                strVal = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text.ToString().Trim();
                strCol = cell.Column.Header.ToString();

                if (cell.Item != null)
                {
                    var items = CB_FILTR.Items as IList;

                    if (items == null)
                    {
                        MessageBox.Show("Nie znaleziono kolumny!", "Sprawdź nazwę kolumny!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    var item = items.Cast<DataColumn>().SingleOrDefault(i => i.ColumnName == strCol);

                    if (item != null)
                    {
                        CB_FILTR.FindName(item.ColumnName);
                        CB_FILTR.Text = item.ColumnName;
                    }
                    else
                    {
                        MessageBox.Show("Wybierz kolumnę!", "Sprawdź nazwę kolumny!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                    string str_where = "";

                    foreach (DataGridCellInfo itemx in resultGrid.SelectedCells)
                    {
                        var col = itemx.Column as DataGridColumn;
                        var row = itemx.Item as DataRowView;

                        if (row != null)
                        {
                            if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.String")
                            {
                                str_where += col.Header.ToString() + " = '" + row.Row[col.Header.ToString()].ToString().Trim() + "' OR ";
                            }
                            else if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.Int32" || dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.Decimal")
                            {
                                str_where += col.Header.ToString() + " = " + row.Row[col.Header.ToString()].ToString().Replace(",", ".") + " OR ";
                            }
                            else if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.DateTime")
                            {
                                str_where += col.Header.ToString() + " = " + "CAST('" + row.Row[col.Header.ToString()].ToString() + "' AS DateTime) OR ";
                            }
                            else if (dv.Table.Columns[col.Header.ToString()].DataType.ToString() == "System.Guid")
                            {

                            }
                            else
                            {
                                str_where += col.Header.ToString() + " = '" + row.Row[col.Header.ToString()].ToString().TrimEnd() + "' OR ";
                            }

                        }
                    }

                    str_where = str_where.Substring(0, str_where.Length - 3);


                    if (((FrameworkElement)e.OriginalSource).Tag.ToString() == "SELECT")
                    {
                        TXT_FILTR.Text = "";

                        TXT_FILTR_COPY.Text = str_where;

                        try
                        {
                            dv.RowFilter = str_where;

                            TXT_FILTR.Background = System.Windows.Media.Brushes.Yellow;

                            LBL_INFO.Content = "Załadowano dane - inteligentny filtr.... :)";

                            resultGrid.ItemsSource = dv;

                            Title = string.Format("Dane filtr z {0} z {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()) + " [" + dv.Count + " z " + dbl_row_count + "]";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Popraw filtr", MessageBoxButton.OK, MessageBoxImage.Error);
                            TXT_FILTR.Text = "";
                            ruwnum.IsChecked = true;
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            var col_dt = item.DataType;

                            switch (col_dt.ToString())
                            {
                                case "System.String":
                                    like.IsChecked = true;
                                    TXT_FILTR.Text = Convert.ToString(strVal).ToString();
                                    break;
                                case "System.DateTime":
                                    if (strVal.Length < 10) return;
                                    TXT_FILTR.Text = Convert.ToDateTime(strVal).ToString();
                                    break;
                                case "System.Int32":
                                    ruwnum.IsChecked = true;
                                    TXT_FILTR.Text = Convert.ToDouble(strVal).ToString();
                                    break;
                                case "System.Decimal":
                                    ruwnum.IsChecked = true;
                                    TXT_FILTR.Text = strVal.Replace(".", ",");
                                    break;
                                case "System.Guid":
                                    ruwnum.IsChecked = true;
                                    TXT_FILTR.Text = strVal;
                                    break;
                                default:
                                    like.IsChecked = true;
                                    TXT_FILTR.Text = Convert.ToString(strVal).ToString();
                                    break;
                            }
                        }
                        else
                        {
                            like.IsChecked = true;
                            TXT_FILTR.Text = strVal;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Błąd filtru", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Ustawiono szybki filtr [" + strCol + "->" + strVal + "] ......";
        }

        private void resultGrid_Delete_W_Click(object sender, RoutedEventArgs e)
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
                            copy_data_where += "DELETE FROM " + Nazwa_Tabeli + "  where ";
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

                if (new_window.IsChecked == true && copy_data_where != "")
                {
                    MainWindowSQL sp = new MainWindowSQL();
                    sp.txtCode.Text = copy_data_where;
                    sp.B_EXIT.Visibility = Visibility.Hidden;

                    Window nw = new Window();
                    nw.Title = "Zmiany w tabeli: " + Nazwa_Tabeli;
                    nw.Content = sp;
                    nw.Show();

                    copy_data_where = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LBL_INFO.Content = "Ustawiono warunek DELETE WHERE";
        }

        private void Dodaj_do_boom_Click(object sender, RoutedEventArgs e)
        {

            if (resultGrid.SelectedCells.Count == 0) return;

            string str_d = "";

            foreach (DataGridCellInfo itemx in resultGrid.SelectedCells)
            {
                var col = itemx.Column as DataGridColumn;
                var row = itemx.Item as DataRowView;

                if (row != null)
                {

                    str_d += add_boom.AddBoom(resultGrid, spc, ((FrameworkElement)e.OriginalSource).Tag.ToString(), row.Row[col.Header.ToString()].ToString().Trim());
                }
            }

            Clipboard.SetDataObject(str_d);

        }

        private void Columns_Del_select_Click(object sender, RoutedEventArgs e)
        {

            ArrayList selc_col_name = new ArrayList();

            for (int j = 0; j < resultGrid.SelectedCells.Count; j++)
            {
                if (selc_col_name.Contains(resultGrid.SelectedCells[j].Column.Header.ToString()) == false)
                {
                    selc_col_name.Add(resultGrid.SelectedCells[j].Column.Header.ToString());
                }
            }

            if (((FrameworkElement)e.OriginalSource).Tag.ToString() == "DEL")
            {
                for (int k = 0; k < selc_col_name.Count; k++)
                {
                    dv.Table.Columns.Remove(selc_col_name[k].ToString());
                }
            }
            else if (((FrameworkElement)e.OriginalSource).Tag.ToString() == "")
            {
                int i = 0;

                while (dv.Table.Columns.Count != selc_col_name.Count)
                {

                    string str_kol_poz = dv.Table.Columns[i].ColumnName;

                    for (int k = 0; k < selc_col_name.Count; k++)
                    {
                        if (str_kol_poz == selc_col_name[k].ToString())
                        {
                            str_kol_poz = "";
                            break;
                        };
                    }

                    if (str_kol_poz != "")
                    {
                        dv.Table.Columns.Remove(str_kol_poz);
                    }
                    else
                    {
                        i++;
                    }

                }

            }

            resultGrid.ItemsSource = null;
            resultGrid.ItemsSource = dv;

            LBL_INFO.Content = "Nie zaznaczone kolumny usunięto...";

        }
    }
}
