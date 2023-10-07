using System;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Data.Common;
using System.Xml.Linq;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Windows.Controls.Primitives;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

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
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

            string str_2 = "CREATE TABLE IF NOT EXISTS `sql_zapytania` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_zapytania` varchar(255) NOT NULL DEFAULT '', " +
                " `sql` varchar(15000) NOT NULL DEFAULT '', " +
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
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

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
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

            string str_4 = "CREATE TABLE IF NOT EXISTS `objekty` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_objektu` varchar(255) NOT NULL, " +
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
                " `objekt` varbinary, " +
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

            string str_5 = "CREATE TABLE IF NOT EXISTS `procedury` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_procedury` varchar(255) NOT NULL, " +
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
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

            string str_6 = "CREATE TABLE IF NOT EXISTS `funkcje` (`id` integer NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                " `nazwa_funkcji` varchar(255) NOT NULL, " +
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
                " `kto_zmienil` varchar(255) NOT NULL, " +
                " `data_utworzenia` DATETIME DEFAULT current_timestamp); ";

            //SQLiteConnection connection = new SQLiteConnection
            SQLiteConnection connection = new SQLiteConnection
            {
                ConnectionString = "Data Source=" + sqlite_file
            };
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(connection)
            {
                CommandText = str_1 + str_2 + str_3 + str_4 + str_5 + str_6
            };

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();
            connection.Dispose();
        }
        public static DataTable SelectQuery_DB_MASTER(string query, string path)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                SQLiteConnection connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=" + path
                };

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                cmd = connection.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }

            return dt;
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

        public static DataTable UpdateData(string Sql_query, DataTable dt)
        {
            SQLiteDataAdapter ad;
  
            if (sqlite_file == null)
            {
                Existsdb("");
            }

            DataTable dtr = new DataTable();

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

                using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(Sql_query, connection))
                {
                    SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(sQLiteDataAdapter);
                    sQLiteDataAdapter.Update(dt);
                }


                cmd = connection.CreateCommand();
                cmd.CommandText = Sql_query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dtr); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }

            return dtr;
        }
        /// <summary>
        /// Funkcja do zapisu danych do tabeli ustawienia programu
        /// </summary>
        /// <param name="str_serwer"></param>
        /// <param name="str_nazwa_bazy"></param>
        /// <returns></returns>
        public static Boolean DODAJ_REKORD_PAR_POLACZENIA(string str_serwer, string str_nazwa_bazy,
            string pole8 = "",string pole10 ="", string nazwa_pola = "", bool boolZastap = false, string pole2 = "", string pole3 = "", 
            string pole4 = "", string pole5 = "", string pole6 = "", string pole11 = "")
        {

            if ((str_serwer.Trim() == "" || str_nazwa_bazy.Trim() == "") && boolZastap == false) return false;

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

            var guid = Guid.NewGuid().ToString();

            command_insert.CommandType = CommandType.Text;

            if (boolZastap == true)
            {
                command_insert.CommandText = "DELETE FROM `ParametryPalaczenia` WHERE `serwer` ='" + str_serwer + "' and `nazwa_bazy` = '" + str_nazwa_bazy 
                    + "' and `pole9` = '" + nazwa_pola + "'";
                command_insert.ExecuteNonQuery();
            }

            command_insert.CommandText = "INSERT INTO `ParametryPalaczenia` (`serwer`,`nazwa_bazy`, `pole10`, `pole11`, `pole9`, `pole8`, `pole7`, `pole6`, `pole5`, `kto_zmienil`, " +
                "`data_utworzenia`, `pole2`, `pole3`, `pole4`)" +
                " VALUES(@serwer, @nazwa_bazy, @pole10, @pole11, @pole9, @pole8, @pole7, @pole6, @pole5, @kto_zmienil, @data_utworzenia, @pole2, @pole3, @pole4)";

            command_insert.Parameters.AddWithValue("@serwer", str_serwer);
            command_insert.Parameters.AddWithValue("@nazwa_bazy", str_nazwa_bazy);
            command_insert.Parameters.AddWithValue("@pole11", pole11);
            command_insert.Parameters.AddWithValue("@pole10", pole10);
            command_insert.Parameters.AddWithValue("@pole8", pole8);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@pole9", nazwa_pola);
            command_insert.Parameters.AddWithValue("@pole2", pole2);
            command_insert.Parameters.AddWithValue("@pole3", pole3);
            command_insert.Parameters.AddWithValue("@pole4", pole4);
            command_insert.Parameters.AddWithValue("@pole5", pole5);
            command_insert.Parameters.AddWithValue("@pole6", pole6);
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);
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
        public static Boolean DODAJ_REKORD_PAR_POLACZENIA_Z_MASTER(string str_serwer, string str_nazwa_bazy,
    string pole7 = "", string pole8 = "", string pole9 = "", string pole10 = "", string kto_zmienil = "")
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

            command_insert.CommandType = CommandType.Text;

            command_insert.CommandText = "INSERT INTO `ParametryPalaczenia` (`serwer`,`nazwa_bazy`, `pole10`, `pole9`, `pole8`, `pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@serwer, @nazwa_bazy, @pole10, @pole9, @pole8, @pole7, @kto_zmienil, @data_utworzenia)";

            command_insert.Parameters.AddWithValue("@serwer", str_serwer);
            command_insert.Parameters.AddWithValue("@nazwa_bazy", str_nazwa_bazy);
            command_insert.Parameters.AddWithValue("@pole8", pole8);
            command_insert.Parameters.AddWithValue("@pole7", pole7);
            command_insert.Parameters.AddWithValue("@pole9", pole9);
            command_insert.Parameters.AddWithValue("@pole10", pole10);
            command_insert.Parameters.AddWithValue("@kto_zmienil", kto_zmienil);
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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

            command_insert.CommandText = "DELETE FROM `ParametryPalaczenia` where `id` IN (SELECT `id` from `ParametryPalaczenia` order by `id` asc limit 500) AND nazwa_bazy <> ''";
         
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
        public static Boolean USUN_REKORD_PAR_POLACZENIA(string str_serwer, string str_nazwa_bazy)
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

            command_insert.CommandText = "DELETE FROM `ParametryPalaczenia` where serwer=@serwer and nazwa_bazy=@nazwa_bazy";

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
        public static Boolean DODAJ_REKORD_SQL_ZAPYTANIA(string nazwa_zapytania, string sql, string poziom1, string poziom2,
             string poziom3, string poziom4, string poziom5, string poziom6)
        {

            if (sql.Trim() == "") return false;
   
            if (nazwa_zapytania.Trim() == "")
            {
                if (MessageBox.Show("Nie podałeś opisu czy mimo to zapisac?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return false;
                }

                nazwa_zapytania = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            if (nazwa_zapytania.Trim() == "") return false;
      
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

            command_insert.CommandText = "INSERT INTO `sql_zapytania` (`nazwa_zapytania`,`sql`, `pole1`, `pole2`, " +
                "`pole3`, `pole4`, `pole5`, `pole6`, `pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@nazwa_zapytania, @sql, @pole1, @pole2, @pole3, @pole4, @pole5, @pole6, @pole7, @kto_zmienil, @data_utworzenia)";

            command_insert.CommandType = CommandType.Text;

            if (poziom1 == "") poziom1 = "nieprzypisany";

            var guid = Guid.NewGuid().ToString();

            command_insert.Parameters.AddWithValue("@nazwa_zapytania", nazwa_zapytania);
            command_insert.Parameters.AddWithValue("@sql", sql);
            command_insert.Parameters.AddWithValue("@pole1", poziom1);
            command_insert.Parameters.AddWithValue("@pole2", poziom2);
            command_insert.Parameters.AddWithValue("@pole3", poziom3);
            command_insert.Parameters.AddWithValue("@pole4", poziom4);
            command_insert.Parameters.AddWithValue("@pole5", poziom5);
            command_insert.Parameters.AddWithValue("@pole6", poziom6);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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
        public static Boolean DODAJ_REKORD_SQL_ZAPYTANIA_Z_MASTER(string nazwa_zapytania, string sql, string pole1, string pole2,
     string pole3, string pole4, string pole5, string pole6, string kto_zmienil, string pole7)
        {

            if (sql.Trim() == "") return false;

            if (nazwa_zapytania.Trim() == "")
            {
                if (MessageBox.Show("Nie podałeś opisu czy mimo to zapisac?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return false;
                }

                nazwa_zapytania = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            if (nazwa_zapytania.Trim() == "") return false;

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

            command_insert.CommandText = "INSERT INTO `sql_zapytania` (`nazwa_zapytania`,`sql`, `pole1`, `pole2`, " +
                "`pole3`, `pole4`, `pole5`, `pole6`, `pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@nazwa_zapytania, @sql, @pole1, @pole2, @pole3, @pole4, @pole5, @pole6, @pole7, @kto_zmienil, @data_utworzenia)";

            command_insert.CommandType = CommandType.Text;

            if (pole1 == "") pole1 = "nieprzypisany";

            command_insert.Parameters.AddWithValue("@nazwa_zapytania", nazwa_zapytania);
            command_insert.Parameters.AddWithValue("@sql", sql);
            command_insert.Parameters.AddWithValue("@pole1", pole1);
            command_insert.Parameters.AddWithValue("@pole2", pole2);
            command_insert.Parameters.AddWithValue("@pole3", pole3);
            command_insert.Parameters.AddWithValue("@pole4", pole4);
            command_insert.Parameters.AddWithValue("@pole5", pole5);
            command_insert.Parameters.AddWithValue("@pole6", pole6);
            command_insert.Parameters.AddWithValue("@pole7", pole7);
            command_insert.Parameters.AddWithValue("@kto_zmienil",kto_zmienil);
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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
        public static Boolean DODAJ_REKORD_OBJEKT(string nazwa_objektu, string opis, string scieszka_do_pliku, string nazwa_pliku)
        {

            if (nazwa_objektu.Trim() == "") return false;

            if (nazwa_objektu.Trim() == "") nazwa_objektu = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

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

            var guid = Guid.NewGuid().ToString();

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@nazwa_objektu", nazwa_objektu);
            command_insert.Parameters.AddWithValue("@opis", opis);
            command_insert.Parameters.AddWithValue("@pole1", nazwa_pliku);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

            if (scieszka_do_pliku != "")
            {
                command_insert.CommandText = "INSERT INTO `objekty` (`nazwa_objektu`,`opis`, `objekt`, `pole1`, `pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@nazwa_objektu, @opis, @objekt, @pole1, @pole7, @kto_zmienil, @data_utworzenia)";
  
                command_insert.Parameters.AddWithValue("@objekt", ARR_BYTE_FILE_XSL(scieszka_do_pliku));
            }
            else
            {
                command_insert.CommandText = "INSERT INTO `objekty` (`nazwa_objektu`,`opis`, `pole1`,`pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@nazwa_objektu, @opis, @pole1, @pole7, @kto_zmienil, @data_utworzenia)";
            }

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
        public static Boolean ZMIEN_REKORD_OBJEKT_FULL(string nazwa_objektu, string opis, string scieszka_do_pliku, string nazwa_pliku, string id_rec)
        {

            if (nazwa_objektu.Trim() == "") return false;

            if (nazwa_objektu.Trim() == "") nazwa_objektu = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

            SQLiteCommand command_update = new SQLiteCommand();

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

                command_update = connection.CreateCommand();

            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                MessageBox.Show(ex.Message, "Błąd");
            }

            var guid = Guid.NewGuid().ToString();

            command_update.CommandType = CommandType.Text;

            command_update.Parameters.AddWithValue("@nazwa_objektu", nazwa_objektu);
            command_update.Parameters.AddWithValue("@opis", opis);
            command_update.Parameters.AddWithValue("@pole1", nazwa_pliku);
            command_update.Parameters.AddWithValue("@pole7", guid);
            command_update.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_update.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);
            command_update.Parameters.AddWithValue("@id", id_rec);

            if (scieszka_do_pliku != "")
            {
                command_update.CommandText = "UPDATE `objekty` SET `nazwa_objektu` = @nazwa_objektu,`opis` = @opis, `objekt` = @objekt, `pole1` = @pole1, `pole7` = @pole7, " +
                    "`kto_zmienil` = @kto_zmienil, `data_utworzenia` = @data_utworzenia where `id` = @id";

                command_update.Parameters.AddWithValue("@objekt", ARR_BYTE_FILE_XSL(scieszka_do_pliku));
            }
            else
            {
                command_update.CommandText = "UPDATE `objekty` SET `nazwa_objektu` = @nazwa_objektu,`opis` = @opis, `pole1` = @pole1, `pole7` = @pole7, " +
                    "`kto_zmienil` = @kto_zmienil, `data_utworzenia` = @data_utworzenia where `id` = @id";
            }

            try
            {
                command_update.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }

            return true;
        }
        public static Boolean DODAJ_REKORD_OBJEKT_Z_MASTER(string nazwa_objektu, string opis, string pole1, 
            string pole7, string kto_zmienil, object objekt)
        {

            if (nazwa_objektu.Trim() == "") return false;

            if (nazwa_objektu.Trim() == "") nazwa_objektu = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

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

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@nazwa_objektu", nazwa_objektu);
            command_insert.Parameters.AddWithValue("@opis", opis);
            command_insert.Parameters.AddWithValue("@pole1", pole1);
            command_insert.Parameters.AddWithValue("@pole7", pole7);
            command_insert.Parameters.AddWithValue("@kto_zmienil", kto_zmienil);
            command_insert.Parameters.AddWithValue("@objekt", objekt);
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

            command_insert.CommandText = "INSERT INTO `objekty` (`nazwa_objektu`,`opis`, `objekt`, `pole1`, `pole7`, `kto_zmienil`, `data_utworzenia`)" +
                " VALUES(@nazwa_objektu, @opis, @objekt, @pole1, @pole7, @kto_zmienil, @data_utworzenia)";

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
        public static Boolean DODAJ_REKORD_OBROBKI_Z_MASTER(string nazwa_obrobki, string pole1, string pole2, string pole3, string pole4, string pole5, string pole6,
    string pole7,int poleint1, int poleint2,string opis, string kto_zmienil)
        {

            if (nazwa_obrobki.Trim() == "") return false;

            //if (nazwa_obrobki.Trim() == "") nazwa_obrobki = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

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
                return false;
            }

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@nazwa_obrobki", nazwa_obrobki);
            command_insert.Parameters.AddWithValue("@pole1", pole1);
            command_insert.Parameters.AddWithValue("@pole2", pole2);
            command_insert.Parameters.AddWithValue("@pole3", pole3);
            command_insert.Parameters.AddWithValue("@pole4", pole4);
            command_insert.Parameters.AddWithValue("@pole5", pole5);
            command_insert.Parameters.AddWithValue("@pole6", pole6);
            command_insert.Parameters.AddWithValue("@pole7", pole7);
            command_insert.Parameters.AddWithValue("@poleint1", poleint1);
            command_insert.Parameters.AddWithValue("@poleint2", poleint2);
            command_insert.Parameters.AddWithValue("@opis", opis);
            command_insert.Parameters.AddWithValue("@kto_zmienil", kto_zmienil);
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

            command_insert.CommandText = "INSERT INTO (nazwa_obrobki ,pole1	,pole2 ,pole3 ,pole4 ,pole5	,pole6 ,pole7 ,poleint1	,poleint2 ,opis, kto_zmienil, data_utworzenia) " +
            "VALUES(@nazwa_obrobki, @pole1, @pole2, @pole3, @pole4, @pole5, @pole6, @pole7, @poleint1, @poleint2, @opis, @kto_zmienil, @data_utworzenia)";

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
        public static Boolean ZMIEN_REKORD_OBJEKT(string nazwa_objektu, string opis, string id_obj)
        {

            if (nazwa_objektu.Trim() == "") return false;

            if (nazwa_objektu.Trim() == "") nazwa_objektu = string.Format("Zmieniono dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

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

            var guid = Guid.NewGuid().ToString();

            command_insert.CommandType = CommandType.Text;

            command_insert.Parameters.AddWithValue("@nazwa_objektu", nazwa_objektu);
            command_insert.Parameters.AddWithValue("@opis", opis);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

            command_insert.CommandText = "UPDATE `objekty` SET nazwa_objektu=@nazwa_objektu, opis=@opis, pole7=@pole7, data_utworzenia=@data_utworzenia  WHERE `id` = " + id_obj;

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
        public static Boolean USUN_REKORD_OBJEKT(string id_obj)
        {

            if (id_obj.Trim() == "") return false;

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

            command_insert.CommandType = CommandType.Text;

            command_insert.CommandText = "DELETE FROM `objekty` WHERE `id` = " + id_obj;
   
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
        public static Boolean ZAPISZ_ZMIANY_SQL(string StrSQL)
        {

            if (StrSQL.Trim() == "") return false;

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

            command_insert.CommandType = CommandType.Text;

            command_insert.CommandText = StrSQL;

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
        public static Boolean USUN_REKORD_SQL_ZAPYTANIA(string id_obj)
        {

            if (id_obj.Trim() == "") return false;

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
   
            command_insert.CommandType = CommandType.Text;

            command_insert.CommandText = "DELETE FROM `sql_zapytania` WHERE`id` = " + id_obj;

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
        public static Boolean ZMIEN_OPIS_REKORD_SQL_ZAPYTANIA(string str_opis, string poziom1, string poziom2,
             string poziom3, string poziom4, string poziom5, string poziom6, string id_obj)
        {

            if (id_obj.Trim() == "") return false;

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

            var guid = Guid.NewGuid().ToString();

            command_insert.CommandType = CommandType.Text;
     
            command_insert.CommandText = "UPDATE `sql_zapytania` SET `nazwa_zapytania` = '" + str_opis + "'"
                + ", `pole1` = '" + poziom1 + "'"
                + ", `pole2` = '" + poziom2 + "'"
                + ", `pole3` = '" + poziom3 + "'"
                + ", `pole4` = '" + poziom4 + "'"
                + ", `pole5` = '" + poziom5 + "'"
                + ", `pole6` = '" + poziom6 + "'"
                + ", `pole7` = '" + guid + "'"
                + " WHERE `id` = " + id_obj;

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
        public static Boolean ZMIEN_OPIS_POZIOMU_SQL_ZAPYTANIA(string strSQL, string id_obj)
        {

            if (id_obj.Trim() == "") return false;

            SQLiteCommand command_update = new SQLiteCommand();

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

                command_update = connection.CreateCommand();

            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                MessageBox.Show(ex.Message, "Błąd");
            }

            var guid = Guid.NewGuid().ToString();

            command_update.CommandType = CommandType.Text;
            command_update.Parameters.AddWithValue("@sql", strSQL);
            command_update.Parameters.AddWithValue("@pole7", guid);

            command_update.CommandText = "UPDATE `sql_zapytania` SET `sql` = @sql, pole7 = @pole7 WHERE `id` = " + id_obj;

            try
            {
                command_update.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }

            return true;
        }
        public static string ZAPISZ_DO_PLIKU_XSL(string STR_KATALOG, string STR_NAZWA_PLIKU, string INT_ID)
        {

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
                cmd.CommandText = "SELECT objekt FROM objekty WHERE id = " + INT_ID + "";
           
                string STR_FILE;

                STR_FILE = STR_KATALOG;// + @"\" + STR_NAZWA_PLIKU;

                byte[] objData = (byte[])cmd.ExecuteScalar();

                if (objData != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(objData, 0, objData.Length))
                    {
                        ms.Write(objData, 0, objData.Length);
                        if (objData.Length > 0)
                        {
                            if (STR_FILE.IndexOf("XSL/XSLX/ZIP") > -1)
                            {
                                System.IO.FileStream file = new System.IO.FileStream(STR_FILE.Replace("XSL/XSLX/ZIP", "XSL_XSLX_ZIP"), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                                ms.WriteTo(file);
                                file.Close();
                                ms.Close();
                            }
                            else
                            {
                                System.IO.FileStream file = new System.IO.FileStream(STR_FILE, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                                ms.WriteTo(file);
                                file.Close();
                                ms.Close();
                            }
                        }
                    }

                    return STR_KATALOG;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return "";
            }

            return "";
        }
        public static object ARR_BYTE_FILE_XSL(string STR_SCIESZKA)
        {
            if (STR_SCIESZKA == "") return null;

            byte[] arrImageXLS = null;
            string filename = STR_SCIESZKA;
            UInt32 FileSize;
            System.IO.MemoryStream mstream = new System.IO.MemoryStream();
        
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
            {
                arrImageXLS = br.ReadBytes((int)fs.Length);
            }
            FileSize = (uint)mstream.Length;
            mstream.Close();
            return arrImageXLS;
        }
        public static Boolean DODAJ_REKORD_SQL_FUKCJE(string nazwa_funkcji, string sql, string nazwa_plik_excel,
        string nazwa_arkusza, string komorka_start, string pole4, string pole5, string pole6, string pole8, string pole9, string pole10, string pole11)
        {

            if (sql.Trim() == "") return false;

            if (nazwa_funkcji.Trim() == "")
            {
                if (MessageBox.Show("Nie podałeś opisu czy mimo to zapisać?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return false;
                }

                nazwa_funkcji = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            if (nazwa_funkcji.Trim() == "") return false;

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

            command_insert.CommandText = "INSERT INTO funkcje (nazwa_funkcji, opis, pole1, pole2, pole3, pole4, pole5, pole6, pole7, pole8, pole9, pole10, pole11, " +
                "kto_zmienil, data_utworzenia)" +
                " VALUES (@nazwa_funkcji, @opis, @pole1, @pole2, @pole3, @pole4, @pole5, @pole6, @pole7, @pole8, @pole9, @pole10, @pole11, @kto_zmienil, @data_utworzenia)";

            command_insert.CommandType = CommandType.Text;

            var guid = Guid.NewGuid().ToString();

            command_insert.Parameters.AddWithValue("@nazwa_funkcji", nazwa_funkcji);
            command_insert.Parameters.AddWithValue("@opis", sql);
            command_insert.Parameters.AddWithValue("@pole1", nazwa_plik_excel);
            command_insert.Parameters.AddWithValue("@pole2", nazwa_arkusza);
            command_insert.Parameters.AddWithValue("@pole3", komorka_start);
            command_insert.Parameters.AddWithValue("@pole4", pole4);
            command_insert.Parameters.AddWithValue("@pole5", pole5);
            command_insert.Parameters.AddWithValue("@pole6", pole6);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@pole8", pole8);
            command_insert.Parameters.AddWithValue("@pole9", pole9);
            command_insert.Parameters.AddWithValue("@pole10", pole10);
            command_insert.Parameters.AddWithValue("@pole11", pole11);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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
        public static Boolean ZMIEN_REKORD_SQL_FUKCJE(string nazwa_funkcji, string sql, string nazwa_plik_excel,
string nazwa_arkusza, string komorka_start, string pole4, string pole5, string pole6, string pole8, string pole9, string pole10, string pole11, string id)
        {

            if (sql.Trim() == "") return false;

            if (nazwa_funkcji.Trim() == "")
            {
                if (MessageBox.Show("Nie podałeś opisu czy mimo to zapisac?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return false;
                }

                nazwa_funkcji = string.Format("Zapisano dnia: {0} at {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            if (nazwa_funkcji.Trim() == "") return false;

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

            command_insert.CommandText = "UPDATE funkcje SET nazwa_funkcji=@nazwa_funkcji, opis=@opis, pole1=@pole1, pole2=@pole2, pole3=@pole3, pole4=@pole4, " +
                "pole5=@pole5, pole6=@pole6, pole7=@pole7, pole8=@pole8, pole9=@pole9, pole10=@pole10, pole11=@pole11, " +
                "kto_zmienil=@kto_zmienil, data_utworzenia=@data_utworzenia where id = " + id;

            command_insert.CommandType = CommandType.Text;

            var guid = Guid.NewGuid().ToString();

            command_insert.Parameters.AddWithValue("@nazwa_funkcji", nazwa_funkcji);
            command_insert.Parameters.AddWithValue("@opis", sql);
            command_insert.Parameters.AddWithValue("@pole1", nazwa_plik_excel);
            command_insert.Parameters.AddWithValue("@pole2", nazwa_arkusza);
            command_insert.Parameters.AddWithValue("@pole3", komorka_start);
            command_insert.Parameters.AddWithValue("@pole4", pole4);
            command_insert.Parameters.AddWithValue("@pole5", pole5);
            command_insert.Parameters.AddWithValue("@pole6", pole6);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            command_insert.Parameters.AddWithValue("@pole8", pole8);
            command_insert.Parameters.AddWithValue("@pole9", pole9);
            command_insert.Parameters.AddWithValue("@pole10", pole10);
            command_insert.Parameters.AddWithValue("@pole11", pole11);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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

        public static Boolean DODAJ_ZMIEN_REKORD_SQL_OPERACJE(string opis, string nazwa_obrobki, string int_id, string typ_parametru,
string typ_danych, string nazwa_parametru, string pole_powiazane, string dodakowe_opcje, string int_id_u, string tab_name, int lp)
        {

            if (typ_parametru.Trim() == "")
            {
                MessageBox.Show("Typ parametru wymagana!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (nazwa_parametru.Trim() == "" && typ_parametru != "¶")
            {
                if (typ_danych != "Kod języka" && typ_danych != "Pusty wiersz")
                {
                    MessageBox.Show("Nazwa parametru wymagana!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }

            if (typ_danych == "Kod języka" || typ_danych == "Pusty wiersz")
            {
                pole_powiazane = "";
            }

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

            if (int_id_u != "-1")
            {

                if (tab_name == "")
                {
                    MessageBox.Show("Bład konfiguracji nazwy zakładki!");
                }

                command_insert.CommandText = "UPDATE obrobki SET opis = @opis, nazwa_obrobki=@nazwa_obrobki, pole1=@pole1, pole2=@pole2, pole3=@pole3, pole4=@pole4, pole5=@pole5, " +
                "pole6=@pole6, pole7=@pole7, poleint2=@poleint2, kto_zmienil=@kto_zmienil, data_utworzenia=@data_utworzenia" +
                " WHERE id = " + int_id_u + " and nazwa_obrobki = '" + tab_name + "'";
            }
            else
            {
                command_insert.CommandText = "INSERT INTO obrobki (opis, nazwa_obrobki, poleint1, poleint2, pole1, pole2, pole3, pole4, pole5, pole6, pole7, " +
                "kto_zmienil, data_utworzenia)" +
                " VALUES (@opis, @nazwa_obrobki, @poleint1, @poleint2, @pole1, @pole2, @pole3, @pole4, @pole5, @pole6, @pole7, @kto_zmienil, @data_utworzenia)";
            }


            command_insert.CommandType = CommandType.Text;

            var guid = Guid.NewGuid().ToString();

            command_insert.Parameters.AddWithValue("@opis", opis);
            command_insert.Parameters.AddWithValue("@nazwa_obrobki", nazwa_obrobki);
            command_insert.Parameters.AddWithValue("@poleint1", int_id);
            command_insert.Parameters.AddWithValue("@poleint2", lp);
            command_insert.Parameters.AddWithValue("@pole1", typ_parametru);
            command_insert.Parameters.AddWithValue("@pole2", typ_danych);
            command_insert.Parameters.AddWithValue("@pole3", nazwa_parametru);
            command_insert.Parameters.AddWithValue("@pole4", pole_powiazane);
            command_insert.Parameters.AddWithValue("@pole5", dodakowe_opcje);
            command_insert.Parameters.AddWithValue("@pole6", tab_name);
            command_insert.Parameters.AddWithValue("@pole7", guid);
            //command_insert.Parameters.AddWithValue("@pole8", pole8);
            //command_insert.Parameters.AddWithValue("@pole9", pole9);
            //command_insert.Parameters.AddWithValue("@pole10", pole10);
            //command_insert.Parameters.AddWithValue("@pole11", pole11);
            command_insert.Parameters.AddWithValue("@kto_zmienil", Environment.UserName.ToString());
            command_insert.Parameters.AddWithValue("@data_utworzenia", DateTime.Now);

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
        public static Boolean USUN_REKORD_SQL_OPERACJE(string int_id_u)
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
                return false;
            }

                 command_insert.CommandText = "DELETE FROM obrobki WHERE id = " + int_id_u;
   
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
