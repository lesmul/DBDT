using DBDT.DrzewoSQL.Directory.Data;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DBDT
{
    /// <summary>
    /// A helper class to query information about directories
    /// </summary>
    public static class DirectoryStructureSQL
    {
        /// <summary>
        /// Gets all logical drives on the computer
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItemSQL> GetLogicalDrivesSQL()
        {
            DataTable dt = new DataTable();

            dt = _PUBLIC_SqlLite.SelectQuery("SELECT DISTINCT pole1 FROM sql_zapytania WHERE sql <>'' order by pole1");
           
            var items = new List<DirectoryItemSQL> { };

            var sql = from DataRow myRow in dt.Rows where (string)myRow["pole1"] != "" select myRow["pole1"];

            return sql.Select(pole1 => new DirectoryItemSQL { FullPath = pole1.ToString(), Type = DirectoryItemTypeSQL.Folder, level1 = pole1.ToString() }).ToList();

        }

        /// <summary>
        /// Gets the directories top-level content
        /// </summary>
        /// <param name="fullPath">The full path to the directory</param>
        /// <returns></returns>
        public static List<DirectoryItemSQL> GetDirectoryContentsSQL(string fullPath, int idI, 
            string level1, string level2, string level3, string level4, string level5, string level6, int idR)
        {
            // Create empty list
            var items = new List<DirectoryItemSQL>();

            #region Get Folders

            // Try and get directories from the folder
            // ignoring any issues doing so
      
            try
            {
                DataTable dt = new DataTable();

                if (level1 == null) level1 = "%";
                if (level2 == null) level2 = "%";
                if (level3 == null) level3 = "%";
                if (level4 == null) level4 = "%";
                if (level5 == null) level5 = "%";
                if (level6 == null) level6 = "%";

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT DISTINCT pole" + idI + " FROM sql_zapytania " +
                                                "WHERE sql <> '' and pole1 LIKE '" + level1 + "' and " +
                                                " pole2 LIKE '" + level2 + "' and pole3 LIKE '" + level3 + "' and " +
                                                " pole4 LIKE '" + level4 + "' and pole5 LIKE '" + level5 + "' and pole6 LIKE '" + level6 + "'" +
                                                " order by pole" + (idI - 1));

                if (idI > 6) idI = 6;

                var sql = from DataRow myRow in dt.Rows where (string)myRow["pole"+ idI] != "" select myRow["pole" + idI];
                if (sql.Count() > 0)
                    items.AddRange(sql.Select(pole => new DirectoryItemSQL { FullPath = pole.ToString(), Type = DirectoryItemTypeSQL.Folder, idItem = idI + 1, idRec = -1}).ToList());

            }
            catch { }

            #endregion

            #region Get Files

            // Try and get files from the folder
            // ignoring any issues doing so
            try
            {
                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT DISTINCT nazwa_zapytania, id FROM sql_zapytania " +
                                                "WHERE sql <> '' and pole1 = '" + level1.TrimEnd('%') + "' and " +
                                                " pole2 = '" + level2.TrimEnd('%') + "' and pole3 = '" + level3.TrimEnd('%') + "' and " +
                                                " pole4 = '" + level4.TrimEnd('%') + "' and pole5 = '" + level5.TrimEnd('%') + "' and pole6 = '" + level6.TrimEnd('%') + "'" +
                                                " order by nazwa_zapytania");

               // var sql = from DataRow myRow in dt.Rows where (string)myRow["nazwa_zapytania"] != "" select myRow["nazwa_zapytania"];
         
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        items.Add( new DirectoryItemSQL { FullPath = dt.Rows[i]["nazwa_zapytania"].ToString(), Type = DirectoryItemTypeSQL.File, idRec = Convert.ToInt32(dt.Rows[i]["id"]) });
                    }
                }
                    //items.AddRange(sql.Select(pole2 => new DirectoryItemSQL { FullPath = pole2.ToString(), Type = DirectoryItemTypeSQL.File, idRec = -1 }).ToList());

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Błąd");
            }

            #endregion

            return items;
        }

        #region Helpers

        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns></returns>
        public static string GetFileFolderNameSQL(string path)
        {
            // If we have no path, return empty
            if (path==null)
                return "%";

            return path;

        }

        #endregion
    }
}