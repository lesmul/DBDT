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

namespace DBDT.SQL.SQL_SELECT
{
    public partial class ResultWindow : Window
    {

        private static string Nazwa_Tabeli;
        public ResultWindow(DataTable resultTable, string TableName)
        {
            InitializeComponent();
            resultGrid.ItemsSource = resultTable.DefaultView;
            Title = string.Format("Dane z {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            this.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Nazwa_Tabeli = TableName;

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

                    DataGridCellInfo cell = resultGrid.SelectedCells[i];
                    string value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;

                    if (intdindexst == -1)
                    {
                        intdindexst = cell.Column.DisplayIndex;
                    }

                    if (intdindex == -1)
                    {
                        copy_data += " where ";
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
        }

        void resultGrid_Click(object sender, RoutedEventArgs e)
        {
            if (resultGrid.SelectedCells.Count == 0) return;

            string copy_data = "";
            int intdindex = -1;
            int intdindexst = -1;

            for (int i = 0; i < resultGrid.SelectedCells.Count; i++)
            {
                // System.Data.DataRowView F_R = (DataRowView)resultGrid.SelectedCells[i].Item;

                DataGridCellInfo cell = resultGrid.SelectedCells[i];
                string value = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;

                if (intdindexst == -1)
                {
                    intdindexst = cell.Column.DisplayIndex;
                }

                if (intdindex == -1)
                {
                    copy_data += " where ";
                }

                if (intdindex == intdindexst)
                {
                    copy_data += "\r\n";
                }

                intdindex = cell.Column.DisplayIndex;
                Regex rgx2 = new Regex("\t|\\s+");
                string result = rgx2.Replace(value, " ");
                copy_data += cell.Column.Header.ToString() + " = '" + result.Trim() + "'" + "\r\n" + " and ";

            }
            copy_data = copy_data.Substring(0, copy_data.Length - 4);
            Clipboard.SetText(copy_data.Trim());

        }

    }

}
