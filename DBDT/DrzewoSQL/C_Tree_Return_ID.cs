using DBDT.DrzewoSQL.Directory.Data;
using DBDT.DrzewoSQL.Directory.ViewModels;
using DBDT.SQL.SQL_SELECT;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using WPF.MDI;
using System.Data;

namespace DBDT.DrzewoSQL
{
    internal class C_Tree_Return_ID
    {
        public static string[] TreeViewItem_Select_Id(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;

            string[] id_r_x =  { "-1", "" };

            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is TreeViewItem)
            {
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as TreeViewItem;
                // this.Title = "Selected header: " + item.Header.ToString();
                MessageBox.Show(item.Header.ToString());
                id_r_x[0] = "-1";
                id_r_x[1] = "";
                return id_r_x;
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                // this.Title = "Selected: " + tree.SelectedItem.ToString();
                MessageBox.Show(tree.SelectedItem.ToString());
                id_r_x[0] = "-1";
                id_r_x[1] = "";
                return id_r_x;
            }
            else if (tree.SelectedItem != null)
            {
                //   DTBD.DrzewoProcesu.Directory.ViewModels.DirectoryItemViewModel)tree.SelectedItem).level1;

                try
                {
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).Type != DirectoryItemTypeSQL.File)
                    {
                        id_r_x[0] = "-1";
                        id_r_x[1] = "";
                        return id_r_x;
                    }

                    DataTable dt = new DataTable();

                    string Xlevel1 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level1;
                    string Xlevel2 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level2;
                    string Xlevel3 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level3;
                    string Xlevel4 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level4;
                    string Xlevel5 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level5;
                    string Xlevel6 = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level6;

                    string stId = ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).idItem.ToString();

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level1 == null) Xlevel1 = "";
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level2 == null) Xlevel2 = "";
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level3 == null) Xlevel3 = "";
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level4 == null) Xlevel4 = "";
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level5 == null) Xlevel5 = "";
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level6 == null) Xlevel6 = "";

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level1 && stId == "1")
                    {
                        Xlevel1 = "";
                    }

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level2 && stId == "2")
                    {
                        Xlevel2 = "";
                    }

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level3 && stId == "3")
                    {
                        Xlevel3 = "";
                    }

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level4 && stId == "4")
                    {
                        Xlevel4 = "";
                    }

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level5 && stId == "5")
                    {
                        Xlevel5 = "";
                    }

                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).FullPath
                        == ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).level6 && stId == "6")
                    {
                        Xlevel6 = "";
                    }

                    dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, sql FROM sql_zapytania " +
                                                    "WHERE sql <> '' and pole1 = '" + Xlevel1.ToString() + "' and " +
                                                    " pole2 = '" + Xlevel2.ToString() + "' and pole3 = '" + Xlevel3.ToString() + "' and " +
                                                    " pole4 = '" + Xlevel4.ToString() + "' and pole5 = '" + Xlevel5.ToString() + "' and pole6 = '" + Xlevel6.ToString() + "'" +
                                                    " and id = " + ((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).idRec);

                    if (dt.Rows.Count == 0)
                    {
                        id_r_x[0] = "-1";
                        id_r_x[1] = "";
                        return id_r_x;
                    }
                    id_r_x[0] = dt.Rows[0]["id"].ToString();
                    id_r_x[1] = dt.Rows[0]["sql"].ToString();
                    return id_r_x;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                    id_r_x[0] = "-1";
                    id_r_x[1] = "";
                    return id_r_x;
                }
            }

            id_r_x[0] = "-1";
            id_r_x[1] = "";
            return id_r_x;
        }
    }
}
