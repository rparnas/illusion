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
  }
}
