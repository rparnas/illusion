using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

[assembly: AssemblyTitle("Illusion")]
[assembly: AssemblyProduct("Illusion")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace Illusion
{
  public partial class MainForm : Form
  {
    public static Dictionary<string, HashSet<string>> NonProjects = new Dictionary<string, HashSet<string>>
    {
      { "Admin", new HashSet<string> { "1-on-1", "Business", "Cascade", "Celebration", "Management", "Marketing", "Metrics", "Performance", "Recruitment", "Training" } },
      { "Enrichment", new HashSet<string> { "Talks", "Talk Prep", "Socializing", "Workshops" } },
      { "Misc", new HashSet<string> { "Chitchat", "Food", "PTO" } }
    };

    public static HashSet<string> DevelopmentActivities = new HashSet<string>
    {
      "Code",
      "Code (Build)",
      "Code (Fixes)",
      "Code (Review)",
      "Design",
      "Design (Fixes)",
      "Design (Review)",
      "Discussion",
      "Investigation",
      "Reqs",
      "Research",
      "Study",
      "Test (Ad-hoc)",
      "Test (Automated)",
      "Test (Manual)",
      "Test (Manual Execution)",
      "VOC",
    };

    public static HashSet<string> OtherNDA = new HashSet<string>
    {
      "Meeting",
    };

    public static HashSet<string> TechnicalNDA = new HashSet<string>
    {
      "Config",
      "Deployment",
      "Support",
    };

    public static List<Highlight> Highlights = new List<Highlight>
    {
      new Highlight("Red",    196, 2, 51),
      new Highlight("Orange", 255, 120, 19),
      new Highlight("Yellow", 255, 211, 0),
      new Highlight("Green",  0, 163, 104),
      new Highlight("Blue",   0, 136, 191),
      new Highlight("Violet", 131, 63, 135)
    };

    static List<Stat> Stats = new List<Stat>
    {
      new Stat("Start",                           g => g.Blocks.Min(b => b.Date)                                              ),
      new Stat("Stop",                            g => g.Blocks.Max(b => b.Date)                                              ),
      new Stat("Hours",                           g => g.Blocks.Sum(b => b.Hours)                                             ),
      new Stat("Dev Hours",                       g => g.Blocks.Sum(b => b.DevHours)                                          ),
      new Stat("Dev Hour Ratio",                  g => g.Blocks.Sum(b => b.DevHours) / g.Blocks.Sum(b => b.Hours)             ),
      new Stat("Inspections",                     g => g.Inspections.Count()                                                  ),
      new Stat("Inspection Findings",             g => g.Inspections.Sum(i => i.Findings)                                     ),
      new Stat("Inspection Dev Hours",            g => g.Inspections.Sum(i => i.DevHours)                                     ),
      new Stat("Inspection Findings / Dev Hours", g => g.Inspections.Sum(i => i.Findings) / g.Inspections.Sum(i => i.DevHours))
    };

    List<Stat> Overviews = new List<Stat>
    {
      new Stat("Work Hours",                      g => g.Blocks.Where(b => IsWork(b)).Sum(b => b.Hours)                               ),
      new Stat("Food Hours",                      g => g.Blocks.Where(b => IsFood(b)).Sum(b => b.Hours)                               ),
      new Stat("PTO Hours",                       g => g.Blocks.Where(b => IsPTO(b)).Sum(b => b.Hours)                                ),
      new Stat("Total Hours",                     g => g.Blocks.Sum(b => b.Hours)                                                     ),
      new Stat("Work Hours / Week",               g => g.Blocks.Where(b => IsWork(b)).Sum(b => b.Hours) / g.FilterWeeks               ),
      new Stat("Food Hours / Week",               g => g.Blocks.Where(b => IsFood(b)).Sum(b => b.Hours) / g.FilterWeeks               ),
      new Stat("PTO Hours / Week",                g => g.Blocks.Where(b => IsPTO(b)).Sum(b => b.Hours) / g.FilterWeeks                ),
      new Stat("Total Hours / Week",              g => g.Blocks.Sum(b => b.Hours) / g.FilterWeeks                                     ),
      new Stat("Income",                          g => g.Incomes.Sum(i => i.Amount)                                                   ),
      new Stat("Income / Work Hours",             g => g.Incomes.Sum(i => i.Amount) / g.Blocks.Where(b => IsWork(b)).Sum(b => b.Hours)),
      new Stat("Income / Total Hours",            g => g.Incomes.Sum(i => i.Amount) / g.Blocks.Sum(b => b.Hours)                      ),
      new Stat("Weeks",                           g => g.FilterWeeks                                                                  ),
    };

    static List<Grouper> Groupers = new List<Grouper>
    {
      new Grouper("Company",  c => c.Company),
      new Grouper("Project",  c => c.Project),
      new Grouper("Feature",  c => c.Feature),
      new Grouper("Activity", c => c.Activity),
      new Grouper("People",   c => c.People)
    };

    static List<TimeFilter> TimeFilters = new List<TimeFilter>
    {
      new TimeFilter("This Week", () => DateTime.Now.RoundWeekDown(DayOfWeek.Sunday),
                                  () => DateTime.Now.RoundWeekDown(DayOfWeek.Sunday).AddDays(6)),
      new TimeFilter("Last Week", () => DateTime.Now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-7),
                                  () => DateTime.Now.RoundWeekDown(DayOfWeek.Sunday).AddDays(-1)),
      new TimeFilter("This Month", () => DateTime.Now.RoundMonthDown(), 
                                   () => DateTime.Now.RoundMonthDown().AddMonths(1).AddDays(-1)),
      new TimeFilter("Last Month", () => DateTime.Now.RoundMonthDown().AddMonths(-1), 
                                   () => DateTime.Now.RoundMonthDown().AddDays(-1))
    };

    static bool IsWork(Block b)
    {
      return !IsFood(b) && !IsPTO(b);
    }

    static bool IsFood(Block b)
    {
      return b.Project == "_Misc" && b.Feature == "Food";
    }

    static bool IsPTO(Block b)
    {
      return b.Project == "_Misc" && b.Feature == "PTO";
    }

    static int VisualizationScaleFactor = 4;

    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    List<Block> AllBlocks;
    List<Inspection> AllInspections;
    List<Income> AllIncomes;
    bool IgnoreSetup;

    public MainForm()
    {
      AllBlocks = new List<Block>();
      AllInspections = new List<Inspection>();
      AllIncomes = new List<Income>();
      IgnoreSetup = true;

      InitializeComponent();
      cb_Grouping.DataSource = Groupers;
      cb_Grouping.SelectedItem = Groupers.First(g => g.Name == "Feature");

      cb_Time.DataSource = TimeFilters;
      cb_Time.SelectedItem = null;

      var lastPath = Settings.Default.LastPath;
      if (File.Exists(lastPath))
      {
        LoadBlocks(lastPath, null, null);
        DisplayBlocks();
      }

      var allLists = new List<IllusionCheckedListBox> { iclb_Companies, iclb_Projects, iclb_Features, iclb_Activities, iclb_People };
      for (int i = 0; i < allLists.Count; i++)
      {
        allLists[i].Forward = allLists.Skip(i + 1).ToList();
      }

      var table = new DataTable();
      table.Columns.Add("Start");
      table.Columns.Add("Stop");
      table.Columns.Add("Duration");
      table.Columns.Add("Raw");
      foreach (var block in AllBlocks)
      {
        if (block.Start == null)
        {
          continue;
        }
        if (block.Start.Value < new DateTime(2021, 12, 1))
        {
          continue;
        }
        if (block.Company != "?")
        {
          continue;
        }

        var row = table.NewRow();
        row["Start"] = block.Start.Value.ToString("MM/dd/YY");
        row["Stop"] = block.Stop.Value.ToString("MM/dd/YY");
        row["Duration"] = block.Hours.ToString();
        row["Raw"] = block.Raw;

        table.Rows.Add(row);
      }
    }

    void LoadBlocks(string path, DateTime? dtpStart, DateTime? dtpStop)
    {
      var file = new FileInfo(path);
      var company = file.Name
        .Replace(".xlsx", "");

      var ledger = Loader.GetXLSX(path, "Ledger");
      var inspections = Loader.GetXLSX(path, "Inspections");
      var income = Loader.GetXLSX(path, "Income");
      if (ledger == null || inspections == null || income == null)
        return;

      AllBlocks.Clear();
      foreach (var row in ledger.Rows.Cast<DataRow>())
      {
        var dateStr = GetRawString(row, "Date");
        var startStr = GetRawString(row, "Start");
        var stopStr = GetRawString(row, "Stop");
        var hourStr = GetRawString(row, "Hours");

        // Skip any ongoing entries
        if (!string.IsNullOrEmpty(dateStr) && !string.IsNullOrEmpty(startStr) && string.IsNullOrEmpty(stopStr))
        {
          continue;
        }

        var date = DateTime.ParseExact(dateStr, "M/d/yy", CultureInfo.InvariantCulture);
        var start = string.IsNullOrWhiteSpace(startStr) ? null : (DateTime?)DateTime.ParseExact(dateStr + " " + startStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);
        var stop = string.IsNullOrWhiteSpace(stopStr) ? null : (DateTime?)DateTime.ParseExact(dateStr + " " + stopStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);
        if (start.HasValue && stop.HasValue && stop < start)
        {
          stop = stop.Value.AddDays(1);
        }

        var people = new[] { "<Self>" }
          .Concat(row["People"].ToString().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()))
          .Select(initials => initials == "Unspecified" ? "<Unspecified>" : initials)
          .ToList();

        if (!start.HasValue && !stop.HasValue && string.IsNullOrWhiteSpace(hourStr))
        {
          continue;
        }

        var hours = start.HasValue && stop.HasValue ? (stop.Value - start.Value).TotalHours : double.Parse(hourStr);
        var devHours = hours * (people.Count - 1);

        var block = new Block
        {
          Date = date,
          Start = start,
          Stop = stop,
          Hours = hours,
          DevHours = devHours,
          Company = company,
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Activity = row["Activity"].ToString().Trim(),
          People = people,
          Raw = row["Raw"].ToString().Trim()
        };

        FormatCategory(block);

        AllBlocks.Add(block);
      }

      AllBlocks.Sort((a, b) =>
      {
        if (a.Start.HasValue && b.Start.HasValue)
        {
          return a.Start.Value.CompareTo(b.Start.Value);
        }
        return a.Date.CompareTo(b.Date);
      });

      AllInspections.Clear();
      foreach (DataRow row in inspections.Rows)
      {
        var dateStr = row["Date"].ToString().Trim();
        var date = DateTime.ParseExact(dateStr, "M/d/yy", CultureInfo.InvariantCulture);

        var issuesStr = row["Findings"].ToString().Trim();
        var issues = int.Parse(issuesStr);

        var devHoursStr = row["Total"].ToString().Trim();
        var devHours = double.Parse(devHoursStr);

        AllInspections.Add(new Inspection
        {
          Date = date,
          Company = company,
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Findings = issues,
          DevHours = devHours
        });
      }

      AllIncomes.Clear();
      foreach (DataRow row in income.Rows)
      {
        var amountStr = row["Amount"].ToString().Trim();
        var amount = double.TryParse(amountStr, out double doubleAmount) ? (int)Math.Round(doubleAmount) : 0;
        var paidStr = row["Paid"].ToString();
        var startStr = row["Start"].ToString();
        var stopStr = row["Stop"].ToString();

        AllIncomes.Add(new Income
        {
          Amount = amount,
          Paid = string.IsNullOrWhiteSpace(paidStr) || paidStr == "?" ? null : (DateTime?)DateTime.ParseExact(paidStr, "M/d/yy", CultureInfo.InvariantCulture),
          Company = company,
          Start = DateTime.ParseExact(startStr, "M/d/yy", CultureInfo.InvariantCulture),
          Stop = DateTime.ParseExact(stopStr, "M/d/yy", CultureInfo.InvariantCulture),
        });
      }

      IgnoreSetup = true;
      dtp_Start.Value =  dtpStart ?? AllBlocks.Min(b => b.Date);
      dtp_Stop.Value = dtpStop ?? AllBlocks.Max(b => b.Date);
      IgnoreSetup = false;

      // FindOverlong(AllBlocks);
      // FindOverlaps(AllBlocks);
    }

    static void FindOverlong(List<Block> blocks)
    {
      Console.WriteLine();

      var overlong = blocks.Where(b => b.Hours > 8 && b.Start.HasValue && b.Stop.HasValue && !b.Raw.Contains("[OK]")).ToList();
      if (overlong.Any())
      {
        Console.WriteLine("Overlong:");
        foreach (var block in overlong)
        {
            Console.WriteLine($@"  {block.Start.Value} | {block.Raw}");
        }
      }
      else
      {
        Console.WriteLine("No Overlong.");
      }
      Console.WriteLine();
    }

    static void FindOverlaps(List<Block> blocks)
    {
      var allOverlaps = new List<Block>();
      foreach (var block in blocks.Reverse<Block>())
      {
        var overlaps = blocks.Where(other => block.Start.HasValue && other.Start.HasValue && block != other && block.Start < other.Stop && other.Start < block.Stop).ToList();
        if (overlaps.Any())
        {
          allOverlaps.AddRange(overlaps);
        }
      }

      Console.WriteLine();
      if (allOverlaps.Any())
      {
        Console.WriteLine("Overlaps:");
        foreach (var date in allOverlaps.Select(o => o.Date).Distinct())
        {
          Console.WriteLine($@"  {date:M/d/yy}");
        }
      }
      else
      {
        Console.WriteLine("No Overlaps.");
      }
      Console.WriteLine();
    }

    void DisplayBlocks()
    {
      if (IgnoreSetup)
      {
        return;
      }

      var blocks = AllBlocks.Where(b => b.Date >= dtp_Start.Value && b.Date <= dtp_Stop.Value).ToList();
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new[] { b.Company }, iclb_Companies);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new[] { b.Project }, iclb_Projects);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new[] { b.Feature }, iclb_Features);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new[] { b.Activity }, iclb_Activities);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.People, iclb_People);

      var inspections = AllInspections.Where(i => i.Date >= dtp_Start.Value.Date && i.Date <= dtp_Stop.Value).ToList();
      inspections = Filter(inspections, i => new[] { i.Company }, iclb_Companies);
      inspections = Filter(inspections, i => new[] { i.Project }, iclb_Projects);
      inspections = Filter(inspections, i => new[] { i.Feature }, iclb_Features);

      var incomes = AllIncomes.Select(i => Chop(i, dtp_Start.Value, dtp_Stop.Value)).Where(i => i != null).ToList();
      incomes = Filter(incomes, i => new[] { i.Company }, iclb_Companies);

      if (!blocks.Any())
      {
        dgv_Stats.Columns.Clear();
        dgv_Overview.Columns.Clear();
        pb_Visualization.Image = null;
        return;
      }

      // Display stats and overview.
      var groups = ((Grouper)cb_Grouping.SelectedItem).GetGroups(blocks, inspections, incomes, dtp_Start.Value, dtp_Stop.Value, cb_CollapseParenthesis.Checked);
      DisplayStats("Stats", dgv_Stats, groups, Stats);
      DisplayStats("Overview", dgv_Overview, groups, Overviews);

      // Generate Visualization
      var bmp = new Bitmap(24 * 4, Math.Max(1, (int)Math.Ceiling((dtp_Stop.Value - dtp_Start.Value).TotalDays)));

      using (var g = Graphics.FromImage(bmp))
      {
        g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

        foreach (var block in blocks)
        {
          if (!block.Start.HasValue || !block.Stop.HasValue)
          {
            continue;
          }
          var start = block.Start.Value;
          var stop = block.Stop.Value;

          int yStart = (start - dtp_Start.Value).Days;
          int yStop = (stop - dtp_Start.Value).Days;

          for (int y = yStart; y <= yStop; y++)
          {
            int xStart = y == yStart ? (start.Hour * 4) + (start.Minute / 15) : 0;
            int xStop = y == yStop ? (stop.Hour * 4) + (stop.Minute / 15) : bmp.Width - 1;

            var brush = iclb_Companies.GetHighlight(new[] { block.Company }) ??
                        iclb_Projects.GetHighlight(new[] { block.Project }) ??
                        iclb_Features.GetHighlight(new[] { block.Feature }) ??
                        iclb_Activities.GetHighlight(new[] { block.Activity }) ??
                        iclb_People.GetHighlight(block.People) ??
                        (Brushes.White);

            g.FillRectangle(brush, new Rectangle(xStart, y, (xStop - xStart) + 1, 1));
          }
        }
        
        // TODO: Save Button
        // bmp.Save("C:\\Users\\Ry\\Desktop\\bob.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
      }

      // Display Visualization
      pnl_Visualization.VerticalScroll.Visible = true;
      pnl_Visualization.VerticalScroll.Value = 0;
      var zoomed = new Bitmap(bmp, new Size(bmp.Width * VisualizationScaleFactor, bmp.Height * VisualizationScaleFactor));
      pb_Visualization.Size = zoomed.Size;
      pb_Visualization.Location = new Point((tpVisualization.Width - pb_Visualization.Width) / 2, 3);
      pb_Visualization.Image = zoomed;
    }

    static Income Chop(Income income, DateTime start, DateTime stop)
    {
      if (income.Start >= start && income.Stop <= stop)
      {
        return income;
      }
      if ((income.Start <= start && income.Stop <= start) || (income.Start >= stop && income.Stop >= stop))
      {
        return null;
      }

      var ret = new Income
      {
        Amount = income.Amount,
        Paid = income.Paid,
        Company = income.Company,
        Start = income.Start,
        Stop = income.Stop
      };

      if (income.Start < start)
      {
        ret.Start = start;
      }
      if (income.Stop > stop)
      {
        ret.Stop = stop;
      }
      ret.Amount = (int)Math.Round(((double)ret.Amount) * ((income.Hours - ret.Hours) / income.Hours));

      return ret;
    }

    static void DisplayStats(string name, DataGridView dgv, List<Group> groups, List<Stat> stats)
    {
      dgv.Columns.Clear();
      dgv.Rows.Clear();

      dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Group", ValueType = typeof(string) });
      foreach (var stat in stats)
      {
        var col = new DataGridViewTextBoxColumn
        {
          Name = stat.Name,
          SortMode = DataGridViewColumnSortMode.Automatic,
          ValueType = stat.Type
        };
        col.DefaultCellStyle.Format = stat.Format;
        dgv.Columns.Add(col);
      }

      foreach (var group in groups)
      {
        var row = dgv.Rows[dgv.Rows.Add()];
        row.Cells["Group"].Value = group.Name;
        foreach (var stat in stats)
        {
          row.Cells[stat.Name].Value = stat.Compute(group);
        }

        if (group.Name == "Total")
        {
          row.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);
        }
      }

      dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      dgv.AutoResizeColumns();
    }

    static string GetRawString(DataRow row, string columnName)
    {
      if (!row.Table.Columns.Contains(columnName))
      {
        return null;
      }

      return row[columnName]
        .ToString()
        .Trim();
    }

    static void FormatCategory(Categorizable cat)
    {
      string Braketize(string _s) => $@"<{_s}>";
      string MarkError(string _s) => $@"!{_s}!";

      if (cat.Company == "")
      {
        cat.Company = "<Unspecified>";
      }
      if (cat.Project == "")
      {
        cat.Project = "<Unpsecified>";
      }
      if (cat.Feature == "")
      {
        cat.Feature = "<Unspecified>";
      }
      if (cat.Activity == "")
      {
        cat.Activity = "<Unspecified>";
      }

      if (NonProjects.ContainsKey(cat.Project))
      {
        if (!NonProjects[cat.Project].Contains(cat.Feature))
        {
          cat.Feature = MarkError(cat.Feature);
        }
        cat.Project = Braketize(cat.Project);
      }
      else if (DevelopmentActivities.Contains(cat.Activity)) { }
      else if (OtherNDA.Contains(cat.Activity) || TechnicalNDA.Contains(cat.Activity))
      {
        cat.Activity = Braketize(cat.Activity);
      }
      else
      {
        cat.Activity = MarkError(cat.Activity);
      }
    }

    List<Block> UpdateListBoxAndFilterBlocks(List<Block> blocks, Func<Block, IEnumerable<string>> getCategories, IllusionCheckedListBox iclb)
    {
      var items = new HashSet<string>();
      var ret = new List<Block>();

      foreach (var block in blocks)
      {
        var categories = getCategories(block);
        var contains = false;
        foreach (var category in categories)
        {
          contains = contains || iclb.AllCheckedItems.Contains(category);
          items.Add(category);
        }
        if (contains)
        {
          ret.Add(block);
        }
      }

      var sortedItems = new List<string>(items);
      sortedItems.Sort();
      iclb.SetItems(sortedItems);

      return ret;
    }

    List<T> Filter<T>(List<T> items, Func<T, IEnumerable<string>> getCategories, IllusionCheckedListBox iclb)
    {
      var ret = new List<T>();

      foreach (var item in items)
      {
        var categories = getCategories(item);
        var contains = false;
        foreach (var category in categories)
        {
          contains = contains || iclb.AllCheckedItems.Contains(category);
        }
        if (contains)
        {
          ret.Add(item);
        }
      }

      return ret;
    }

    void btn_Load_Click(object sender, EventArgs e)
    {
      var ofd = new OpenFileDialog();
      ofd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
      if (ofd.ShowDialog() != DialogResult.OK)
      {
        return;
      }
      LoadBlocks(ofd.FileName, null, null);
      DisplayBlocks();

      Settings.Default.LastPath = ofd.FileName;
      Settings.Default.Save();
    }

    void btn_Reload_Click(object sender, EventArgs e)
    {
      var lastPath = Settings.Default.LastPath;
      if (!File.Exists(lastPath))
      {
        return;
      }

      LoadBlocks(lastPath, dtp_Start.Value, dtp_Stop.Value);
      DisplayBlocks();
    }

    void cb_Grouping_SelectedIndexChanged(object sender, EventArgs e) { DisplayBlocks(); }

    void cb_IgnoreParenthesis_CheckedChanged(object sender, EventArgs e) { DisplayBlocks(); }

    void cb_Time_SelectionChangeCommitted(object sender, EventArgs e)
    {
      var timeFilter = (TimeFilter)cb_Time.SelectedItem;

      cb_Time.SelectedItem = null;
      dtp_Start.Value = timeFilter.GetStart();
      dtp_Stop.Value = timeFilter.GetStop();
    }

    void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
    {
      var dgv = (DataGridView)sender;
      if (e.RowIndex1 == dgv.Rows.Count - 1)
      {
        e.Handled = true;
      }
      if (e.RowIndex2 == dgv.Rows.Count - 1)
      {
        e.Handled = true;
      }
    }

    void dtp_Start_ValueChanged(object sender, EventArgs e) { DisplayBlocks(); }

    void dtp_Stop_ValueChanged(object sender, EventArgs e) { DisplayBlocks(); }

    void pb_Visualization_MouseLeave(object sender, EventArgs e)
    {
      lbl_Visualization.Text = "";
    }

    void pb_Visualization_MouseMove(object sender, MouseEventArgs e)
    {
      if (pb_Visualization.Image == null)
      {
        lbl_Visualization.Text = "";
        return;
      }

      var pos = pb_Visualization.PointToClient(Cursor.Position);
      var x = pos.X / VisualizationScaleFactor;
      var y = pos.Y / VisualizationScaleFactor;
      var time = dtp_Start.Value.AddMinutes(x * 15).AddDays(y);

      lbl_Visualization.Text = time.ToString("M/d/yy h:mmtt").Replace("AM", "a").Replace("PM", "p");
    }
  }

  public class Highlight
  {
    public readonly Brush Brush;
    public readonly Color Color;
    public readonly string Name;

    public Highlight(string name, int r, int g, int b)
    {
      var color = Color.FromArgb(r, g, b);

      Brush = new SolidBrush(color);
      Color = color;
      Name = name;
    }
  }

  public class Stat
  {
    public readonly string Name;
    readonly Func<Group, object> ComputeInner;
    public readonly string Format;
    public readonly Type Type;

    public Stat(string name, Func<Group, DateTime> compute)
    {
      Name = name;
      ComputeInner = group => compute(group);
      Format = "M/d/yy";
      Type = typeof(DateTime);
    }

    public Stat(string name, Func<Group, double> compute)
    {
      Name = name;
      ComputeInner = group => compute(group);
      Format = "F2";
      Type = typeof(double);
    }

    public Stat(string name, Func<Group, int> compute)
    {
      Name = name;
      ComputeInner = group => compute(group);
      Type = typeof(int);
    }

    public object Compute(Group group)
    {
      return ComputeInner(group);
    }
  }

  public class Group
  {
    public List<Block> Blocks;
    public List<Inspection> Inspections;
    public List<Income> Incomes;
    public string Name;
    public double FilterWeeks;

    public Group(string name, DateTime filterStart, DateTime filterStop)
    {
      Blocks = new List<Block>();
      Inspections = new List<Inspection>();
      Incomes = new List<Income>();
      Name = name;
      FilterWeeks = (filterStop - filterStart).TotalDays / 7.0;
    }
  }

  public class Grouper
  {
    public readonly string Name;
    Func<Categorizable, List<string>> GetKeys;

    public Grouper(string name, Func<Categorizable, string> getKey)
    {
      Name = name;
      GetKeys = c => new List<string> { getKey(c) };
    }

    public Grouper(string name, Func<Categorizable, List<string>> getKeys)
    {
      Name = name;
      GetKeys = getKeys;
    }

    public List<Group> GetGroups(IEnumerable<Block> blocks, IEnumerable<Inspection> inspections, IEnumerable<Income> incomes, DateTime filterStart, DateTime filterStop, bool ignoreParenthesis)
    {
      var dict = new Dictionary<string, Group>();

      Func<Categorizable, List<string>> getKeys = item =>
      {
        var keys = GetKeys(item).Where(k => !string.IsNullOrWhiteSpace(k)).ToList();
        if (ignoreParenthesis)
        {
          keys = keys.Select(k => Regex.Replace(k, @" ?\(.*?\)", string.Empty).Trim()).ToList();
        }

        foreach (var newKey in keys.Where(k => !dict.ContainsKey(k)))
        {
          dict[newKey] = new Group(newKey, filterStart, filterStop);
        }

        return keys;
      };

      foreach (var block in blocks)
      {
        foreach (var key in getKeys(block))
        {
          dict[key].Blocks.Add(block);
        }
      }

      foreach (var inspection in inspections)
      {
        foreach (var key in getKeys(inspection))
        {
          dict[key].Inspections.Add(inspection);
        }
      }

      foreach (var income in incomes)
      {
        foreach (var key in getKeys(income))
        {
          dict[key].Incomes.Add(income);
        }
      }

      dict.Add("Total", new Group("Total", filterStart, filterStop)
      {
        Blocks = blocks.ToList(),
        Inspections = inspections.ToList()
      });

      return dict.Values.Where(g => g.Blocks.Any()).ToList();
    }

    public override string ToString()
    {
      return Name;
    }
  }

  public class TimeFilter
  {
    public readonly string Name;
    public readonly Func<DateTime> GetStart;
    public readonly Func<DateTime> GetStop;

    public TimeFilter(string name, Func<DateTime> getStart, Func<DateTime> getStop)
    {
      GetStart = getStart;
      GetStop = getStop;
      Name = name;
    }

    public override string ToString() => Name;
  }

  public class DailyStat
  {
    public readonly DateTime Date;
    public readonly DateTime Wake;

    public DailyStat(DateTime date, DateTime wake)
    {
      Date = date;
      Wake = wake;
    }
  }
}
