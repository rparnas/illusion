namespace Illusion.Utilities;

internal class Stat<T>
{
  public readonly string Name;
  public readonly string? Format;
  public readonly bool IsDate;
  public readonly bool IsNumber;
  public readonly Type Type;

  readonly Func<T, object> ComputeInner;

  public Stat(string name, string? format, Func<T, DateTime> compute)
  {
    Name = name;
    Format = format;
    IsDate = true;
    IsNumber = false;
    Type = typeof(DateTime);

    ComputeInner = group => compute(group);
  }

  public Stat(string name, string? format, Func<T, decimal> compute)
  {
    Name = name;
    Format = format;
    IsDate = false;
    IsNumber = true;
    Type = typeof(double);

    ComputeInner = item => compute(item);
  }

  public Stat(string name, string? format, Func<T, double> compute)
  {
    Name = name;
    Format = format;
    IsDate = false;
    IsNumber = true;
    Type = typeof(double);

    ComputeInner = item => compute(item);
  }

  public Stat(string name, string? format, Func<T, int> compute)
  {
    Name = name;
    Format = null;
    IsDate = false;
    IsNumber = true;
    Type = typeof(int);

    ComputeInner = item => compute(item);
  }

  public object Compute(T item) => ComputeInner(item);
}
