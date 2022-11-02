using DBDT.DrzewoSQL.Directory.Data;
using DBDT.DrzewoProcesu.Directory.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace DBDT.DrzewoSQL.Directory.ViewModels
{
    /// <summary>
    /// The view model for the applications main Directory view
    /// </summary>
    public class DirectoryStructureViewModelSQL : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModelSQL> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoryStructureViewModelSQL()
        {
            // Get the logical level
            var children = DirectoryStructureSQL.GetLogicalDrivesSQL();

            // Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModelSQL>(
            children.Select(level => new DirectoryItemViewModelSQL(level.FullPath, DirectoryItemTypeSQL.Drive, 1, 
            level.FullPath,null,null,null,null,null,level.idRec)));

        }

        #endregion
    }
}
