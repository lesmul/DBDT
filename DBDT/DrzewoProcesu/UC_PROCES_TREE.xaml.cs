using DBDT.DrzewoProcesu.Directory.ViewModels;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace DBDT.DrzewoProcesu
{
    /// <summary>
    /// Logika interakcji dla klasy UC_PROCES_TREE.xaml
    /// </summary>
    public partial class UC_PROCES_TREE : UserControl
    {
        string nazwa_obiektu;
        public UC_PROCES_TREE(string nazwa_ob)
        {
            InitializeComponent();

            nazwa_obiektu = nazwa_ob;

            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu +  "' order by id desc");
      
            if(dt.Rows.Count == 0)
            {
                this.DataContext = new DirectoryStructureViewModel();
            }
            else
            {
                this.DataContext = new DirectoryStructureViewModel(dt.Rows[0]["pole10"].ToString());
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu + "' order by id desc");

            if (dt.Rows.Count == 0)
            {
                this.DataContext = new DirectoryStructureViewModel();
            }
            else
            {
                this.DataContext = new DirectoryStructureViewModel(dt.Rows[0]["pole10"].ToString());
            }
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
                Process.Start(((DirectoryItemViewModel)tree.SelectedItem).FullPath);
            }
        }
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
           var tree = this.FolderView;
           if (tree == null) return;

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
            else if (tree.SelectedItem != null)
            {
                Clipboard.SetDataObject(((DirectoryItemViewModel)tree.SelectedItem).FullPath);
            }
        }
    }
}
