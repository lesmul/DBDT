using DBDT.DrzewoProcesu.Directory.Data;
using DBDT.DrzewoProcesu.Directory.ViewModels;
using DBDT.Konfiguracja;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections;
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
        public string wartosc_obiektu;
        ArrayList Arr_szukaj = new ArrayList();

        bool wcisniety_esc = false;
        public UC_PROCES_TREE(string nazwa_ob)
        {
            InitializeComponent();

            nazwa_obiektu = nazwa_ob;

            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10, pole8 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu + "' order by id desc");

            if (dt.Rows.Count == 0)
            {
                this.DataContext = new DirectoryStructureViewModel();
            }
            else
            {
                this.DataContext = new DirectoryStructureViewModel(dt.Rows[0]["pole10"].ToString());
            }

            wartosc_obiektu = dt.Rows[0]["pole8"].ToString();

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

            CB_FIND.Text = "";
   
        }
        private void zastosuj_filtr(object sender, RoutedEventArgs e)
        {
         
            if (CB_FIND.Text.Trim().Length > 2)
            {
             
                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 = '" + nazwa_obiektu + "' order by id desc");

                string folder_find = "";
                if (dt.Rows.Count > 0 )
                    folder_find = dt.Rows[0]["pole10"].ToString();

                var tree = this.FolderView;
                 
                if (tree.SelectedItem != null)
                {
                    if (((DirectoryItemViewModel)tree.SelectedItem).Type == DirectoryItemType.Folder)
                    {
                        folder_find = ((DirectoryItemViewModel)tree.SelectedItem).FullPath;
                    }
      
                }

                if (folder_find == "") return;

                this.DataContext = new DirectoryStructureViewModel(folder_find, CB_FIND.Text);

                foreach (var item in FolderView.Items)
                {
                    if (FolderView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeViewItem)
                        treeViewItem.ExpandSubtree();

                    if (wcisniety_esc == true)
                    {
                        wcisniety_esc = false;
                        goto koniec_na_zadanie;
                    }
                }

                koniec_na_zadanie:
       
                var search = Arr_szukaj.Cast<string>().ToList().Where(p => p.Contains(CB_FIND.Text));

                if (search.Count() == 0)
                {
                    Arr_szukaj.Add(CB_FIND.Text);

                    CB_FIND.ItemsSource = null;
                    CB_FIND.ItemsSource = Arr_szukaj;

                    if (Arr_szukaj.Count > 21)
                    {
                        Arr_szukaj.RemoveAt(0);
                    }
                }

            } 
            else
            {
                B_ODSWIEZ.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
            else if (tree.SelectedItem != null)
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

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Rename_File_OnClick(object sender, RoutedEventArgs e)
        {
            var tree = this.FolderView;
            if (tree == null) return;
            if (tree.SelectedItem == null) return;

            FRM_ZMIEN_NAZ_PLIKU FRM = new FRM_ZMIEN_NAZ_PLIKU(((DirectoryItemViewModel)tree.SelectedItem).FullPath);
  
          if(  FRM.ShowDialog() == true )
            {
                if (CB_FIND.Text.Trim().Length > 0)
                {
                    b_filtr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else
                {
                    B_ODSWIEZ.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                   
            }
        }

        private void Size_Changen(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 450 || e.NewSize.Height < 350)
            {
                MINI_PODGLAD.Visibility = Visibility.Hidden;
            }
            else
            {
                MINI_PODGLAD.Visibility = Visibility.Visible;
            }
        }

        private void PrevKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape) wcisniety_esc = true;
        }
    }
}