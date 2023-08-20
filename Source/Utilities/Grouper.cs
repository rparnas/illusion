using System.Text.RegularExpressions;

namespace Illusion.Utilities
{
  internal class Grouper<T>
  {
    Func<T, bool, string[]> GetKeys;
    public readonly string Name;

    public Grouper(string name, Func<T, string[]> getKeys)
    {
      GetKeys = (item, ignoreParenthesis) =>
      {
        var keys = getKeys(item);

        if (ignoreParenthesis)
        {
          keys = keys
            .Select(k => Regex.Replace(k, @" ?\(.*?\)", string.Empty).Trim())
            .ToArray();
        }

        return keys;
      };

      Name = name;
    }

    public List<Group<T>> Group(List<T> items, bool ignoreParenthesis)
    {
      var dict = new Dictionary<string, List<T>>();

      foreach (var item in items)
      {
        foreach (var key in GetKeys(item, ignoreParenthesis))
        {
          if (!dict.ContainsKey(key))
          {
            dict[key] = new List<T>();
          }

          dict[key].Add(item);
        }
      }

      var ret = dict
        .Select(pair => new Group<T>(pair.Key, pair.Value))
        .ToList();

      ret.Add(new Group<T>("Total", items.ToList()));

      return ret;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
