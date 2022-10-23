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

namespace DBDT.DXF
{
    /// <summary>
    /// Logika interakcji dla klasy W_EDIT_DATA_DXF.xaml
    /// </summary>
    public partial class W_EDIT_DATA_DXF : Window
    {
        public W_EDIT_DATA_DXF()
        {
            InitializeComponent();
        }

        private void Click_OK(object sender, RoutedEventArgs e)
        {
           DialogResult=true;
            this.Close(); 
        }

        private void Loaded_win(object sender, RoutedEventArgs e)
        {
            DG_DXF_OBJ.DataContext = null;
        }
    }
}
