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
      new Stat("Start",                           (blks, insps) => blks.Min(b => b.Start).Date                            ),
      new Stat("Stop",                            (blks, insps) => blks.Max(b => b.Stop).Date                             ),
      new Stat("Hours",                           (blks, insps) => blks.Sum(b => b.Hours)                                 ),
      new Stat("Dev Hours",                       (blks, insps) => blks.Sum(b => b.DevHours)                              ),
      new Stat("Dev Hour Ratio",                  (blks, insps) => blks.Sum(b => b.DevHours) / blks.Sum(b => b.Hours)     ),
      new Stat("Inspections",                     (blks, insps) => insps.Count()                                          ),
      new Stat("Inspection Findings",             (blks, insps) => insps.Sum(i => i.Findings)                             ),
      new Stat("Inspection Dev Hours",            (blks, insps) => insps.Sum(i => i.DevHours)                             ),
      new Stat("Inspection Findings / Dev Hours", (blks, insps) => insps.Sum(i => i.Findings) / insps.Sum(i => i.DevHours))
    };

    static List<Grouper> Groupers = new List<Grouper>
    {
      new Grouper("Company",  b => b.Company , i => i.Company           ),
      new Grouper("Project",  b => b.Project , i => i.Project           ),
      new Grouper("Feature",  b => b.Feature , i => i.Feature           ),
      new Grouper("Activity", b => b.Activity, i => "Inspection"        ),
      new Grouper("People",   b => b.People  , i => new List<string> { })
    };

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
    bool IgnoreSetup;

    public MainForm()
    {
      AllBlocks = new List<Block>();
      AllInspections = new List<Inspection>();
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
    }

    void LoadBlocks(string path)
    {
      var timeSheet = Loader.GetXLSX(path, "Time Sheet");
      var inspectionSheet = Loader.GetXLSX(path, "Inspections");
      if (timeSheet == null || inspectionSheet == null)
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
          Company = row["Company"].ToString().Trim(),
          Start = start,
          Stop = stop,
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

      IgnoreSetup = true;
      dtp_Start.Value = AllBlocks.Min(b => b.Start).Date;
      dtp_Stop.Value = AllBlocks.Max(b => b.Stop).Date;
      IgnoreSetup = false;
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
      inspections = FilterInspections(inspections, i => new[] { i.Company }, iclb_Companies);
      inspections = FilterInspections(inspections, i => new[] { i.Project }, iclb_Projects);
      inspections = FilterInspections(inspections, i => new[] { i.Feature }, iclb_Features);

      if (!blocks.Any())
      {
        dgv_Stats.DataSource = null;
        pb_Visualization.Image = null;
        return;
      }

      // Display stats
      var dt = new DataTable("Stats");
      dt.Columns.Add("Group", typeof(string));
      foreach (var stat in Stats)
      {
        dt.Columns.Add(stat.Name, stat.Type);
      }
      var groups = ((Grouper)cb_Grouping.SelectedItem).GetGroups(blocks, inspections);
      foreach (var group in groups)
      {
        var row = dt.NewRow();
        row["Group"] = group.Name;
        foreach (var stat in Stats)
        {
          row[stat.Name] = stat.Compute(group.Blocks, group.Inspections);
        }
        dt.Rows.Add(row);
      }

      dgv_Stats.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dgv_Stats.DataSource = dt;

      foreach (var stat in Stats)
      {
        dgv_Stats.Columns[stat.Name].DefaultCellStyle.Format = stat.Format;
      }

      dgv_Stats.AutoResizeColumns();

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
      pnl_Visualization.VerticalScroll.Value = 0;
      var zoomed = new Bitmap(bmp, new Size(bmp.Width * VisualizationScaleFactor, bmp.Height * VisualizationScaleFactor));
      pb_Visualization.Size = zoomed.Size;
      pb_Visualization.Location = new Point((tpVisualization.Width - pb_Visualization.Width) / 2, 3);
      pb_Visualization.Image = zoomed;
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
          contains = contains || iclb.CheckedItems.Contains(category);
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

    List<Inspection> FilterInspections(List<Inspection> inspections, Func<Inspection, IEnumerable<string>> getCategories, IllusionCheckedListBox iclb)
    {
      var ret = new List<Inspection>();

      foreach (var inspection in inspections)
      {
        var categories = getCategories(inspection);
        var contains = false;
        foreach (var category in categories)
        {
          contains = contains || iclb.CheckedItems.Contains(category);
        }
        if (contains)
        {
          ret.Add(inspection);
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

  public class Block
  {
    public DateTime Start;
    public DateTime Stop;
    public string Company;
    public string Project;
    public string Feature;
    public string Activity;
    public List<string> People;

    public double Hours { get { return (Stop - Start).TotalHours; } }
    public double DevHours { get { return Hours * (People.Count - 1); } }
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

  public class Inspection
  {
    public DateTime Date;
    public string Company;
    public string Project;
    public string Feature;
    public Dictionary<string, double> People;

    public int Findings;
    public double DevHours;
  }

  public class Stat
  {
    public readonly string Name;
    readonly Func<IEnumerable<Block>, IEnumerable<Inspection>, object> ComputeInner;
    public readonly string Format;
    public readonly Type Type;

    public Stat(string name, Func<IEnumerable<Block>, IEnumerable<Inspection>, DateTime> compute)
    {
      Name = name;
      ComputeInner = (blocks, inspections) => compute(blocks, inspections);
      Format = "M/d/yy";
      Type = typeof(DateTime);
    }

    public Stat(string name, Func<IEnumerable<Block>, IEnumerable<Inspection>, double> compute)
    {
      Name = name;
      ComputeInner = (blocks, inspections) => compute(blocks, inspections);
      Format = "F2";
      Type = typeof(double);
    }

    public Stat(string name, Func<IEnumerable<Block>, IEnumerable<Inspection>, int> compute)
    {
      Name = name;
      ComputeInner = (blocks, inspections) => compute(blocks, inspections);
      Type = typeof(int);
    }

    public object Compute(IEnumerable<Block> blocks, IEnumerable<Inspection> inspections)
    {
      return ComputeInner(blocks, inspections);
    }
  }

  public class Group
  {
    public List<Block> Blocks;
    public List<Inspection> Inspections;
    public string Name;

    public Group(string name)
    {
      Blocks = new List<Block>();
      Inspections = new List<Inspection>();
      Name = name;
    }
  }

  public class Grouper
  {
    public readonly string Name;
    Func<Block, List<string>> GetBlockKeys;
    Func<Inspection, List<string>> GetInspectionKeys;

    public Grouper(string name, Func<Block, string> getBlockKey, Func<Inspection, string> getInspectionKey)
    {
      Name = name;
      GetBlockKeys = b => new List<string> { getBlockKey(b) };
      GetInspectionKeys = i => new List<string> { getInspectionKey(i) };
    }

    public Grouper(string name, Func<Block, List<string>> getBlockKeys, Func<Inspection, List<string>> getInspectionKeys)
    {
      Name = name;
      GetBlockKeys = getBlockKeys;
      GetInspectionKeys = getInspectionKeys;
    }

    public List<Group> GetGroups(IEnumerable<Block> blocks, IEnumerable<Inspection> inspections)
    {
      var dict = new Dictionary<string, Group>();
      dict.Add("Total", new Group("Total")
      {
        Blocks = blocks.ToList(),
        Inspections = inspections.ToList()
      });

      foreach (var block in blocks)
      {
        foreach (var key in GetBlockKeys(block))
        {
          if (!dict.ContainsKey(key))
          {
            dict[key] = new Group(key);
          }
          dict[key].Blocks.Add(block);
        }
      }

      foreach (var inspection in inspections)
      {
        foreach (var key in GetInspectionKeys(inspection))
        {
          if (!dict.ContainsKey(key))
          {
            dict[key] = new Group(key);
          }
          dict[key].Inspections.Add(inspection);
        }
      }
      return dict.Values.ToList();
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
