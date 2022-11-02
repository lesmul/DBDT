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
using System.Xml;
using System.Data.SqlTypes;
using System.Windows.Controls;

namespace DBDT.SQL.SQL_SELECT
{
    public class SqlHandler
    {
        #region Commands
        public static RoutedUICommand ParseCommand;
        public static RoutedUICommand ExecuteCommand;
        public static RoutedUICommand ExecuteCommand_ds;
        public static RoutedUICommand ConnectCommand;
        public static RoutedUICommand DisconnectCommand;
        public static RoutedUICommand ParseOpis;
        public static RoutedUICommand ParseTable;

        public static string NazwaTabeli;
        public static string[] WartoscLike;

        static SqlHandler()
        {
            InputGestureCollection parseGestures = new InputGestureCollection();
            ParseCommand = new RoutedUICommand("Parse", "Parse", typeof(SqlHandler), parseGestures);
            parseGestures.Add(new KeyGesture(Key.F6));

            ParseOpis = new RoutedUICommand("SQL_Title", "SQL_Title", typeof(SqlHandler));

            InputGestureCollection executeStrGestures = new InputGestureCollection();
            ParseTable = new RoutedUICommand("StrukturaTabel", "StrukturaTabel", typeof(SqlHandler), executeStrGestures);
            executeStrGestures.Add(new KeyGesture(Key.F12));

            InputGestureCollection executeGestures = new InputGestureCollection();
            ExecuteCommand = new RoutedUICommand("Execute", "Execute", typeof(SqlHandler), executeGestures);
            executeGestures.Add(new KeyGesture(Key.F5));

            InputGestureCollection executeCommand_ds = new InputGestureCollection();
            ExecuteCommand_ds = new RoutedUICommand("ExecuteDS", "ExecuteDS", typeof(SqlHandler), executeCommand_ds);
            executeCommand_ds.Add(new KeyGesture(Key.F11));

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
            NazwaTabeli = "";
            if (IsConnected)
                conn.Close();
        }

        public string Nazwa_Tabeli()
        {
            if (NazwaTabeli == null)
            {
                return "";
            }
            else
            {
                return NazwaTabeli.Trim();
            }
        }

        public string[] Wartosc_Like()
        {
            return WartoscLike;
        }

        public DataTable Execute(string sqlText, out SqlError[] errorsArray, bool bprocedura = false, DataTable dtexec = null)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można wykonać zapytania SQL, gdy połączenie jest zamknięte!");

            errors.Clear();

            WartoscLike = Wat_like(sqlText);

            if (bprocedura == false)
            {

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlText;
                DataTable tbl = new DataTable();
                adapter.Fill(tbl);
                errorsArray = errors.ToArray();
                return tbl;
            }
            else
            {
                //cmd.Parameters.Add("@Un", SqlDbType.Text);
                //cmd.Parameters["@Un"].Value = user.UserName;
                if (dtexec == null)
                {
                    throw new InvalidOperationException("Brak konfiguracji cmd.Parameters!");
                }
                if (dtexec.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Procedura jest niepoprawnie skonstruowana! Popraw dane.");
                }

                //cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = "Bob";
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dtexec.Rows)
                {
                    if (row["TypDanych"].ToString() == "adVarChar")
                    {
                        cmd.Parameters.Add(row["NazwaParametru"].ToString(), SqlDbType.VarChar).Value = row["WartoscParametru"].ToString();
                    }
                    else if (row["TypDanych"].ToString() == "adVarInteger")
                    {
                        cmd.Parameters.Add(row["NazwaParametru"].ToString(), SqlDbType.Int).Value = row["WartoscParametru"].ToString();
                    }

                }

                cmd.CommandText = dtexec.Rows[0]["NazwaProcedury"].ToString();
                cmd.ExecuteScalar();
                errorsArray = errors.ToArray();

                cmd.Parameters.Clear();

