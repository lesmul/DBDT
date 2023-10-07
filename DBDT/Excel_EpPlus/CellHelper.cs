//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Media;

//namespace DBDT.Excel
//{
//    public static class CellHelper
//    {
//        public static DataGridCell GetCell(DataGrid dataGrid, int rowIndex, int columnIndex)
//        {
//            if (dataGrid == null || rowIndex < 0 || columnIndex < 0)
//            {
//                return null;
//            }

//            DataGridRow row = GetRow(dataGrid, rowIndex);

//            if (row == null)
//            {
//                return null;
//            }

//            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

//            if (presenter == null)
//            {
//                return null;
//            }

//            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

//            return cell;
//        }

//        public static DataGridRow GetRow(DataGrid dataGrid, int index)
//        {
//            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);

//            if (row == null)
//            {
//                // Jeśli nie jest widoczny, przewiń do wiersza
//                dataGrid.ScrollIntoView(dataGrid.Items[index]);
//                row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
//            }

//            return row;
//        }

//        public static T GetVisualChild<T>(Visual parent) where T : Visual
//        {
//            T child = default(T);
//            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

//            for (int i = 0; i < numVisuals; i++)
//            {
//                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
//                child = v as T;

//                if (child == null)
//                {
//                    child = GetVisualChild<T>(v);
//                }

//                if (child != null)
//                {
//                    break;
//                }
//            }

//            return child;
//        }
//    }
//}