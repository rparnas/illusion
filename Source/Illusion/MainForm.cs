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
    [STAThread]
    public static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    const int VisualizationScaleFactor = 4;
    List<Block> AllBlocks;
    bool IgnoreSetup;
    DateTime VisualizationStartDate;
    DateTime VisualizationStopDate;

    public MainForm()
    {
      AllBlocks = new List<Block>();
      IgnoreSetup = false;
      VisualizationStartDate = DateTime.Now;
      VisualizationStopDate = DateTime.Now;

      InitializeComponent();
    }

    void Setup()
    {
      if (IgnoreSetup)
      {
        return;
      }

      var blocks = AllBlocks.Where(b => b.Start.Date >= dtp_Start.Value && b.Stop.Date <= dtp_Stop.Value).ToList();
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Project, iclb_Projects);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Feature, iclb_Features);
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Activity, iclb_Activities);

      if (!blocks.Any())
      {
        dgv_Stats.DataSource = null;
        pb_Visualization.Image = null;
        return;
      }

      // Compute stats
      var start = blocks.Min(b => b.Start).Date;
      var stop = blocks.Max(b => b.Stop).Date;
      var hours = blocks.Sum(b => b.Hours);
      var devHours = blocks.Sum(b => b.DevHours);
      var hourRatio = devHours / hours;

      // Display stats
      var dt = new DataTable("Stats");
      dt.Columns.Add("Item");
      dt.Columns.Add("Value");
      Action<string, string> addItem = (item, value) =>
      {
        var row = dt.NewRow();
        row["Item"] = item;
        row["Value"] = value;
        dt.Rows.Add(row);
      };
      addItem("Start",             start.ToString("M/d/yy"));
      addItem("Stop",              stop.ToString("M/d/yy"));
      addItem("Hours",             hours.ToString("F2"));
      addItem("Dev Hours",         devHours .ToString("F2"));
      addItem("Dev Hours / Hours", hourRatio.ToString("F2"));
      dgv_Stats.DataSource = dt;

      // Generate Visualization
      VisualizationStartDate = blocks.First().Start.Date;
      VisualizationStopDate = blocks.Last().Stop.Date.AddDays(1);
      var bmp = new Bitmap(24 * 4, (int)Math.Ceiling((VisualizationStopDate - VisualizationStartDate).TotalDays));

      using (var g = Graphics.FromImage(bmp))
      {
        g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

        foreach (var block in blocks)
        {
          int yStart = (block.Start - VisualizationStartDate).Days;
          int yStop = (block.Stop - VisualizationStartDate).Days;

          for (int y = yStart; y <= yStop; y++)
          {
            int xStart = y == yStart ? (block.Start.Hour * 4) + (block.Start.Minute / 15) : 0;
            int xStop = y == yStop ? (block.Stop.Hour * 4) + (block.Stop.Minute / 15) : bmp.Width - 1;

            g.FillRectangle(Brushes.White, new Rectangle(xStart, y, (xStop - xStart) + 1, 1));
          }
        }
      }

      // Display Visualization
      var zoomed = new Bitmap(bmp, new Size(bmp.Width * VisualizationScaleFactor, bmp.Height * VisualizationScaleFactor));
      pb_Visualization.Size = zoomed.Size;
      pb_Visualization.Left = (tpVisualization.Width - pb_Visualization.Width) / 2;
      pb_Visualization.Top = 6;
      pb_Visualization.Image = zoomed;
    }

    List<Block> UpdateListBoxAndFilterBlocks(List<Block> blocks, Func<Block, string> getCategory, IllusionCheckedListBox iclb)
    {
      var items = blocks.Select(b => getCategory(b)).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
      items.Sort();
      iclb.SetItems(items);
      return blocks.Where(b =>
      {
        var cat = getCategory(b);
        return string.IsNullOrWhiteSpace(cat) || iclb.CheckedItems.Contains(cat);
      }).ToList();
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

      AllBlocks.Clear();
      foreach (DataRow row in file.Rows)
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
          Project = row["Project"].ToString().Trim(),
          Feature = row["Feature"].ToString().Trim(),
          Activity = row["Activity"].ToString().Trim(),
          People = row["People"].ToString().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList()
        });
      }
      AllBlocks.Sort((a, b) => a.Start.CompareTo(b.Start));

      IgnoreSetup = true;
      dtp_Start.Value = AllBlocks.Min(b => b.Start).Date;
      dtp_Stop.Value = AllBlocks.Max(b => b.Stop).Date;
      IgnoreSetup = false;

      Setup();
    }

    void dtp_Start_ValueChanged(object sender, EventArgs e) { Setup(); }

    void dtp_Stop_ValueChanged(object sender, EventArgs e) { Setup(); }

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
      var time = VisualizationStartDate.AddMinutes(x * 15).AddDays(y);

      lbl_Visualization.Text = time.ToString("M/d/yy h:mmtt").Replace("AM", "a").Replace("PM", "p");
    }
  }

  class Block
  {
    public DateTime Start;
    public DateTime Stop;
    public string Project;
    public string Feature;
    public string Activity;
    public List<string> People;

    public double Hours { get { return (Stop - Start).TotalHours; } }
    public double DevHours { get { return Hours * (People.Count); } }
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
