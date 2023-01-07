using System;
using System.Collections.Generic;

public class Categorizable
{
  public string Company;
  public string Project;
  public string Feature;
  public string Activity;
  public List<string> People;
  public string Raw;
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
  public DateTime? Paid;

  public double Hours { get { return (Stop - Start).TotalHours; } }
}

