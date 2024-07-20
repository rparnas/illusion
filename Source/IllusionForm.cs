using Illusion.Controls;
using Illusion.Data;
using Illusion.Utilities;
using System.Collections.Specialized;
using System.Data;

namespace Illusion;

public partial class IllusionForm : Form
{
  const int VisualizationScaleFactor = 4;

  static readonly List<Grouper<Block>> Groupers = new List<Grouper<Block>>
  {
    new Grouper<Block>(nameof(Scope.Company), b => new [] { b.Scope.Company }),
    new Grouper<Block>(nameof(Scope.Project), b => new [] { b.Scope.Project }),
    new Grouper<Block>(nameof(Scope.Feature), b => new [] { b.Scope.Feature }),
    new Grouper<Block>(nameof(Scope.Activity), b => new [] { b.Scope.Activity }),
    new Grouper<Block>(nameof(Scope.People), b => b.Scope.People),
  };

  static readonly List<Stat<Group<Block>>> Stats = new List<Stat<Group<Block>>>
  {
    new Stat<Group<Block>>("Start",          b => b.Items.Min(b => b.Time.Date)                                ),
    new Stat<Group<Block>>("Stop",           b => b.Items.Max(b => b.Time.Date)                                ),
    new Stat<Group<Block>>("Hours",          b => b.Items.Sum(b => b.Time.Hours)                               ),
    new Stat<Group<Block>>("Dev Hours",      b => b.Items.Sum(b => b.DevHours)                                 ),
    new Stat<Group<Block>>("Dev Hour Ratio", b => b.Items.Sum(b => b.DevHours) / b.Items.Sum(b => b.Time.Hours)),
    new Stat<Group<Block>>("$",              b => b.Items.Sum(b => b.Amount)                                   ),
    new Stat<Group<Block>>("$ / hour",       b => b.Items.Sum(b => b.Amount) / b.Items.Sum(b => b.Time.Hours)  ),
  };

  IllusionSet? Data;
  IgnoreToken IgnoreDisplayBlocks;

  [STAThread]
  static void Main()
  {
    ApplicationConfiguration.Initialize();
    Application.Run(new IllusionForm());
  }

  public IllusionForm()
  {
    InitializeComponent();

    // btn_Reload
    btn_Reload.Enabled = btn_Reload_CanClick();

    // cb_Grouping
    cb_Grouping.DataSource = Groupers;
    cb_Grouping.SelectedItem = Groupers.First(g => g.Name == "Feature");

    // cb_Time
    cb_Time.DataSource = TimeFilter.Standard;
    cb_Time.SelectedItem = null;

    // iclb
    var allICLB = new List<IllusionCheckedListBox> { iclb_Companies, iclb_Projects, iclb_Features, iclb_Activities, iclb_People };
    for (int i = 0; i < allICLB.Count; i++)
    {
      allICLB[i].Forward = allICLB
        .Skip(i + 1)
        .ToList();
    }

    Data = null;
    IgnoreDisplayBlocks = new IgnoreToken();

    if (btn_Reload_CanClick())
    {
      btn_Reload_Click(null!, null!);
    }
  }

  void DisplayBlocks()
  {
    static List<Block> DisplayAndFilterBlocks(
      List<Block> blocks,
      Func<Block, IEnumerable<string>> getCategories,
      IllusionCheckedListBox iclb)
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

      var sortedItems = items
        .OrderBy(item => item)
        .ToList();
      iclb.SetItems(sortedItems);

      return ret;
    }

