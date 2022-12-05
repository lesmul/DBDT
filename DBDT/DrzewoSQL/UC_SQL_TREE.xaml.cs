using DBDT.DrzewoSQL.Directory.Data;
using DBDT.DrzewoSQL.Directory.ViewModels;
using DBDT.SQL.SQL_SELECT;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.MDI;

namespace DBDT.DrzewoSQL
{
    /// <summary>
    /// Logika interakcji dla klasy UC_PROCES_TREE.xaml
    /// </summary>
    public partial class UC_SQL_TREE : UserControl
    {
        string nazwa_obiektu;
        public UC_SQL_TREE(string nazwa_ob)
        {
            InitializeComponent();

            nazwa_obiektu = nazwa_ob;

            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu +  "' order by id desc");
          
            this.DataContext = new DirectoryStructureViewModelSQL();

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu + "' order by id desc");

            this.DataContext = new DirectoryStructureViewModelSQL();
   
        }
        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)

        {
            var tree = sender as TreeView;

            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is TreeViewItem)
            {
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as TreeViewItem;
                // this.Title = "Selected header: " + item.Header.ToString();
                MessageBox.Show(item.Header.ToString());
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                // this.Title = "Selected: " + tree.SelectedItem.ToString();
                MessageBox.Show(tree.SelectedItem.ToString());
            }
            else if (tree.SelectedItem !=null)
            {
                //   DTBD.DrzewoProcesu.Directory.ViewModels.DirectoryItemViewModel)tree.SelectedItem).level1;
               
                try
                {
                    if (((DirectoryItemViewModelSQL)((TreeView)e.Source).SelectedItem).Type != DirectoryItemTypeSQL.File)
                    {
                        return;
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

                    if (dt.Rows.Count == 0) return;

                    MainWindowSQL sp = new MainWindowSQL();

                    sp.txtCode.Text = dt.Rows[0]["sql"].ToString();
                    sp.txtCode.Tag = dt.Rows[0]["id"].ToString();

                    ScrollViewer sv = new ScrollViewer
                    {
                        Content = sp,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };

                    var parentWindow = Window.GetWindow(this.Parent);

                    ((MainWindow)parentWindow).Container.Children.Add(new MdiChild { Content = sp, Name = "FindSQLWindow", Title = "Zapytanie SQL " + ((DBDT.MainWindow)parentWindow).ooo++, WindowState = WindowState.Maximized, Width = ((DBDT.MainWindow)parentWindow).SHT_W, Height = ((DBDT.MainWindow)parentWindow).SHT_H });

                }
                catch { }
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var item = this.FolderView.SelectedItem;
            if (item != null)
            {

                SqlHandler sqlHandler = new SqlHandler();

                string idR = ((DirectoryItemViewModelSQL)item).idItem.ToString();

                string Xlevel1 = ((DirectoryItemViewModelSQL)item).level1;
                string Xlevel2 = ((DirectoryItemViewModelSQL)item).level2;
                string Xlevel3 = ((DirectoryItemViewModelSQL)item).level3;
                string Xlevel4 = ((DirectoryItemViewModelSQL)item).level4;
                string Xlevel5 = ((DirectoryItemViewModelSQL)item).level5;
                string Xlevel6 = ((DirectoryItemViewModelSQL)item).level6;

                if (((DirectoryItemViewModelSQL)item).level1 == null) Xlevel1 = "";
                if (((DirectoryItemViewModelSQL)item).level2 == null) Xlevel2 = "";
                if (((DirectoryItemViewModelSQL)item).level3 == null) Xlevel3 = "";
                if (((DirectoryItemViewModelSQL)item).level4 == null) Xlevel4 = "";
                if (((DirectoryItemViewModelSQL)item).level5 == null) Xlevel5 = "";
                if (((DirectoryItemViewModelSQL)item).level6 == null) Xlevel6 = "";

                if (((DirectoryItemViewModelSQL)item).Type == DirectoryItemTypeSQL.File) 
                { 
                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level1 && idR == "1")
                    {
                        Xlevel1 = "";
                    }

                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level2 && idR == "2")
                    {
                        Xlevel2 = "";
                    }

                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level3 && idR == "3")
                    {
                        Xlevel3 = "";
                    }

                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level4 && idR == "4")
                    {
                        Xlevel4 = "";
                    }

                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level5 && idR == "5")
                    {
                        Xlevel5 = "";
                    }

                    if (((DirectoryItemViewModelSQL)item).FullPath
                        == ((DirectoryItemViewModelSQL)item).level6 && idR == "6")
                    {
                        Xlevel6 = "";
                    }
                }
                string[] opiszw;

                if (((DirectoryItemViewModelSQL)item).Type== DirectoryItemTypeSQL.File)
                {
                    opiszw = sqlHandler.SQL_Title(((DirectoryItemViewModelSQL)item).FullPath,
                    Xlevel1,
                    Xlevel2,
                    Xlevel3,
                    Xlevel4,
                    Xlevel5,
                    Xlevel6);

                    if (opiszw != null)
                    {
                        _PUBLIC_SqlLite.ZMIEN_OPIS_REKORD_SQL_ZAPYTANIA(opiszw[0], opiszw[1], opiszw[2], opiszw[3], opiszw[4], opiszw[5], opiszw[6],
                            ((DirectoryItemViewModelSQL)item).idRec.ToString());

                        DataTable dt = new DataTable();

                        dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu + "' order by id desc");

                        this.DataContext = new DirectoryStructureViewModelSQL();

                    }
                }
          
            }
        }
    }
}
