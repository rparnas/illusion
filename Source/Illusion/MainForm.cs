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
    List<Block> AllBlocks = new List<Block>();

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
          Activity = row["Activity"].ToString().Trim()
        });
      }

      dtp_Start.Value = AllBlocks.Min(b => b.Start).Date;
      dtp_Stop.Value = AllBlocks.Max(b => b.Stop).Date;

      Setup();
    }

    void Setup()
    {
      var blocks = AllBlocks.Where(b => b.Start.Date >= dtp_Start.Value && b.Stop.Date <= dtp_Stop.Value).ToList();
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Project,  iclb_Projects  );
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Feature,  iclb_Features  );
      blocks = UpdateListBoxAndFilterBlocks(blocks, b => b.Activity, iclb_Activities);

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

      if (blocks.Any())
      {
        addItem("Start",     blocks.Min(b => b.Start).ToString("M/d/yy"));
        addItem("Stop",      blocks.Max(b => b.Stop).ToString("M/d/yy"));
        addItem("Dev Hours", blocks.Sum(b => b.DevHours).ToString());
      }

      dgv.DataSource = dt;
    }

    List<Block> UpdateListBoxAndFilterBlocks(List<Block> blocks, Func<Block, string> getCategory, IllusionCheckedListBox iclb)
    {
      var items = blocks.Select(b => getCategory(b)).Distinct().ToList();
      items.Sort();
      iclb.SetItems(items);
      return blocks.Where(b => iclb.CheckedItems.Contains(getCategory(b))).ToList();
    }

    void dtp_Start_ValueChanged(object sender, EventArgs e) { Setup(); }

    void dtp_Stop_ValueChanged(object sender, EventArgs e) { Setup(); }
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
