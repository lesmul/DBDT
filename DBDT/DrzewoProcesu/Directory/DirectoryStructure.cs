using DBDT.DrzewoProcesu.Directory.Data;
using DBDT.DrzewoSQL.Directory.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBDT
{
    /// <summary>
    /// A helper class to query information about directories
    /// </summary>
    public static class DirectoryStructure
    {
        /// <summary>
        /// Gets all logical drives on the computer
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItem> GetLogicalDrives(string lokalizacja_start ="")
        {
            if (lokalizacja_start == "")
            {
                // Get every logical drive on the machine
                return Directory.GetLogicalDrives().Select(drive => new DirectoryItem { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
            }
            else
            {
                var items = new List<DirectoryItem>
                {
                    new DirectoryItem { FullPath = lokalizacja_start, Type = DirectoryItemType.Folder }
                };
                return items.ToList();
            }

        }

        /// <summary>
        /// Gets the directories top-level content
        /// </summary>
        /// <param name="fullPath">The full path to the directory</param>
        /// <returns></returns>
        public static List<DirectoryItem> GetDirectoryContents(string fullPath, string findFile = "")
        {
            // Create empty list
            var items = new List<DirectoryItem>();

            #region Get Folders

            // Try and get directories from the folder
            // ignoring any issues doing so
            try
            {

                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder }));
            }
            catch { }

            #endregion

            #region Get Files

            // Try and get files from the folder
            // ignoring any issues doing so
            try
            {
                if (findFile != "")
                {
                    findFile = "*" + findFile + "*.*";
                }
                else
                {
                    findFile = "*";
                }
                var fs = Directory.GetFiles(fullPath, findFile);

                //if (fs.Length > 0)
                //    items.AddRange(fs.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File, TypeFile = typeFile }));
                if (fs.Length > 0)
                {
                    for (int i = 0; i < fs.Length; i++)
                    {
                        items.Add(new DirectoryItem { FullPath = fs[i].ToString(), Type = DirectoryItemType.File, 
                            TypeFile = fs[i].Substring(fs[i].LastIndexOf('.')) });
                    }
                }
            }
            catch { }

            #endregion

            return items;
        }

        #region Helpers

        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        #endregion
    }
}