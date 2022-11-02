using DBDT.DrzewoSQL.Directory.Data;
using DBDT.DrzewoProcesu.Directory.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace DBDT.DrzewoSQL.Directory.ViewModels
{
    /// <summary>
    /// A view model for each directory item
    /// </summary>
    public class DirectoryItemViewModelSQL : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The type of this item
        /// </summary>
        public DirectoryItemTypeSQL Type { get; set; }

        public string ImageName => Type == DirectoryItemTypeSQL.Drive ? "drive" : (Type == DirectoryItemTypeSQL.File ? "file" : (IsExpanded ? "folder-open" : "folder-closed"));

        /// <summary>
        /// The full path to the item
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The full path to the item id
        /// </summary>
        public int idItem { get; set; }

        /// <summary>
        /// The full path to the id record
        /// </summary>
        public int idRec { get; set; }

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

        /// <summary>
        /// A list of all children contained inside this item
        /// </summary>
        public ObservableCollection<DirectoryItemViewModelSQL> Children { get; set; }

        /// <summary>
        /// Indicates if this item can be expanded
        /// </summary>
        public bool CanExpand { get { return this.Type != DirectoryItemTypeSQL.File; } }

        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.Children?.Count(f => f != null) > 0;
            }
            set
            {
                // If the UI tells us to expand...
                if (value == true)
                    // Find all children
                    Expand();
                // If the UI tells us to close
                else
                    this.ClearChildren();
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to expand this item
        /// </summary>
        public ICommand ExpandCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fullPath">The full path of this item</param>
        /// <param name="type">The type of item</param>
        public DirectoryItemViewModelSQL(string fullPath, DirectoryItemTypeSQL type, int idIt, string level1, string level2, 
            string level3, string level4, string level5, string level6, int idR)
        {
            // Create commands
            this.ExpandCommand = new RelayCommand(Expand);

            // Set path and type
            this.FullPath = fullPath;
            this.Type = type;
            this.idItem = idIt;

            this.idRec = idR;

            this.level1 = level1;
            this.level2 = level2;
            this.level3 = level3;
            this.level4 = level4;
            this.level5 = level5;
            this.level6 = level6;

            switch (idIt)
            {
                case 1:
                    this.level1 = fullPath;
                    break;
                case 2:
                    this.level2 = fullPath;
                    break;
                case 3:
                    this.level3 = fullPath;
                    break;
                case 4:
                    this.level4 = fullPath;
                    break;
                case 5:
                    this.level5 = fullPath;
                    break;
                case 6:
                    this.level6 = fullPath;
                    break;
            }

            // Setup the children as needed
            this.ClearChildren();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Removes all children from the list, adding a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            // Clear items
            this.Children = new ObservableCollection<DirectoryItemViewModelSQL>();

            // Show the expand arrow if we are not a file
            if (this.Type != DirectoryItemTypeSQL.File)
                this.Children.Add(null);
        }

        #endregion

        /// <summary>
        ///  Expands this directory and finds all children
        /// </summary>
        private void Expand()
        {
            // We cannot expand a file
            if (this.Type == DirectoryItemTypeSQL.File)
                return;
            //if (this.idItem == 0) this.idItem = 2;
            // Find all children
            var children = DirectoryStructureSQL.GetDirectoryContentsSQL(this.FullPath, this.idItem + 1, this.level1, this.level2, 
                this.level3, this.level4, this.level5, this.level6, this.idRec);
            this.Children = new ObservableCollection<DirectoryItemViewModelSQL>(
                                children.Select(content => new DirectoryItemViewModelSQL(content.FullPath, content.Type,
                                this.idItem  + 1, this.level1, this.level2, this.level3, this.level4, this.level5, this.level6, content.idRec)));
        }
    }
}