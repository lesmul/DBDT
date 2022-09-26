using DBDT.DXF;
using DBDT.Konfiguracja;
using DBDT.SQL;
using DBDT.USTAWIENIA_PROGRAMU;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace DBDT
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double SHT_W = 1280;
        double SHT_H = 620;
        public MainWindow()
        {
            InitializeComponent();
            _original_title = Title;
            Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();
            Container.MdiChildTitleChanged += Container_MdiChildTitleChanged;

        }

        #region Mdi-like title

        string _original_title;

        void Container_MdiChildTitleChanged(object sender, RoutedEventArgs e)
        {
            if (Container.ActiveMdiChild != null && Container.ActiveMdiChild.WindowState == WindowState.Maximized)
                Title = _original_title + " - [" + Container.ActiveMdiChild.Title + "]";
            else
                Title = _original_title;
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

        int ooo = 1;

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
        /// Handles the Click event of the 'Scroll window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddScrollWindow_Click(object sender, RoutedEventArgs e)
        {
            //StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
            //sp.Children.Add(new TextBlock { Text = "Window with scroll", Margin = new Thickness(5) });
            //sp.Children.Add(new ComboBox { Margin = new Thickness(20), Width = 300 });
            //ScrollViewer sv = new ScrollViewer { Content = sp, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Okno " + ooo++ });

            UWPF_ZAPYTANIE_SQL sp = new UWPF_ZAPYTANIE_SQL();
        
            ScrollViewer sv = new ScrollViewer { Content = sp,HorizontalContentAlignment = HorizontalAlignment.Center, 
                VerticalContentAlignment = VerticalAlignment.Center };
            //Container.Children.Add(new MdiChild { Content = sv, Title = "Zapytanie SQL " + ooo++, WindowState=WindowState.Maximized, Width= SHT_W, Height= SHT_H });
            Container.Children.Add(new MdiChild { Content = sp, Title = "Zapytanie SQL " + ooo++, WindowState = WindowState.Maximized, Width = SHT_W, Height = SHT_H });

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
    }
}
