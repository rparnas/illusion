using System.Windows.Forms.VisualStyles;

namespace Illusion.Controls;

internal class IllusionCheckedListBox : CheckedListBox
{
  public event IllusionCheckedListBoxCheckChanged? CheckChanged;
  public List<IllusionCheckedListBox> Forward;
  Dictionary<string, Highlight> Highlighted;
  int IgnoreItemCheckCount;

  public IllusionCheckedListBox()
  {
    CheckChanged = null;
    Forward = new List<IllusionCheckedListBox>();
    Highlighted = new Dictionary<string, Highlight>();
    IgnoreItemCheckCount = 0;

    CheckOnClick = true;
    DoubleBuffered = true;

    ItemCheck += (_, _) =>
    {
      if (IgnoreItemCheckCount != 0)
      {
        return;
      }

      BeginInvoke((MethodInvoker)(() => FireCheckChanged(true, true)));
    };

    MouseUp += (_, e) =>
    {
      if (e.Button == MouseButtons.Right)
      {
        // right-clicking changes the selected index (overriding the base behavior where it would not change on right-click).
        SelectedIndex = IndexFromPoint(e.Location);
        if (SelectedIndex == NoMatches)
        {
          return;
        }

        // show a context menu
        var cm = MakeContextMenu();
        cm.Show(this, e.Location);
      }
    };
  }

  public HashSet<string> GetCheckedItems()
  {
    var ret = new HashSet<string>(CheckedItems.Count);

    for (var i = 0; i < CheckedItems.Count; i++)
    {
      ret.Add(CheckedItems[i]?.ToString() ?? string.Empty);
    }

    return ret;
  }

  public Brush? GetHighlight(IEnumerable<string> items)
  {
    foreach (var item in items)
    {
      if (Highlighted.TryGetValue(item, out var highlight))
      {
        return highlight.Brush;
      }
    }

    return null;
  }

  public override string ToString() => $@"{Name} ({Items.Count})";

  protected override void OnDrawItem(DrawItemEventArgs e)
  {
    if (e.Index < 0 || e.Index >= Items.Count)
    {
      base.OnDrawItem(e);
      return;
    }

    var checkSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.MixedNormal);
    var dx = (e.Bounds.Height - checkSize.Width) / 2;
    var item = Items[e.Index].ToString() ?? string.Empty;

