using DBDT.DrzewoProcesu.Directory.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace DBDT.Python
{
    /// <summary>
    /// Logika interakcji dla klasy UC_AppPython.xaml
    /// </summary>
    public partial class UC_AppPython : UserControl
    {
        public UC_AppPython()
        {
            InitializeComponent();
        }

        private void load_uc(object sender, RoutedEventArgs e)
        {

            string ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\Python\";

            DirectoryInfo fl = new DirectoryInfo(ScieszkaProgramu);

            if (fl.Exists == false) return;

            var children = DirectoryStructure.GetDirectoryContents(ScieszkaProgramu, "*.exe");

            PLIKI_EXE.ItemsSource = children;

        }

        private void mouse_dbl_click(object sender, MouseButtonEventArgs e)
        {
            string ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }
            ScieszkaProgramu += @"\Python\";

            DirectoryInfo fl = new DirectoryInfo(ScieszkaProgramu);

            if (fl.Exists == false) return;

            try
            {

                Process.Start(ScieszkaProgramu + ((TextBlock)e.OriginalSource).Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
