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

namespace DBDT.SQL.SQL_SELECT
{
    public partial class MainWindowSQL : UserControl
    {
        private SqlHandler sqlHandler;

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
                MessageBox.Show(ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateUIStatus();
            }
        }

        private void UpdateUIStatus()
        {
            if (sqlHandler.IsConnected)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("Images/bullet_blue.png", UriKind.Relative);
                logo.EndInit();
                connStatusIcon.Source = logo;
                txtStatus.Text = "Connected:  ";
                txtConnection.Text = sqlHandler.ConnectionString;
            }
            else
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("Images/bullet_red.png", UriKind.Relative);
                logo.EndInit();
                connStatusIcon.Source = logo;
                txtStatus.Text = "Not Connected";
                txtConnection.Text = string.Empty;
            }
        }
        #endregion

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect();
        }
        #endregion

        #region Commands
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "Sql files|*.sql|Text files|*.txt|All files|*.*";
            fileChooser.CheckPathExists = true;
            fileChooser.Multiselect = false;
            fileChooser.Title = "Choose a file containing T-SQL code to open";
            if (fileChooser.ShowDialog().Value == false)
                return;

            using (StreamReader sr = new StreamReader(fileChooser.FileName))
            {
                txtCode.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.Filter = "Sql files|*.sql|Text files|*.txt|All files|*.*";
            fileChooser.AddExtension = true;
            fileChooser.OverwritePrompt = true;
            fileChooser.Title = "Choose a path and file name";
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
            try
            {
                SqlError[] errors = sqlHandler.Parse(txtCode.Text);
                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);
                if (errors.Length == 0)
                    MessageBox.Show("Zapytanie wykonano pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Execute_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SqlError[] errors;
                DataTable result = sqlHandler.Execute(txtCode.Text, out errors);
                errorsGrid.ItemsSource = errors;
                errorsExpander.IsExpanded = (errors.Length != 0);

                if (result.Rows.Count > 0)
                    new ResultWindow(result).Show();
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
                MessageBox.Show(ex.Message, "Błąd zapytania SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IsConnected_Executed(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsLoaded)
                e.CanExecute = sqlHandler.IsConnected;
        }

        #endregion
    }
}
