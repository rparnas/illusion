namespace Illusion.Data
{
  internal class Time : IComparable
  {
    public DateTime Date { get; }
    public DateTime? Start { get; }
    public DateTime? Stop { get; }
    public double Hours { get; }

    public Time(DateTime date, double hours)
    {
      Date = date;
      Start = null;
      Stop = null;
      Hours = hours;
    }

    public Time(DateTime start, DateTime stop)
    {
      Date = start.Date;
      Start = start;
      Stop = stop;
      Hours = (stop - start).TotalHours;
    }

    public int CompareTo(object? obj)
    {
      if (obj is null)
      {
        return 1;
      }
      else if (obj is Time other)
      {
        if (Start.HasValue && other.Start.HasValue)
        {
          return Start.Value.CompareTo(other.Start.Value);
        }
        return Date.CompareTo(other.Date);
      }
      else
      {
        throw new ArgumentException($@"Object is not a '{nameof(Block)}'");
      }
    }

    public override string ToString()
    {
      if (Start.HasValue && Stop.HasValue)
      {
        var startStr = Start.Value.ToString("htt")
          .Replace("AM", "a")
          .Replace("PM", "p");

        var stopStr = Stop.Value.ToString("htt")
          .Replace("AM", "a")
          .Replace("PM", "p");

        return $@"{Date:M/d/yy} {startStr} ~ {stopStr}";
      }
      else
      {
        return $@"{Date:M/d/yy} ({Hours}hrs)";
      }
    }
  }
}
