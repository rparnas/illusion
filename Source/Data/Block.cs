namespace Illusion.Data;

internal class Block : IScope
{
  public double Amount { get; set; }
  public string[] Others { get; }
  public string Raw { get; }
  public Scope Scope { get; }
  public Time Time { get; }

  public double DevHours => Time.Hours * (Scope.People.Length - 1);

  public Block(string[] others, string raw, Scope scope, Time time)
  {
    Others = others.OrderBy(name => name).ToArray();
    Raw = raw;
    Scope = scope;
    Time = time;
  }

  public bool GetOverlaps(Block other)
  {
    return this != other &&
      Time.Start.HasValue && Time.Stop.HasValue && 
      other.Time.Start.HasValue && other.Time.Stop.HasValue &&
      Time.Start < other.Time.Stop && other.Time.Start < Time.Stop;
  }

  public bool GetOverlaps(DateTime start, DateTime stop)
  {
    return 
      Time.Start.HasValue && Time.Stop.HasValue &&
      Time.Start < stop && start < Time.Stop;
  }
}
