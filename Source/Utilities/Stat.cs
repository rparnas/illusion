namespace Illusion.Utilities;

internal class Stat<T>
{
  public readonly string Name;
  public readonly string? Format;
  public readonly bool IsDate;
  public readonly bool IsNumber;
  public readonly Type Type;

  readonly Func<T, object> ComputeInner;

  public Stat(string name, Func<T, DateTime> compute)
  {
    Name = name;
    Format = "MM/dd/yy";
    IsDate = true;
    IsNumber = false;
    Type = typeof(DateTime);

    ComputeInner = group => compute(group);
  }

  public Stat(string name, Func<T, double> compute)
  {
    Name = name;
    Format = "F2";
    IsDate = false;
    IsNumber = true;
    Type = typeof(double);

    ComputeInner = item => compute(item);
  }

  public Stat(string name, Func<T, int> compute)
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
