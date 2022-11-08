using System.Windows;
using System.Data;
using System;

namespace DBDT.SQL.SQL_SELECT
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(DataTable resultTable)
        {
            InitializeComponent();
            resultGrid.ItemsSource = resultTable.DefaultView;
            Title = string.Format("Dane z {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
        }
    }
}
