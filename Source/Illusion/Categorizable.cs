using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Categorizable
{
  public string Company;
  public string Project;
  public string Feature;
  public string Activity;
  public List<string> People;
}

public class Block : Categorizable
{
  public DateTime Date;
  public DateTime? Start;
  public DateTime? Stop;

  public double Hours;
  public double DevHours;
}

public class Inspection : Categorizable
{
  public DateTime Date;
  public int Findings;
  public double DevHours;
}

public class Income : Categorizable
{
  public DateTime Start;
  public DateTime Stop;
  public int Amount;
  public DateTime? CheckDate;
  public bool Bonus;

  public double Hours { get { return (Stop - Start).TotalHours; } }
}

