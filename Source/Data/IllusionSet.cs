namespace Illusion.Data;

internal class IllusionSet
{
  public readonly List<Block> Blocks;
  public readonly List<Income> Income;
  public readonly List<Person> People;

  public readonly List<string> Errors;

  public IllusionSet(List<Block> blocks, List<Income> income, List<Person> people, List<string> errors)
  {
    Blocks = blocks;
    Income = income;
    People = people;
    Errors = errors;
  }
      
  public static IllusionSet Merge(List<IllusionSet> sets)
  {
    if (sets.Count == 1)
    {
      return sets.First();
    }

    return new IllusionSet(
      sets.SelectMany(x => x.Blocks).ToList(),
      sets.SelectMany(x => x.Income).ToList(),
      sets.SelectMany(x => x.People).ToList(),
      sets.SelectMany(x => x.Errors).ToList());
  }
}
