namespace Illusion.Data
{
  internal class Income
  {
    public readonly double Amount;
    public readonly string Company;
    public readonly DateTime Start;
    public readonly DateTime Stop;

    public Income(double amount, string company, DateTime start, DateTime stop)
    {
      Amount = amount;
      Company = company;
      Start = start;
      Stop = stop;
    }
  }
}
