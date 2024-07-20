namespace Illusion.Utilities;

internal class TimeFilter
{
  public static List<TimeFilter> Standard = new List<TimeFilter>
  {
    new TimeFilter("All", (min, max, now) => new DateTimeRange(
      min,
      max)),

    new TimeFilter("Today", (min, max, now) => new DateTimeRange(
      now.Date,
      now.Date)),

    new TimeFilter("This Week", (min, max, now) => new DateTimeRange(
      now.RoundWeekDown(DayOfWeek.Sunday),
      now.RoundWeekDown(DayOfWeek.Sunday).AddDays(6))),

    new TimeFilter("Last Week", (min, max, now) => new DateTimeRange(
      now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-7),                  
      now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-1))),

    new TimeFilter("This Month", (min, max, now) => new DateTimeRange(
      now.RoundMonthDown(),
      now.RoundMonthDown().AddMonths(1).AddDays(-1))),

    new TimeFilter("Last Month", (min, max, now) => new DateTimeRange(
      now.RoundMonthDown().AddMonths(-1),
      now.RoundMonthDown().AddDays(-1))),
  };

  public readonly string Name;

  readonly GetRangeDelegate GetRangeInner;

  public TimeFilter(string name, GetRangeDelegate getRange)
  {
    GetRangeInner = getRange;
    Name = name;
  }

  public DateTimeRange GetRange(DateTime min, DateTime max) => GetRangeInner(min, max, DateTime.Now);

  public override string ToString() => Name;
}

internal delegate DateTimeRange GetRangeDelegate(DateTime min, DateTime max, DateTime now);
