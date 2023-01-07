using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Illusion
{
  public static class Loader
  {
    public static DataTable GetXLSX(string path, string sheetName)
    {
      DataTable ret = null;

      var tmpPath = Path.GetTempPath() + Guid.NewGuid().ToString();
      File.Copy(path, tmpPath);

      using (var ep = new ExcelPackage(new FileInfo(tmpPath)))
      {
        foreach (var worksheet in ep.Workbook.Worksheets)
        {
          if (worksheet.Name == sheetName && worksheet.Dimension != null)
          {
            ret = SheetToTable(worksheet);
            break;
          }
        }
      }
      File.Delete(tmpPath);

      return ret;
    }

    static DataTable SheetToTable(ExcelWorksheet ws)
    {
      var ret = new DataTable(ws.Name);

      var usedCols = new Dictionary<string, int>();
      for (int col = 1; col <= ws.Dimension.End.Column; col++)
      {
        var cell = ws.Cells[1, col];
        var text = cell.Text.Replace('\n', '_').Replace('\r', '_');

        // Make up a name for columns without text
        if (string.IsNullOrWhiteSpace(text))
          text = cell.Address;

        if (!usedCols.ContainsKey(text))
          usedCols[text] = 0;

        usedCols[text]++;
        ret.Columns.Add(text + (usedCols[text] == 1 ? "" : usedCols[text].ToString()));
      }

      for (int row = 2; row <= ws.Dimension.End.Row; row++)
      {
        var newRow = ret.NewRow();

        for (int col = 1; col <= ws.Dimension.End.Column; col++)
          newRow[col - 1] = ws.Cells[row, col].Text;

        ret.Rows.Add(newRow);
      }

      return ret;
    }
  }
}
