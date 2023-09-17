using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using DBDT.Excel.DS;
using System.IO;

namespace DBDT.Excel_EpPlus
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_KONFIG_SQL.xaml
    /// </summary>
    public partial class WPF_Doplaty_Narz : System.Windows.Window
    {

        private DataTable dt_daneceny = new DataTable();
		private DataTable dt_dane_doplat = new DataTable();

        private DataSet dataSet = new DataSet(); // Deklaracja DataSet na poziomie klasy

        private SqlHandler sqlHandler = new SqlHandler();

        public WPF_Doplaty_Narz()
        {
            InitializeComponent();

            IntIloscZnakow.Text = "4";

        }
        private void state_changed(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void Button_Save_Close_Click(object sender, RoutedEventArgs e)
        {
            // Zapisz dane do pliku XML.
            string xmlFilePath = "dane_doplat.xml";
            dataSet.Tables.Clear(); // Wyczyść istniejące tabele w DataSet

            // Dodaj ponownie istniejące tabele do DataSet
            dataSet.Tables.Add(dt_daneceny);
            dataSet.Tables.Add(dt_dane_doplat);

            dataSet.WriteXml(xmlFilePath);

            this.Close();
        }

        private void loaded_xml(object sender, RoutedEventArgs e)
        {
            // Ładowanie danych z pliku XML do DataTable.
            string xmlFilePath = "dane_doplat.xml";

			if (File.Exists(xmlFilePath))
			{
                dataSet.ReadXml(xmlFilePath);

                if (dataSet.Tables.Count >= 2)
                {
                    dt_daneceny = dataSet.Tables[0];
                    dt_dane_doplat = dataSet.Tables[1];

                    CenyGrid.ItemsSource = null;
                    DoplatyGrid.ItemsSource = null;

                    CenyGrid.ItemsSource = dt_daneceny.DefaultView;
                    DoplatyGrid.ItemsSource = dt_dane_doplat.DefaultView;

                    ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dt_daneceny.Rows.Count;
                }
				else if(dataSet.Tables.Count == 1)
				{
                    dt_daneceny = dataSet.Tables[0];
                    CenyGrid.ItemsSource = dt_daneceny.DefaultView;

                    ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dt_daneceny.Rows.Count;
                }
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resoluts = MessageBox.Show("Czy ropocząć proces dodawania cen", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resoluts == MessageBoxResult.No) return;

            int ilosc_Bledow = 0;
			int iloscPomin = Convert.ToInt32(IntIloscZnakow.Text);

			try
            {

				foreach (DataRow rowDoplat in dt_dane_doplat.Rows)
				{
					// Pobierz numer z tabeli dt_dane_doplat.

					string numer = rowDoplat["indeks"].ToString();

					// Pomiń 4 pierwsze znaki w numerze.
					if (numer.Length > iloscPomin)
					{
						numer = numer.Substring(iloscPomin);
					}

					if (chek_do_znaku.IsChecked == true)
					{
                        // Przytnij numer od początku do znaku '/'.
                        int indexOfSlash = numer.IndexOf('/');
                        if (indexOfSlash >= 0)
                        {
                            numer = numer.Substring(0, indexOfSlash);
                        }
                    }

                    // Szukaj odpowiedniej ceny w tabeli dt_daneceny.

                    DataRow[] foundRows = dt_daneceny.Select("indeks LIKE '%" + numer + "%'");

					if (foundRows.Length > 0)
					{

						string cenaString = foundRows[0]["Cena"].ToString();
						decimal cena = 0;

						// Spróbuj skonwertować cenę na liczbę, jeśli nie uda się, ustaw na 0.
						if (!decimal.TryParse(cenaString, out cena))
						{
							cena = 0;
						}
						else
						{
							if (cenaString.TrimEnd().EndsWith("%"))
							{
                                cena = 0;
                            }
							else
							{
                                cena = Convert.ToDecimal(foundRows[0]["Cena"]);
                            }
						}

						// Zapisz cenę do kolumny 2 w tabeli dt_dane_doplat.
						rowDoplat["CenaWyszukana"] = cena;
					}
				}

			}
            catch (Exception ex)
            {
                if (System.Environment.UserName == "Leszek Mularski")
                {
                    MessageBox.Show(ex.Message + "\r\n" + "\r\n" + ex.StackTrace, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Błąd odczytu", MessageBoxButton.OK, MessageBoxImage.Error);
                }

				ilosc_Bledow++;

			}

			if(ilosc_Bledow>0)
            MessageBox.Show("Zakończyłem sprawdzanie!" + "\r\n" + "Ilość błędów: " + ilosc_Bledow, "Koniec pracy...", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ClickDelSelectRow(object sender, RoutedEventArgs e)
        {
            string value = "";
 
            for (int i = 0; i < DoplatyGrid.SelectedCells.Count; i++)
            {
                DataGridCellInfo cell = DoplatyGrid.SelectedCells[i];

                if (cell.Item != null)
                {
                    value += cell.Column.Header.ToString() + ", " + "\r\n";

                   ((System.Data.DataRowView)cell.Item).Row.Delete();

                }
            }
        }

        private void ClickPasteValuesDop(object sender, RoutedEventArgs e)
        {
            string clipboardText = Clipboard.GetText();
            if (!string.IsNullOrEmpty(clipboardText))
            {
                // Tutaj możesz przetworzyć tekst z bufora schowka i przekształcić go na DataTable.
                DataTable dataTable = ConvertClipboardTextToDataTable(clipboardText);

                if (dataTable != null)
                {
                    // Jeśli tabela dt_dane_doplat jest pusta, możesz ją zastąpić nową.
                    // W przeciwnym razie możesz dodać dane do istniejącej tabeli.
                    if (dt_dane_doplat == null)
                    {
                        dt_dane_doplat = dataTable;
                        dt_dane_doplat.Columns[0].ColumnName = "indeks";
                        AddColumn<decimal>("CenaWyszukana", dt_dane_doplat);
                    }
                    else
                    {
                        // Przyjmij, że istniejące dane są w tabeli dt_dane_doplat.
                        // Dodaj nowe dane do istniejących danych w tabeli.
                        foreach (DataRow newRow in dataTable.Rows)
                        {
                            DataRow newRowCopy = dt_dane_doplat.NewRow();
                            newRowCopy.ItemArray = newRow.ItemArray;
                            dt_dane_doplat.Rows.Add(newRowCopy);
                        }
                    }

                    // Zaktualizuj źródło danych dla DataGrid.
                    DoplatyGrid.ItemsSource = null;
                    DoplatyGrid.ItemsSource = dt_dane_doplat.DefaultView;
                }
            }
        }


        private void ClickPasteValues(object sender, RoutedEventArgs e)
        {

			string clipboardText = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboardText))
			{
				// Tutaj możesz przetworzyć tekst z bufora schowka i przekształcić go na DataTable.
				DataTable dataTable = ConvertClipboardTextToDataTable(clipboardText);

				if (dataTable != null && dataTable.Columns.Count > 1)
				{
					dt_daneceny = (DataTable)dataTable;

					if(dt_daneceny.Rows.Count > 0 && dt_daneceny.Columns.Count > 0)
					{
                        for (int rowIndex = 0; rowIndex < dt_daneceny.Rows.Count; rowIndex++)
                        {
                            dt_daneceny.Rows[rowIndex][0] = dt_daneceny.Rows[rowIndex][0].ToString().Trim();
                        }
                    } 

                    CenyGrid.ItemsSource = null;
                    CenyGrid.ItemsSource = dt_daneceny.DefaultView;

					ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dt_daneceny.Rows.Count;
				}
			}

		}
		private void AddColumn<TData>(string columnName, DataTable targetDataTable, int columnIndex = -1)
		{
			DataColumn newColumn = new DataColumn(columnName, typeof(TData));

			targetDataTable.Columns.Add(newColumn);
			if (columnIndex > -1)
			{
				newColumn.SetOrdinal(columnIndex);
			}

			int newColumnIndex = targetDataTable.Columns.IndexOf(newColumn);

			// Initialize existing rows with default value for the new column
			foreach (DataRow row in targetDataTable.Rows)
			{
				row[newColumnIndex] = default(TData);
			}
		}
		private DataTable ConvertClipboardTextToDataTable(string clipboardText)
		{
			DataTable dataTable = new DataTable();

			string[] lines = clipboardText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			if (lines.Length > 0)
			{
				// Wprowadź kolumny na podstawie pierwszego wiersza.
				//string[] columns = lines[0].Split(',');
				string[] columns = lines[0].Split('\t');
				int nazkol = 1;
				foreach (string column in columns)
				{
					nazkol++;

					if(dataTable.Columns.Contains(column) == false)
					{
						dataTable.Columns.Add(column);
					}
					else
					{
						dataTable.Columns.Add(column + nazkol);
					}
				}

				// Dodaj dane z pozostałych wierszy.
				for (int i = 0; i < lines.Length; i++)
				{
					//string[] values = lines[i].Split(',');
					string[] values = lines[i].Split('\t');
					if (values.Length == columns.Length)
					{
						dataTable.Rows.Add(values);
					}
				}
			}

			return dataTable;
		}

		private void IntIloscZnakow_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// Sprawdź, czy wprowadzony tekst składa się tylko z cyfr.
			if (!IsNumeric(e.Text))
			{
				e.Handled = true; // Odrzuć wprowadzone znaki, jeśli nie są cyframi.
			}
		}

		private bool IsNumeric(string text)
		{
			return int.TryParse(text, out _); // Sprawdź, czy tekst można przekonwertować na liczbę całkowitą.
		}
		private void ClickIndeks(object sender, RoutedEventArgs e)
		{
			if (CenyGrid == null) return;
			if (CenyGrid.SelectedCells.Count == 0) return;

			DataGridCellInfo cell = CenyGrid.SelectedCells[0];

			for (int columnIndex = 0; columnIndex < dt_daneceny.Columns.Count; columnIndex++)
			{

				if(cell.Column.DisplayIndex == columnIndex)
				{
					if (dt_daneceny.Columns.Contains("indeks"))
					{
						for (int columnIndextmp = 0; columnIndextmp < dt_daneceny.Columns.Count; columnIndextmp++)
						{
							if (dt_daneceny.Columns[columnIndextmp].ColumnName == "indeks")
							{
								dt_daneceny.Columns[columnIndextmp].ColumnName = "indeks_" + columnIndex.ToString();
								break;
							}
						}

						dt_daneceny.Columns[columnIndex].ColumnName = "indeks";

					}
					else
					{
						dt_daneceny.Columns[columnIndex].ColumnName = "indeks";
					}
				}
				else
				{ 
					if(dt_daneceny.Columns[columnIndex].ColumnName != "cena")
					{
						if (dt_daneceny.Columns.Contains(columnIndex.ToString()))
						{
							dt_daneceny.Columns[columnIndex].ColumnName = columnIndex.ToString() + (columnIndex*1000).ToString();
						}
						else
						{
							dt_daneceny.Columns[columnIndex].ColumnName = columnIndex.ToString();
						}
					}
				}
			}

			CenyGrid.ItemsSource = null;
			CenyGrid.ItemsSource = dt_daneceny.DefaultView;
		}

		private void ClickCena(object sender, RoutedEventArgs e)
		{
			if (CenyGrid == null) return;
			if (CenyGrid.SelectedCells.Count == 0) return;

			DataGridCellInfo cell = CenyGrid.SelectedCells[0];

			for (int columnIndex = 0; columnIndex < dt_daneceny.Columns.Count; columnIndex++)
			{

				if (cell.Column.DisplayIndex == columnIndex)
				{
					if (dt_daneceny.Columns.Contains("cena"))
					{
						for (int columnIndextmp = 0; columnIndextmp < dt_daneceny.Columns.Count; columnIndextmp++)
						{
							if (dt_daneceny.Columns[columnIndextmp].ColumnName == "cena")
							{
								dt_daneceny.Columns[columnIndextmp].ColumnName = "cena_" + columnIndex.ToString();
								break;
							}
						}

						dt_daneceny.Columns[columnIndex].ColumnName = "cena";

					}
					else
					{
						dt_daneceny.Columns[columnIndex].ColumnName = "cena";
					}		
				}
				else
				{
					if (dt_daneceny.Columns[columnIndex].ColumnName != "indeks")
					{
						if (dt_daneceny.Columns.Contains(columnIndex.ToString()))
						{
							dt_daneceny.Columns[columnIndex].ColumnName = columnIndex.ToString() + (columnIndex * 250).ToString();
						}
						else
						{
							dt_daneceny.Columns[columnIndex].ColumnName = columnIndex.ToString();
						}
					}
				}
			}

			CenyGrid.ItemsSource = null;
			CenyGrid.ItemsSource = dt_daneceny.DefaultView;
		}

		private void textFiltrChanged(object sender, TextChangedEventArgs e)
		{

			if (dt_daneceny.Columns.Contains("indeks") == false) return;

			if(filtrIndeks.Text.Trim() != "")
			{
				var dtv = dt_daneceny.DefaultView;
				dtv.RowFilter = "indeks like'%" + filtrIndeks.Text.Trim() + "%'";
				if(dtv.Count  > 0)
				{
					CenyGrid.ItemsSource = null;
					CenyGrid.ItemsSource = dtv;
					filtrIndeks.Background = Brushes.Green;

					ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dtv.Count;
				}
				else
				{
					filtrIndeks.Background = Brushes.Red;

					ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dtv.Count;
				}

			}
			else
			{
				CenyGrid.ItemsSource = null;
				CenyGrid.ItemsSource = dt_daneceny.DefaultView;
				filtrIndeks.Background = Brushes.White;

				ilosc_danych.Content = "Ilość danych w tabeli ceny: " + dt_daneceny.Rows.Count;
			}
		}

        private void ClickCzyscTabeleCeny(object sender, RoutedEventArgs e)
        {
            CenyGrid.ItemsSource = null;
			dt_daneceny.Rows.Clear();
			dt_daneceny.Columns.Clear();
            CenyGrid.ItemsSource = dt_daneceny.DefaultView;
        }

        private void ClickUsunTabeleZDoplatami(object sender, RoutedEventArgs e)
        {
            DoplatyGrid.ItemsSource = null;
            dt_dane_doplat.Rows.Clear();
            dt_dane_doplat.Columns.Clear();
            DoplatyGrid.ItemsSource = dt_dane_doplat.DefaultView;
        }
    }
}
