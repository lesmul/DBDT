using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DBDT.DrzewoProcesu
{
    /// <summary>
    /// Logika interakcji dla klasy FRM_ZMIEN_NAZ_PLIKU.xaml
    /// </summary>
    public partial class FRM_ZMIEN_NAZ_PLIKU : Window
    {
        string str_scieszka_pliku  ="";
        string str_typ_pliku = "";
        string str_dir_name = "";
        public FRM_ZMIEN_NAZ_PLIKU(string scieszka_pliku)
        {
            InitializeComponent();

            str_scieszka_pliku = scieszka_pliku;

            if (str_scieszka_pliku == "")
            {
                B_KOPIUJ.IsEnabled = false;
                B_ZMIEN.IsEnabled = false;
                TXT_NOWA_NAZWA.Text = "";
                TXT_ORYGINALNA_NAZWA.Text = "";

                return;
            }

            FileInfo fi = new FileInfo(str_scieszka_pliku);

            if (fi.Exists == false)
            {
                B_KOPIUJ.IsEnabled = false;
                B_ZMIEN.IsEnabled = false;
                TXT_NOWA_NAZWA.Text = "";
                TXT_ORYGINALNA_NAZWA.Text = "";
                del_file.IsEnabled = false;
            }
            else
            {
                TXT_NOWA_NAZWA.Text = fi.Name.ToString();
                TXT_ORYGINALNA_NAZWA.Text = fi.Name.ToString();

                str_typ_pliku = fi.Extension;
                str_dir_name = fi.DirectoryName;

                TXT_INFO.Text = "Plik powstał: " + fi.CreationTime.ToString()  + " rozmiar [" + (fi.Length/1000).ToString() + "KB]" + "\r\n";
                TXT_INFO.Text += "Ostatnia zmiana: " + fi.LastWriteTime.ToString()+ "\r\n";
                TXT_INFO.Text += "Ostatni odczyt: " + fi.LastAccessTime.ToString();
            }

        }

        private void b_zmien(object sender, RoutedEventArgs e)
        {
            FileInfo fi = new FileInfo(str_scieszka_pliku);

            if (fi.Exists == true)
            {

                if (TXT_NOWA_NAZWA.Text.ToLower().EndsWith(str_typ_pliku.ToLower()) == false)
                {
                    TXT_NOWA_NAZWA.Text += str_typ_pliku;
                }

                FileInfo fix = new FileInfo(str_dir_name + "\\" + TXT_NOWA_NAZWA.Text);

                if (fix.Exists == false)
                {

                    try
                    {
                        System.IO.File.Move(str_scieszka_pliku, str_dir_name + "\\" + TXT_NOWA_NAZWA.Text);
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (InvalidCastException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Nie zmieniłem nazwy ponieważ plik o tej nazwie jest już w katalogu", "Uwaga!!!");
                }

            }

        }

        private void b_kopiuj(object sender, RoutedEventArgs e)
        {
            FileInfo fi = new FileInfo(str_scieszka_pliku);

            if (fi.Exists == true)
            {
                if (TXT_NOWA_NAZWA.Text.ToLower().EndsWith(str_typ_pliku.ToLower()) == false)
                {
                    TXT_NOWA_NAZWA.Text += str_typ_pliku;
                }

                FileInfo fix = new FileInfo(str_dir_name + "\\" + TXT_NOWA_NAZWA.Text);
                if (fix.Exists == false)
                {

                    try
                    {
                        System.IO.File.Copy(str_scieszka_pliku, str_dir_name + "\\" + TXT_NOWA_NAZWA.Text);
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (InvalidCastException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                   
                }
                else
                {
                    MessageBox.Show("Nie zmieniłem nazwy ponieważ plik o tej nazwie jest już w katalogu","Uwaga!!!");
                }
            }
        }

        private void b_anuluj(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Del_file_click(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("Czy usunąć plik?", "Uwaga!!!", MessageBoxButton.YesNo);

            if (dr == MessageBoxResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(str_scieszka_pliku);
                    this.DialogResult = true;          
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //str_dir_name
        }
    }
}
