namespace Illusion.Data;

internal class Person
{
  public readonly string Company;
  public readonly string FirstName;
  public readonly string Initials;
  public readonly string LastName;

  public Person(string company, string initials, string firstName, string lastName)
  {
    Company = company;
    FirstName = firstName;
    Initials = initials;
    LastName = lastName;
  }

  public override string ToString() => $@"{Initials} ({FirstName} {LastName})";
}
