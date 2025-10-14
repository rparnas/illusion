namespace Illusion.Data;

internal record IllusionSet(
  List<Block> Blocks,
  List<Income> Income,
  List<Person> People)
{      
  public static IllusionSet Merge(List<IllusionSet> sets)
  {
    if (sets.Count == 1)
    {
      return sets.First();
    }

    return new IllusionSet(
      sets.SelectMany(x => x.Blocks).OrderBy(x => x.Time).ToList(),
      sets.SelectMany(x => x.Income).ToList(),
      sets.SelectMany(x => x.People).ToList());
  }
}
