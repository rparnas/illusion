namespace Illusion.Data
{
  internal class IllusionSet
  {
    public readonly List<Block> Blocks;
    public readonly List<Income> Income;
    public readonly List<Person> People;

    public IllusionSet(List<Block> blocks, List<Income> income, List<Person> people)
    {
      Blocks = blocks;
      Income = income;
      People = people;
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
        sets.SelectMany(x => x.People).ToList());
    }
  }
}
