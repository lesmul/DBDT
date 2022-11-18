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

namespace DBDT.SQL.SQL_SELECT
{
    public partial class MainWindowSQL : UserControl
    {
        private SqlHandler sqlHandler;
        public bool procedura = false;
    
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
    ;                   return;
                    }
                }

                if(((DBDT.MainWindow)parentWindow).Container.Children.Count == 1)
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
            UpdateUIStatus(false, "Staus  - Analiza SQL");

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

                    if (info > 10000)
                    {
                        UpdateUIStatus(false, "Zapytanie zwróciło dużo wyników jest ich: " + info);
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

                if (result.Rows.Count > 0)
                    new ResultWindow(result, sqlHandler.Nazwa_Tabeli()).Show();
                    
                else if (errors == null)
                    MessageBox.Show("Nie zwrócono żadnych wyników.", "Wykonano zapytanie", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void textChengen(object sender, TextChangedEventArgs e)
        {
            b_wykonaj.IsEnabled = false;
        }

    }

}
