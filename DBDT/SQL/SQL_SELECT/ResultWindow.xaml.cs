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

namespace DBDT.SQL.SQL_SELECT
{
    public partial class ResultWindow : Window
    {
    

        public ResultWindow(DataTable resultTable)
        {
            InitializeComponent();
            resultGrid.ItemsSource = resultTable.DefaultView;
            Title = string.Format("Dane z {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            this.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

        }

        void resultGrid_select_Click(object sender, RoutedEventArgs e)
        {

           if(resultGrid.SelectedCells.Count == 0) return;

           System.Data.DataRowView F_R = (DataRowView)resultGrid.SelectedCells[0].Item;

            string copy_data = "select * from " + F_R.DataView.Table.TableName.ToString() + "  ";
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
                    copy_data += "\r\n";
                }

                intdindex = cell.Column.DisplayIndex;
                copy_data += cell.Column.Header.ToString() + " = '" + value + "'" + "\t" + " and ";

            }
            copy_data = copy_data.Substring(0, copy_data.Length - 4);
            Clipboard.SetText(copy_data.Trim());


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

                if(intdindexst == -1)
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
                copy_data += cell.Column.Header.ToString() + " = '" + value + "'" + "\t" + " and ";
 
            }
            copy_data = copy_data.Substring(0, copy_data.Length - 4);
            Clipboard.SetText(copy_data.Trim());

        }
 
    }

}
