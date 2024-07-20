using Illusion.Utilities;
using System.Data;
using System.Globalization;

namespace Illusion.Data;

static internal class Parser
{
  public static IllusionSet Parse(Dictionary<string, DataTable> tables)
  {
    var ledgerTable = tables.GetValueOrDefault("Ledger");
    var peopleTable = tables.GetValueOrDefault("People");
    var incomeTable = tables.GetValueOrDefault("Income");

    var blocks = ParseBlocks(ledgerTable);
    var people = ParsePeople(peopleTable);
    var income = ParseIncomes(incomeTable);
    var errors = new List<string>();

    // check for duplicate initials
    var duplicateInitials = people
      .GroupBy(p => $@"{p.Company}: {p.Initials}")
      .Where(group => group.Count() > 1)
      .ToList();
    if (duplicateInitials.Count != 0)
    {
      errors.Add("Duplicate initials for the same company in the 'People' table:\r\n" + string.Join("\r\n", duplicateInitials.Select(group => $@"  * {group.Key}").ToArray()));
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

        var person = people.FirstOrDefault(p => p.Company == company && p.Initials == initials);
        if (person is null)
        {
          if (!missingInitialsByCompany.ContainsKey(company))
          {
            missingInitialsByCompany[company] = [];
          }
          missingInitialsByCompany[company].Add(initials);
        }
      }
    }
    if (missingInitialsByCompany.Count != 0)
    {
      var error = new List<string>
      {
        $@"Some initials are referenced by the 'Ledger' but have no initials defined in 'People'. By 'Company', these are:",
      };

      foreach (var company in missingInitialsByCompany.OrderBy(x => x.Key))
      {
        error.Add($@"  * {company.Key}");
        foreach (var initials in company.Value.OrderBy(x => x))
        {
          error.Add($@"    - {initials}");
        }
      }

      errors.Add(string.Join("\r\n", error));
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
      var errorLines = new List<string>
      {
        $@"These 'Leger' entries are overlong (>8 hrs). Add the text '[LONG]' to their 'Raw' description if this is legitimate:",
      };

      foreach (var o in overlong)
      {
        errorLines.Add($@"  * {o.Time} | {o.Raw}");
      }

      errors.Add(string.Join("\r\n", errorLines.ToArray()));
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
      var errorLines = new List<string>
      {
        $@"There are overlapping blocks",
      };

      foreach (var o in overlaps)
      {
        errorLines.Add($@"  * {o.Time} | {o.Raw}");
      }

      errors.Add(string.Join("\r\n", errorLines.ToArray()));
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

      if (overlappingDateBlocks.Count != 0 && overlappingTimeBlocks.Count != 0)
      {
        throw new ParsingException("Date-only blocks and specific time blocks overlap the same income");
      }
      else if (overlappingDateBlocks.Count == 0 && overlappingTimeBlocks.Count == 0)
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

    // process initials
    var peopleByCompanyThenInitials = new Dictionary<string, Dictionary<string, List<Person>>>();
    foreach (var person in people)
    {
      var company = person.Company;
      var initials = person.Initials;

      if (!peopleByCompanyThenInitials.ContainsKey(company))
      {
        peopleByCompanyThenInitials[company] = new Dictionary<string, List<Person>>();
      }

      if (!peopleByCompanyThenInitials[company].ContainsKey(initials))
      {
        peopleByCompanyThenInitials[company][initials] = new List<Person>();
      }

      peopleByCompanyThenInitials[company][initials].Add(person);
    }
    foreach (var block in blocks)
    {
      var peopleByInitials = peopleByCompanyThenInitials.GetValueOrDefault(block.Scope.Company);

      for (var i = 0; i < block.Scope.People.Length; i++)
      {
        var name = block.Scope.People[i];

        var persons = peopleByInitials?.GetValueOrDefault(name);

        var newName = persons is null || persons.Count == 0 ? name :
                      persons.Count == 1 ? persons.Single().ToString() :
                      $@"{name} (?)";

        block.Scope.People[i] = newName;
      }
    }

    return new IllusionSet(blocks, income, people, errors);
  }

  #region Blocks

  static List<Block> ParseBlocks(DataTable? table)
  {
    if (table is null)
    {
      return new List<Block>();
    }

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

  static List<Income> ParseIncomes(DataTable? table)
  {
    if (table is null)
    {
      return new List<Income>();
    }

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
    var amount = GetDecimal(row, amountColumnName);
    var start = GetDate(row, statColumnName);
    var stop = GetDate(row, stopColumnName);

    if (amount is null || company is null || start is null || stop is null)
    {
      var requriredColumns = new[] { companyColumnName, amountColumnName, statColumnName, stopColumnName };
      throw new ParsingException($@"The following columns are required: {string.Join(" ,", requriredColumns)}");
    }

    return new Income((double)amount.Value, company, start.Value, stop.Value.AddDays(1));
  }

  #endregion

  #region People

  static List<Person> ParsePeople(DataTable? table)
  {
    if (table is null)
    {
      return new List<Person>();
    }

    return table.Rows
      .Cast<DataRow>()
      .Select(ParsePerson)
      .ToList();
  }

  static Person ParsePerson(DataRow row)
  {
    var company = GetString(row, Constants.PeopleTable.Columns.Company);
    var initials = GetString(row, Constants.PeopleTable.Columns.Initials);
    var firstName = GetString(row, Constants.PeopleTable.Columns.FirstName);
    var lastName = GetString(row, Constants.PeopleTable.Columns.LastName);

    if (string.IsNullOrWhiteSpace(company) ||
        string.IsNullOrWhiteSpace(initials) ||
        string.IsNullOrWhiteSpace(firstName) ||
        string.IsNullOrWhiteSpace(lastName))
    {
      var requriredColumns = new[] 
      {
        Constants.PeopleTable.Columns.Company,
        Constants.PeopleTable.Columns.Initials,
        Constants.PeopleTable.Columns.FirstName,
        Constants.PeopleTable.Columns.LastName,
      };
      throw new ParsingException($@"The following columns are required: {string.Join(" ,", requriredColumns)}");
    }

    return new Person(company, initials, firstName, lastName);
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

  static decimal? GetDecimal(DataRow row, string columnName)
  {
    var str = GetString(row, columnName);
    if (str is null)
    {
      return null;
    }

    if (!decimal.TryParse(str, out var parsed))
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
