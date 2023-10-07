using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExcelDataProcessor
{
    public List<ExcelCellData> ExcelCellDataList { get; set; }

    public ExcelDataProcessor(List<ExcelCellData> excelCellDataList)
    {
        ExcelCellDataList = excelCellDataList;
    }

    public string GenerateUniqueIFConditions(string filterColor)
    {

        if (ExcelCellDataList == null) return "Błąd!!!";

        StringBuilder conditions = new StringBuilder();
        HashSet<string> uniqueConditions = new HashSet<string>();

        foreach (var excelCellData in ExcelCellDataList)
        {
            int startRow = excelCellData.StartRow;
            int endRow = excelCellData.EndRow;
            int startColumn = excelCellData.StartColumn;
            int endColumn = excelCellData.EndColumn;
            string cellColor = excelCellData.CellColor;

            // Sprawdzamy, czy kolor komórki pasuje do podanego filtra.
            if (string.Equals(cellColor, filterColor, StringComparison.OrdinalIgnoreCase))
            {
                // Generuj warunek IF w zależności od wartości startRow, endRow, startColumn, endColumn i koloru komórki.
                string condition = $"IF[L1 > {startRow} AND L2 < {startColumn} && CellColor == \"{cellColor}\"] THEN %Błąd; ENDIF\n";

                // Dodaj warunek do unikalnego zbioru, jeśli jeszcze nie istnieje.
                if (!uniqueConditions.Contains(condition))
                {
                    conditions.Append(condition);
                    uniqueConditions.Add(condition);
                }
            }
        }

        return conditions.ToString();
    }

}