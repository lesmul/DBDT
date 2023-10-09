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
using DocumentFormat.OpenXml.Spreadsheet;
using static IronPython.Modules._ast;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml.FormulaParsing.Ranges;
using DBDT.Excel_EpPlus;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using DocumentFormat.OpenXml.Drawing;

namespace DBDT.Excel
{

    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_EXCEL_RESTR_WYM : System.Windows.Window
    {

        // private List<ExcelCellData> excelCellDataList;

        private string EXCELFilePath { get; set; }

        // private string WybranySkoroszyt { get; set; }

        List<Brush> uniqueColors = new List<Brush>();

        List<ConditionData> conditionsList = new List<ConditionData>();

        DataTable dataTableAll = new DataTable();
        public ExcelWorksheet worksheet;

        private Dictionary<Tuple<int, int>, SolidColorBrush> cellColors = new Dictionary<Tuple<int, int>, SolidColorBrush>();

        bool zaladowalem = false;
        public WPF_EXCEL_RESTR_WYM()
        {
            InitializeComponent();

            TXT_WYNIK_IF_ENDIF.Text = "Wybierz arkusz excel, potem skoroszyt, wybierz kolor do analizy i generuj warunki";

            integerTextBoxwiersz.Text = "1500";
            integerTextBoxkolumna.Text = "100";

            CB_Znak_RozdzielL1.SelectedIndex = 2;
            CB_Znak_RozdzielL2.SelectedIndex = 0;

            CB_Czcionka.Text = "7";
        }
        private void DataGridScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (zaladowalem == false) return;
            //RefreshCellColors();
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
                zaladowalem = false;

                CB_NazwaArkusza.ItemsSource = null;

                EXCELFilePath = openFileDialog.FileName;

                xmlFilePathTextBox.Text = EXCELFilePath;

                using (var package = new ExcelPackage(new FileInfo(EXCELFilePath)))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var worksheet = package.Workbook.Worksheets[0];

                    if (CB_NazwaArkusza.ItemsSource == null)
                    {
                        // Utwórz listę na nazwy arkuszy
                        List<string> nazwyArkuszy = new List<string>();

                        // Dodaj nazwy arkuszy do listy
                        foreach (var sheet in package.Workbook.Worksheets)
                        {
                            nazwyArkuszy.Add(sheet.Name);
                        }

                        // Ustaw źródło danych ComboBoxa
                        CB_NazwaArkusza.ItemsSource = nazwyArkuszy;
                        if (nazwyArkuszy.Count > 0) CB_NazwaArkusza.SelectedIndex = 0;
                    }
                }

                // LoadExcelDataAndColors(EXCELFilePath, Convert.ToInt16(integerTextBoxkolumna.Text), Convert.ToInt16(integerTextBoxwiersz.Text), dataGrid1, uniqueColors, 0);

                CB_KOLORY.ItemsSource = uniqueColors;

                CB_KOLORY_SLAVE.ItemsSource = uniqueColors;

                zaladowalem = true;

