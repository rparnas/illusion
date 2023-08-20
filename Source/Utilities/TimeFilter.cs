namespace Illusion.Utilities
{
  internal class TimeFilter
  {
    public static List<TimeFilter> Standard = new List<TimeFilter>
    {
      new TimeFilter("Today", now => new DateTimeRange(
        now.Date,
        now.Date)),

      new TimeFilter("This Week", now => new DateTimeRange(
        now.RoundWeekDown(DayOfWeek.Sunday),
        now.RoundWeekDown(DayOfWeek.Sunday).AddDays(6))),

      new TimeFilter("Last Week", now => new DateTimeRange(
        now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-7),                  
        now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-1))),

      new TimeFilter("This Month", now => new DateTimeRange(
        now.RoundMonthDown(),
        now.RoundMonthDown().AddMonths(1).AddDays(-1))),

      new TimeFilter("Last Month", now => new DateTimeRange(
        now.RoundMonthDown().AddMonths(-1),
        now.RoundMonthDown().AddDays(-1))),
    };

    public readonly string Name;

    readonly Func<DateTime, DateTimeRange> GetRangeFunc;

    public TimeFilter(string name, Func<DateTime, DateTimeRange> getRange)
    {
      GetRangeFunc = getRange;
      Name = name;
    }

    public DateTimeRange GetRange() => GetRangeFunc(DateTime.Now);

    public override string ToString() => Name;
  }
}
