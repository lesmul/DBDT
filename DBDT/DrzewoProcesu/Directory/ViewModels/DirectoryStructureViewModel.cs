using DBDT.DrzewoProcesu.Directory.Data;
using DBDT.DrzewoProcesu.Directory.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace DBDT.DrzewoProcesu.Directory.ViewModels
{
    /// <summary>
    /// The view model for the applications main Directory view
    /// </summary>
    public class DirectoryStructureViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoryStructureViewModel(string lokalizacja_start = "", string findfile = "")
        {
            // Get the logical drives
            var children = DirectoryStructure.GetLogicalDrives(lokalizacja_start);

            // Create the view models from the data
            if (lokalizacja_start == "")
            {
                this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, "")));
            }
            else
            {
                this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(folder => new DirectoryItemViewModel(folder.FullPath, DirectoryItemType.Drive, folder.TypeFile, findfile)));
            }
       
        }

        #endregion
    }
}
