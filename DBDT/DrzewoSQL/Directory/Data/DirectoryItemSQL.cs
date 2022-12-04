namespace DBDT.DrzewoSQL.Directory.Data
{
    /// <summary>
    /// Information about a directory item such as a drive, a file or a folder
    /// </summary>
    public class DirectoryItemSQL
    {
        /// <summary>
        /// The type of this item
        /// </summary>
        public DirectoryItemTypeSQL Type { get; set; }

        /// <summary>
        /// The absolute path to this item
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The id record
        /// </summary>
        public int idRec { get; set; }
        
        /// <summary>
        /// The full path to the item id
        /// </summary>
        public int idItem { get; set; }

        /// <summary>
        /// The absolute path level 1-6 to this item
        /// </summary>
        public string level1 { get; set; }
        public string level2 { get; set; }
        public string level3 { get; set; }
        public string level4 { get; set; }
        public string level5 { get; set; }
        public string level6 { get; set; }

        /// <summary>
        /// The name of this directory item
        /// </summary>
        public string Name { get { return this.Type == DirectoryItemTypeSQL.Drive ? this.FullPath : DirectoryStructureSQL.GetFileFolderNameSQL(this.FullPath); } }
    }
}