    static void DisplayStats(string name, DataGridView dgv, bool showIncome, List<Group<Block>> groups, List<Stat<Group<Block>>> stats)
    {
      dgv.Columns.Clear();
      dgv.Rows.Clear();

      if (!groups.Any(g => g.Items.Any()))
      {
        return;
      }

      dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Group", ValueType = typeof(string) });

      foreach (var stat in stats)
      {
        if (!showIncome && stat.Name.Contains("$"))
        {
          continue;
        }

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
          if (!showIncome && stat.Name.Contains("$"))
          {
            continue;
          }

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

    static Bitmap MakeVisualizationBitmap(List<Block> blocks, DateTime bmpStart, DateTime bmpStop, Func<Block, Brush> getBrush)
    {
      var bmp = new Bitmap(24 * 4, Math.Max(1, (int)Math.Ceiling((bmpStop - bmpStart).TotalDays)));

      using (var g = Graphics.FromImage(bmp))
      {
        g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);

        foreach (var block in blocks)
        {
          if (!block.Time.Start.HasValue || !block.Time.Stop.HasValue)
          {
            continue;
          }
          var start = block.Time.Start.Value;
          var stop = block.Time.Stop.Value;

          int yStart = (start - bmpStart).Days;
          int yStop = (stop - bmpStart).Days;

          for (int y = yStart; y <= yStop; y++)
          {
            int xStart = y == yStart ? (start.Hour * 4) + (start.Minute / 15) : 0;
            int xStop = y == yStop ? (stop.Hour * 4) + (stop.Minute / 15) : bmp.Width - 1;

            var brush = getBrush(block);

            g.FillRectangle(brush, new Rectangle(xStart, y, (xStop - xStart) + 1, 1));
          }
        }
      }

      return bmp;
    }

    static string[] ProccessInitials(string[] initials, List<Person> people)
    {
      return initials
        .Select(i =>
        {
          var persons = people
            .Where(p => p.Initials == i)
            .ToList();

          return persons.Count == 0 ? i :
                 persons.Count == 1 ? persons.Single().ToString() :
                 $@"{i} (?)";
        })
        .ToArray();
    }

    if (Data == null || IgnoreDisplayBlocks.Ignore)
    {
      return;
    }

    // list boxes
    var blocks = Data.Blocks
      .Where(b => b.Time.Date >= dtp_Start.Value && b.Time.Date <= dtp_Stop.Value)
      .ToList();
    blocks = DisplayAndFilterBlocks(blocks, b => new[] { b.Scope.Company }, iclb_Companies);
    blocks = DisplayAndFilterBlocks(blocks, b => new[] { b.Scope.Project }, iclb_Projects);
    blocks = DisplayAndFilterBlocks(blocks, b => new[] { b.Scope.Feature }, iclb_Features);
    blocks = DisplayAndFilterBlocks(blocks, b => new[] { b.Scope.Activity }, iclb_Activities);
    blocks = DisplayAndFilterBlocks(blocks, b => ProccessInitials(b.Scope.People, Data.People), iclb_People);

    // data grids
    var grouper = (Grouper<Block>)cb_Grouping.SelectedItem;
    var ignoreParenthesis = cb_CollapseParenthesis.Checked;
    var groups = grouper.Group(blocks, ignoreParenthesis);
    DisplayStats("Stats", dgv_Stats, cb_Income.Checked, groups, Stats);

    // visualization
    if (blocks.Any())
    {
      var bmp = MakeVisualizationBitmap(blocks, dtp_Start.Value, dtp_Stop.Value, block =>
      {
        return
          iclb_Companies.GetHighlight(new[] { block.Scope.Company }) ??
          iclb_Projects.GetHighlight(new[] { block.Scope.Project }) ??
          iclb_Features.GetHighlight(new[] { block.Scope.Feature }) ??
          iclb_Activities.GetHighlight(new[] { block.Scope.Activity }) ??
          iclb_People.GetHighlight(block.Scope.People) ??
          (Brushes.White);
      });
      pnl_Visualization.VerticalScroll.Visible = true;
      pnl_Visualization.VerticalScroll.Value = 0;
      var zoomed = new Bitmap(bmp, new Size(bmp.Width * VisualizationScaleFactor, bmp.Height * VisualizationScaleFactor));
      pb_Visualization.Size = zoomed.Size;
      pb_Visualization.Location = new Point((tpVisualization.Width - pb_Visualization.Width) / 2, 3);
      pb_Visualization.Image = zoomed;
    }
    else
    {
      pb_Visualization.Image = null;
    }

    // errors
    tb_Errors.Text = string.Join("\r\n\r\n", Data.Errors.ToArray());
    tpErrors.Text = $@"Errors ({Data.Errors.Count})";
    if (Data.Errors.Any() && !tc.TabPages.Contains(tpErrors))
    {
      tc.TabPages.Add(tpErrors);
    }
    else if (!Data.Errors.Any() && tc.TabPages.Contains(tpErrors))
    {
      tc.TabPages.Remove(tpErrors);
    }
  }

