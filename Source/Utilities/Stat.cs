namespace Illusion.Utilities
{
  internal class Stat<T>
  {
    public readonly string Name;
    public readonly string? Format;
    public readonly Type Type;

    readonly Func<T, object> ComputeInner;

    public Stat(string name, Func<T, DateTime> compute)
    {
      Name = name;
      Format = "M/d/yy";
      Type = typeof(DateTime);

      ComputeInner = group => compute(group);
    }

    public Stat(string name, Func<T, double> compute)
    {
      Name = name;
      Format = "F2";
      Type = typeof(double);

      ComputeInner = item => compute(item);
    }

    public Stat(string name, Func<T, int> compute)
    {
      Name = name;
      Format = null;
      Type = typeof(int);

      ComputeInner = item => compute(item);
    }

    public object Compute(T item) => ComputeInner(item);
  }
}
