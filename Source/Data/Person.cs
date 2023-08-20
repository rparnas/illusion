namespace Illusion.Data
{
  internal class Person
  {
    public readonly List<string> Companies;
    public readonly string FirstName;
    public readonly string Initials;
    public readonly string LastName;

    public Person(string initials, string firstName, string lastName, List<string> companies)
    {
      Companies = companies;
      FirstName = firstName;
      Initials = initials;
      LastName = lastName;
    }

    public override string ToString() => $@"{Initials} ({FirstName} {LastName})";
  }
}
