using DBDT.DrzewoProcesu;
using DBDT.DrzewoSQL;
using DBDT.DXF;
using DBDT.Excel;
using DBDT.Indeksy;
using DBDT.Konfiguracja;
using DBDT.Python;
using DBDT.SQL;
using DBDT.SQL.SQL_SELECT;
using DBDT.USTAWIENIA_PROGRAMU;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.IO;
using OfficeOpenXml;
using Microsoft.Win32;
using DBDT.Excel_EpPlus;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using MenuItem = System.Windows.Controls.MenuItem;
using Window = System.Windows.Window;

namespace DBDT
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public double SHT_W = 1280;
        public double SHT_H = 620;
        public MainWindow()
        {
            InitializeComponent();

            _original_title = Title;
            Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();
            Container.MdiChildTitleChanged += Container_MdiChildTitleChanged;

            LadujIni.Laduj_SQLLite();

            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole8, pole9 FROM ParametryPalaczenia WHERE pole9 LIKE 'TXT_LOKALIZACJA_PLIKOW_%'");

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_1'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_1.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_1.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_2'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_2.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_2.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_3'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_3.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_3.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_4'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_4.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_4.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_5'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_5.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_5.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_6'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_6.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_6.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_7'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_7.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_7.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_8'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_8.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_8.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_9'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_9.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_9.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_10'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_10.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_10.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_11'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_11.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_11.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_12'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_12.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_12.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_13'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_13.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_13.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_14'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_14.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_14.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_15'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_15.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_15.IsEnabled = false;
                }

                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_16'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_16.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_16.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_17'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_17.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_17.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_18'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_18.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_18.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_19'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_19.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_19.IsEnabled = false;
                }
                dv.RowFilter = "pole9='TXT_LOKALIZACJA_PLIKOW_20'";
                if (dv.Count > 0)
                {
                    if (dv[0]["pole8"].ToString() != "") Drzewo_20.Header = dv[0]["pole8"].ToString();
                }
                else
                {
                    Drzewo_20.IsEnabled = false;
                }

            }
        }

        #region Mdi-like title

        string _original_title;

        void Container_MdiChildTitleChanged(object sender, RoutedEventArgs e)
        {
            if (Container.ActiveMdiChild != null && Container.ActiveMdiChild.WindowState == WindowState.Maximized)
            {
                Title = _original_title + " - [" + Container.ActiveMdiChild.Title + "]";

                //mdiChild.buttonsPanel.Children.Add(mdiChild.minimizeButton);
                //mdiChild.buttonsPanel.Children.Add(mdiChild.maximizeButton);
                //mdiChild.buttonsPanel.Children.Add(mdiChild.closeButton);

                ;

            }
            else
            {
                Title = _original_title;
            }
        }

        #endregion

        #region Theme Menu Events

        /// <summary>
        /// Handles the Click event of the Generic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Generic_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = true;
            Luna.IsChecked = false;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Generic;

        }

        /// <summary>
        /// Handles the Click event of the Luna control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Luna_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = true;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Luna;

        }

        /// <summary>
        /// Handles the Click event of the Aero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Aero_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = false;
            Aero.IsChecked = true;

            Container.Theme = ThemeType.Aero;

        }

        #endregion

        #region Menu Events

        public int ooo = 1;

        /// <summary>
        /// Handles the Click event of the 'Normal window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void KonfiguracjaPolaczenia_Click(object sender, RoutedEventArgs e)
        {
            WPF_KONFIG_CONNECTION FRM = new WPF_KONFIG_CONNECTION();
            FRM.ShowDialog();

        }

        /// <summary>
        /// Handles the Click event of the 'Fixed window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddDXFWindow_Click(object sender, RoutedEventArgs e)
        {
            //Container.Children.Add(new MdiChild { Content = new Label { Content = "Fixed width window" }, Title = "Window " + ooo++, Resizable = false });
            UC_RYS_DXF sp = new UC_RYS_DXF();

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Title = "DXF " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });
        }

        /// <summary>
        /// Handles the Click event of the 'Fixed window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void XMLXSLTWindow_Click(object sender, RoutedEventArgs e)
        {
            //Container.Children.Add(new MdiChild { Content = new Label { Content = "Fixed width window" }, Title = "Window " + ooo++, Resizable = false });
            UC_XMLXSLT sp = new UC_XMLXSLT();

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Title = "XSLT - XML" + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });
        }

        /// <summary>
        /// Handles the Click event of the 'Scroll window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void FindSQLWindow_Click(object sender, RoutedEventArgs e)
        {
            //StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
            //sp.Children.Add(new TextBlock { Text = "Window with scroll", Margin = new Thickness(5) });
            //sp.Children.Add(new ComboBox { Margin = new Thickness(20), Width = 300 });
            //ScrollViewer sv = new ScrollViewer { Content = sp, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Okno " + ooo++ });

            for (int i = 0; i <= this.Container.Children.Count - 1; i++)
            {
                MdiChild child = this.Container.Children[i];
                if (child.Name == "FindSQLWindow")
                {
                    child.Focus();
                    return;
                }
            }

            UWPF_ZAPYTANIE_SQL sp = new UWPF_ZAPYTANIE_SQL();

            sp.szer = (int)SHT_W;
            sp.wys = (int)SHT_H;

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Name = "FindSQLWindow", Title = "Zapytanie SQL " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });

        }
        private void AddNewSQLWindow_Click(object sender, RoutedEventArgs e)
        {

            if (((System.Windows.FrameworkElement)e.OriginalSource).Tag.ToString() == "NEW")
            {
                Window window = new Window
                {
                    Title = "Nowe zaptanie SQL",
                    Content = new MainWindowSQL()
                };
                ((MainWindowSQL)window.Content).B_EXIT.Visibility = Visibility.Collapsed;
                ((MainWindowSQL)window.Content).B_ZAPISZ_DO_BAZY.Visibility = Visibility.Collapsed;
                window.Show();

            }
            else
            {
                MainWindowSQL sp = new MainWindowSQL();

                //ScrollViewer sv = new ScrollViewer
                //{
                //    Content = sp,
                //    HorizontalContentAlignment = HorizontalAlignment.Center,
                //    VerticalContentAlignment = VerticalAlignment.Center
                //};
                //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
                Container.Children.Add(new MdiChild
                {
                    Content = sp,
                    Title = "Dodaj nowe zaptanie SQL " + ooo++,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    WindowState = WindowState.Maximized,
                    Width = SHT_W,
                    Height = SHT_H
                });
            }

        }

        private void AddNewEXCEL_Click(object sender, RoutedEventArgs e)
        {

            UWPF_EXCEL_SQL sp = new UWPF_EXCEL_SQL();

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Title = "Dodaj automatyzacja w plikach EXCEL " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });

        }

        /// <summary>
        /// Refresh windows list
        /// </summary>
        void Menu_RefreshWindows()
        {
            WindowsMenu.Items.Clear();
            MenuItem mi;
            for (int i = 0; i < Container.Children.Count; i++)
            {
                MdiChild child = Container.Children[i];
                mi = new MenuItem { Header = child.Title };
                mi.Click += (o, e) => child.Focus();
                WindowsMenu.Items.Add(mi);
            }

            WindowsMenu.Items.Add(new Separator());
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Kaskada" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.Cascade;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Poziomo" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileHorizontal;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Pionowo" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileVertical;

            WindowsMenu.Items.Add(new Separator());
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Zamknij okna" });
            mi.Click += (o, e) => Container.Children.Clear();
        }

        #endregion

        private void loadApp(object sender, RoutedEventArgs e)
        {
            //_PUBLIC_SQL.connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + ScieszkaProgramu + "Database1.mdf;Integrated Security=True";
        }

        private void SizeChan(object sender, SizeChangedEventArgs e)
        {
            SHT_W = e.NewSize.Width - 15;
            SHT_H = e.NewSize.Height - 57;
        }

        private void Window_TreeSQL_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= this.Container.Children.Count - 1; i++)
            {
                MdiChild child = this.Container.Children[i];
                if (child.Name == "FindTreeSQLWindow")
                {
                    child.Focus();
                    return;
                }
            }

            UC_SQL_TREE sp = new UC_SQL_TREE();

            //ScrollViewer sv = new ScrollViewer
            //{
            //    Content = sp,
            //    HorizontalContentAlignment = HorizontalAlignment.Center,
            //    VerticalContentAlignment = VerticalAlignment.Center
            //};

            Container.Children.Add(new MdiChild
            {
                Content = sp,
                Name = "FindTreeSQLWindow",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Title = "Drzewo informacji SQL " + ooo++,
                WindowState = WindowState.Maximized,
                Width = SHT_W,
                Height = SHT_H
            });

        }

        private void Window_Tree_Click(object sender, RoutedEventArgs e)
        {
            string nazwa_obiektu = "TXT_LOKALIZACJA_PLIKOW" + ((FrameworkElement)sender).Tag.ToString();

            if (((FrameworkElement)sender).Tag.ToString() == "WT_ALL")
            {
                nazwa_obiektu = "";
            }

            UC_PROCES_TREE sp = new UC_PROCES_TREE(nazwa_obiektu);

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            Container.Children.Add(new MdiChild { Content = sp, Title = (sp.wartosc_obiektu == "" ? "Mój komputer" : sp.wartosc_obiektu) + " " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });

        }
        private void Lokalizacja_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAutomatEXCEL_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i <= this.Container.Children.Count - 1; i++)
            {
                MdiChild child = this.Container.Children[i];
                if (child.Name == "UC_Konfig")
                {
                    child.Focus();
                    return;
                }
            }

            UC_Konfig sp = new UC_Konfig();

            Container.Children.Add(new MdiChild
            {
                Content = sp,
                Name = "UC_Konfig",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Title = "Plik konfiguracji " + ooo++,
                WindowState = WindowState.Maximized,
                Width = SHT_W,
                Height = SHT_H
            });

        }

        private void ExecAutomatEXCEL_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= this.Container.Children.Count - 1; i++)
            {
                MdiChild child = this.Container.Children[i];
                if (child.Name == "UC_EXEC_DATA")
                {
                    child.Focus();
                    return;
                }
            }

            UC_EXEC_DATA sp = new UC_EXEC_DATA();

            Container.Children.Add(new MdiChild
            {
                Content = sp,
                Name = "UC_EXEC_DATA",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Title = "Plik automatyzacji uzupełnij dane " + ooo++,
                WindowState = WindowState.Maximized,
                Width = SHT_W,
                Height = SHT_H
            });
        }
		
	    private void EXCEL_DOPLATY_DATA_Click(object sender, RoutedEventArgs e)
        {
			WPF_Doplaty_Narz FRM = new WPF_Doplaty_Narz();
			FRM.Show();

		}
		private void EXCEL_SELECT_DATA_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki Excel (*.xlsx)|*.xlsx|Pliki Excel 97-2003 (*.xls)|*.xls";
            openFileDialog.Title = "Wybierz plik Excel";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                DataTable dataTable = new DataTable();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;// LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Załóżmy, że arkusz znajduje się na indeksie 0

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Dodawanie kolumn do DataTable
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[1, col].Value == null)
                        {
                            string columnName = "XKol" + col.ToString();

                            if (dataTable.Columns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName + col.ToString());
                            }
                            else
                            {
                                dataTable.Columns.Add(columnName);
                            }
                        }
                        else
                        {
                            string columnName = worksheet.Cells[1, col].Value.ToString().Trim();

                            if (dataTable.Columns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName + col.ToString());
                            }
                            else
                            {
                                dataTable.Columns.Add(columnName);
                            }
                        }

                    }

                    // Dodawanie danych do DataTable
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                WPF_Excel FRM = new WPF_Excel(dataTable);
                FRM.dt_exec = dataTable;
                FRM.ShowDialog();

            }
        }

        private void EXCEL_COPY_DATA_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki Excel (*.xlsx)|*.xlsx|Pliki Excel 97-2003 (*.xls)|*.xls";
            openFileDialog.Title = "Wybierz plik Excel";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

				DataTable dataTable = new DataTable();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;// LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Załóżmy, że arkusz znajduje się na indeksie 0

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Dodawanie kolumn do DataTable
                    for (int col = 1; col <= colCount; col++)
                    {
                        if(worksheet.Cells[1, col].Value == null)
                        {
                            string columnName = "XKol"+ col.ToString();

                            if (dataTable.Columns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName + col.ToString());
                            }
                            else
                            {
                                dataTable.Columns.Add(columnName);
                            }
                        }
                        else
                        {
                            string columnName = worksheet.Cells[1, col].Value.ToString().Trim();

                            if (dataTable.Columns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName + col.ToString());
                            }
                            else
                            {
                                dataTable.Columns.Add(columnName);
                            }
                        }
                        
                    }

                    // Dodawanie danych do DataTable
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                WPF_ZAPIS_EXCEL_SQL FRM = new WPF_ZAPIS_EXCEL_SQL(dataTable);
                FRM.dt_exec = dataTable;
                FRM.ShowDialog();

            }
        }

        private void EXCEL_RESTR_WYMIAROWE_DATA_Click(object sender, RoutedEventArgs e)
        {
            WPF_EXCEL_RESTR_WYM FRM = new WPF_EXCEL_RESTR_WYM();
            FRM.ShowDialog();
        }

        private void Config_Indeks_Click(object sender, RoutedEventArgs e)
        {
            WPF_TYPY_INDEKSOW FRM = new WPF_TYPY_INDEKSOW();
            FRM.ShowDialog();
        }

        private void close_sql(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void closeng_sql(object sender, CancelEventArgs e)
        {
            Container.Children.Clear();
        }

        private void Python_CMD_Click(object sender, RoutedEventArgs e)
        {
            //CIronPython cIronPython = new CIronPython();
            //string str_x;
            //str_x = cIronPython.run_cmd("test","a=1");
            //MessageBox.Show(str_x, "Test CMD", MessageBoxButton.OK, MessageBoxImage.Information);

            for (int i = 0; i <= this.Container.Children.Count - 1; i++)
            {
                MdiChild child = this.Container.Children[i];
                if (child.Name == "PythonWindow")
                {
                    child.Focus();
                    return;
                }
            }

            UC_AppPython sp = new UC_AppPython();

            //sp.szer = (int)SHT_W;
            //sp.wys = (int)SHT_H;

            ScrollViewer sv = new ScrollViewer
            {
                Content = sp,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Name = "PythonWindow", Title = "Aplikacje napisane w Python " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });


        }
    }
}
