using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
      new Stat("Start",                           g => g.Blocks.Min(b => b.Start).Date                                        ),
      new Stat("Stop",                            g => g.Blocks.Max(b => b.Stop).Date                                         ),
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

      var lastPath = Settings.Default.LastPath;
      if (File.Exists(lastPath))
      {
        LoadBlocks(lastPath);
        DisplayBlocks();
      }

      var allLists = new List<IllusionCheckedListBox> { iclb_Companies, iclb_Projects, iclb_Features, iclb_Activities, iclb_People };
      for (int i = 0; i < allLists.Count; i++)
      {
        allLists[i].Forward = allLists.Skip(i + 1).ToList();
      }
    }

    void LoadBlocks(string path)
    {
      var timeSheet = Loader.GetXLSX(path, "Time Sheet");
      var inspectionSheet = Loader.GetXLSX(path, "Inspections");
      var incomeSheet = Loader.GetXLSX(path, "Income");
      if (timeSheet == null || inspectionSheet == null || incomeSheet == null)
        return;

      AllBlocks.Clear();
      foreach (DataRow row in timeSheet.Rows)
      {
        var dateStr = row["Date"].ToString().Trim();
        var startStr = row["Start"].ToString().Trim();
        var stopStr = row["Stop"].ToString().Trim();
        var start = DateTime.ParseExact(dateStr + " " + startStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);
        var stop = DateTime.ParseExact(dateStr + " " + stopStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);
        if (stop < start)
          stop = stop.AddDays(1);

        AllBlocks.Add(new Block
        {
          Start = start,
          Stop = stop,
          Company = row["Company"].ToString().Trim(),
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Activity = row["Activity"].ToString().Trim(),
          People = row["People"].ToString().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Concat(new[] { "Self" }).ToList()
        });
      }
      AllBlocks.Sort((a, b) => a.Start.CompareTo(b.Start));

      AllInspections.Clear();
      foreach (DataRow row in inspectionSheet.Rows)
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
          Company = row["Company"].ToString().Trim(),
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Findings = issues,
          DevHours = devHours
        });
      }

      AllIncomes.Clear();
      foreach (DataRow row in incomeSheet.Rows)
      {
        var amountStr = row["Amount"].ToString().Trim();
        var amount = int.Parse(amountStr);

        var checkDateStr = row["Check Date"].ToString();
        var startStr = row["Start"].ToString();
        var stopStr = row["Stop"].ToString();

        AllIncomes.Add(new Income
        {
          Amount = amount,
          Bonus = !string.IsNullOrWhiteSpace(row["Bonus"].ToString()),
          CheckDate = string.IsNullOrWhiteSpace(checkDateStr) ? null : (DateTime?)DateTime.ParseExact(checkDateStr, "M/d/yy", CultureInfo.InvariantCulture),
          Company = row["Company"].ToString().Trim(),
          Start = DateTime.ParseExact(startStr, "M/d/yy", CultureInfo.InvariantCulture),
          Stop = DateTime.ParseExact(stopStr, "M/d/yy", CultureInfo.InvariantCulture),
        });
      }

      IgnoreSetup = true;
      dtp_Start.Value = AllBlocks.Min(b => b.Start).Date;
      dtp_Stop.Value = AllBlocks.Max(b => b.Stop).Date;
      IgnoreSetup = false;
    }

    static void FindOverlaps(List<Block> blocks)
    {
      var allOverlaps = new List<Block>();
      foreach (var block in blocks.Reverse<Block>())
      {
        var overlaps = blocks.Where(other => block != other && block.Start < other.Stop && other.Start < block.Stop).ToList();
        if (overlaps.Any())
        {
          allOverlaps.AddRange(overlaps);
        }
      }

      foreach (var date in allOverlaps.Select(o => o.Start.Date).Distinct())
      {
        Console.WriteLine(date);
      }
    }

    static void FindMissingDays(int year, List<Block> blocks)
    {
      var start = new DateTime(year, 1, 1);
      var stop = new DateTime(year + 1, 1, 1);
      for (var date = start; date < stop && date < DateTime.Now.Date; date = date.AddDays(1))
      {
        if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
        {
          continue;
        }
        if (blocks.Any(b => b.Start.Date == date))
        {
          continue;
        }
        Console.WriteLine(date.ToString("M/d/yy"));
      }
    }

    void DisplayBlocks()
    {
      if (IgnoreSetup)
      {
        return;
      }

      var blocks = AllBlocks.Where(b => b.Start.Date >= dtp_Start.Value && b.Stop.Date <= dtp_Stop.Value).ToList();
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new [] { b.Company  }, iclb_Companies);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new [] { b.Project  }, iclb_Projects);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new [] { b.Feature  }, iclb_Features);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => new [] { b.Activity }, iclb_Activities);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.People,              iclb_People);

      var inspections = AllInspections.Where(i => i.Date >= dtp_Start.Value.Date && i.Date <= dtp_Stop.Value).ToList();
      inspections = Filter(inspections, i => new[] { i.Company }, iclb_Companies);
      inspections = Filter(inspections, i => new[] { i.Project }, iclb_Projects);
      inspections = Filter(inspections, i => new[] { i.Feature }, iclb_Features);

      var incomes = AllIncomes.Select(i => Chop(i, dtp_Start.Value, dtp_Stop.Value)).Where(i => i != null).ToList();
      incomes = Filter(incomes, i => new[] { i.Company }, iclb_Companies);

      if (!blocks.Any())
      {
        dgv_Stats.DataSource = null;
        dgv_Overview.DataSource = null;
        pb_Visualization.Image = null;
        return;
      }

      // Display stats and overview.
      var groups = ((Grouper)cb_Grouping.SelectedItem).GetGroups(blocks, inspections, incomes, dtp_Start.Value, dtp_Stop.Value);
      DisplayStats("Stats", dgv_Stats, groups, Stats);
      DisplayStats("Overview", dgv_Overview, groups, Overviews);

      // Generate Visualization
      var bmp = new Bitmap(24 * 4, (int)Math.Ceiling((dtp_Stop.Value - dtp_Start.Value).TotalDays));

      using (var g = Graphics.FromImage(bmp))
      {
        g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

        foreach (var block in blocks)
        {
          int yStart = (block.Start - dtp_Start.Value).Days;
          int yStop = (block.Stop - dtp_Start.Value).Days;

          for (int y = yStart; y <= yStop; y++)
          {
            int xStart = y == yStart ? (block.Start.Hour * 4) + (block.Start.Minute / 15) : 0;
            int xStop = y == yStop ? (block.Stop.Hour * 4) + (block.Stop.Minute / 15) : bmp.Width - 1;

            var brush = iclb_Companies.GetHighlight(new[] { block.Company }) ??
                        iclb_Projects.GetHighlight(new[] { block.Project }) ??
                        iclb_Features.GetHighlight(new[] { block.Feature }) ??
                        iclb_Activities.GetHighlight(new[] { block.Activity }) ??
                        iclb_People.GetHighlight(block.People) ??
                        (Brushes.White);

            g.FillRectangle(brush, new Rectangle(xStart, y, (xStop - xStart) + 1, 1));
          }
        }
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
        Bonus = income.Bonus,
        CheckDate = income.CheckDate,
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
      ret.Amount = (int)Math.Round(((double)ret.Amount) * ((income.Hours - ret.Hours)/income.Hours));

      return ret;
    }

    static void DisplayStats(string name, DataGridView dgv, List<Group> groups, List<Stat> stats)
    {
      var dt = new DataTable("Stats");
      dt.Columns.Add("Group", typeof(string));

      foreach (var stat in stats)
      {
        dt.Columns.Add(stat.Name, stat.Type);
      }

      foreach (var group in groups)
      {
        var row = dt.NewRow();
        row["Group"] = group.Name;
        foreach (var stat in stats)
        {
          row[stat.Name] = stat.Compute(group);
        }
        dt.Rows.Add(row);
      }

      dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv.DataSource = dt;

      foreach (var stat in stats)
      {
        dgv.Columns[stat.Name].DefaultCellStyle.Format = stat.Format;
      }

      dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      dgv.AutoResizeColumns();
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
      LoadBlocks(ofd.FileName);
      DisplayBlocks();

      Settings.Default.LastPath = ofd.FileName;
      Settings.Default.Save();
    }

    void cb_Grouping_SelectedIndexChanged(object sender, EventArgs e) { DisplayBlocks(); }

    void cb_IgnoreParenthesis_CheckedChanged(object sender, EventArgs e)
    {
      foreach (var block in AllBlocks)
      {

      }
      DisplayBlocks();
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

    public List<Group> GetGroups(IEnumerable<Block> blocks, IEnumerable<Inspection> inspections, IEnumerable<Income> incomes, DateTime filterStart, DateTime filterStop)
    {
      var dict = new Dictionary<string, Group>();

      Action<string> keyCheck = k =>
      {
        if (!dict.ContainsKey(k))
        {
          dict[k] = new Group(k, filterStart, filterStop);
        }
      };

      foreach (var block in blocks)
      {
        foreach (var key in GetKeys(block))
        {
          if (key == null)
          {
            continue;
          }

          keyCheck(key);
          dict[key].Blocks.Add(block);
        }
      }

      foreach (var inspection in inspections)
      {
        foreach (var key in GetKeys(inspection))
        {
          if (key == null)
          {
            continue;
          }

          keyCheck(key);
          dict[key].Inspections.Add(inspection);
        }
      }

      foreach (var income in incomes)
      {
        foreach (var key in GetKeys(income))
        {
          if (key == null)
          {
            continue;
          }

          keyCheck(key);
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
}
