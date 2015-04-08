using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyTitle("Illusion")]
[assembly: AssemblyProduct("Illusion")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace Illusion
{
  public partial class MainForm : Form
  {
    List<Block> Blocks = new List<Block>();

    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    public MainForm()
    {
      InitializeComponent();
    }

    void btn_Load_Click(object sender, EventArgs e)
    {
      var ofd = new OpenFileDialog();
      ofd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
      if (ofd.ShowDialog() != DialogResult.OK)
        return;

      var file = Loader.GetXLSX(ofd.FileName, "Time Sheet");
      if (file == null)
        return;

      Blocks.Clear();
      foreach (DataRow row in file.Rows)
      {
        var dateStr = row["Date"].ToString().Trim();
        var startStr = row["Start"].ToString().Trim();
        var stopStr = row["Stop"].ToString().Trim();
        var start = DateTime.ParseExact(dateStr + " " + startStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);
        var stop = DateTime.ParseExact(dateStr + " " + stopStr.Replace("a", "AM").Replace("p", "PM"), "M/d/yy h:mmtt", CultureInfo.InvariantCulture);

        Blocks.Add(new Block
        {
          Start = start,
          Stop = stop,
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Activity = row["Activity"].ToString().Trim()
        });
      }
      Setup();
    }

    void Setup()
    {
      var projects = Blocks.Select(b => b.Project).Distinct().ToList();
      projects.Sort();
      iclb_Projects.Label = "Projects";
      iclb_Projects.SetItems(projects);
      var checkedProjects = iclb_Projects.CheckedItems;

      var features = Blocks.Where(b => checkedProjects.Contains(b.Project)).Select(b => b.Feature).Distinct().ToList();
      features.Sort();
      iclb_Features.Label = "Features";
      iclb_Features.SetItems(features);
      var checkedFeatures = iclb_Features.CheckedItems;

      var activities = Blocks.Where(b => checkedProjects.Contains(b.Project) && checkedFeatures.Contains(b.Feature)).Select(b => b.Activity).Distinct().ToList();
      activities.Sort();
      iclb_Activities.Label = "Activities";
      iclb_Activities.SetItems(activities);
      var checkedActivites = iclb_Activities.CheckedItems;


      var selectedBlocks = Blocks.Where(b => checkedProjects.Contains(b.Project) && checkedFeatures.Contains(b.Feature) && checkedActivites.Contains(b.Activity)).ToList();
      var dt = new DataTable();
      dt.Columns.Add("Item");
      dt.Columns.Add("Value");
      Action<string, string> addItem = (item, value) =>
      {
        var row = dt.NewRow();
        row["Item"] = item;
        row["Value"] = value;
        dt.Rows.Add(row);
      };

      if (selectedBlocks.Any())
      {
        addItem("Start", selectedBlocks.First().Start.ToString("M/d/yy"));
        addItem("Stop", selectedBlocks.Last().Stop.ToString("M/d/yy"));
        addItem("Dev Hours", selectedBlocks.Sum(b => b.DevHours).ToString());
      }

      dgv.DataSource = dt;
    }
  }

  class Block
  {
    public DateTime Start;
    public DateTime Stop;
    public string Project;
    public string Feature;
    public string Activity;

    public double DevHours { get { return (Stop - Start).TotalHours; } }
  }

  class Loader
  {
    public static DataTable GetXLSX(string path, string sheetName)
    {
      var ep = new ExcelPackage(new FileInfo(path));
      foreach (var worksheet in ep.Workbook.Worksheets)
      {
        if (worksheet.Name == sheetName && worksheet.Dimension != null)
          return SheetToTable(worksheet);
      }
      return null;
    }

    static DataTable SheetToTable(ExcelWorksheet ws)
    {
      var ret = new DataTable(ws.Name);

      var usedCols = new Dictionary<string, int>();
      for (int col = 1; col <= ws.Dimension.End.Column; col++)
      {
        var cell = ws.Cells[1, col];
        var text = cell.Text.Replace('\n', '_').Replace('\r', '_');

        // Make up a name for columns without text
        if (string.IsNullOrWhiteSpace(text))
          text = cell.Address;

        if (!usedCols.ContainsKey(text))
          usedCols[text] = 0;

        usedCols[text]++;
        ret.Columns.Add(text + (usedCols[text] == 1 ? "" : usedCols[text].ToString()));
      }

      for (int row = 2; row <= ws.Dimension.End.Row; row++)
      {
        var newRow = ret.NewRow();

        for (int col = 1; col <= ws.Dimension.End.Column; col++)
          newRow[col - 1] = ws.Cells[row, col].Text;

        ret.Rows.Add(newRow);
      }

      return ret;
    }
  }
}
