using Illusion.Data;
using System.Data;

namespace Illusion.Utilities;

internal class Reports
{
  public static DataTable MonthReport(List<Block> blocks, Func<Block, string> scope)
  {
    var startMonth = new DateTime(2015, 5, 1);
    var endMonth = new DateTime(2023, 8, 1);

    var scopes = blocks
      .Select(scope)
      .Distinct()
      .OrderBy(feature => feature)
      .ToList();

    var ret = new DataTable("Month Report");
    ret.Columns.Add("Month");
    ret.Columns.AddRange(scopes.Select(name => new DataColumn(name)).ToArray());
    ret.Columns.Add("Total");

    for (var month = startMonth; month <= endMonth; month = month.AddMonths(1))
    {
      var startDate = month.Date;
      var endDate = month.Date.AddMonths(1);

      var monthBlocks = blocks
        .Where(x => x.Time.Start >= startDate && x.Time.Start < endDate)
        .ToList();

      var totalHours = monthBlocks
        .Sum(x => x.Time.Hours);

      var hoursByFeature = monthBlocks
        .GroupBy(x => x.Scope.Feature)
        .ToDictionary(group => group.Key, group => group.Sum(x => x.Time.Hours));

      var row = ret.NewRow();
      row["Month"] = month.ToString("MMM yyyy");
      row["Total"] = totalHours;

      foreach (var pair in hoursByFeature)
      {
        row[pair.Key] = pair.Value;
      }

      ret.Rows.Add(row);
    }

    return ret;
  }
}
