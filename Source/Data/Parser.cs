using Illusion.Utilities;
using System.Data;
using System.Globalization;
using System.Text;

namespace Illusion.Data
{
  static internal class Parser
  {
    public static IllusionSet Parse(Dictionary<string, DataTable> tables)
    {
      var blocks = ParseBlocks(tables["Ledger"]);
      var people = ParsePeople(tables["People"]);
      var income = ParseIncomes(tables["Income"]);

      // check for duplicate initials
      var duplicates = people
        .GroupBy(p => p.Initials)
        .Where(group => group.Count() > 1)
        .ToList();
      if (duplicates.Any())
      {
        var errorMessage = $@"Duplicate initials defined in 'People' table: {string.Join(", ", duplicates.Select(group => group.Key).ToArray())}";
        throw new ParsingException(errorMessage);
      }

      // check for blocks referencing unknown initials
      var missingInitialsByCompany = new Dictionary<string,HashSet<string>>();
      foreach (var block in blocks)
      {
        foreach (var initials in block.Scope.People)
        {
          var company = block.Scope.Company;

          if (initials == Constants.Initials.Self || initials == Constants.Initials.Unspecified)
          {
            continue;
          }

          var person = people.FirstOrDefault(p => p.Companies.Contains(company) && p.Initials == initials);
          if (person is null)
          {
            if (!missingInitialsByCompany.ContainsKey(company))
            {
              missingInitialsByCompany[company] = new HashSet<string>();
            }
            missingInitialsByCompany[company].Add(initials);
          }
        }
      }
      if (missingInitialsByCompany.Any())
      {
        var dict = missingInitialsByCompany;
        var newline = "\r\n";
        var errorMessage = 
          $@"The following initials are referenced in 'Blocks' but have no people defined in 'People':{newline}" +
          $@"{string.Join(newline, dict.Select(x => $@" * {x.Key}: {string.Join(", ", x.Value.OrderBy(i => i).ToArray())}").ToArray())}";
        throw new ParsingException(errorMessage);
      }

      // output others so they can be manually reviewed for typos
      var others = blocks
        .SelectMany(b => b.Others)
        .Distinct()
        .OrderBy(name => name)
        .ToList();
      var othersStr = string.Join("\r\n", others.ToArray());

      // check for overlong blocks
      var overlong = blocks
        .Where(b => b.Time.Hours > 8 && b.Time.Start.HasValue && b.Time.Stop.HasValue && !b.Raw.Contains("[LONG]"))
        .ToList();
      if (overlong.Any())
      {
        var sb = new StringBuilder();
        sb.AppendLine();
        foreach (var o in overlong)
        {
          sb.AppendLine($@"  * {o.Time} | {o.Raw}");
        }
        var message = $@"There are some overlong blocks (>8 hrs). Add the text '[LONG]' to their 'Raw' description to ignore:{sb}";
        throw new ParsingException(message);
      }

      // check for overlapping blocks
      var overlaps = new HashSet<Block>();
      for (var i = 0; i < blocks.Count - 1; i++)
      {
        if (blocks[i].GetOverlaps(blocks[i + 1]))
        {
          overlaps.Add(blocks[i]);
          overlaps.Add(blocks[i + 1]);
        }
      }
      if (overlaps.Any())
      {
        var sb = new StringBuilder();
        sb.AppendLine();
        foreach (var o in overlaps)
        {
          sb.AppendLine($@"  * {o.Time} | {o.Raw}");
        }
        var message = $@"There are overlapping blocks:{sb}";
        throw new ParsingException(message);
      }

      // distribute income
      foreach (var i in income)
      {
        var overlappingDateBlocks = blocks
          .Where(b => b.Scope.Company == i.Company &&
                      !b.Time.Start.HasValue && !b.Time.Start.HasValue &&
                      b.Time.Date >= i.Start && b.Time.Date < i.Stop)
          .ToList();

        var overlappingTimeBlocks = blocks
          .Where(b => b.Scope.Company == i.Company &&
                      b.Time.Start.HasValue && b.Time.Stop.HasValue &&
                      b.Time.Start >= i.Start && b.Time.Stop < i.Stop)
          .ToList();

        if (overlappingDateBlocks.Any() && overlappingTimeBlocks.Any())
        {
          throw new ParsingException("Date-only blocks and specific time blocks overlap the same income");
        }
        else if (!overlappingDateBlocks.Any() && !overlappingTimeBlocks.Any())
        {
          throw new ParsingException("Non-distributed income");
        }

        var blocksByHours = new Dictionary<Block, double>();
        foreach (var block in overlappingDateBlocks)
        {
          blocksByHours.Add(block, block.Time.Hours);
        }
        foreach (var block in overlappingTimeBlocks)
        {
          var blockRange = new DateTimeRange(block.Time.Start!.Value, block.Time.Stop!.Value);
          var incomeRange = new DateTimeRange(i.Start, i.Stop);
          var overlapRange = blockRange.Intersection(incomeRange);
          var overlapHours = (overlapRange.Stop - overlapRange.Start).TotalHours;
          blocksByHours.Add(block, overlapHours);
        }
        var blocksHoursSum = blocksByHours.Values.Sum();
        var amountPerHour = i.Amount / blocksHoursSum;

        foreach (var block in overlappingDateBlocks)
        {
          var amt = block.Time.Hours * amountPerHour;
          block.Amount += amt;
        }
        foreach (var block in overlappingTimeBlocks)
        {
          var amt = block.Time.Hours * amountPerHour;
          block.Amount += amt;
        }
      }

      return new IllusionSet(blocks, income, people);
    }

