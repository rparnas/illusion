using OfficeOpenXml;
using System.Data;

namespace Illusion.Utilities;

public static class Loader
{
  static Loader()
  {
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
  }

  internal static DataTable? GetExcelSheet(string path, string worksheetName)
  {
    var dict = GetExcelSheets(path, ws => ws == worksheetName);
    return dict.TryGetValue(worksheetName, out var dt) ? dt : null;
  }

  internal static Dictionary<string, DataTable> GetExcelSheets(string path, Func<string, bool>? sheetNamePredicate)
  {
    var ret = new Dictionary<string, DataTable>();

    var tmpPath = Path.Combine(Path.GetTempPath(), $@"{Guid.NewGuid()}.xlsx");
    File.Copy(path, tmpPath);

    using (var ep = new ExcelPackage(tmpPath))
    {
      foreach (var worksheet in ep.Workbook.Worksheets)
      {
        if (sheetNamePredicate?.Invoke(worksheet.Name) ?? true && worksheet.Dimension != null)
        {
          ret.Add(worksheet.Name, SheetToTable(worksheet));
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
    for (var col = 1; col <= ws.Dimension.End.Column; col++)
    {
      var cell = ws.Cells[1, col];
      var text = cell.Text.Replace('\n', '_').Replace('\r', '_');

      // Make up a name for columns without text
      if (string.IsNullOrWhiteSpace(text))
      {
        text = cell.Address;
      }

      if (!usedCols.ContainsKey(text))
      {
        usedCols[text] = 0;
      }
      
      usedCols[text]++;
      ret.Columns.Add(text + (usedCols[text] == 1 ? "" : usedCols[text].ToString()));
    }

    for (var row = 2; row <= ws.Dimension.End.Row; row++)
    {
      var newRow = ret.NewRow();

      for (var col = 1; col <= ws.Dimension.End.Column; col++)
      {
        newRow[col - 1] = ws.Cells[row, col].Text;
      }

      ret.Rows.Add(newRow);
    }

    return ret;
  }
}
