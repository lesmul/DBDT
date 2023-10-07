public class ExcelCellData
{
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int StartColumn { get; set; }
    public int EndColumn { get; set; }
    public string CellColor { get; set; }

    // Konstruktor klasy.
    public ExcelCellData(int startRow, int endRow, int startColumn, int endColumn, string cellColor)
    {
        StartRow = startRow;
        EndRow = endRow;
        StartColumn = startColumn;
        EndColumn = endColumn;
        CellColor = cellColor;
    }
}