    #region Blocks

    static List<Block> ParseBlocks(DataTable table)
    {
      return table.Rows
        .Cast<DataRow>()
        .Select(ParseBlock)
        .Where(block => block is not null)
        .Cast<Block>()
        .OrderBy(block => block.Time)
        .ToList();
    }

    static Block? ParseBlock(DataRow row)
    {
      var raw = GetString(row, "Raw") ?? "";
      var others = ParseBlockOthers(row);
      var scope = ParseBlockScope(row);

      var time = ParseBlockTime(row);
      if (time is null)
      {
        return null;
      }

      return new Block(others, raw, scope, time);
    }

    static string[] ParseBlockOthers(DataRow row)
    {
      var str = GetString(row, "Others") ?? "";

      return str
        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
        .Select(s => s.Trim())
        .ToArray();
    }

    static string[] ParseBlockPeople(DataRow row)
    {
      return (Constants.Initials.Self + "," + (GetString(row, "People") ?? ""))
        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
        .Select(s => s.Trim())
        .ToArray();
    }

    static Scope ParseBlockScope(DataRow row)
    {
      return new Scope(
        company: GetString(row, "Company") ?? "",
        project: GetString(row, "Project") ?? "",
        feature: GetString(row, "Feature") ?? "",
        activity: GetString(row, "Activity") ?? "",
        people: ParseBlockPeople(row));
    }

    static Time? ParseBlockTime(DataRow row)
    {
      const string dateColumnName = "Date";
      const string hoursColumnName = "Hours";
      const string startColumnName = "Start";
      const string stopColumnName = "Stop";

      var date = GetDate(row, dateColumnName);
      if (!date.HasValue)
      {
        throw new ParsingException($@"'{dateColumnName}' is required.");
      }

      var hours = GetDouble(row, hoursColumnName);
      var start = GetTime(row, startColumnName, date);
      var stop = GetTime(row, stopColumnName, date);
      if (hours.HasValue)
      {
        // parse via hours

        if (start.HasValue || stop.HasValue)
        {
          throw new ParsingException($@"Must provide (1) '{dateColumnName}' or (2) '{startColumnName}' and '{stopColumnName}'");
        }

        return new Time(date.Value, hours.Value);
      }
      else
      {
        // parse vis start and stop
        if (!start.HasValue)
        {
          throw new ParsingException($@"Must provide (1) '{dateColumnName}' or (2) '{startColumnName}' and '{stopColumnName}'");
        }

        if (!stop.HasValue)
        {
          // ignore unfinished blocks
          return null;
        }

        // If stop is less than start, assume it refers to the next day.
        var adjustedStart = start.Value;
        var adjustedStop = stop.Value.AddDays(stop < start ? 1 : 0);

        return new Time(adjustedStart, adjustedStop);
      }
    }

