//using System.Windows.Controls;
//using System.Windows.Media;
//using OfficeOpenXml;
//using System;
//using System.Data;
//using System.IO;
//using System.Windows;
//using System.Windows.Controls.Primitives;

//namespace DBDT.Excel_EpPlus
//{
//    public static class ExcelHelper
//    {
//        private static ExcelWorksheet worksheet;

//        public static void SetWorksheet(ExcelWorksheet ws)
//        {
//            worksheet = ws;
//        }
//        public static void LoadExcelDataAndColors(string excelFilePath, int maxColumn, int maxRow, DataGrid dataGrid)
//        {
//            try
//            {
//                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
//                {
//                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//                    var worksheet = package.Workbook.Worksheets[0]; // Zakładamy, że dane są na pierwszym arkuszu.

//                    SetWorksheet(worksheet);

//                    // Sprawdź, czy maxColumn i maxRow są w zakresie arkusza.
//                    maxColumn = Math.Min(maxColumn, worksheet.Dimension.Columns) + 1;
//                    maxRow = Math.Min(maxRow, worksheet.Dimension.Rows) + 1;

//                    DataTable dataTable = new DataTable();

//                    // Dodajemy kolumny na podstawie liczby maksymalnej kolumny.
//                    for (int col = 1; col <= maxColumn; col++)
//                    {
//                        dataTable.Columns.Add("Kol" + col, typeof(string));
//                    }

//                   dataGrid.ItemsSource = dataTable.DefaultView;

//                    for (int row = 1; row <= maxRow; row++)
//                    {
//                        var dataRow = dataTable.NewRow();
//                        for (int col = 1; col <= maxColumn; col++)
//                        {
//                            string cellText = worksheet.Cells[row, col].Text;
//                            dataRow[col - 1] = cellText;

//                            //// Pobierz kolor tła komórki z arkusza Excel i przekształć go na kolor WPF
//                            //var excelColor = worksheet.Cells[row, col].Style.Fill.BackgroundColor;
//                            //var excelRgb = excelColor?.Rgb; // Obsługa null
//                            //var hexColor = excelRgb != null ? "#" + excelRgb.PadLeft(6, '0') : "#FFFFFF"; // Ustaw kolor na biały, jeśli excelRgb jest null

//                            //// Utwórz kolor WPF na podstawie kodu heksadecymalnego
//                            //System.Windows.Media.Color color = (System.Windows.Media.Color)ColorConverter.ConvertFromString(hexColor);

//                            //DataGridCell cell = GetCell(dataGrid, row - 1, col - 1);

//                            //if (cell != null)
//                            //{
//                            //    cell.Background = new SolidColorBrush(color);
//                            //}
//                        }

//                        dataTable.Rows.Add(dataRow);
//                    }

//                   // dataGrid.ItemsSource = dataTable.DefaultView;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Błąd wczytywania danych z pliku Excel: {ex.Message}");
//            }
//        }

//        public static void ApplyCellBackgroundColors(DataGrid dataGrid, int maxRow, int maxColumn)
//        {
//            if (worksheet == null)
//            {
//                return; // Nie rób nic, jeśli arkusz nie jest dostępny
//            }

//            var items = dataGrid.Items;

//            for (int rowIndex = 0; rowIndex < maxRow; rowIndex++)
//            {
//                if (rowIndex < items.Count)
//                {
//                    var dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;


//                    if (dataGridRow != null)
//                    {
//                        for (int columnIndex = 0; columnIndex < maxColumn; columnIndex++)
//                        {
//                            DataGridCell cell = GetCell(dataGrid, rowIndex, columnIndex);

//                            if (cell != null)
//                            {
//                                // Pobierz kolor tła komórki z arkusza Excel
//                                var excelColor = worksheet.Cells[rowIndex + 1, columnIndex + 1].Style.Fill.BackgroundColor;

//                                // Przekształć kolor i ustaw go jako tło komórki
//                                var excelRgb = excelColor?.Rgb;
//                                var hexColor = excelRgb != null ? "#" + excelRgb.PadLeft(6, '0') : "#FFFFFF"; // Ustaw kolor na biały, jeśli excelRgb jest null
//                                System.Windows.Media.Color color = (System.Windows.Media.Color)ColorConverter.ConvertFromString(hexColor);
//                                cell.Background = new SolidColorBrush(color);
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        public static DataGridCell GetCell(DataGrid dataGrid, int rowIndex, int columnIndex)
//        {
//            if (dataGrid == null) return null;
//            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

//            if (row == null) return null;
//            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

//            if (presenter == null) return null;
//            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
//            return cell;
//        }

//        private static T GetVisualChild<T>(Visual parent) where T : Visual
//        {
//            T child = default(T);
//            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

//            for (int i = 0; i < numVisuals; i++)
//            {
//                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
//                child = v as T;

//                if (child == null)
//                {
//                    child = GetVisualChild<T>(v);
//                }

//                if (child != null)
//                {
//                    break;
//                }
//            }
//            return child;
//        }

//        // Pozostała część klasy, w tym metoda dataGrid1_LoadingRow
//    }
//}
