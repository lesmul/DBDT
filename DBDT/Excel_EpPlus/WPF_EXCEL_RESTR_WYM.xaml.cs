using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using DBDT.SQL.SQL_SELECT;
using System.Data.SqlClient;
using System.Globalization;
using DataTable = System.Data.DataTable;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls.Primitives;
using System.IO;
using OfficeOpenXml;

namespace DBDT.Excel
{

    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_EXCEL_RESTR_WYM : System.Windows.Window
    {

        private List<ExcelCellData> excelCellDataList;

        private string EXCELFilePath { get; set; }


        List<Brush> uniqueColors = new List<Brush>();

        public WPF_EXCEL_RESTR_WYM()
        {
            InitializeComponent();

        }

        private void OpenEXCELFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Pliki Excel (*.xlsx)|*.xlsx|Pliki Excel 97-2003 (*.xls)|*.xls",
                Title = "Wybierz plik Excel"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                EXCELFilePath = openFileDialog.FileName;

                xmlFilePathTextBox.Text = EXCELFilePath;

                List<ExcelNamedRange> name = new List<ExcelNamedRange>();

                LoadExcelDataAndColors(EXCELFilePath, 10, 500, dataGrid1, uniqueColors, name);

                CB_KOLORY.ItemsSource = uniqueColors;

                CB_NazwaArkusza.ItemsSource = name; 

                // PopulateColorComboBox(dataGrid1, CB_KOLORY);

            }
        }

        public static void LoadExcelDataAndColors(string excelFilePath, int maxColumn, int maxRow, DataGrid dataGrid, 
            List<Brush> uniqueColorsx, List<ExcelNamedRange> name)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var worksheet = package.Workbook.Worksheets[0];

                    name = package.Workbook.Names.ToList();

                    // Sprawdź, czy maxColumn i maxRow są w zakresie arkusza.
                    maxColumn = Math.Min(maxColumn, worksheet.Dimension.Columns) + 1;
                    maxRow = Math.Min(maxRow, worksheet.Dimension.Rows) + 1;

                    DataTable dataTable = new DataTable();

                    // Dodajemy kolumny na podstawie liczby maksymalnej kolumny.
                    for (int col = 1; col <= maxColumn; col++)
                    {
                        dataTable.Columns.Add("Kol" + col, typeof(string));
                    }

                    dataGrid.ItemsSource = dataTable.DefaultView;

                    for (int row = 1; row <= maxRow; row++)
                    {
                        var dataRow = dataTable.NewRow();
                        for (int col = 1; col <= maxColumn; col++)
                        {
                            string cellText = worksheet.Cells[row, col].Text;
                            dataRow[col - 1] = cellText;

                        }

                        dataTable.Rows.Add(dataRow);

                        for (int col = 1; col <= maxColumn; col++)
                        {
                            //string cellText = worksheet.Cells[row, col].Text;
                            //dataRow[col - 1] = cellText;

                            // Pobierz kolor tła komórki z arkusza Excel i przekształć go na kolor WPF
                            var excelColor = worksheet.Cells[row, col].Style.Fill.BackgroundColor;
                            var excelRgb = excelColor?.Rgb; // Obsługa null
                            var hexColor = excelRgb != null ? "#" + excelRgb.PadLeft(6, '0') : "#FFFFFF"; // Ustaw kolor na biały, jeśli excelRgb jest null

                            // Utwórz kolor WPF na podstawie kodu heksadecymalnego
                            Color color = (Color)ColorConverter.ConvertFromString(hexColor);

                            DataGridCell cell = GetCell(dataGrid, row - 1, col - 1);

                            if (cell != null)
                            {
                                cell.Background = new SolidColorBrush(color);

                                // Jeśli kolor nie istnieje jeszcze na liście, dodaj go
                                SolidColorBrush brush = cell.Background as SolidColorBrush;
                                if (brush != null)
                                {
                                    // Sprawdź, czy kolor jest już w liście unikalnych kolorów na podstawie wartości heksadecymalnej
                                    string brushColorHex = brush.Color.ToString();
                                    if (!uniqueColorsx.Any(existingBrush => existingBrush.ToString() == brushColorHex))
                                    {
                                        // Dodaj kolor do listy unikalnych kolorów
                                        uniqueColorsx.Add(brush);
                                    }
                                }

                            }
                        }

                    }

                    dataGrid.ItemsSource = dataTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wczytywania danych z pliku Excel: {ex.Message}");
            }
        }

        public static DataGridCell GetCell(DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            if (dataGrid == null || rowIndex < 0 || columnIndex < 0)
            {
                return null;
            }

            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row == null)
            {
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                dataGrid.UpdateLayout();
                row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }

            if (row != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);

                if (presenter != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    return cell;
                }
            }

            return null;
        }

        private static T FindVisualChild<T>(Visual parent) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }


        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void B_TW_FUNKCJE_Click(object sender, RoutedEventArgs e)
        {
            ExcelDataProcessor dataProcessor = new ExcelDataProcessor(excelCellDataList);

            // Generujemy warunki IF i przypisujemy wynik do kontrolki TXT_WYNIK_IF_ENDIF.
            TXT_WYNIK_IF_ENDIF.Text = dataProcessor.GenerateUniqueIFConditions(CB_KOLORY.SelectedValue.ToString());

        }



    }
}


