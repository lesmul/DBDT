using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;
using System;
using System.Windows.Media.Imaging;
using System.Data;
using DBDT.SQL.SQL_SELECT;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Windows.Controls;
using DBDT.USTAWIENIA_PROGRAMU;
using System.ComponentModel;
using WPF.MDI;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Documents;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.Collections.Generic;

namespace DBDT.SQL.SQL_SELECT
{
    public partial class MainWindowSQL : UserControl
    {
        private SqlHandler sqlHandler;
        public bool procedura = false;
        public int id_rec = -1;
        private bool auto_u = false;
        private DataTable dt_str_danych = new DataTable();

        public MainWindowSQL()
        {
            InitializeComponent();
            sqlHandler = new SqlHandler();

            if (auto_u == true)
            {
                B_EXIT.Visibility = Visibility.Collapsed;
                B_ZAPISZ_DO_BAZY.Visibility = Visibility.Collapsed;
            }
        }

        #region Private Methods
        private void Connect()
        {
            try
            {

                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count == 0)
                {
                    return;
                }
                if(sqlHandler == null)
                sqlHandler = new SqlHandler();

                sqlHandler.Connect(@"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateUIStatus();
            }
        }

        private void UpdateUIStatus(bool status_zapytania = false, string stausAnalizy = null)
        {
            if (stausAnalizy != null)
            {

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();

                if (status_zapytania)
                {
                    logo.UriSource = new Uri("Images/status_ok.png", UriKind.Relative);
                }
                else
                {
                    if (stausAnalizy != null)
                    {
                        if (stausAnalizy.IndexOf("zapytania: [-1]") > -1)
                        {
                            logo.UriSource = new Uri("Images/status_uwaga.png", UriKind.Relative);
                        }
                        else
                        {
                            logo.UriSource = new Uri("Images/staus_notok.png", UriKind.Relative);
                        }
                    }
                    else
                    {
                        logo.UriSource = new Uri("Images/staus_notok.png", UriKind.Relative);
                    }
                }

                logo.EndInit();
                connStatusIcon.Source = logo;
                txtStatus.Text = "Analiza zapytania SQL: ";
                txtConnection.Text = stausAnalizy;
            }
            else
            {
                if (sqlHandler.IsConnected)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri("Images/bullet_blue.png", UriKind.Relative);
                    logo.EndInit();
                    connStatusIcon.Source = logo;
                    txtStatus.Text = "Połączony:  ";
                    txtConnection.Text = sqlHandler.ConnectionString;
                }
                else
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri("Images/bullet_red.png", UriKind.Relative);
                    logo.EndInit();
                    connStatusIcon.Source = logo;
                    txtStatus.Text = "Nie połączony";
                    txtConnection.Text = string.Empty;
                }
            }
        }
        #endregion

        #region Events
        void ClickAuto(object sender, RoutedEventArgs e)
        {
            if (auto_u == false)
            {
                auto_u = true;
                BitmapImage Image1 = new BitmapImage(new Uri("/SQL/SQL_SELECT/Images/auto_on.png", UriKind.Relative));
                auto_on.Source = Image1;
            }
            else
            {
                auto_u = false;
                BitmapImage Image1 = new BitmapImage(new Uri("/SQL/SQL_SELECT/Images/auto.png", UriKind.Relative));
                auto_on.Source = Image1;
            }

            if (SuggestionValuesTbName == null)
            {
                try
                {
                    dt_str_danych = sqlHandler.StrukturaTabel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var distinctRows = (from r in dt_str_danych.AsEnumerable()
                                    select r["TableName"]).Distinct().ToList();

                string myStringOutput = String.Join(",", distinctRows.ToArray().Select(p => p.ToString()).ToArray());
                SuggestionValuesTbName = myStringOutput.Split(',');
            }
        }
        void frm_exit(object sender, RoutedEventArgs e)
        {
            this.Tag = "CLOSE";

            var parentWindow = Window.GetWindow(this.Parent);
            //parentWindow.Close(); // zakończ program

            foreach (MdiChild mdiChild in ((DBDT.MainWindow)parentWindow).Container.Children)
            {

                if (((System.Windows.FrameworkElement)mdiChild.Content).Tag != null)
                {
                    if (((System.Windows.FrameworkElement)mdiChild.Content).Tag.ToString() == "CLOSE")
                    {
                        mdiChild.Content = null;
                        mdiChild.Close();
                        ; return;
                    }
                }

                if (((DBDT.MainWindow)parentWindow).Container.Children.Count == 1)
                {
                    mdiChild.Content = null;
                    mdiChild.Close();
                    ; return;
                }

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect();
            b_wykonaj.IsEnabled = false;
            b_wykonaj_ds.IsEnabled = false;

            Window currentWindow = Window.GetWindow(this);
            //currentWindow.Closing += currentWindow_Closing;
        }
        #endregion

        #region Commands
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "Sql pliki|*.sql|Text files|*.txt|Wszystkie pliki|*.*";
            fileChooser.CheckPathExists = true;
            fileChooser.Multiselect = false;
            fileChooser.Title = "Wybierz plik zawierający kod SQL";
            if (fileChooser.ShowDialog().Value == false)
                return;

            using (StreamReader sr = new StreamReader(fileChooser.FileName))
            {
                txtCode.Text = sr.ReadToEnd();
                sr.Close();
            }
        }
        private void click_find_error(object sender, RoutedEventArgs e)
        {

            UpdateUIStatus();

            try
            {
                b_wykonaj.IsEnabled = false;
                b_wykonaj_ds.IsEnabled = false;
                SqlError[] errors;

                System.Data.DataTable dt = new System.Data.DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, pole10 FROM ParametryPalaczenia WHERE pole9 LIKE 'POLE_KONFIGURACJI_INDEKSOW' order by id desc");

                string strSQL = "";

                if (dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["pole10"].ToString().Trim() != "")
                    {
                        strSQL = dt.Rows[0]["pole10"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Brak danych w konfiguraorze SQL", "Błąd", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                DataTable result = sqlHandler.Execute(strSQL, out errors, procedura);

                if(result.Rows.Count == 0)
                {
                    MessageBox.Show("Brak danych", "Błąd", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string strcolname = result.Columns[0].ColumnName;

                DataTable dtpuste = new DataTable();
                DataColumn dc = new DataColumn
                {
                    ColumnName = strcolname,
                    DataType = typeof(int)
                };
                dtpuste.Columns.Add(dc);
                DataColumn dco = new DataColumn
                {
                    ColumnName = "Informacja DBDT",
                    DataType = typeof(string),
                    DefaultValue ="Pusty numer błędu"
                };
                dtpuste.Columns.Add(dco);

                int lpmax = result.AsEnumerable().Max(row => row.Field<int>(0)) + 10;
                int lpmin = result.AsEnumerable().Min(row => row.Field<int>(0));

                int i;

                DataView dv = new DataView(result);

                for (i = lpmin; i < lpmax; i++)
                {
                    dv.RowFilter = strcolname + "=" + i;
                    if (dv.Count == 0)
                    {
                        DataRow newRow = dtpuste.NewRow();
                        newRow[strcolname] = i;
                        dtpuste.Rows.Add(newRow);
                    }
                }

                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);

                UpdateUIStatus(true, "Zapytanie zwróciło wyników: " + dtpuste.Rows.Count.ToString()); ;

                if (dtpuste.Rows.Count > 0)
                {
                    ResultWindow resultWindow = new ResultWindow(dtpuste, sqlHandler.Nazwa_Tabeli(), sqlHandler.Wartosc_Like());
                    resultWindow.Topmost = (bool)ChkTopmost.IsChecked; // Ustawienie okna wynikowego na zawsze na wierzchu
                    resultWindow.Show();
                }
                else if (errors == null)
                {
                    MessageBox.Show("Ilość wyników <null>.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Save_ExecutedSQL(object sender, ExecutedRoutedEventArgs e)
        {

            if (txtCode.Text.Trim() == "") return;

            if (id_rec != -1)
            {
                var dr = MessageBox.Show("Czy chcesz dodać nowe zapytanie SQL ?", "Uwaga!!!", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {
                    id_rec = -1;
                }
            }

            if (id_rec == -1)
            {
                string[] opiszw = sqlHandler.SQL_Title();

                if (opiszw == null) return;

                if (opiszw[0] == null) opiszw[0] = "Brak opisu";
                if (opiszw[1] == null) opiszw[0] = "nieprzypisany";
                if (opiszw[2] == null) opiszw[0] = "";
                if (opiszw[3] == null) opiszw[0] = "";
                if (opiszw[4] == null) opiszw[0] = "";
                if (opiszw[5] == null) opiszw[0] = "";
                if (opiszw[6] == null) opiszw[0] = "";

                _PUBLIC_SqlLite.DODAJ_REKORD_SQL_ZAPYTANIA(opiszw[0], txtCode.Text.Trim(), opiszw[1], opiszw[2], opiszw[3], opiszw[4], opiszw[5], opiszw[6]);
            }
            else
            {
                _PUBLIC_SqlLite.ZMIEN_OPIS_POZIOMU_SQL_ZAPYTANIA(txtCode.Text.Trim(), id_rec.ToString());
            }

        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.Filter = "Sql pliki|*.sql|Text files|*.txt|Wszystkie pliki|*.*";
            fileChooser.AddExtension = true;
            fileChooser.OverwritePrompt = true;
            fileChooser.Title = "Wybierz ścieżkę i nazwę pliku";
            if (fileChooser.ShowDialog().Value == false)
                return;

            using (StreamWriter sw = new StreamWriter(fileChooser.FileName))
            {
                sw.Write(txtCode.Text);
                sw.Close();
            }
        }

        private void Connect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Connect();
        }

        private void Disconnect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                sqlHandler.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateUIStatus();
            }
        }

        private void Parse_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateUIStatus();

            if (txtCode.Text.Trim() == "") return;

            string sql = txtCode.Text;

            if (txtCode.SelectedText.Trim() != "")
            {
                sql = txtCode.SelectedText.Trim();
            }

            try
            {
                SqlError[] errors;

                errors = sqlHandler.Parse(sql);

                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);

                if (errors.Length == 0)
                {

                    double info = sqlHandler.RowCount(sql);

                    //MessageBox.Show("Zapytanie wykonano pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (info < 0)
                    {
                        UpdateUIStatus(false, "Sprawdź poprawność zapytania: [" + info.ToString() + "]");
                    }
                    else
                    {
                        UpdateUIStatus(true, "Zapytanie nie ma błędów [ilość wyników: " + info.ToString() + "]");
                    }

                    b_wykonaj.IsEnabled = true;
                    b_wykonaj_ds.IsEnabled = true;

                    if ((double)info > 100000)
                    {
                        UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + info);
                        //}
                        //else
                        //{
                        //    UpdateUIStatus(true, "Zapytanie zwróciło wyników: " + info);
                    }
                }
                else
                {
                    UpdateUIStatus(false, "Sprawdź składnie SQL");
                    b_wykonaj.IsEnabled = false;
                    b_wykonaj_ds.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                b_wykonaj.IsEnabled = false;
                b_wykonaj_ds.IsEnabled = false;
            }
        }

        private void Execute_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateUIStatus();

            try
            {
                b_wykonaj.IsEnabled = false;
                b_wykonaj_ds.IsEnabled = false;
                SqlError[] errors;

                string strSQL = "";

                if (txtCode.SelectedText.Trim() == "")
                {
                    strSQL = txtCode.Text;
                }
                else
                {
                    strSQL = txtCode.SelectedText;
                }

                DataTable result = sqlHandler.Execute(strSQL, out errors, procedura);
                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);

                if ((double)result.Rows.Count > 100000)
                {
                    UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + result.Rows.Count.ToString());
                }
                else
                {
                    UpdateUIStatus(true, "Zapytanie zwróciło wyników: " + result.Rows.Count.ToString()); ;
                }

                if (result.Rows.Count > 0)
                {
                    ResultWindow resultWindow = new ResultWindow(result, sqlHandler.Nazwa_Tabeli(), sqlHandler.Wartosc_Like());
                    resultWindow.Topmost = (bool)ChkTopmost.IsChecked; // Ustawienie okna wynikowego na zawsze na wierzchu
                    resultWindow.Show();
                }
                else if (errors == null)
                {
                    MessageBox.Show("Ilość wyników <null>.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Execute_ExecutedDS(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateUIStatus();

            try
            {
                b_wykonaj.IsEnabled = false;
                b_wykonaj_ds.IsEnabled = false;
                SqlError[] errors;

                string strSQL = "";

                if (txtCode.SelectedText.Trim() == "")
                {
                    strSQL = txtCode.Text;
                }
                else
                {
                    strSQL = txtCode.SelectedText;
                }

                DataSet resultDS = sqlHandler.ExecuteDS(strSQL, out errors, Convert.ToInt32(IntTimeOut.Text));
                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);
                int i_tx = 0;

                var rows = resultDS.Tables
                  .Cast<DataTable>()
                  .Sum(x => x.Rows.Count);

                //var rowsWOZ = resultDS.Tables.Cast<DataTable>().Select(dt => dt.Rows.Count > 0).Count();

                //MessageBox.Show("Serwer zwrócił: " + resultDS.Tables.Count.ToString() + " tabel. Z czego: "
                //    + rowsWOZ.ToString() + " ma ilość wierszy większą od 0 suma wierszy wszystkich tabel wynosi: " + rows.ToString(), "Dodatkowa informacja!", MessageBoxButton.OK, MessageBoxImage.Information);

                if (resultDS.Tables.Count == 1)
                {

                    if (resultDS.Tables[0].Rows.Count > 100000)
                    {
                        UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + resultDS.Tables[0].Rows.Count.ToString());
                    }
                    else
                    {
                        UpdateUIStatus(true, "Zapytanie zwróciło wyników: " + resultDS.Tables[0].Rows.Count.ToString());
                    }

                    if (resultDS.Tables[0].Rows.Count > 0)
                        new ResultWindow(resultDS.Tables[0], sqlHandler.Nazwa_Tabeli(), sqlHandler.Wartosc_Like()).Show();

                    else if (errors == null)
                        MessageBox.Show("Ilość wyników <null>.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    foreach (DataTable result in resultDS.Tables)
                    {

                        if ((double)result.Rows.Count > 0)
                        {
                            i_tx++;

                            if ((double)result.Rows.Count > 100000)
                            {
                                UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + result.Rows.Count.ToString());
                            }
                            else
                            {
                                UpdateUIStatus(true, "Zapytanie zwróciło wyników: " + result.Rows.Count.ToString()); ;
                            }

                            if (result.Rows.Count > 0)
                                new ResultWindow(result, sqlHandler.Nazwa_Tabeli(), sqlHandler.Wartosc_Like(), result.TableName, resultDS,
                                    "Ilość tabel " + resultDS.Tables.Count.ToString() + " / w:" + rows.ToString()).Show();

                            else if (errors == null)
                                MessageBox.Show("Ilość wyników <null>.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);

                            break;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void IsConnected_Executed(object sender, CanExecuteRoutedEventArgs e)
        {

            if (IsLoaded)
            {
                e.CanExecute = sqlHandler.IsConnected;
            }
 
        }

        #endregion

        private void close_sql(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true) return;

            try
            {
                sqlHandler.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string _currentInput = "";
        private string _currentSuggestion = "";
        private string _currentText = "";

        private int _selectionStart;
        private int _selectionLength;


        private static string[] SuggestionValuesTbName;
        private static string[] SuggestionValuesColName;
        private void textChengen(object sender, TextChangedEventArgs e)
        {
            b_wykonaj.IsEnabled = false;
            b_wykonaj_ds.IsEnabled = false;

            if (auto_u == true)
            {
                string[] strCurText = txtCode.Text.Split(' ');

                var input = strCurText[strCurText.Length - 1];

                if (strCurText.Length > 1 && input.Length > 1)
                {
                    if (strCurText[strCurText.Length - 2].ToLower() == "from"
                    || strCurText[strCurText.Length - 2].ToLower() == "update" || strCurText[strCurText.Length - 2].ToLower() == "insert into")
                    {

                        SuggestionValuesColName = null;

                        if (input.Length > _currentInput.Length && input != _currentSuggestion)
                        {

                            if (SuggestionValuesTbName == null) return;

                            _currentSuggestion = SuggestionValuesTbName.FirstOrDefault(x => x.StartsWith(input));
                            if (_currentSuggestion != null)
                            {
                                _currentText = _currentSuggestion;
                                _selectionStart = input.Length;
                                _selectionLength = _currentSuggestion.Length - input.Length;

                                char znak = Convert.ToChar(input.Substring(input.Length - 1, 1));
                                _currentText = _currentText.TrimStart(znak);
                                txtCode.Text += _currentText.Substring(_selectionStart, _currentText.Length - _selectionStart);

                                int ile = 0;
                                //txtCode.Select(_selectionStart + 1, _selectionLength);
                                for (int i = 0; i < strCurText.Length; i++)
                                {
                                    ile += strCurText[i].Length + 1;
                                }

                                txtCode.Select(ile - 1, _selectionLength);

                            }
                        }
                        _currentInput = input;

                    }

                    if (strCurText[strCurText.Length - 2].ToLower() == "select"
                    || strCurText[strCurText.Length - 2].ToLower() == "where" || strCurText[strCurText.Length - 2].ToLower() == "and"
                    || strCurText[strCurText.Length - 2].ToLower() == "or")
                    {

                        if (input.Length > _currentInput.Length && input != _currentSuggestion && SuggestionValuesColName != null)
                        {
                            _currentSuggestion = SuggestionValuesColName.FirstOrDefault(x => x.StartsWith(input));
                            if (_currentSuggestion != null)
                            {
                                _currentText = _currentSuggestion;
                                _selectionStart = input.Length;
                                _selectionLength = _currentSuggestion.Length - input.Length;

                                char znak = Convert.ToChar(input.Substring(input.Length - 1, 1));
                                _currentText = _currentText.TrimStart(znak);
                                txtCode.Text += _currentText.Substring(_selectionStart, _currentText.Length - _selectionStart);

                                int ile = 0;
                                //txtCode.Select(_selectionStart + 1, _selectionLength);
                                for (int i = 0; i < strCurText.Length; i++)
                                {
                                    ile += strCurText[i].Length + 1;
                                }

                                txtCode.Select(ile - 1, _selectionLength);

                            }
                        }
                        _currentInput = input;
                    }

                }

                if (_currentSuggestion != "" && _currentSuggestion != null && SuggestionValuesColName == null)
                {
                    var distinctRows = (from r in dt_str_danych.AsEnumerable()
                                        .Where(myRow => myRow.Field<string>("TableName") == _currentSuggestion.ToLower())
                                        select r["ColumnName"]).Distinct().ToList();

                    //var distinctRows = from myRow in dt_str_danych.Rows.Cast<DataRow>()
                    //              where myRow.Field<string>("TableName") == _currentSuggestion
                    //              select myRow;

                    string myStringOutput = String.Join(",", distinctRows.ToArray().Select(p => p.ToString()).ToArray());

                    SuggestionValuesColName = myStringOutput.Split(',');

                }

                _currentSuggestion = "";

            }
        }
        private void MenuChange(Object sender, RoutedEventArgs ags)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null || cxm == null) return;

            switch (rb.Name)
            {
                case "rbCustom":
                    txtCode.ContextMenu = cxm;
                    break;
                case "rbDefault":
                    // Clearing the value of the ContextMenu property
                    // restores the default TextBox context menu.
                    txtCode.ClearValue(ContextMenuProperty);
                    break;
                case "rbDisabled":
                    // Setting the ContextMenu propety to
                    // null disables the context menu.
                    txtCode.ContextMenu = null;
                    break;
                default:
                    break;
            }
        }

        void ClickPaste(Object sender, RoutedEventArgs args) { txtCode.Paste(); }
        void ClickCopy(Object sender, RoutedEventArgs args) { txtCode.Copy(); }
        void ClickCut(Object sender, RoutedEventArgs args) { txtCode.Cut(); }
        void ClickSelectAll(Object sender, RoutedEventArgs args) { txtCode.SelectAll(); }
        void ClickClear(Object sender, RoutedEventArgs args) { txtCode.Clear(); }
        void ClickUndo(Object sender, RoutedEventArgs args) { txtCode.Undo(); }
        void ClickRedo(Object sender, RoutedEventArgs args) { txtCode.Redo(); }
        void ClickLike(Object sender, RoutedEventArgs args)
        {
            //string sql = txtCode.SelectedText.ToLower();
            string sql = txtCode.SelectedText;
            //sql = sql.Replace("\nset", " ");
            //sql = sql.Replace("\n", " ");
            //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
            string[] spl = sql.Split(' ');
            string sqlr = "";
            foreach (var sub in spl)
            {
                if (sub.IndexOf("=") > -1)
                {
                    sqlr += "like ";
                }
                else
                {
                    sqlr += sub + " ";
                }
            }

            txtCode.SelectedText = sqlr;
        }
        void ClickLikeProc(Object sender, RoutedEventArgs args)
        {
            string sql = txtCode.SelectedText;
            sql = sql.Replace("\nset", "");
            sql = sql.Replace("\n", "");
            sql = sql.Replace("\r\n", "");
            sql = sql.Replace("\t", "");
            string[] spl = sql.Split((char)39);
            string sqlr = "";
            foreach (var sub in spl)
            {
                string str_cls = sub.Trim();

                if (str_cls.TrimEnd().EndsWith("="))
                {
                    sqlr += str_cls.TrimEnd('=').TrimEnd() + " like ";
                }
                else
                {
                    if (str_cls.Trim() != "" && str_cls.IndexOf("=") < 0)
                    {
                        sqlr += "'%" + str_cls + "%'" + "\r\n";
                    }
                    else
                    {
                        sqlr += str_cls;
                    }
                }
            }

            txtCode.SelectedText = sqlr;
        }
        void ClickRowna(Object sender, RoutedEventArgs args)
        {
            //string sql = txtCode.SelectedText.ToLower();
            string sql = txtCode.SelectedText;
            //sql = sql.Replace("\nset", " ");
            //sql = sql.Replace("\n", " ");
            //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
            string[] spl = sql.Split(' ');
            string sqlr = "";
            foreach (var sub in spl)
            {
                if (sub.ToLower().StartsWith("like"))
                {
                    sqlr += "= ";
                }
                else
                {
                    sqlr += sub + " ";
                }
            }

            txtCode.SelectedText = sqlr;
        }
        void ClickAnd(Object sender, RoutedEventArgs args)
        {
            //string sql = txtCode.SelectedText.ToLower();
            string sql = txtCode.SelectedText;
            //sql = sql.Replace("\nset", " ");
            //sql = sql.Replace("\n", " ");
            //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
            string[] spl = sql.Split(' ');
            string sqlr = "";
            foreach (var sub in spl)
            {
                if (sub.ToLower().ToString().StartsWith("or") || sub.ToLower().ToString().EndsWith("\r\nor"))
                {
                    sqlr += "and ";
                }
                else
                {
                    sqlr += sub + " ";
                }
            }

            txtCode.SelectedText = sqlr;
        }
        void ClickOr(Object sender, RoutedEventArgs args)
        {
            //string sql = txtCode.SelectedText.ToLower();
            string sql = txtCode.SelectedText;
            //sql = sql.Replace("\nset", " ");
            //sql = sql.Replace("\n", " ");
            //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
            string[] spl = sql.Split(' ');
            string sqlr = "";
            foreach (var sub in spl)
            {
                if (sub.ToLower().ToString().StartsWith("and") || sub.ToLower().ToString().EndsWith("\r\nand"))
                {
                    sqlr += "or ";
                }
                else
                {
                    sqlr += sub + " ";
                }
            }

            txtCode.SelectedText = sqlr;
        }
        void ClickSelectLine(Object sender, RoutedEventArgs args)
        {
            int lineIndex = txtCode.GetLineIndexFromCharacterIndex(txtCode.CaretIndex);
            int lineStartingCharIndex = txtCode.GetCharacterIndexFromLineIndex(lineIndex);
            int lineLength = txtCode.GetLineLength(lineIndex);
            txtCode.Select(lineStartingCharIndex, lineLength);
        }

        void CxmOpened(Object sender, RoutedEventArgs args)
        {
            // Only allow copy/cut if something is selected to copy/cut.
            if (txtCode.SelectedText == "")
            {
                cxmItemCopy.IsEnabled = false;
                cxmItemLike.IsEnabled = false;
                cxmItemLikeProc.IsEnabled = false;
                cxmItemLikeRow.IsEnabled = false;
                cxmItemLikeAnd.IsEnabled = false;
                cxmItemLikeOr.IsEnabled = false;
                cxmItemColumsTable.IsEnabled = false;
                cxmItemLowerText.IsEnabled = false;
                cxmItemUpperText.IsEnabled = false;

            }
            else
            {
                cxmItemCopy.IsEnabled = true;
                cxmItemLike.IsEnabled = true;
                cxmItemLikeProc.IsEnabled = true;
                cxmItemLikeRow.IsEnabled = true;
                cxmItemLikeAnd.IsEnabled = true;
                cxmItemLikeOr.IsEnabled = true;
                cxmItemColumsTable.IsEnabled = true;
                cxmItemLowerText.IsEnabled = true;
                cxmItemUpperText.IsEnabled = true;

            }

            // Only allow paste if there is text on the clipboard to paste.
            if (Clipboard.ContainsText())
                cxmItemPaste.IsEnabled = true;
            else
                cxmItemPaste.IsEnabled = false;
        }

        private void ClickColumnTable(object sender, RoutedEventArgs e)
        {

            if (dt_str_danych.Rows.Count == 0)
            {
                dt_str_danych = sqlHandler.StrukturaTabel();
                auto_u = true;
                BitmapImage Image1 = new BitmapImage(new Uri("/SQL/SQL_SELECT/Images/auto_on.png", UriKind.Relative));
                auto_on.Source = Image1;
            }

            string sql = txtCode.SelectedText;

            var distinctRows = (from r in dt_str_danych.AsEnumerable()
                                        .Where(myRow => myRow.Field<string>("TableName") == sql.ToLower().Trim())
                                select r["ColumnName"] + "[" + r["MaxLength"] + "]").ToList();
          
            if (distinctRows.Count > 0)
            {
                ColumsTable spc = new ColumsTable();

                var positionOnRootGrid = Mouse.GetPosition(this);
                var xDifference = (int)(positionOnRootGrid.X - spc.ActualWidth / 2);
                var yDifference = (int)(positionOnRootGrid.Y - spc.ActualHeight / 2);

                spc.Left = xDifference;
                spc.Top = yDifference;

                spc.itContr.ItemsSource = distinctRows;
                spc.ShowDialog();

                txtCode.CaretIndex = txtCode.Text.Length;
                txtCode.ScrollToEnd();
                txtCode.Focus();

                if (spc.columsselect.Trim() != "")
                {
                    txtCode.Text += "\r\n" + "where " + spc.columsselect.Trim().TrimEnd(',').Trim();
                }

            }

        }

        private void ClickSelecAllTable(object sender, RoutedEventArgs e)
        {
            if (dt_str_danych.Rows.Count == 0)
            {
                dt_str_danych = sqlHandler.StrukturaTabel();
                auto_u = true;
                BitmapImage Image1 = new BitmapImage(new Uri("/SQL/SQL_SELECT/Images/auto_on.png", UriKind.Relative));
                auto_on.Source = Image1;
            }

            var distinctRows = (from r in dt_str_danych.AsEnumerable()
                                        .Where(myRow => myRow.Field<string>("TableName") != "")
                                select r["TableName"].ToString()).Distinct().ToList();

            if (distinctRows.Count > 0)
            {
                ColumsTable spc = new ColumsTable();

                var positionOnRootGrid = Mouse.GetPosition(this);
                var xDifference = (int)(positionOnRootGrid.X - spc.ActualWidth / 2);
                var yDifference = (int)(positionOnRootGrid.Y - spc.ActualHeight / 2);

                spc.Left = xDifference;
                spc.Top = yDifference;

                spc.itContr.ItemsSource = distinctRows;
                spc.ShowDialog();

                txtCode.CaretIndex = txtCode.Text.Length;
                txtCode.ScrollToEnd();
                txtCode.Focus();

                string[] zws = spc.columsselect.TrimEnd(',').Split(',');

                switch (zws.Length)
                {
                    case 1:
                        if (spc.columsselect.TrimEnd(',').Trim() != "")
                        {
                            txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',').Trim();
                        }
                        break;
                    case 2:

                        if (zws[1].Trim() == "")
                        {
                            txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',').Trim();
                        }
                        else
                        {
                            txtCode.Text += "\r\n" + "select top 100 * from " + zws[0].Trim() + " " + "a" +
                            " inner join " + zws[1].Trim() + " " + "b" + " on " + "a" + ".???? = "
                            + "b" + ".????";
                        }

                        break;
                    case 3:
                        txtCode.Text += "\r\n" + "select top 100 * from " + zws[0].Trim() + " " + "a" +
                            " inner join " + zws[1].Trim() + " " + "b" + " on " + "a" + ".???? = "
                            + "b" + ".????" +
                            " inner join " + zws[2].Trim() + " " + "c" + " on " + "a" + ".???? = " +
                            "c" + ".????";
                        break;
                    case 4:
                        txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',');
                        break;
                    case 5:
                        txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',');
                        break;
                    case 6:
                        txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',');
                        break;
                    default:
                        txtCode.Text += "\r\n" + "select top 100 * from " + spc.columsselect.Trim().TrimEnd(',');
                        break;
                }
            }
        }
        private void click_procedura(object sender, RoutedEventArgs e)
        {
            if (procedura == false)
            {
                praca_procedura.Header = "Ustaw tryb pracy - wykonaj";
                procedura = true;
            }
            else
            {
                praca_procedura.Header = "Ustaw tryb pracy - procedura";
                procedura = false;
            }

        }

        private void ClickUpperText(object sender, RoutedEventArgs e)
        {
            txtCode.SelectedText = txtCode.SelectedText.ToUpper();
        }

        private void ClickLowerText(object sender, RoutedEventArgs e)
        {
            txtCode.SelectedText = txtCode.SelectedText.ToLower();
        }

        private void click_coments(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).Tag.ToString() == "comments")
            {
                txtCode.SelectedText = "--" + txtCode.SelectedText.Replace(System.Environment.NewLine, System.Environment.NewLine + "--"); //\r\n
            }
            else if (((FrameworkElement)sender).Tag.ToString() == "uncomments")
            {
                txtCode.SelectedText = txtCode.SelectedText.TrimStart('-').TrimStart('-').Replace(System.Environment.NewLine + "--", System.Environment.NewLine);
            }
        }

        private void replace_text_click(object sender, RoutedEventArgs e)
        {
            if (TXT_OLD.Text == "" || TXT_NEW.Text == "")
            {
                MessageBox.Show("Wypełnij pola tekst przed i po zmianie [CTRL + Z - cofnij]", "Ważna informacja!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            txtCode.Text = txtCode.Text.Replace(TXT_OLD.Text, TXT_NEW.Text);
        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            var caretIndex = txtCode.CaretIndex;
            var line = txtCode.GetLineIndexFromCharacterIndex(caretIndex);
            var colStart = txtCode.GetCharacterIndexFromLineIndex(line);
            var pos = caretIndex - colStart;
            var lineCount = txtCode.LineCount;

            txtLineKlik.Text = ($@"Linia {line + 1}, IndeksPierwsz. {colStart}, IndeksOgólny {caretIndex}, Poz {pos}, Linii: {lineCount}");
        }

        private void paste_and_move_click(object sender, RoutedEventArgs e)
        {
            TXT_OLD.Text = TXT_NEW.Text;
            TXT_NEW.Text = Clipboard.GetText();
        }
        int indeks_old = 0;
		private void find_next_text_click(object sender, RoutedEventArgs e)
		{

			if (TXT_FIND.Text.Length < 2)
			{
				txtCode.Select(0, 0);
				return;

			}
			//if (currentIndex == 0 && searchLength == 0) return;

			string searchText = TXT_FIND.Text.TrimEnd();
			string textToSearchIn = txtCode.Text;
			int currentIndex = intfindindex;
			int searchLength = searchText.Length;

			while (currentIndex < textToSearchIn.Length)
			{
				currentIndex = textToSearchIn.IndexOf(searchText, currentIndex, StringComparison.InvariantCultureIgnoreCase);

				if (currentIndex == -1)
				{
					break;
				}

				txtCode.Select(currentIndex, searchLength);
				currentIndex += searchLength;

				intfindindex = currentIndex;

				if (searchLength > -1) break;
			}

            if(indeks_old == intfindindex)
            {
			     intfindindex = 0;
			}
            else
            {
                indeks_old = intfindindex;
			}
		}
		
		private int intfindindex = 0;
		private void TextChanged(object sender, TextChangedEventArgs e)
		{
			intfindindex = 0;

			if (TXT_FIND.Text.Length < 2)
            {
                txtCode.Select(0, 0);
                return;

			}
			//if (currentIndex == 0 && searchLength == 0) return;

			string searchText = TXT_FIND.Text.TrimEnd();
			string textToSearchIn = txtCode.Text;
			int currentIndex = intfindindex;
			int searchLength = searchText.Length;

			while (currentIndex < textToSearchIn.Length)
			{
				currentIndex = textToSearchIn.IndexOf(searchText, currentIndex, StringComparison.InvariantCultureIgnoreCase);

				if (currentIndex == -1)
				{
					break;
				}

				txtCode.Select(currentIndex, searchLength);
				currentIndex += searchLength;

                intfindindex = currentIndex;

				if (searchLength>-1) break;
			}
		}
        private void IntTimeOut_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }

}