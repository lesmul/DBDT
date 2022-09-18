﻿using DBDT.USTAWIENIA_PROGRAMU;
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
            if (TXT_NAZWA_SERWERA.Text.Trim() != "" && TXT_NAZWE_BAZY.Text.Trim() != "")
            { 
                var dr = MessageBox.Show("Czy zapisać zmiany ?", "Uwaga!!!", MessageBoxButton.YesNo);
                if (dr== MessageBoxResult.Yes)
                {

                    _PUBLIC_SqlLite.Existsdb("");
                
                    _PUBLIC_SqlLite.DODAJ_REKORD_PAR_POLACZENIA(TXT_NAZWA_SERWERA.Text.Trim(), TXT_NAZWE_BAZY.Text.Trim());

                }
            }

            Close();

        }
        private void test_click(object sender, RoutedEventArgs e)
        {

        }
    }
}