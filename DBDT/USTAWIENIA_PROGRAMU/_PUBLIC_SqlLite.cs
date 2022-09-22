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

            System.IO.FileInfo fi = new System.IO.FileInfo(ScieszkaProgramu + "dbdt.db");

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
                ", `nazwa_bazy` varchar(255) NOT NULL, " +
                " `pole1` varchar(255) NOT NULL, " +
                " `pole2` varchar(255) NOT NULL, " +
                " `pole3` varchar(255) NOT NULL, " +
                " `pole4` varchar(255) NOT NULL, " +
                " `pole5` varchar(255) NOT NULL, " +
                " `pole6` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole8` varchar(255) NOT NULL, " +
                " `pole9` varchar(255) NOT NULL, " +
                " `pole10` text, " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            string str_2 = "CREATE TABLE IF NOT EXISTS `sql_zapytania` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                ", `nazwa_zapytania` varchar(255) NOT NULL, " +
                " `sql` varchar(5000) NOT NULL, " +
                " `pole1` varchar(255) NOT NULL, " +
                " `pole2` varchar(255) NOT NULL, " +
                " `pole3` varchar(255) NOT NULL, " +
                " `pole4` varchar(255) NOT NULL, " +
                " `pole5` varchar(255) NOT NULL, " +
                " `pole6` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole8` varchar(255) NOT NULL, " +
                " `pole9` varchar(255) NOT NULL, " +
                " `pole10` text, " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            string str_3 = "CREATE TABLE IF NOT EXISTS `obrobki` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                ", `nazwa_obrobki` varchar(255) NOT NULL, " +
                " `opis` text NOT NULL, " +
                " `pole1` varchar(255) NOT NULL, " +
                " `pole2` varchar(255) NOT NULL, " +
                " `pole3` varchar(255) NOT NULL, " +
                " `pole4` varchar(255) NOT NULL, " +
                " `pole5` varchar(255) NOT NULL, " +
                " `pole6` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole7` varchar(255) NOT NULL, " +
                " `pole8` varchar(255) NOT NULL, " +
                " `pole9` varchar(255) NOT NULL, " +
                " `pole10` text, " +
                " `poleint1` int DEFAULT 0, " +
                " `poleint2` int DEFAULT 0, " +
                " `poleint3` int DEFAULT 0, " +
                " `poleint4` int DEFAULT 0, " +
                " `poleint5` int DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `polesng1` single DEFAULT 0, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " data_utworzenia` DATETIME DEFAULT SYSDATE NOT NULL); ";

            //SQLiteConnection connection = new SQLiteConnection
            SQLiteConnection connection = new SQLiteConnection
            {
                ConnectionString = "Data Source=" + sqlite_file
            };
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = str_1 + str_2 + str_3
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
        /// Fukcja do zapisu danych do tabeli ustawienia programu
        /// </summary>
        /// <param name="str_serwer"></param>
        /// <param name="str_nazwa_bazy"></param>
        /// <returns></returns>
        public static Boolean DODAJ_REKORD_PAR_POLACZENIA(string str_serwer, string str_nazwa_bazy)
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

            command_insert.CommandText = "INSERT INTO `ParametryPalaczenia` (`serwer`,`nazwa_bazy`)" +
                " VALUES(@serwer,@nazwa_bazy)";

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@serwer", str_serwer);
            command_insert.Parameters.AddWithValue("@nazwa_bazy", str_nazwa_bazy);

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
