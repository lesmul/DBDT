using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DBDT.Konfiguracja
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_CONNECTION.xaml
    /// </summary>
    public partial class WPF_KONFIG_CONNECTION : Window
    {
        public WPF_KONFIG_CONNECTION()
        {
            InitializeComponent();
        }

        private void zakoncz_click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void test_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