  void LoadBlocks(string[] paths, bool preserveDateFilter)
  {
    var files = paths
      .Select(path => new FileInfo(path))
      .Where(file => file.Extension.ToUpperInvariant() == ".XLSX")
      .ToArray();
    if (!files.All(file => file.Exists))
    {
      return;
    }


    var sets = files
      .Select(file => Loader.GetExcelSheets(file.FullName, null))
      .Select(Parser.Parse)
      .ToList();
    Data = IllusionSet.Merge(sets);

    if (Settings.Default.LogPath == null)
    {
      Settings.Default.LogPath = new StringCollection();
    }
    Settings.Default.LogPath.Clear();
    Settings.Default.LogPath.AddRange(files.Select(file => file.FullName).ToArray());
    Settings.Default.Save();

    IgnoreDisplayBlocks.Do(() =>
    {
      dtp_Start.Value = preserveDateFilter ? dtp_Start.Value : Data!.Blocks.First().Time.Date;
      dtp_Stop.Value = preserveDateFilter ? dtp_Stop.Value : Data!.Blocks.Last().Time.Date;
    });

    DisplayBlocks();
  }

  void btn_Load_Click(object sender, EventArgs e)
  {
    var ofd = new OpenFileDialog
    {
      Filter = "Excel Workbook (*.xlsx)|*.xlsx",
      Multiselect = true,
    };

    if (ofd.ShowDialog() != DialogResult.OK)
    {
      return;
    }

    var paths = ofd.FileNames;

    if (!paths.All(File.Exists))
    {
      return;
    }

    LoadBlocks(paths, false);
  }

  bool btn_Reload_CanClick()
  {
    return
      Settings.Default.LogPath is not null &&
      Settings.Default.LogPath.Cast<string>().All(File.Exists);
  }

  void btn_Reload_Click(object sender, EventArgs e)
  {
    if (!btn_Reload_CanClick())
    {
      return;
    }

    var paths = Settings.Default.LogPath
      .Cast<string>()
      .ToArray();

    LoadBlocks(paths, sender == btn_Reload);
  }

  void cb_Grouping_SelectedIndexChanged(object sender, EventArgs e) => DisplayBlocks();

  void cb_CollapseParenthesis_CheckedChanged(object sender, EventArgs e) => DisplayBlocks();

  void cb_Income_CheckedChanged(object sender, EventArgs e) => DisplayBlocks();

  void cb_Time_SelectionChangeCommitted(object sender, EventArgs e)
  {
    var timeFilter = (TimeFilter)cb_Time.SelectedItem;
    var range = timeFilter.GetRange();

    cb_Time.SelectedItem = null;
    dtp_Start.Value = range.Start;
    dtp_Stop.Value = range.Stop;

    iclb_Companies.SelectAll(true);
    iclb_Companies.SelectAllForward(DisplayBlocks);
  }

  void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
  {
    var dgv = (DataGridView)sender;
    if (e.RowIndex1 == dgv.Rows.Count - 1 || e.RowIndex2 == dgv.Rows.Count - 1)
    {
      e.Handled = true;
    }
  }

  void dtp_Start_ValueChanged(object sender, EventArgs e) => DisplayBlocks();

  void dtp_Stop_ValueChanged(object sender, EventArgs e) => DisplayBlocks();

  void iclb_Projects_ItemCheckChanged() => DisplayBlocks();

  void iclb_Features_ItemCheckChanged() => DisplayBlocks();

  void iclb_Activities_ItemCheckChanged() => DisplayBlocks();

  void iclb_Companies_ItemCheckChanged() => DisplayBlocks();

  void iclb_People_ItemCheckChanged() => DisplayBlocks();

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