                // PopulateColorComboBox(dataGrid1, CB_KOLORY);

            }
        }

        public void LoadExcelDataAndColors(string excelFilePath, int maxColumn, int maxRow, DataGrid dataGrid,
            List<Brush> uniqueColorsx, int nrSkoroszytu)
        {
            try
            {
                dataTableAll = null;
                dataTableAll = new DataTable();
                worksheet = null;

                // Usuń stare dane i kolory
                dataGrid1.ItemsSource = null;
                conditionsList.Clear();
                cellColors.Clear();

                DG_LISTA_L1_L2.ItemsSource = null;

                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    if (nrSkoroszytu > package.Workbook.Worksheets.Count - 1) nrSkoroszytu = 0;

                    worksheet = package.Workbook.Worksheets[nrSkoroszytu];

                    // Sprawdź, czy maxColumn i maxRow są w zakresie arkusza.
                    maxColumn = Math.Min(maxColumn, worksheet.Dimension.Columns) + 1;
                    maxRow = Math.Min(maxRow, worksheet.Dimension.Rows) + 1;

                    // Dodajemy kolumny na podstawie liczby maksymalnej kolumny.
                    for (int col = 1; col <= maxColumn; col++)
                    {
                        dataTableAll.Columns.Add("L1-" + col, typeof(string));
                    }

                    dataGrid.ItemsSource = dataTableAll.DefaultView;

                    for (int row = 1; row <= maxRow; row++)
                    {
                        var dataRow = dataTableAll.NewRow();
                        for (int col = 1; col <= maxColumn; col++)
                        {
                            string cellText = worksheet.Cells[row, col].Text;
                            dataRow[col - 1] = cellText;

                        }

                        dataTableAll.Rows.Add(dataRow);

                        for (int col = 1; col <= maxColumn; col++)
                        {
                            //string cellText = worksheet.Cells[row, col].Text;
                            //dataRow[col - 1] = cellText;

                            // Pobierz kolor tła komórki z arkusza Excel i przekształć go na kolor WPF
                            var excelColor = worksheet.Cells[row, col].Style.Fill.BackgroundColor;
                            var excelRgb = excelColor?.Rgb; // Obsługa null
                            var hexColor = excelRgb != null ? "#" + excelRgb.PadLeft(6, '0') : "#FFFFFF"; // Ustaw kolor na biały, jeśli excelRgb jest null

                            if (hexColor == "#000000") hexColor = "#808080";

                          //  Console.WriteLine(hexColor);

                            // Utwórz kolor WPF na podstawie kodu heksadecymalnego
                            System.Windows.Media.Color color = (System.Windows.Media.Color)ColorConverter.ConvertFromString(hexColor);

                            DataGridCell cell = GetCell(dataGrid, row - 1, col - 1);

                            if (cell != null)
                            {
                                cell.Background = new SolidColorBrush(color);

                                cellColors[System.Tuple.Create(row, col)] = (SolidColorBrush)cell.Background;

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

                    // dataGrid.ItemsSource = dataTableAll.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wczytywania danych z pliku Excel: {ex.Message}\r\n" + ex.StackTrace);
            }
        }

        private void RefreshCellColors()
        {
            if(dataGrid1.Items.Count == 0) return;

            var scrollViewer = FindVisualChild<ScrollViewer>(dataGrid1);

            double firstVisibleRowIndex = scrollViewer.VerticalOffset;
            double lastVisibleRowIndex = firstVisibleRowIndex + scrollViewer.ViewportHeight;

            for (int rowIndex = (int)firstVisibleRowIndex; rowIndex <= (int)lastVisibleRowIndex; rowIndex++)
            {

                for (int columnIndex = 0; columnIndex < dataGrid1.Columns.Count; columnIndex++)
                {
                    Tuple<int, int> cellKey = System.Tuple.Create(rowIndex, columnIndex);

                    if (cellColors.TryGetValue(cellKey, out SolidColorBrush colorBrush))
                    {
                        DataGridCell cell = GetCell(dataGrid1, rowIndex, columnIndex);

                        if (cell != null)
                        {
                            cell.Background = colorBrush;
                        }
                    }
                }
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
        private void CB_NazwaArkusza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // WybranySkoroszyt = CB_NazwaArkusza.Text;
            dataGrid1.ItemsSource = null;
            CB_KOLORY.ItemsSource = null;
            CB_KOLORY_SLAVE.ItemsSource = null;

            if (EXCELFilePath != null)
                LoadExcelDataAndColors(EXCELFilePath, Convert.ToInt16(integerTextBoxkolumna.Text), Convert.ToInt16(integerTextBoxwiersz.Text), dataGrid1, uniqueColors, CB_NazwaArkusza.SelectedIndex);

            CB_KOLORY.ItemsSource = uniqueColors;
            CB_KOLORY_SLAVE.ItemsSource = uniqueColors;

            //   CB_NazwaArkusza.Text = WybranySkoroszyt;

        }
        private void Odswiez_z_excel_Click(object sender, RoutedEventArgs e)
        {
            //  WybranySkoroszyt = CB_NazwaArkusza.Text;

            if (dataGrid1 != null && EXCELFilePath != null)
                LoadExcelDataAndColors(EXCELFilePath, Convert.ToInt16(integerTextBoxkolumna.Text), Convert.ToInt16(integerTextBoxwiersz.Text), dataGrid1, uniqueColors, CB_NazwaArkusza.SelectedIndex);
            //  asdasd
            CB_KOLORY.ItemsSource = uniqueColors;
            CB_KOLORY_SLAVE.ItemsSource = uniqueColors;

            // CB_NazwaArkusza.Text = WybranySkoroszyt;
        }
        private void KodKoloru_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells[0].Item);
                int columnIndex = dataGrid1.SelectedCells[0].Column.DisplayIndex;

                DataGridCell cell = GetCell(dataGrid1, rowIndex, columnIndex);

                if (cell != null)
                {
                    SolidColorBrush cellBrush = cell.Background as SolidColorBrush;

                    if (cellBrush != null)
                    {
                        System.Windows.Media.Color color = cellBrush.Color;

                        MessageBox.Show($"Kolor komórki: {color.ToString()}");
                    }
                }
            }
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            //  WybranySkoroszyt = CB_NazwaArkusza.Text;

            RefreshCellColors();
            // CB_NazwaArkusza.Text = WybranySkoroszyt;
        }
        private void GenerateIFConditions_Click(object sender, RoutedEventArgs e)
        {
            if (CB_KOLORY.SelectedValue == null) return;
            //  ExcelDataProcessor dataProcessor = new ExcelDataProcessor(excelCellDataList);

            // Generujemy warunki IF i przypisujemy wynik do kontrolki TXT_WYNIK_IF_ENDIF.
            // TXT_WYNIK_IF_ENDIF.Text = dataProcessor.GenerateUniqueIFConditions(CB_KOLORY.SelectedValue.ToString());

            TXT_WYNIK_IF_ENDIF.Text = GenerateIFConditions(dataGrid1, CB_KOLORY.SelectedValue.ToString(), CB_KOLORY_SLAVE.SelectedValue.ToString());

            dataGrid1.SelectedCells.Clear();

            DG_LISTA_L1_L2.ItemsSource = conditionsList;

        }
        // public string GenerateIFConditions(DataGrid dataGrid, string targetColor)
        //{
        //    StringBuilder conditions = new StringBuilder();

        //    // Pobierz zaznaczone komórki z DataGrid.
        //    var selectedCells = dataGrid.SelectedCells;

        //    // Warunki będą generowane tylko dla zaznaczonych komórek o kolorze targetColor.
        //    foreach (DataGridCellInfo cellInfo in selectedCells)
        //    {
        //        int row = dataGrid.Items.IndexOf(cellInfo.Item); // Indeks wiersza.
        //        int column = dataGrid.Columns.IndexOf(cellInfo.Column); // Indeks kolumny.

        //        // Pobierz zawartość komórki w formie TextBlock.
        //        TextBlock cellContent = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
        //        if (cellContent != null)
        //        {
        //            SolidColorBrush cellBackground = cellContent.Background as SolidColorBrush;
        //            Console.WriteLine($"--->{targetColor}");
        //            if (cellBackground != null)
        //            Console.WriteLine($"{cellBackground.Color.ToString()} == {targetColor}");  
        //            if (cellBackground != null && cellBackground.Color.ToString() == targetColor)
        //            {
        //                // Warunki IF w postaci "IF(L1 = {column} AND L2 = {row}) THEN %Błąd; ENDIF".
        //                string condition = $"IF(L1 = {column} AND L2 = {row}) THEN %Błąd; ENDIF";

        //                conditions.AppendLine(condition);
        //            }
        //        }
        //    }

        //    return conditions.ToString();
        //}



        private string GenerateIFConditions(DataGrid dataGrid, string targetColor, string targetColor_SLAVE)
        {
            // StringBuilder conditionsBuilder = new StringBuilder();

            //  if(dataGrid.SelectedCells.Count == 0 ) return conditionsBuilder.ToString();//Musi być zazanczony obszar

            conditionsList.Clear();

            // conditionsBuilder.AppendLine("IF [");

            int minRow, maxRow, minColumn, maxColumn;

            minRow = int.MaxValue;
            maxRow = int.MinValue;
            minColumn = int.MaxValue;
            maxColumn = int.MinValue;

            foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
            {
                int rowIndex = dataGrid.Items.IndexOf(cellInfo.Item);

                if (maxColumn < cellInfo.Column.DisplayIndex)
                    maxColumn = cellInfo.Column.DisplayIndex;

                if (minColumn > cellInfo.Column.DisplayIndex)
                    minColumn = cellInfo.Column.DisplayIndex;

                //if(minRow > Math.Min(cellInfo.Column.DisplayIndex, rowIndex))
                //minRow = Math.Min(cellInfo.Column.DisplayIndex, rowIndex);

                if (minRow > rowIndex)
                    minRow = rowIndex;

                if (maxRow < Math.Max(cellInfo.Column.DisplayIndex, rowIndex))
                    maxRow = Math.Max(maxRow, rowIndex);
            }


            // Przeszukaj zaznaczone komórki w DataGrid.
            foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
            {
                if (cellInfo.IsValid)
                {
                    int rowIndex = dataGrid.Items.IndexOf(cellInfo.Item);
                    int columnIndex = cellInfo.Column.DisplayIndex;

                    // Pobierz wartość i kolor komórki na podstawie indeksu wiersza i kolumny.
                    var cellData = GetCellData(dataGrid, rowIndex, columnIndex);

                    if (cellData != null)
                    {

                        //minRow = Math.Min(columnIndex, rowIndex);
                        //maxRow = Math.Max(maxRow, rowIndex);
                        //minColumn = Math.Min(columnIndex, columnIndex);
                        //maxColumn = Math.Max(maxColumn, columnIndex);

                        // Utwórz warunek IF na podstawie wartości, koloru i innych kryteriów.
                        string condition = GenerateCondition(cellData, targetColor, targetColor_SLAVE, dataGrid1, minRow, maxRow, minColumn, maxColumn, cellInfo);
                        //if (!string.IsNullOrEmpty(condition))
                        //{
                        //    if (!conditionsBuilder.ToString().Contains(condition))
                        //    {
                        //        conditionsBuilder.AppendLine(condition);
                        //    }
                        //}
                    }
                }
            }

            //conditionsBuilder.AppendLine("]THEN");
            //conditionsBuilder.AppendLine("    SETERROREX([\u0022\u0022],\u0022[NE]\u0022);");
            //conditionsBuilder.AppendLine("ENDIF");

            //  var xxx = conditionsList.Count();

            // return conditionsBuilder.ToString();

            return GenerujWarunkiIF();
        }


        private Tuple<string, System.Windows.Media.Color> GetCellData(DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            // Pobierz zaznaczoną komórkę na podstawie indeksu wiersza i kolumny.
            DataGridCell cell = GetCell(dataGrid, rowIndex, columnIndex);

            if (cell != null)
            {
                // Pobierz zawartość komórki i kolor tła.
                string cellValue = cell.Content?.ToString();
                SolidColorBrush cellBackgroundBrush = cell.Background as SolidColorBrush;

                if (cellBackgroundBrush != null)
                {
                    System.Windows.Media.Color cellColor = cellBackgroundBrush.Color;

                    // Zwróć Tuple z wartością i kolorem komórki.
                    return System.Tuple.Create(cellValue, cellColor);
                }
            }

            return null;
        }

        private string GenerateCondition(Tuple<string, System.Windows.Media.Color> cellData, string selectColor, string selectColor_SLAVE,
        DataGrid dataGrid, int minRow, int maxRow, int minColumn, int maxColumn, DataGridCellInfo selectedCell)
        {
            // Pobierz zaznaczone komórki.
            // var selectedCells = dataGrid.SelectedCells;

            if (selectedCell != null)
            {
                //foreach (var selectedCell in selectedCells)
                // {
                // Pobierz indeksy zaznaczonej komórki.
                int selectedRowIndex = dataGrid.Items.IndexOf(selectedCell.Item);
                int selectedColumnIndex = selectedCell.Column.DisplayIndex;

                if (selectedRowIndex >= 0 && selectedColumnIndex >= 0)
                {
                    //  if (selectedColumnIndex > -1) selectedColumnIndex += 1;

                    // Sprawdź, czy kolor zaznaczonej komórki jest taki sam jak selectColor.
                    if (cellData.Item2.ToString() == selectColor || cellData.Item2.ToString() == selectColor_SLAVE)
                    {

                        // Pobierz wartość zaznaczonej komórki min.
                        DataGridCell selectedDataGridCellKolumnMin = GetCell(dataGrid, selectedRowIndex, minColumn);
                        string selectedCellValueKolumnMin = ((TextBlock)selectedDataGridCellKolumnMin.Content).Text;

                        // Pobierz wartość zaznaczonej komórki min.
                        DataGridCell selectedDataGridCellWierszMin = GetCell(dataGrid, minRow, selectedColumnIndex);
                        string selectedCellValueWierszMin = ((TextBlock)selectedDataGridCellWierszMin.Content).Text;

                        // Pobierz wartość zaznaczonej komórki min.
                        if (maxColumn <= 0)
                        {
                            maxColumn = 1;
                        }
                        DataGridCell selectedDataGridCellKolumnMax = GetCell(dataGrid, selectedRowIndex, maxColumn);
                        string selectedCellValueKolumnMax = ((TextBlock)selectedDataGridCellKolumnMax.Content).Text;

                        // Pobierz wartość zaznaczonej komórki min.
                        DataGridCell selectedDataGridCellWierszMax = GetCell(dataGrid, maxRow, selectedColumnIndex);
                        string selectedCellValueWierszMax = ((TextBlock)selectedDataGridCellWierszMax.Content).Text;
                        // Pobierz wartość pierwszej kolumny i pierwszego wiersza.
                        // DataGridCell firstColumnCell = GetCell(dataGrid, 0, selectedColumnIndex);
                        //string firstColumnValue = ((TextBlock)firstColumnCell.Content).Text;

                        // DataGridCell firstRowCell = GetCell(dataGrid, selectedRowIndex, 0);
                        // string firstRowValue = ((TextBlock)firstRowCell.Content).Text;

                        // Utwórz warunek IF, uwzględniając kolory komórki oraz wartości zaznaczonej komórki, pierwszej kolumny i pierwszego wiersza.
                        string L2 = selectedCellValueKolumnMin;
                        if (L2 == null || L2 == "")
                        {
                            L2 = selectedCellValueKolumnMax;
                        }

                        string L1 = selectedCellValueWierszMin;
                        if (L1 == null || L1 == "")
                        {
                            L1 = selectedCellValueWierszMax;
                        }
                        //if(L1 == "" || L2 == "")
                        //{
                        //    return string.Empty;
                        //}
                        //else
                        // {

                        if (IsNumeric(L1) == false)
                        {
                            L1 = "88888888";
                        }
                        if (IsNumeric(L2) == false)
                        {
                            L2 = "88888888";
                        }

                        conditionsList.Add(new ConditionData { L1 = Convert.ToInt32(L1), L2 = Convert.ToInt32(L2) });
                        return $"(L1 {CB_Znak_RozdzielL1.Text} {L1} AND L2 {CB_Znak_RozdzielL2.Text} {L2}) OR";

                        // }

                        //return $"IF (Warunek dla komórki o kolorze {cellData.Item2}, " +
                        //       $"Wartość zaznaczonej komórki: {selectedCellValue}, " +
                        //       $"Wartość z pierwszej kolumny: {L1}, " +
                        //       $"Wartość z pierwszego wiersza: {L2})";
                    }
                }
                //}
            }

            return string.Empty; // Zwróć pusty warunek, jeśli nie spełniono żadnego kryterium.
        }

        private bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        private string GenerujWarunkiIF()
        {
            //var uniqueL1Values = conditionsList.Select(c => c.L1).Distinct().ToList();
            // var uniqueL2Values = conditionsList.Select(c => c.L2).Distinct().ToList();

            //var uniqueL1AndMaxL2Values = conditionsList
            //    .GroupBy(c => c.L1)
            //    .Select(group => new
            //    {
            //        L1 = group.Key,
            //        MaxL2 = group.Max(c => c.L2)
            //    })
            //    .ToList();
            //var uniqueL1AndMaxL2Values = conditionsList
            //.GroupBy(c => c.L2)
            //.Select(group => new
            //{
            //    L2 = group.Key,
            //    L1 = group.Max(c => c.L1)
            //})
            //.ToList();

            // Użyj LINQ do wyodrębnienia unikalnych wartości L1
            var uniqueL1 = conditionsList.Select(c => c.L1).Distinct().ToList();

            // Użyj LINQ do stworzenia listy wynikowej (L1, MinL2, MaxL2)
            var result = uniqueL1.Select(l1 => new
            {
                L1 = l1,
                MinL2 = conditionsList.Where(c => c.L1 == l1).Min(c => c.L2),
                MaxL2 = conditionsList.Where(c => c.L1 == l1).Max(c => c.L2)
            }).Distinct().ToList(); // Dodaj Distinct() tutaj, aby usunąć powtarzające się wyniki


            StringBuilder conditionsBuilder = new StringBuilder();

           // conditionsBuilder.AppendLine("IF [");

            foreach (var item in result)
            {
                if (conditionsBuilder.Length == 0)
                {
                    conditionsBuilder.AppendLine($"IF [(L1 {CB_Znak_RozdzielL1.Text} {item.L1} AND L2 {CB_Znak_RozdzielL2.Text} {item.MaxL2}) OR");
                    conditionsBuilder.AppendLine($"(L1 {CB_Znak_RozdzielL1.Text} {item.L1} AND L2 {CB_Znak_RozdzielL1.Text} {item.MinL2}) OR");
                }
                else
                {
                    conditionsBuilder.AppendLine($"(L1 {CB_Znak_RozdzielL1.Text} {item.L1} AND L2 {CB_Znak_RozdzielL2.Text} {item.MaxL2}) OR");
                    conditionsBuilder.AppendLine($"(L1 {CB_Znak_RozdzielL1.Text} {item.L1} AND L2 {CB_Znak_RozdzielL1.Text} {item.MinL2}) OR");
                }
                   
            }


            if (conditionsBuilder.Length > 0)
            {
                conditionsBuilder.Remove(conditionsBuilder.Length - 4, 4);
                conditionsBuilder.AppendLine("] THEN");
                conditionsBuilder.AppendLine("    SETERROREX([\"\"],[\"[NE]\"]);");
                conditionsBuilder.AppendLine("ENDIF");
            }

            string finalCondition = conditionsBuilder.ToString();
            return finalCondition;
        }

        //private void FindMinMaxSelectedRowAndColumn(DataGrid dataGrid, out int minRow, out int maxRow, out int minColumn, out int maxColumn)
        //{
        //    minRow = int.MaxValue;
        //    maxRow = int.MinValue;
        //    minColumn = int.MaxValue;
        //    maxColumn = int.MinValue;

        //    var selectedCells = dataGrid.SelectedCells;
        //    if (selectedCells != null && selectedCells.Count > 0)
        //    {
        //        foreach (var selectedCell in selectedCells)
        //        {
        //            int rowIndex = dataGrid.Items.IndexOf(selectedCell.Item);
        //            int columnIndex = selectedCell.Column.DisplayIndex;

        //            minRow = Math.Min(minRow, rowIndex);
        //            maxRow = Math.Max(maxRow, rowIndex);
        //            minColumn = Math.Min(minColumn, columnIndex);
        //            maxColumn = Math.Max(maxColumn, columnIndex);
        //        }
        //    }
        //}


        private void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Sprawdź, czy wprowadzony znak jest cyfrą lub znakiem minus (dla liczb ujemnych)
            if (!char.IsDigit(e.Text, 0) && e.Text != "-")
            {
                e.Handled = true; // Odrzuć wprowadzony znak
            }
        }

        private void CB_Czcionka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CB_Czcionka.SelectedItem != null)
            {
                // Pobierz wybraną wartość z ComboBox i przekształć ją na double
                if (double.TryParse(((System.Windows.Controls.ContentControl)CB_Czcionka.SelectedItem).Content.ToString(), out double fontSize))
                {
                    // Ustaw wielkość czcionki w kontrolce DataGrid
                    dataGrid1.FontSize = fontSize;
                    switch(fontSize)
                    {
                        case 6:
                            dataGrid1.MaxColumnWidth = 25;
                            break;
                        case 7:
                            dataGrid1.MaxColumnWidth = 26;
                            break;
                        case 8:
                            dataGrid1.MaxColumnWidth = 27;
                            break;
                        case 9:
                            dataGrid1.MaxColumnWidth = 30;
                            break;
                        case 10:
                            dataGrid1.MaxColumnWidth = 32;
                            break;
                        case 12:
                            dataGrid1.MaxColumnWidth = 34;
                            break;
                        case 14:
                            dataGrid1.MaxColumnWidth = 36;
                            break;
                        default:
                            dataGrid1.MaxColumnWidth = 25;
                            break;
                    }

                }
                else
                {
                    // Obsłuż błąd w przypadku niepowodzenia przekształcenia
                    dataGrid1.FontSize = 6;
                }
            }

        }



        //private string GenerateCondition(Tuple<string, System.Windows.Media.Color> cellData, string selectColor, DataGrid dataGrid)
        //{
        //    if (cellData.Item2.ToString() == selectColor)
        //    {
        //        // Pobierz wartości zaznaczonych komórek pierwszej kolumny i pierwszego wiersza.
        //        List<string> selectedValues = GetSelectedRowAndColumnValues(dataGrid);

        //        // Jeśli znaleziono wartości, użyj ich w warunku IF.
        //        if (selectedValues.Count == 2)
        //        {
        //            string firstCellValue = selectedValues[0];
        //            string firstRowValue = selectedValues[1];

        //            // Utwórz warunek IF, uwzględniając wartość z pierwszego wiersza i kolumny.
        //            return $"IF (Warunek dla komórki o kolorze {cellData.Item2}, Wartość z pierwszego wiersza: {firstRowValue}, Wartość z pierwszej kolumny: {firstCellValue})";
        //        }
        //    }

        //    return string.Empty; // Zwróć pusty warunek, jeśli nie spełniono żadnego kryterium.
        //}


        //private List<string> GetSelectedRowAndColumnValues(DataGrid dataGrid)
        //{
        //    List<string> values = new List<string>();

        //    // Pobierz zaznaczone komórki.
        //    var selectedCells = dataGrid.SelectedCells;

        //    // Pobierz indeksy pierwszej kolumny i pierwszego wiersza.
        //    int firstColumnIndex = int.MaxValue;
        //    int firstRowIndex = int.MaxValue;

        //    foreach (DataGridCellInfo cellInfo in selectedCells)
        //    {
        //        int columnIndex = dataGrid.Columns.IndexOf(cellInfo.Column);
        //        int rowIndex = dataGrid.Items.IndexOf(cellInfo.Item);

        //        if (columnIndex < firstColumnIndex)
        //        {
        //            firstColumnIndex = columnIndex;
        //        }

        //        if (rowIndex < firstRowIndex)
        //        {
        //            firstRowIndex = rowIndex;
        //        }
        //    }

        //    // Pobierz wartości z pierwszej kolumny i pierwszego wiersza.
        //    if (firstColumnIndex != int.MaxValue && firstRowIndex != int.MaxValue)
        //    {
        //        DataGridCell firstColumnCell = GetCell(dataGrid, firstRowIndex, firstColumnIndex);
        //        values.Add(((System.Windows.Controls.TextBlock)firstColumnCell.Content).Text);

        //        DataGridCell firstRowCell = GetCell(dataGrid, firstRowIndex - 1, 0); // Indeks kolumny pierwszego wiersza zawsze 0.
        //        values.Add(((System.Windows.Controls.TextBlock)firstRowCell.Content).Text);
        //    }

        //    return values;
        //}



    }
}