    #endregion

    #region Income

    static List<Income> ParseIncomes(DataTable table)
    {
      return table.Rows
        .Cast<DataRow>()
        .Select(ParseIncome)
        .ToList();
    }

    static Income ParseIncome(DataRow row)
    {
      const string companyColumnName = "Company";
      const string amountColumnName = "Amount";
      const string statColumnName = "Start";
      const string stopColumnName = "Stop";

      var company = GetString(row, companyColumnName);
      var amount = GetDouble(row, amountColumnName);
      var start = GetDate(row, statColumnName);
      var stop = GetDate(row, stopColumnName);

      if (company is null || amount is null || start is null || stop is null)
      {
        var requriredColumns = new[] { companyColumnName, amountColumnName, statColumnName, stopColumnName };
        throw new ParsingException($@"The following columns are required: {string.Join(" ,", requriredColumns)}");
      }

      return new Income(amount.Value, company, start.Value, stop.Value.AddDays(1));
    }

    #endregion

    #region People

    static List<Person> ParsePeople(DataTable table)
    {
      return table.Rows
        .Cast<DataRow>()
        .Select(ParsePerson)
        .ToList();
    }

    static Person ParsePerson(DataRow row)
    {
      const string initialsColumnName = "Initials";
      const string lastNameColumnName = "Last Name";
      const string firstNameColumnName = "First Name";
      const string companiesColumnName = "Companies";

      var initials = GetString(row, initialsColumnName);
      var firstName = GetString(row, firstNameColumnName);
      var lastName = GetString(row, lastNameColumnName);

      var companies = (GetString(row, companiesColumnName) ?? "")
        .Split(",", StringSplitOptions.TrimEntries)
        .OrderBy(company => company)
        .ToList();

      if (string.IsNullOrEmpty(initials) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || !companies.Any())
      {
        var requriredColumns = new[] { initialsColumnName, lastNameColumnName, firstNameColumnName, companiesColumnName };
        throw new ParsingException($@"The following columns are required: {string.Join(" ,", requriredColumns)}");
      }

      return new Person(initials, firstName, lastName, companies);
    }

    #endregion

    #region Utilities

    static DateTime? GetDate(DataRow row, string columnName)
    {
      var str = GetString(row, columnName);
      if (str is null)
      {
        return null;
      }

      if (!DateTime.TryParseExact(str, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
      {
        return null;
      }

      return parsed;
    }

    static double? GetDouble(DataRow row, string columnName)
    {
      var str = GetString(row, columnName);
      if (str is null)
      {
        return null;
      }

      if (!double.TryParse(str, out var parsed))
      {
        return null;
      }

      return parsed;
    }

    static string? GetString(DataRow row, string columnName)
    {
      if (!row.Table.Columns.Contains(columnName))
      {
        return null;
      }

      var raw = row[columnName];
      if (raw is null)
      {
        return null;
      }

      return raw
        .ToString()!
        .Trim();
    }

    static DateTime? GetTime(DataRow row, string columnName, DateTime? date)
    {
      var str = GetString(row, columnName);
      if (str is null || date is null)
      {
        return null;
      }

      var timeStr = str
        .Replace("a", "AM")
        .Replace("p", "PM");

      var dateTimeStr = $@"{date:M/d/yy} {timeStr}";

      if (!DateTime.TryParseExact(dateTimeStr, "M/d/yy h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
      {
        return null;
      }

      return parsed;
    }

    #endregion
  }
}
