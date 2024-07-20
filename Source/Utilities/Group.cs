namespace Illusion.Utilities;

internal class Group<T>
{
  public readonly string Name;
  public readonly List<T> Items;

  public Group(string name, List<T> items)
  {
    Name = name;
    Items = items;
  }
}
