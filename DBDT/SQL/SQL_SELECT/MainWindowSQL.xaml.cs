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

namespace DBDT.SQL.SQL_SELECT
{
    public partial class MainWindowSQL : UserControl
    {
        private SqlHandler sqlHandler;
        public bool procedura = false;
        private bool auto_u = false;
        private DataTable dt_str_danych = new DataTable();

        public MainWindowSQL()
        {
            InitializeComponent();
            sqlHandler = new SqlHandler();
        }

        #region Private Methods
        private void Connect()
        {
            try
            {

                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia order by id desc");

                if (dt.Rows.Count == 0)
                {
                    return;
                }

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
                    logo.UriSource = new Uri("Images/staus_notok.png", UriKind.Relative);
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
                //DataTable dt_str_danych = new DataTable();

                dt_str_danych = sqlHandler.StrukturaTabel();

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
        void currentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect();
            b_wykonaj.IsEnabled = false;

            Window currentWindow = Window.GetWindow(this);
            currentWindow.Closing += currentWindow_Closing;
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

        private void Save_ExecutedSQL(object sender, ExecutedRoutedEventArgs e)
        {

            if (txtCode.Text.Trim() == "") return;

            string opis = sqlHandler.SQL_Title();

            _PUBLIC_SqlLite.DODAJ_REKORD_SQL_ZAPYTANIA(opis, txtCode.Text.Trim());
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
            try
            {
                SqlError[] errors;

                if (txtCode.SelectedText.Trim() == "")
                {
                    errors = sqlHandler.Parse(txtCode.Text);
                }
                else
                {
                    errors = sqlHandler.Parse(txtCode.SelectedText);
                }

                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);
                if (errors.Length == 0)
                {

                    double info = sqlHandler.RowCount(txtCode.Text);

                    //MessageBox.Show("Zapytanie wykonano pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (info < 0)
                    {
                        UpdateUIStatus(false, "Sprawdź poprawność zapytania: " + info.ToString() + "]");
                    }
                    else
                    {
                        UpdateUIStatus(true, "Zapytanie nie ma błędów [ilość wyników: " + info.ToString() + "]");
                    }

                    b_wykonaj.IsEnabled = true;

                    if ((int)info > 100000)
                    {
                        UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + info);
                    }
                    else
                    {
                        UpdateUIStatus(false, "Zapytanie wyników: " + info);
                    }
                }
                else
                {
                    UpdateUIStatus(false, "Sprawdź składnie SQL");
                    b_wykonaj.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                b_wykonaj.IsEnabled = false;
            }
        }

        private void Execute_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateUIStatus();

            try
            {
                b_wykonaj.IsEnabled = false;
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

                UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + result.Rows.Count.ToString());

                if (result.Rows.Count > 0)
                    new ResultWindow(result, sqlHandler.Nazwa_Tabeli()).Show();

                else if (errors == null)
                    MessageBox.Show("Ilość wyników <null>.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                /* Please Note:
                 * This catch block is essentially redundant because the SqlConnection object's property FireInfoMessageEventOnUserErrors within SqlHandler is set to true.
                 * When FireInfoMessageEventOnUserErrors = false:  Errors will be raised as Exceptions (and caught by this catch block, in this instance).
                 * When FireInfoMessageEventOnUserErrors = true:  Errors will be raised as an Event (InfoMessage).
                 * SqlHandler encapsulates and hides the implementation of the event, and both the SqlHandler.Execute and SqlHandler.Parse methods will return these errors caught through the event
                */
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IsConnected_Executed(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsLoaded)
                e.CanExecute = sqlHandler.IsConnected;
            //b_wykonaj.IsEnabled = true;
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
                                        .Where(myRow => myRow.Field<string>("TableName") == _currentSuggestion)
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
            //sql = sql.Replace("\nset", " ");
            //sql = sql.Replace("\n", " ");
            //sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);
            string[] spl = sql.Split(' ');
            string sqlr = "";
            foreach (var sub in spl)
            {

                if (sub.StartsWith("="))
                {
                    sqlr += "like ";
                }
                else if (sub.IndexOf((char)39) > -1)
                {

                    int starz = sub.IndexOf((char)39);
                    int koniecz = sub.IndexOf((char)39, starz + 1);

                    if (sub.StartsWith(Convert.ToString((char)39)))
                    {
                        sqlr += "'%" + sub.TrimStart((char)39) + " ";
                    }

                    if (sub.EndsWith(Convert.ToString((char)39)))
                    {
                        sqlr += "" + sub.TrimEnd((char)39) + "%' ";
                    }
                }
                else
                {
                    sqlr += sub + " ";
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
                if (sub.ToLower().ToString().StartsWith("or"))
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
                if (sub.ToLower().ToString().StartsWith("and"))
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
                cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = false;
                cxmItemLike.IsEnabled = cxmItemLike.IsEnabled = false;
                cxmItemLikeProc.IsEnabled = cxmItemLikeProc.IsEnabled = false;
                cxmItemLikeRow.IsEnabled = cxmItemLikeRow.IsEnabled = false;
                cxmItemLikeAnd.IsEnabled = cxmItemLikeAnd.IsEnabled = false;
                cxmItemLikeOr.IsEnabled = cxmItemLikeOr.IsEnabled = false;

            }
            else
            {
                cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = true;
                cxmItemLike.IsEnabled = cxmItemLike.IsEnabled = true;
                cxmItemLikeProc.IsEnabled = cxmItemLikeProc.IsEnabled = true;
                cxmItemLikeRow.IsEnabled = cxmItemLikeRow.IsEnabled = true;
                cxmItemLikeAnd.IsEnabled = cxmItemLikeAnd.IsEnabled = true;
                cxmItemLikeOr.IsEnabled = cxmItemLikeOr.IsEnabled = true;

            }

            // Only allow paste if there is text on the clipboard to paste.
            if (Clipboard.ContainsText())
                cxmItemPaste.IsEnabled = true;
            else
                cxmItemPaste.IsEnabled = false;
        }

    }

}
