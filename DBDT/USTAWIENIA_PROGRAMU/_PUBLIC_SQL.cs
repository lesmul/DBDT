using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace DBDT.USTAWIENIA_PROGRAMU
{
    internal class _PUBLIC_SQL

    {
        public static System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection();
        public static string connectionString;

        /// <summary>
        /// Fukcja zwraca wartości zapytania SQL
        /// </summary>
        /// <param name="STR_SQL"></param>
        /// <returns></returns>
        public static DataTable ZWROC_WARTOSC_MSSQL(string STR_SQL)
        {

            DataSet DS_TMP = new DataSet();

            try
            {
                if (myConnection.State == ConnectionState.Closed)

                    myConnection.ConnectionString = connectionString;

                {
                    if (myConnection.State == ConnectionState.Closed)
                        myConnection.Open();
                }

                SqlCommand command_select = new System.Data.SqlClient.SqlCommand();
                command_select.Connection = myConnection;
                command_select.CommandText = STR_SQL;

                SqlDataAdapter DaneWynik = new System.Data.SqlClient.SqlDataAdapter()
                {
                    SelectCommand = command_select
                };

                DaneWynik.Fill(DS_TMP);

                return DS_TMP.Tables[0];
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return null;

            }
        }

        /// <summary>
        /// Zakończ połączenie do bazy danych SQL
        /// </summary>
        /// <returns></returns>
        public static bool ZAMKNIJ_POLACZENIE()
        {
            try
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Zakończ połączenie do bazy danych SQL
        /// </summary>
        /// <returns></returns>
        public static bool POLACZ()
        {
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                    myConnection.Open();
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
