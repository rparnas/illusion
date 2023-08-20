namespace Illusion.Utilities
{
  internal class DateTimeRange
  {
    public readonly DateTime Start;
    public readonly DateTime Stop;

    public DateTimeRange(DateTime start, DateTime stop)
    {
      Start = start;
      Stop = stop;
    }

    public DateTimeRange Intersection(DateTimeRange other)
    {
      return new DateTimeRange(
        Start < other.Start ? other.Start : Start,
        Stop > other.Stop ? other.Stop : Stop);
    }

    public DateTimeRange Merge(DateTimeRange other)
    {
      return new DateTimeRange(
        Start < other.Start ? Start : other.Start,
        Stop > other.Stop ? Stop : other.Stop);
    }
  }
}