    using (var standardBrush = new SolidBrush(ForeColor))
    {
      var brush = Highlighted.ContainsKey(item) ? Highlighted[item].Brush : standardBrush;

      e.DrawBackground();
      CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), GetItemChecked(e.Index) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
      e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height));
    }
  }

  void DoIgnoreCheckChangedEvents(Action? action)
  {
    try
    {
      SuspendLayout();
      BeginUpdate();
      IgnoreItemCheckCount++;
      action?.Invoke();
    }
    finally
    {
      IgnoreItemCheckCount--;
      EndUpdate();
      ResumeLayout();
    }
  }

  void FireCheckChanged(bool updateBlocks, bool updateDisplay)
  {
    CheckChanged?.Invoke(updateBlocks, updateDisplay);
  }

  ContextMenuStrip MakeContextMenu()
  {
    var selectedItem = SelectedItem?.ToString() ?? string.Empty;

    var ret = new ContextMenuStrip();

    ret.Items.AddRange(
    [
      new ToolStripMenuItem("Select All",          null, (_, _) => SelectAll()        ) { Enabled = true                 },
      new ToolStripMenuItem("Deselect All",        null, (_, _) => DeselectAll()      ) { Enabled = true                 },
      new ToolStripSeparator(),
      new ToolStripMenuItem("Select Forward",      null, (_, _) => SelectForward()    ) { Enabled = SelectedItem != null },
      new ToolStripMenuItem("Select Forward Only", null, (_, _) => SelectForwardOnly()) { Enabled = SelectedItem != null },
      new ToolStripSeparator(),
    ]);

    foreach (var highlight in Illusion.Controls.Highlight.StandardHighlights)
    {
      var isChecked = Highlighted.ContainsKey(selectedItem) && Highlighted[selectedItem] == highlight;
      ret.Items.Add(new ToolStripMenuItem(highlight.Name, null, (_, _) => Highlight(highlight, !isChecked, selectedItem)) { Checked = isChecked });
    }

    return ret;
  }

  void SetAllCheckedWithoutEvents(bool value)
  {
    DoIgnoreCheckChangedEvents(() =>
    {
      for (var i = 0; i < Items.Count; i++)
      {
        SetItemChecked(i, value);
      }
    });
  }

  void SetCheckedWithoutEvents(int index, bool value)
  {
    DoIgnoreCheckChangedEvents(() =>
    {
      SetItemChecked(index, value);
    });
  }

  #region Operations

  public void DeselectAll()
  {
    SetAllCheckedWithoutEvents(false); // check none
    FireCheckChanged(true, true);      // update
  }

  public void SelectAll()
  {
    SetAllCheckedWithoutEvents(true); // check all
    FireCheckChanged(true, true);     // update
  }

  public void SelectAllForward()
  {
    SetAllCheckedWithoutEvents(true); // check all
    FireCheckChanged(true, false);    // update blocks only

    for (var i = 0; i < Forward.Count; i++)
    {
      var forward = Forward[i];
      forward.SetAllCheckedWithoutEvents(true);               // check all
      forward.FireCheckChanged(true, i == Forward.Count - 1); // update blocks only (except for the final box)
    }
  }
  
  public void SelectForward()
  {
    SetCheckedWithoutEvents(SelectedIndex, true); // check clicked (if not already checked)
    FireCheckChanged(true, false);                // update blocks only

    for (var i = 0; i < Forward.Count; i++)
    {
      var forward = Forward[i];
      forward.SetAllCheckedWithoutEvents(true);               // select all
      forward.FireCheckChanged(true, i == Forward.Count - 1); // update blocks only (except for the final box)
    }
  }
  
  public void SelectForwardOnly()
  {
    SetAllCheckedWithoutEvents(false);            // uncheck all
    SetCheckedWithoutEvents(SelectedIndex, true); // check clicked only
    FireCheckChanged(true, false);                // update blocks only

    for (var i = 0; i <Forward.Count; i++)
    {
      var forward = Forward[i];
      forward.SetAllCheckedWithoutEvents(true);               // check all
      forward.FireCheckChanged(true, i == Forward.Count - 1); // update blocks only (except for the final box)
    }
  }

  public void SetItems(List<string> newItems)
  {
    DoIgnoreCheckChangedEvents(() =>
    {
      var previouslyChecked = GetCheckedItems();
      var previouslySelected = (SelectedItem ?? "").ToString();

      // remove unneeded items
      while (Items.Count > newItems.Count)
      {
        Items.RemoveAt(Items.Count - 1);
      }

      var i = 0;

      // re-use some items
      while (i < Items.Count)
      {
        var newItem = newItems[i];
        var isChecked = previouslyChecked.Contains(newItem);

        Items[i] = newItem;
        SetItemChecked(i, isChecked);

        if (newItem == previouslySelected)
        {
          SelectedIndex = i;
        }

        i++;
      }

      // add new items
      while (i < newItems.Count)
      {
        var newItem = newItems[i];
        var isChecked = previouslyChecked.Contains(newItem);

        Items.Add(newItem, isChecked);

        if (newItem == previouslySelected)
        {
          SelectedIndex = i;
        }

        i++;
      }
    });
  }

  void Highlight(Highlight newHighlight, bool newIsChecked, string selectedItem)
  {
    if (newIsChecked)
    {
      Highlighted[selectedItem] = newHighlight;
    }
    else
    {
      Highlighted.Remove(selectedItem);
    }

    Refresh();                    // re-draw (for colors)
    FireCheckChanged(true, true); // update
  }

  #endregion
}

public delegate void IllusionCheckedListBoxCheckChanged(bool updateBlocks, bool updateDisplay);