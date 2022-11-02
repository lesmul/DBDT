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

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_NOWY_PROJEKT.xaml
    /// </summary>
    public partial class WPF_NOWY_PROJEKT : Window
    {
        public WPF_NOWY_PROJEKT()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
