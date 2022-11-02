using DBDT.USTAWIENIA_PROGRAMU;
using Microsoft.SqlServer.Server;
using Microsoft.Win32;
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
using static DBDT.SQL.WPF_DODAJ_EXCEL;

namespace DBDT.SQL
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_DODAJ_EXCEL.xaml
    /// </summary>
    public partial class WPF_DODAJ_EXCEL : Window
    {
        public WPF_DODAJ_EXCEL()
        {
            InitializeComponent();
        }

        bool BOOL_CZYSC_WSZYSTKO = false;
        public List<XLSX> items = new List<XLSX>();

        public string id_rec = "-1";

        public class XLSX
        {
            public string TextExcel { get; set; }
            public string CalaScieszka { get; set; }
            public string ImagePic { get; set; }
            public System.Windows.Media.SolidColorBrush BackgroundColor { get; set; }

            public XLSX(string text, string txtFullScieszka, string image, System.Windows.Media.SolidColorBrush color)
            {
                TextExcel = text;
                CalaScieszka = txtFullScieszka;
                ImagePic = image;
                BackgroundColor = color;
            }
        }
        private void MI_W_ZMIEN_NAZWE_PLIKU(object sender, RoutedEventArgs e)
        {

        }
        private void MI_ZAPISZ_PLIK(object sender, RoutedEventArgs e)
        {

            if (LV_XLSX.SelectedIndex > -1)
            {
                //if (LV_XLSX.Items.CurrentItem[LV_XLSX.SelectedIndex].TextDXF != "")
                //{
                SaveFileDialog saveFileDialogxls = new SaveFileDialog();

                string[] typ_pliku = items[0].TextExcel.Split('.');
                string rozsz = typ_pliku[typ_pliku.Length-1].ToLower();

                if (rozsz == "xlsx")
                {
                    saveFileDialogxls.Filter = "Excel (*.xlsx)|*.xlsx";
                }
                else if (rozsz == "xls")
                {
                    saveFileDialogxls.Filter = "Excel (*.xls)|*.xls";
                }
                else if (rozsz == "xlsm")
                {
                    saveFileDialogxls.Filter = "Excel z obługą makr (*.xls)|*.xlsm";
                }
                else if (rozsz == "zip")
                {
                    saveFileDialogxls.Filter = "Plik ZIP (*.zip)|*.zip";
                }

                saveFileDialogxls.FileName = items[0].TextExcel;

                //saveFileDialogxls.FilterIndex = 2;
                saveFileDialogxls.RestoreDirectory = true;
                if (saveFileDialogxls.ShowDialog() == true)
                {
                    string str_inf = _PUBLIC_SqlLite.ZAPISZ_DO_PLIKU_XSL(saveFileDialogxls.FileName, saveFileDialogxls.SafeFileName, id_rec);
                }

                //}   
            }

        }
        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (LV_XLSX.ItemsSource == null)
            {
                LV_XLSX.Items.Clear();
            }
            if(id_rec != "-1")
            {
                LV_XLSX.ItemsSource = null;
                items.Clear();
                LV_XLSX.Items.Clear();
                B_Zastap_plik_excel.Visibility= Visibility.Visible;
            }
  
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (BOOL_CZYSC_WSZYSTKO == true)
                {
                    items.Clear();

                    BOOL_CZYSC_WSZYSTKO = false;
                }

                LV_XLSX.ItemsSource = null;

                if (files != null && files.Length > 0)
                {
                    foreach (string itemN in files)
                    {
                        // Debug.Write(item & " ")
                        // sender.Text &= files(0) & vbCrLf

                        string[] REDIMX = itemN.Replace(@"\", "/").Split('/');

                        if (items.Exists(x => x.CalaScieszka == itemN))
                            MessageBox.Show("Plik o nazwie: " + REDIMX[REDIMX.Length - 1] + " jest już na liście", "Uwaga!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                        else if (itemN.ToUpper().EndsWith("XLS") | itemN.ToUpper().EndsWith("XLSX") | itemN.ToUpper().EndsWith("XLSM") | itemN.ToUpper().EndsWith("ZIP"))
                        {
                            if (items.Count > 1)
                            {
                                MessageBox.Show("Osiągnięto maksymalna ilość załączników", "Uwaga!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            else if (itemN.ToUpper().EndsWith(".XLS") || itemN.ToUpper().EndsWith(".XLSX") || itemN.ToUpper().EndsWith(".XLSM"))
                            {
                                XLSX iXLSX = new XLSX(REDIMX[REDIMX.Length - 1], itemN, "/IKONY/excel.ico", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green));
                                items.Add(iXLSX);
                            }
                            else
                            {
                                XLSX iXLSX = new XLSX(REDIMX[REDIMX.Length - 1], itemN, "/IKONY/zip.ico", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green));
                                items.Add(iXLSX);
                            }
                        }
                    }

                    //if (items.Count > 0)
                    //{
                    //    LV_XLSX.ItemsSource = items;

                    //}
                    //else
                    //{
                    //    var inv = new ListViewItem();
                    //    inv.Background = Brushes.Red;
                    //    inv.FontSize= 16;
                    //    inv.Content="Coś poszło nie tak - spróbuj jeszcze raz";
                    //    LV_XLSX.Items.Add(inv);
                    //}

                    ZaladujLV();
                }
            }
        }

        public void ZaladujLV()
        {
            if (items.Count > 0)
            {
                if (LV_XLSX.ItemsSource == null)
                {
                    LV_XLSX.Items.Clear();
                }
                LV_XLSX.ItemsSource = items;

            }
            else
            {
                var inv = new ListViewItem();
                inv.Background = Brushes.Red;
                inv.FontSize = 16;
                inv.Content = "Coś poszło nie tak - spróbuj jeszcze raz";
                LV_XLSX.Items.Add(inv);
            }
        }
        private void MI_USUN_PLIK(object sender, RoutedEventArgs e)
        {
            items.Clear();
            LV_XLSX.ItemsSource = null;
        }
        private void ContMenuOpen(object sender, ContextMenuEventArgs e)
        {
            if (LV_XLSX.ItemsSource == null)
            {
                MI_USUN_P.IsEnabled = false;
                MI_ZAPISZ_P.IsEnabled = false;
                MI_ZMIEN_NAZWE.IsEnabled = false;
            }
            else
            {
                MI_USUN_P.IsEnabled = true;
                MI_ZAPISZ_P.IsEnabled = true;
                MI_ZMIEN_NAZWE.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void txtChanged(object sender, TextChangedEventArgs e)
        {
            if (TXT_NAZWA_OBJ.Text.Trim() == "")
            {
                B_ZAPISZ.IsEnabled = false;
            }
            else
            {
                B_ZAPISZ.IsEnabled = true;
            }

        }
        private void loadXSLX(object sender, RoutedEventArgs e)
        {
            if (id_rec != "-1")
            {
                this.Title = "Zapisz plik automatyzacji";
                B_ZAPISZ.Visibility = Visibility.Hidden;
                MI_USUN_P.Visibility = Visibility.Collapsed;
                MI_ZMIEN_NAZWE.Visibility = Visibility.Collapsed;
                this.Topmost = false;
            }
            else 
            {
                MI_ZAPISZ_P.Visibility = Visibility.Collapsed;
            }
        }

        private void B_Zastap_plik_excel_Click(object sender, RoutedEventArgs e)
        {
            _PUBLIC_SqlLite.ZMIEN_REKORD_OBJEKT_FULL(TXT_NAZWA_OBJ.Text.Trim(), TXT_OPIS.Text.Trim(), items[0].CalaScieszka, items[0].TextExcel, id_rec);
            B_Zastap_plik_excel.Visibility = Visibility.Hidden;
            this.DialogResult = true;
        }
    }
}
