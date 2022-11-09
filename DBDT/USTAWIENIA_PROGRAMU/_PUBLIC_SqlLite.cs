using System;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows;


namespace DBDT.USTAWIENIA_PROGRAMU
{
    internal class _PUBLIC_SqlLite
    {
        public static string sqlite_file;
       
        public static bool Existsdb(string path)
        {
            string ScieszkaProgramu = "";

            if (path.Trim() == "")
            {
                ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
                {
                    ScieszkaProgramu += @"\";
                }
            }
            else
            {
                if (path.Trim().EndsWith(@"\") == false)
                {
                    ScieszkaProgramu = path + @"\";
                }
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(ScieszkaProgramu + "_dbdt.db");

            sqlite_file = fi.FullName;

            if (fi.Exists == false)
            {
                SqliteCleate();

                return false;
            }
            else
            {
                return true;
            }
        }

        public static void SqliteCleate()
        {
            SQLiteConnection.CreateFile(sqlite_file);

            string str_1 = "CREATE TABLE IF NOT EXISTS `ParametryPalaczenia` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT,`serwer` varchar(255) NOT NULL, " +
                " `nazwa_bazy` varchar(255) NOT NULL DEFAULT '', " +
                " `pole1` varchar(255) NOT NULL DEFAULT '', " +
                " `pole2` varchar(255) NOT NULL DEFAULT '', " +
                " `pole3` varchar(255) NOT NULL DEFAULT '', " +
                " `pole4` varchar(255) NOT NULL DEFAULT '', " +
                " `pole5` varchar(255) NOT NULL DEFAULT '', " +
                " `pole6` varchar(255) NOT NULL DEFAULT '', " +
                " `pole7` varchar(255) NOT NULL DEFAULT '', " +
                " `pole8` varchar(255) NOT NULL DEFAULT '', " +
                " `pole9` varchar(255) NOT NULL DEFAULT '', " +
                " `pole10` varchar(2500) NOT NULL DEFAULT '', " +
                " `pole11` text DEFAULT '', " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng2` single DEFAULT 0, " +
                " `polesng3` single DEFAULT 0, " +
                " `polereal` REAL DEFAULT 0, " +
                " `kto_zmienil` varchar(500) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            string str_2 = "CREATE TABLE IF NOT EXISTS `sql_zapytania` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_zapytania` varchar(255) NOT NULL DEFAULT '', " +
                " `sql` varchar(5000) NOT NULL DEFAULT '', " +
                " `pole1` varchar(255) NOT NULL DEFAULT '', " +
                " `pole2` varchar(255) NOT NULL DEFAULT '', " +
                " `pole3` varchar(255) NOT NULL DEFAULT '', " +
                " `pole4` varchar(255) NOT NULL DEFAULT '', " +
                " `pole5` varchar(255) NOT NULL DEFAULT '', " +
                " `pole6` varchar(255) NOT NULL DEFAULT '', " +
                " `pole7` varchar(255) NOT NULL DEFAULT '', " +
                " `pole8` varchar(255) NOT NULL DEFAULT '', " +
                " `pole9` varchar(255) NOT NULL DEFAULT '', " +
                " `pole10` varchar(2500) NOT NULL DEFAULT '', " +
                " `pole11` text DEFAULT '', " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng2` single DEFAULT 0, " +
                " `polesng3` single DEFAULT 0, " +
                " `polereal` REAL DEFAULT 0, " +
                " `kto_zmienil` varchar(500) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            string str_3 = "CREATE TABLE IF NOT EXISTS `obrobki` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_obrobki` varchar(255) NOT NULL, " +
                " `opis` text NOT NULL DEFAULT '', " +
                " `pole1` varchar(255) NOT NULL DEFAULT '', " +
                " `pole2` varchar(255) NOT NULL DEFAULT '', " +
                " `pole3` varchar(255) NOT NULL DEFAULT '', " +
                " `pole4` varchar(255) NOT NULL DEFAULT '', " +
                " `pole5` varchar(255) NOT NULL DEFAULT '', " +
                " `pole6` varchar(255) NOT NULL DEFAULT '', " +
                " `pole7` varchar(255) NOT NULL DEFAULT '', " +
                " `pole8` varchar(255) NOT NULL DEFAULT '', " +
                " `pole9` varchar(255) NOT NULL DEFAULT '', " +
                " `pole10` varchar(255) NOT NULL DEFAULT '', " +
                " `pole11` text DEFAULT '', " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng2` single DEFAULT 0, " +
                " `polesng3` single DEFAULT 0, " +
                " `polereal` REAL DEFAULT 0, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            string str_4 = "CREATE TABLE IF NOT EXISTS `objekty` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_obrobki` varchar(255) NOT NULL, " +
                " `opis` text NOT NULL DEFAULT '', " +
                " `pole1` varchar(255) NOT NULL DEFAULT '', " +
                " `pole2` varchar(255) NOT NULL DEFAULT '', " +
                " `pole3` varchar(255) NOT NULL DEFAULT '', " +
                " `pole4` varchar(255) NOT NULL DEFAULT '', " +
                " `pole5` varchar(255) NOT NULL DEFAULT '', " +
                " `pole6` varchar(255) NOT NULL DEFAULT '', " +
                " `pole7` varchar(255) NOT NULL DEFAULT '', " +
                " `pole8` varchar(255) NOT NULL DEFAULT '', " +
                " `pole9` varchar(255) NOT NULL DEFAULT '', " +
                " `pole10` varchar(2500) NOT NULL DEFAULT '', " +
                " `pole11` text DEFAULT '', " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng2` single DEFAULT 0, " +
                " `polesng3` single DEFAULT 0, " +
                " `polereal` REAL DEFAULT 0, " +
                " `objekt` blob, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            //SQLiteConnection connection = new SQLiteConnection
            SQLiteConnection connection = new SQLiteConnection
            {
                ConnectionString = "Data Source=" + sqlite_file
            };
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = str_1 + str_2 + str_3 + str_4
            };

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        public static DataTable SelectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            if (sqlite_file == null)
            {
                Existsdb("");
            }

            try
            {
                SQLiteCommand cmd;
                SQLiteConnection connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=" + sqlite_file
                };

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }

            return dt;
        }
        /// <summary>
        /// Funkcja do zapisu danych do tabeli ustawienia programu
        /// </summary>
        /// <param name="str_serwer"></param>
        /// <param name="str_nazwa_bazy"></param>
        /// <returns></returns>
        public static Boolean DODAJ_REKORD_PAR_POLACZENIA(string str_serwer, string str_nazwa_bazy)
        {

            if (str_serwer.Trim() == "" || str_nazwa_bazy.Trim() == "") return false;

            SQLiteCommand command_insert = new SQLiteCommand();

            try
            {

                SQLiteConnection connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=" + sqlite_file
                };

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command_insert = connection.CreateCommand();

            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                MessageBox.Show(ex.Message, "Błąd");
            }

            command_insert.CommandText = "INSERT INTO `ParametryPalaczenia` (`serwer`,`nazwa_bazy`, `kto_zmienil`)" +
                " VALUES(@serwer, @nazwa_bazy, @kto_zmienil)";

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@serwer", str_serwer);
            command_insert.Parameters.AddWithValue("@nazwa_bazy", str_nazwa_bazy);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());

            try
            {
                command_insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }

            return true;
        }

        public static Boolean USUN_REKORDY_PAR_POLACZENIA()
        {

            SQLiteCommand command_insert = new SQLiteCommand();

            try
            {

                SQLiteConnection connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=" + sqlite_file
                };

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command_insert = connection.CreateCommand();

            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                MessageBox.Show(ex.Message, "Błąd");
            }

            command_insert.CommandText = "DELETE FROM `ParametryPalaczenia` where `id` IN (SELECT `id` from `ParametryPalaczenia` order by `id` asc limit 20)";
         
            command_insert.CommandType = CommandType.Text;

            try
            {
                command_insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }

            return true;
        }

        public static Boolean DODAJ_REKORD_SQL_ZAPYTANIA(string nazwa_zapytania, string sql)
        {

            if (sql.Trim() == "") return false;

            if (nazwa_zapytania.Trim() == "") nazwa_zapytania = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

            SQLiteCommand command_insert = new SQLiteCommand();

            try
            {

                SQLiteConnection connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=" + sqlite_file
                };

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command_insert = connection.CreateCommand();

            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                MessageBox.Show(ex.Message, "Błąd");
            }

            command_insert.CommandText = "INSERT INTO `sql_zapytania` (`nazwa_zapytania`,`sql`, `kto_zmienil`)" +
                " VALUES(@nazwa_zapytania, @sql, @kto_zmienil)";

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@nazwa_zapytania", nazwa_zapytania);
            command_insert.Parameters.AddWithValue("@sql", sql);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());

            try
            {
                command_insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }

            return true;
        }
    }
}
