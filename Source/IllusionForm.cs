using Illusion.Controls;
using Illusion.Data;
using Illusion.Utilities;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;

namespace Illusion;

public partial class IllusionForm : Form
{
  const int VisualizationScaleFactor = 4;

  static readonly List<Grouper<Block>> Groupers =
  [
    new(nameof(Scope.Company ), b => [b.Scope.Company ]),
    new(nameof(Scope.Project ), b => [b.Scope.Project ]),
    new(nameof(Scope.Feature ), b => [b.Scope.Feature ]),
    new(nameof(Scope.Activity), b => [b.Scope.Activity]),
    new(nameof(Scope.People  ), b => b.Scope.People    ),
    
    new(nameof(DateTime.Year),  b => [$@"{b.Time.Date:yyyy}"    ]),
    new(nameof(DateTime.Month), b => [$@"{b.Time.Date:yyyy MMM}"]),
  ];

  static readonly List<Stat<Group<Block>>> Stats =
  [
    new("Start",               "MM/dd/yy", b => b.Items.Min(b => b.Time.Date)                                ),
    new("Stop",                "MM/dd/yy", b => b.Items.Max(b => b.Time.Date)                                ),
    new("Hours",               "N2",       b => b.Items.Sum(b => b.Time.Hours)                               ),
    new("Other Dev\r\nHours",  "N2",       b => b.Items.Sum(b => b.DevHours)                                 ),
    new("Other Dev\r\nRatio",  "N2",       b => b.Items.Sum(b => b.DevHours) / b.Items.Sum(b => b.Time.Hours)),
    new("$",                   "N0",       b => b.Items.Sum(b => b.Amount)                                   ),
    new("$ per\r\nhour",       "N2",       b => b.Items.Sum(b => b.Amount) / b.Items.Sum(b => b.Time.Hours)  ),
  ];

  static List<Block> DisplayedBlocks;
  static IllusionSet? Data;
  DateTime DataMinTime = new(2000, 1, 1);
  DateTime DataMaxTime = new(2099, 1, 1);

  [STAThread]
  static void Main()
  {
    ApplicationConfiguration.Initialize();
    Application.Run(new IllusionForm());
  }

  static IllusionForm()
  {
    Data = null;
    DisplayedBlocks = new List<Block>();
  }

  public IllusionForm()
  {
    InitializeComponent();

    // btn_Reload
    btn_Reload.Enabled = btn_Reload_CanClick();

    // cb_Grouping
    cb_Grouping.DataSource = Groupers;
    cb_Grouping.SelectedItem = Groupers.First(g => g.Name == "Feature");
    cb_Grouping.SelectedIndexChanged += (_, _) => FilterAndDisplayBlocks(new ChangeFlags(Grouping: true));

    // cb_Time
    cb_Time.DataSource = TimeFilter.Standard;
    cb_Time.SelectedItem = null;

    // dtp_Start
    dtp_Start.ValueChanged += (_, _) => FilterAndDisplayBlocks(new ChangeFlags(Filter: true));

    // dtp_Stop
    dtp_Stop.ValueChanged += (_, _) => FilterAndDisplayBlocks(new ChangeFlags(Filter: true));

    // iclb
    var allICLB = new List<IllusionCheckedListBox> { iclb_Companies, iclb_Projects, iclb_Features, iclb_Activities, iclb_People };
    for (int i = 0; i < allICLB.Count; i++)
    {
      allICLB[i].Forward = allICLB
        .Skip(i + 1)
        .ToList();

      allICLB[i].CheckChanged += (updateBlocks, updateDisplay) =>
      {
        FilterAndDisplayBlocks(new ChangeFlags(Filter: true), updateBlocks, updateDisplay);
      };
    }

    if (btn_Reload_CanClick())
    {
      btn_Reload_Click(null!, null!);
    }
  }

  void FilterAndDisplayBlocks(ChangeFlags changes, bool doFilter = true, bool doDisplay = true)
  {
    if (doFilter)
    {
      FilterBlocks(changes);
    }
    if (doDisplay)
    {
      DisplayBlocks(changes);
    }
  }

