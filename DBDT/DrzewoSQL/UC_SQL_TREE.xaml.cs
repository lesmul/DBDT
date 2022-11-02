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
          string id_r_click = "-1";
        public UC_SQL_TREE()
        {
            InitializeComponent();

            this.DataContext = new DirectoryStructureViewModelSQL();

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

            this.DataContext = new DirectoryStructureViewModelSQL();

        }
        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)

        {
            string[] id_r_sel = C_Tree_Return_ID.TreeViewItem_Select_Id(sender, e);

            if (id_r_sel[0] == "-1") return;

            MainWindowSQL sp = new MainWindowSQL();

            sp.id_rec = Convert.ToInt32(id_r_sel[0]);
          
            sp.txtCode.Text = id_r_sel[1];
            sp.txtCode.Tag = id_r_sel[0];

            string strfullp = ((DBDT.DrzewoSQL.Directory.ViewModels.DirectoryItemViewModelSQL)((System.Windows.Controls.TreeView)e.Source).SelectedValue).FullPath;

            sp.ToolTip = strfullp;

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            var parentWindow = Window.GetWindow(this.Parent);

            ((MainWindow)parentWindow).Container.Children.Add(new MdiChild { Content = sp, Name = "FindSQLWindow", Title = "SQL " + strfullp + " - " + ((DBDT.MainWindow)parentWindow).ooo++, WindowState = WindowState.Maximized, Width = ((DBDT.MainWindow)parentWindow).SHT_W, Height = ((DBDT.MainWindow)parentWindow).SHT_H });

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

                if (((DirectoryItemViewModelSQL)item).Type == DirectoryItemTypeSQL.File)
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

                        this.DataContext = new DirectoryStructureViewModelSQL();

                    }
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

        private void MenuItem_DEl_OnClick(object sender, RoutedEventArgs e)
        {

            if(id_r_click  == "-1")
            {
                MessageBox.Show("Nie można usuwać folderów", "Uwaga!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var resolut = MessageBox.Show("Czy usunąć zapis z systemu?", "Uwaga!!!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (resolut == MessageBoxResult.No) return;

            _PUBLIC_SqlLite.ZAPISZ_ZMIANY_SQL("DELETE FROM sql_zapytania WHERE id = " + id_r_click);

            this.DataContext = new DirectoryStructureViewModelSQL();

        }

        private void TreeViewItem_MouseBDownClick(object sender, MouseButtonEventArgs e)
        {
            id_r_click = "-1";
        }

        private void TreeViewItem_MouseBRDownClick(object sender, MouseButtonEventArgs e)
        {
            string[] id_r_sel = C_Tree_Return_ID.TreeViewItem_Select_Id(sender, e);
            id_r_click = id_r_sel[0];
        }
    }
}
