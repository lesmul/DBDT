using System;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Linq;

namespace DBDT.SQL.SQL_SELECT
{
    public class SqlHandler
    {
        #region Commands
        public static RoutedUICommand ParseCommand;
        public static RoutedUICommand ExecuteCommand;
        public static RoutedUICommand ConnectCommand;
        public static RoutedUICommand DisconnectCommand;
        public static RoutedUICommand ParseOpis;

        static SqlHandler()
        {
            InputGestureCollection parseGestures = new InputGestureCollection();
            parseGestures.Add(new KeyGesture(Key.F6));
            ParseCommand = new RoutedUICommand("Parse", "Parse", typeof(SqlHandler), parseGestures);

            ParseOpis = new RoutedUICommand("SQL_Title", "SQL_Title", typeof(SqlHandler));

            InputGestureCollection executeGestures = new InputGestureCollection();
            executeGestures.Add(new KeyGesture(Key.F5));
            ExecuteCommand = new RoutedUICommand("Execute", "Execute", typeof(SqlHandler), executeGestures);

            ConnectCommand = new RoutedUICommand("Connect", "Connect", typeof(SqlHandler));

            DisconnectCommand = new RoutedUICommand("Disconnect", "Disconnect", typeof(SqlHandler));
           
        }
        #endregion

        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private List<SqlError> errors;
        private ArrayList infoCount;

        public string ConnectionString
        {
            get { return conn.ConnectionString; }
            private set { conn.ConnectionString = value; }
        }

        public bool IsConnected
        {
            get { return conn.State == ConnectionState.Open; }
        }

        public SqlHandler()
        {
            conn = new SqlConnection();
            cmd = new SqlCommand("", conn);
            adapter = new SqlDataAdapter(cmd);
            errors = new List<SqlError>(5);

            ConnectionString = "Data Source=; Initial Catalog=; Integrated Security=SSPI";
            conn.FireInfoMessageEventOnUserErrors = true; //when true, the SqlCommand object will not throw an Exception when errors occur
            conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);
        }

        public void Connect(string connStr)
        {
            if (IsConnected)
                Disconnect();
            conn.ConnectionString = connStr;
            conn.Open();
        }

        public void Disconnect()
        {
            conn.Close();
        }

        public DataTable Execute(string sqlText, out SqlError[] errorsArray)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można wykonać zapytania SQL, gdy połączenie jest zamknięte!");

            errors.Clear();
            cmd.CommandText = sqlText;
            DataTable tbl = new DataTable();
            adapter.Fill(tbl);
            errorsArray = errors.ToArray();
            return tbl;
        }

        public SqlError[] Parse(string sqlText)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można przeanalizować zapytania SQL, gdy połączenie jest zamknięte!");

            errors.Clear();
            cmd.CommandText = "SET PARSEONLY ON";
            cmd.ExecuteNonQuery();

            cmd.CommandText = sqlText;
            cmd.ExecuteNonQuery(); //conn_InfoMessage is invoked for every error, e.g. 2 times for 2 errors
            
            cmd.CommandText = "SET PARSEONLY OFF";
            cmd.ExecuteNonQuery();

            return errors.ToArray();
        }

        public double RowCount(string sqlText)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można przeanalizować zapytania SQL, gdy połączenie jest zamknięte!");
            
            if(infoCount != null)
            {
                infoCount.Clear();
            }
            else
            {
                infoCount = new ArrayList();
            }
   
            string sql = sqlText.ToLower().Trim();
            sql = sql.Replace("\nset", " ");
            sql = sql.Replace("\n", " ");
            sql = Regex.Replace(sql, @"\s+",(match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);

            string sprCountSp = null;

            if (sql.StartsWith("select"))
            {
                int from = 0;
                from = sql.IndexOf("from") + 4;
                int spac = 0;
                spac = sql.IndexOf(" ", from);

                int top = -1;
                top = sql.IndexOf("select top");

                string sprCount = null;
                string sprFrom = null;
                string sprTopCount = "count(*)";

                if(spac <= from)
                {
                    sprFrom = sql.Substring(from, sql.Length - from);
                }
                else
                {
                    sprCount = sql.Substring(from + 4, spac);
                }

                if(sprFrom.IndexOf("order by") > -1)
                {
                    sprFrom = sprFrom.Substring(0, sprFrom.IndexOf("order by"));
                }

                sprCountSp = "select " + sprTopCount + " from " + sprFrom;

                if (top > -1)
                {
                    //SELECT * FROM (select top 6 * from [dbo].[DOKUMENTY]) T
                    string[] aSpacja = sql.Split(' '); 
                    sprCount = sql.Substring(top + 7, spac - 10);

                    sprCountSp = "select count(*) from (select " + aSpacja[1] + " " + aSpacja[2] + " * from " + sprFrom + ") T";
                }

            }
            else if(sql.StartsWith("delete"))
            {
                string[] aSpacja = sql.Split(' ');

                int firstIndex = sql.IndexOf("where");

                sprCountSp = "select count(*) from " + aSpacja[2] + " " + sql.Substring(firstIndex, sql.Length - firstIndex);

            }
            else if (sql.StartsWith("update"))
            {
                string[] aSpacja = sql.Split(' ');

                int firstIndex = sql.IndexOf("where");

                sprCountSp = "select count(*) from " + aSpacja[1] + " " + sql.Substring(firstIndex, sql.Length - firstIndex) ;
            }

            cmd.CommandText = sprCountSp;

            object count = 0;

            try
            {
                count = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (count == null)
            {
                return -1;
            }
            else
            {
                return (int)count;
            }
        }

        private void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            //ensure that all errors are caught
            SqlError[] errorsFound = new SqlError[e.Errors.Count];
            e.Errors.CopyTo(errorsFound, 0);
            errors.AddRange(errorsFound);
        }

        public string SQL_Title()
        {

            OpisSQL opis = new OpisSQL();
            opis.ShowDialog();
            var opisx = opis.TXT_OPIS_ZAPYTANIA_SQL.Text.Trim();
            return opisx;

        }
    }
}
