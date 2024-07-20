namespace Illusion.Data;

internal class Scope
{
  public static Dictionary<string, HashSet<string>> NonProjects = new()
  {
    {
      "Admin", new HashSet<string>
       {
         "1-on-1",
         "Business",
         "Cascade",
         "Celebration",
         "Marketing",
         "Metrics",
         "Performance",
         "Recruitment",
         "Training",
       }
    },
    {
      "Enrichment", new HashSet<string>
      {
        "Social",
        "Talks",
        "Talk Prep",
        "Workshops",
      }
    },
    {
      "Misc", new HashSet<string>
      {
        "Chitchat",
        "Food",
        "Other",
        "PTO"
      }
    },
  };

  public static Dictionary<string, HashSet<string>> NonFeatures = new()
  {
    {
      "Op", new HashSet<string>
      {
        "Bug",
        "Config",
        "Deployment",
        "Meeting",
        "Planning",
        "Process",
        "Support",
        "Testing",
      }
    },
  };

  public static HashSet<string> DevActivities = new()
  {
    "Code",
    "Code (Fixes)",
    "Code (Review)",
    "Design",
    "Design (Fixes)",
    "Design (Review)",
    "Discussion",
    "Other",
    "Reqs",
    "Research",
    "Study",
    "Test (Ad-hoc)",
    "Test (Automated)",
    "Test (Manual)",
    "Test (Execution)",
    "VOC",
  };

  public string Company { get; }
  public string Project { get; }
  public string Feature { get; }
  public string Activity { get; }
  public string[] People { get; }

  public Scope(string company, string project, string feature, string activity, string[] people)
  {
    static string Braketize(string s) => $@"<{s}>";
    static string MarkError(string s) => $@"!{s}!";

    const string unspecified = "<Unspecified>";
    var _company = company == string.Empty ? unspecified : company;
    var _project = project == string.Empty ? unspecified : project;
    var _feature = feature == string.Empty ? unspecified : feature;
    var _activity = activity == string.Empty ? unspecified : activity;
    var _people = people;

    if (NonProjects.ContainsKey(_project))
    {
      _feature = NonProjects[_project].Contains(_feature) ?
        Braketize(_feature) :
        MarkError(_feature);

      _project = Braketize(_project);
    }
    else if (NonFeatures.ContainsKey(_feature))
    {
      _activity = NonFeatures[_feature].Contains(_activity) ?
        Braketize(_activity) :
        MarkError(_activity);

      _feature = Braketize(_feature);
    }
    else if (!DevActivities.Contains(_activity))
    {
      _activity = _activity == unspecified ? unspecified :
        MarkError(_activity);
    }

    Company = _company;
    Project = _project;
    Feature = _feature;
    Activity = _activity;
    People = _people;
  }

  public override string ToString()
  {
    return $@"{Company}\{Project}\{Feature}\{Activity}";
  }
}