  void FilterBlocks(ChangeFlags changes)
  {
    static List<Block> DisplayAndFilterBlocks(
      List<Block> blocks,
      Func<Block, string[]> getCategories,
      IllusionCheckedListBox iclb)
    {
      var items = new HashSet<string>();
      var ret = new List<Block>();

      var checkedItems = iclb.GetCheckedItems();

      foreach (var block in blocks)
      {
        var categories = getCategories(block);
        var contains = false;
        foreach (var category in categories)
        {
          contains |= checkedItems.Contains(category);
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

    if (Data is null)
    {
      return;
    }

    if (changes.Data || changes.Filter)
    {
      var startDate = dtp_Start.Value;
      var stopDate = dtp_Stop.Value;

      var blocks = Data.Blocks
        .Where(b => b.Time.Date >= startDate && b.Time.Date <= stopDate)
        .ToList();

      blocks = DisplayAndFilterBlocks(blocks, b => [b.Scope.Company],  iclb_Companies );
      blocks = DisplayAndFilterBlocks(blocks, b => [b.Scope.Project],  iclb_Projects  );
      blocks = DisplayAndFilterBlocks(blocks, b => [b.Scope.Feature],  iclb_Features  );
      blocks = DisplayAndFilterBlocks(blocks, b => [b.Scope.Activity], iclb_Activities);
      blocks = DisplayAndFilterBlocks(blocks, b => b.Scope.People,     iclb_People    );

      DisplayedBlocks = blocks;
    }
  }

  void DisplayBlocks(ChangeFlags changes)
  {
    static bool GetIsOtherDevs<T>(Stat<T> stat)
    {
      return stat.Name.Contains("Other") && stat.Name.Contains("Dev");
    }

    static bool GetIsCurrency<T>(Stat<T> stat)
    {
      return stat.Name.Contains('$');
    }

    static DataGridViewTextBoxColumn MakeGroupColumn()
    {
      return new DataGridViewTextBoxColumn
      {
        DefaultCellStyle = new DataGridViewCellStyle
        {
          Alignment = DataGridViewContentAlignment.MiddleLeft,
        },
        Name = "Group",
        ValueType = typeof(string)
      };
    }

    static DataGridViewTextBoxColumn MakeStatColumn<T>(Stat<T> stat)
    {
      return new DataGridViewTextBoxColumn
      {
        DefaultCellStyle = new DataGridViewCellStyle
        {
          Alignment = stat.IsDate ? DataGridViewContentAlignment.MiddleRight :
                stat.IsNumber ? DataGridViewContentAlignment.MiddleRight :
                DataGridViewContentAlignment.MiddleLeft,
          Format = stat.Format,
        },
        Name = stat.Name,
        SortMode = DataGridViewColumnSortMode.Automatic,
        ValueType = stat.Type
      };
    }

    static void DisplayStats(string name, DataGridView dgv, bool isNewData, bool showOtherDevs, bool showIncome, List<Group<Block>> groups, List<Stat<Group<Block>>> stats)
    {
      var chosenStats = stats
        .Where(stat => showOtherDevs || !GetIsOtherDevs(stat))
        .Where(stat => showIncome || !GetIsCurrency(stat))
        .ToArray();

      var boldFont = new Font(dgv.DefaultCellStyle.Font, FontStyle.Bold);

      // save sorting
      var sortColumnName = dgv.Columns.Count == 0 ? "Group" : dgv.SortedColumn?.Name;
      var sortOrder = dgv.Columns.Count == 0 ? SortOrder.Ascending : dgv.SortOrder;

      // clear
      dgv.Columns.Clear();
      dgv.Rows.Clear();

      // display nothing
      if (!groups.Any(g => g.Items.Any()))
      {
        return;
      }

      try
      {
        dgv.SuspendLayout();

        // add columns
        dgv.Columns.Add(MakeGroupColumn());
        dgv.Columns.AddRange(chosenStats.Select(MakeStatColumn).ToArray());

        // add rows
        var rowBuffer = new object[1 + chosenStats.Length];
        foreach (var group in groups)
        {
          rowBuffer[0] = group.Name;
          for (var i = 0; i < chosenStats.Length; i++)
          {
            var stat = chosenStats[i];
            var value = stat.Compute(group);

            // use the value if it is a non-number or a non-zero number
            if (!double.TryParse(value?.ToString() ?? string.Empty, out var doubleValue) || doubleValue != 0d)
            {
              rowBuffer[i + 1] = value ?? string.Empty;
            }
            else
            {
              rowBuffer[i + 1] = string.Empty;
            }
          }

          var newRow = new DataGridViewRow();
          newRow.CreateCells(dgv, rowBuffer);
          if (group.Name == "Total")
          {
            newRow.DefaultCellStyle.Font = boldFont;
          }
          dgv.Rows.Add(newRow);
        }

        // size
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        dgv.AutoResizeColumns();
        foreach (var dgvc in dgv.Columns.Cast<DataGridViewColumn>())
          dgvc.Width += 12;

        // sort
        if (sortColumnName != null && dgv.Columns.Contains(sortColumnName) && sortOrder != SortOrder.None)
        {
          var dir =
            sortOrder == SortOrder.Ascending ? ListSortDirection.Ascending :
            sortOrder == SortOrder.Descending ? ListSortDirection.Descending :
            throw new NotImplementedException();

          dgv.Sort(dgv.Columns[sortColumnName]!, dir);
        }
      }
      finally
      {
        dgv.ResumeLayout();
      }
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

    if (Data is null)
    {
      if (tc.TabPages.Contains(tp_Errors))
      {
        tc.TabPages.Remove(tp_Errors);
      }

      return;
    }

    // stats
    if (changes.Data || changes.Filter || changes.CollapseParenthesis || changes.Grouping || changes.ShowOtherDevs || changes.ShowIncome)
    {
      var grouper = (Grouper<Block>)cb_Grouping.SelectedItem!;
      var groups = grouper.Group(
        items:             DisplayedBlocks,
        ignoreParenthesis: cb_CollapseParenthesis.Checked);

      DisplayStats(
        name:          "Stats",
        dgv:           dgv_Stats,
        isNewData:     changes.Data,
        showOtherDevs: cb_OtherDevs.Checked,
        showIncome:    cb_Income.Checked,
        groups:        groups, 
        stats:         Stats);
    }

    // visualization
    if (changes.Data || changes.Filter)
    {
      if (DisplayedBlocks.Any())
      {
        var bmp = MakeVisualizationBitmap(DisplayedBlocks, dtp_Start.Value, dtp_Stop.Value, block =>
        {
          return
            iclb_People.GetHighlight(block.Scope.People) ??
            iclb_Activities.GetHighlight([block.Scope.Activity]) ??
            iclb_Features.GetHighlight([block.Scope.Feature]) ??
            iclb_Projects.GetHighlight([block.Scope.Project]) ??
            iclb_Companies.GetHighlight([block.Scope.Company]) ??
            Brushes.White;
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
    }

    // errors
    if (changes.Data)
    {
      tb_Errors.Text = string.Join("\r\n\r\n", Data.Errors.ToArray());
      tp_Errors.Text = $@"Errors ({Data.Errors.Count})";

      if (Data.Errors.Any() && !tc.TabPages.Contains(tp_Errors))
      {
        tc.TabPages.Add(tp_Errors);
      }
      else if (!Data.Errors.Any() && tc.TabPages.Contains(tp_Errors))
      {
        tc.TabPages.Remove(tp_Errors);
      }
    }
  }

  void LoadBlocks(string[] paths, bool preserveDateFilter)
  {
    var files = paths
      .Select(path => new FileInfo(path))
      .Where(file => file.Extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase))
      .ToArray();
    if (!files.All(file => file.Exists))
    {
      return;
    }

    var sets = files
      .Select(file => Loader.GetExcelSheets(file.FullName, null))
      .Select(Parser.Parse)
      .ToList();
    var data = IllusionSet.Merge(sets);
    var dataMinTime = data.Blocks.First().Time.Date;
    var dataMaxTime = data.Blocks.Last().Time.Date;

    if (Settings.Default.LogPath == null)
    {
      Settings.Default.LogPath = new StringCollection();
    }
    Settings.Default.LogPath.Clear();
    Settings.Default.LogPath.AddRange(files.Select(file => file.FullName).ToArray());
    Settings.Default.Save();

    Data = null; // prevent events
    dtp_Start.Value = preserveDateFilter ? dtp_Start.Value : dataMinTime;
    dtp_Stop.Value = preserveDateFilter ? dtp_Stop.Value : dataMaxTime;
    
    Data = data;
    DataMinTime = dataMinTime;
    DataMaxTime = dataMaxTime;
    FilterAndDisplayBlocks(new ChangeFlags(Data: true));
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

  void cb_CollapseParenthesis_CheckedChanged(object sender, EventArgs e) => FilterAndDisplayBlocks(new ChangeFlags(CollapseParenthesis: true));

  void cb_OtherDevs_CheckedChanged(object sender, EventArgs e) => FilterAndDisplayBlocks(new ChangeFlags(ShowOtherDevs: true));

  void cb_Income_CheckedChanged(object sender, EventArgs e) => FilterAndDisplayBlocks(new ChangeFlags(ShowIncome: true));

  void cb_Time_SelectionChangeCommitted(object sender, EventArgs e)
  {
    var timeFilter = (TimeFilter)cb_Time.SelectedItem!;
    var range = timeFilter.GetRange(DataMinTime, DataMaxTime);

    cb_Time.SelectedItem = null;
    dtp_Start.Value = range.Start;
    dtp_Stop.Value = range.Stop;

    iclb_Companies.SelectAllForward();
  }

  void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
  {
    var dgv = (DataGridView)sender;
    if (e.RowIndex1 == dgv.Rows.Count - 1 || e.RowIndex2 == dgv.Rows.Count - 1)
    {
      e.Handled = true;
    }
  }

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