                return null;
            }

        }

        public DataSet ExecuteDS(string sqlText, out SqlError[] errorsArray)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można wykonać zapytania SQL, gdy połączenie jest zamknięte!");

            errors.Clear();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlText;
            DataSet tbl = new DataSet();
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

        public DataTable StrukturaTabel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można wykonać zapytania SQL, gdy połączenie jest zamknięte!");

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT SchemaName = LOWER(sch.name), TableName = LOWER(t.Name), ColumnName = LOWER(c.Name), TypeName = ty.Name,  MaxLength = c.max_length, Precision = c.precision, Scale = c.scale FROM sys.columns c INNER JOIN sys.tables t ON t.object_id = c.object_id INNER JOIN sys.schemas sch ON sch.schema_id = t.schema_id INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id ORDER BY TableName";

            DataTable tbl = new DataTable();
            adapter.Fill(tbl);
            return tbl;
        }

        public DataTable Wartosc_nazwy_pola_scrypt(string sql, bool CommandType_Text = true)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można wykonać zapytania SQL, gdy połączenie jest zamknięte!");

            DataTable tbl = new DataTable();

            try
            {
                if (CommandType_Text)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    adapter.Fill(tbl);
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sql;
                    adapter.Fill(tbl);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return tbl;
        }

        public double RowCount(string sqlText)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Nie można przeanalizować zapytania SQL, gdy połączenie jest zamknięte!");

            if (infoCount != null)
            {
                infoCount.Clear();
            }
            else
            {
                infoCount = new ArrayList();
            }

            string sql = sqlText.ToLower().Trim();
            // - \r\n

            if (sql.StartsWith("--"))
            {
                sql = sql.Substring(sql.IndexOf("\r\n") + 2, sql.Length - (sql.IndexOf("\r\n") + 2));
                if (sql.StartsWith("\r\n"))
                {
                    sql = sql.Substring(sql.IndexOf("\r\n") + 2, sql.Length - (sql.IndexOf("\r\n") + 2));
                }
            }

            sql = sql.Replace("\nset", " ");
            sql = sql.Replace("\n", " ");
            sql = Regex.Replace(sql, @"\s+", (match) => match.Value.IndexOf('\n') > -1 ? "\n" : " ", RegexOptions.Multiline);

            if (sql.StartsWith("exec")) return 0;

            string sprCountSp = null;

            string strdins = " ";
            if (sql.IndexOf("select distinct") > -1)
            {
                strdins = " distinct ";
            }

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

                if (spac <= from)
                {
                    sprFrom = sql.Substring(from, sql.Length - from);
                }
                else
                {
                    sprCount = sql.Substring(from + 4, spac);
                }

                if (sprFrom == null)
                {
                    NazwaTabeli = "???";
                    return -1;
                }

                if (sprFrom.IndexOf("order by") > -1)
                {
                    sprFrom = sprFrom.Substring(0, sprFrom.IndexOf("order by"));
                }

                sprCountSp = "select" + strdins + sprTopCount + " from " + sprFrom;

                if (top > -1)
                {
                    //SELECT * FROM (select top 6 * from [dbo].[DOKUMENTY]) T
                    string[] aSpacja = sql.Split(' ');
                    sprCount = sql.Substring(top + 7, spac - 10);

                    sprCountSp = "select" + strdins + " count(*) from (select " + aSpacja[1] + " " + aSpacja[2] + " * from " + sprFrom + ") T";
                }

                NazwaTabeli = sprFrom;

            }
            else if (sql.StartsWith("delete"))
            {
                string[] aSpacja = sql.Split(' ');

                int firstIndex = sql.IndexOf("where");

                sprCountSp = "select" + strdins + " count(*) from " + aSpacja[2] + " " + sql.Substring(firstIndex, sql.Length - firstIndex);
                NazwaTabeli = aSpacja[2].ToString();

            }
            else if (sql.StartsWith("update"))
            {
                string[] aSpacja = sql.Split(' ');

                int firstIndex = sql.IndexOf("where");

                sprCountSp = "select" + strdins + " count(*) from " + aSpacja[1] + " " + sql.Substring(firstIndex, sql.Length - firstIndex);
                NazwaTabeli = aSpacja[1].ToString();
            }

            if (sprCountSp == null) return -1;

            cmd.CommandText = sprCountSp;

            object count = 0;

            try
            {
                count = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public string[] SQL_Title(string str_opis = "", string str_poziom1 = "", string str_poziom2 = "", string str_poziom3 = "",
            string str_poziom4 = "", string str_poziom5 = "", string str_poziom6 = "", bool blokuj_zmiane_opisu = false)
        {

            OpisSQL opis = new OpisSQL();
            opis.TXT_OPIS_ZAPYTANIA_SQL.Text = str_opis;
            opis.Focus();
            if (blokuj_zmiane_opisu == true) opis.TXT_OPIS_ZAPYTANIA_SQL.IsEnabled = false;
            opis.CBpoziom1.Text = str_poziom1;
            opis.CBpoziom2.Text = str_poziom2;
            opis.CBpoziom3.Text = str_poziom3;
            opis.CBpoziom4.Text = str_poziom4;
            opis.CBpoziom5.Text = str_poziom5;
            opis.CBpoziom6.Text = str_poziom6;
            if (opis.ShowDialog() == true)
            {
                string[] danezw = { opis.TXT_OPIS_ZAPYTANIA_SQL.Text.Trim(), opis.CBpoziom1.Text.Trim(), opis.CBpoziom2.Text.Trim(),
                    opis.CBpoziom3.Text.Trim(), opis.CBpoziom4.Text.Trim(), opis.CBpoziom5.Text.Trim(), opis.CBpoziom6.Text.Trim() };

                return danezw;
            }
            else
            {
                return null;
            }

        }

        private string[] Wat_like(string sql)
        {
            if (sql.ToLower().IndexOf("where") < 0)
            {
                return null;
            }
            else
            {
                if (sql.ToLower().IndexOf("order by", sql.ToLower().IndexOf("where")) < 0)
                {
                    string str_c = sql.ToLower().Substring(sql.ToLower().IndexOf("where"));

                    str_c = str_c.Replace("like", " ");
                    str_c = str_c.Replace("=", " ");
                    str_c = str_c.Replace("'", " ");
                    string[] str_a = str_c.Split(' ');

                    if (str_c == "") return null;

                    str_a = str_a.Except(new List<string> { string.Empty }).ToArray();

                    if (str_a.Length > 2)
                    {
                        string[] zwrot = { str_a[1], str_a[2].TrimStart('%').TrimEnd('%') };
                        return zwrot;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {

                    if (sql.StartsWith("--"))
                    {
                        sql = sql.Substring(sql.IndexOf("\r\n") + 2, sql.Length - (sql.IndexOf("\r\n") + 2));
                        if (sql.StartsWith("\r\n"))
                        {
                            sql = sql.Substring(sql.IndexOf("\r\n") + 2, sql.Length - (sql.IndexOf("\r\n") + 2));
                        }
                    }

                    string str_c = "";

                    if (sql.ToLower().IndexOf("where") > -1 && sql.ToLower().IndexOf("order by") - sql.ToLower().IndexOf("where") > 0)
                    {
                        str_c = sql.ToLower().Substring(sql.ToLower().IndexOf("where"), sql.ToLower().IndexOf("order by") - sql.ToLower().IndexOf("where"));
                    }

                    str_c = str_c.Replace("like", " ");
                    str_c = str_c.Replace("=", " ");
                    str_c = str_c.Replace("'", " ");
                    string[] str_a = str_c.Split(' ');

                    if (str_c == "") return null;

                    str_a = str_a.Except(new List<string> { string.Empty }).ToArray();

                    if (str_a.Length > 2)
                    {
                        string[] zwrot = { str_a[1], str_a[2].TrimStart('%').TrimEnd('%') };
                        return zwrot;
                    }
                    else
                    {
                        return null;
                    }
                }

            }

        }

    }
}