using System;

namespace Illusion
{
  public static class DateTimeUtils
  {
    /// <summary>Returns true if two ranges bounded by the given DateTimes overlap.</summary>
    public static bool Intersects(DateTime a0, DateTime a1, DateTime b0, DateTime b1)
    {
      if (b0 > a1)
      {
        return false;
      }
      return a0 <= b1;
    }

    ///<summary>Returns the later of the two DateTimes.</summary>
    public static DateTime Max(DateTime a, DateTime b)
    {
      return new DateTime(Math.Max(a.Ticks, b.Ticks));
    }

    /// <summary>Returns the earlier of the two DateTimes.</summary>
    public static DateTime Min(DateTime a, DateTime b)
    {
      return new DateTime(Math.Min(a.Ticks, b.Ticks));
    }

    /// <summary>Rounds to the nearest hour.</summary>
    public static DateTime RoundHour(this DateTime dt)
    {
      var roundedDown = dt.RoundHourDown();
      return dt.Ticks - roundedDown.Ticks >= TimeSpan.TicksPerHour / 2 ? roundedDown.AddTicks(TimeSpan.TicksPerHour) : roundedDown;
    }

    /// <summary>Rounds down to the nearest hour.</summary>
    public static DateTime RoundHourDown(this DateTime dt)
    {
      return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
    }

    /// <summary>Rounds up to the nearest hour.</summary>
    public static DateTime RoundHourUp(this DateTime dt)
    {
      var roundedDown = dt.RoundHourDown();
      return dt.Ticks - roundedDown.Ticks > 0 ? roundedDown.AddTicks(TimeSpan.TicksPerHour) : roundedDown;
    }

    /// <summary>Rounds to the nearest day.</summary>
    public static DateTime RoundDay(this DateTime dt)
    {
      var roundedDown = dt.RoundDayDown();
      return dt.Ticks - roundedDown.Ticks >= TimeSpan.TicksPerDay / 2 ? roundedDown.AddTicks(TimeSpan.TicksPerDay) : roundedDown;
    }

    /// <summary>Rounds down to the nearest day.</summary>
    public static DateTime RoundDayDown(this DateTime dt)
    {
      return dt.Date;
    }

    /// <summary>Rounds up to the nearest date.</summary>
    public static DateTime RoundDayUp(this DateTime dt)
    {
      var roundedDown = dt.RoundDayDown();
      return dt.Ticks - roundedDown.Ticks > 0 ? roundedDown.AddTicks(TimeSpan.TicksPerDay) : roundedDown;
    }

    /// <summary>Rounds up to the next date the given day of the week will be on.</summary>
    public static DateTime RoundWeek(this DateTime dt, DayOfWeek dayOfWeek)
    {
      var roundedDown = dt.RoundWeekDown(dayOfWeek);
      return dt.Ticks - roundedDown.Ticks >= (TimeSpan.TicksPerDay * 7) / 2 ? roundedDown.AddTicks(TimeSpan.TicksPerDay * 7) : roundedDown;
    }

    /// <summary>Rounds down to the previous date the given day of the week was on.</summary>
    public static DateTime RoundWeekDown(this DateTime dt, DayOfWeek dayOfWeek)
    {
      while (dt.DayOfWeek != dayOfWeek)
      {
        dt = dt.AddDays(-1);
      }
      return dt.Date;
    }

    /// <summary>Rounds up to the next date the given day of the week will be on.</summary>
    public static DateTime RoundWeekUp(this DateTime dt, DayOfWeek dayOfWeek)
    {
      var roundedDown = dt.RoundWeekDown(dayOfWeek);
      return dt.Ticks - roundedDown.Ticks > 0 ? roundedDown.AddTicks(TimeSpan.TicksPerDay * 7) : roundedDown;
    }

    /// <summary>Rounds to the nearest month.</summary>
    public static DateTime RoundMonth(this DateTime dt)
    {
      var down = RoundMonthDown(dt);
      var up = RoundMonthUp(dt);
      return (dt.Ticks - down.Ticks) > (up.Ticks - dt.Ticks) ? up : down;
    }

    /// <summary>Rounds down to the next month.</summary>
    public static DateTime RoundMonthDown(this DateTime dt)
    {
      return new DateTime(dt.Year, dt.Month, 1);
    }

    /// <summary>Rounds up to the next month.</summary>
    public static DateTime RoundMonthUp(this DateTime dt)
    {
      if (dt.Day == 1 && dt.TimeOfDay.TotalDays == 0)
      {
        return dt;
      }
      return RoundMonthDown(dt).AddMonths(1);
    }
  }
}