using System.Windows.Forms.VisualStyles;

namespace Illusion.Controls
{
  internal class IllusionCheckedListBox : CheckedListBox
  {
    public List<IllusionCheckedListBox> Forward;
    Dictionary<string, Highlight> Highlighted;
    bool IgnoreItemCheck;
    public event Action? ItemCheckChanged;

    public HashSet<string> AllCheckedItems
    {
      get
      {
        return CheckedItems
          .Cast<object>()
          .Select(item => item.ToString()!)
          .ToHashSet();
      }
    }

    public IllusionCheckedListBox()
    {
      Forward = new List<IllusionCheckedListBox>();
      Highlighted = new Dictionary<string, Highlight>();
      IgnoreItemCheck = false;
      ItemCheckChanged = null;

      CheckOnClick = true;
      DoubleBuffered = true;

      ItemCheck += (_, _) =>
      {
        if (!IgnoreItemCheck)
        {
          BeginInvoke((MethodInvoker)(() => FireItemCheckChanged()));
        }
      };

      MouseUp += (s, e) =>
      {
        if (e.Button != MouseButtons.Right)
        {
          return;
        }

        SelectedIndex = IndexFromPoint(e.Location);
        if (SelectedIndex == NoMatches)
        {
          return;
        }

        var item = (SelectedItem ?? "").ToString()!;

        var cm = new ContextMenuStrip();

        // Selection controls
        cm.Items.Add(new ToolStripMenuItem("Select All", null, (_, _) =>
        {
          SelectAll(true);
          FireItemCheckChanged();
        }));
        cm.Items.Add(new ToolStripMenuItem("Deselect All", null, (_, _) =>
        {
          SelectAll(false);
          FireItemCheckChanged();
        }));
        cm.Items.Add(new ToolStripMenuItem("Select Forward", null, (_, _) =>
        {
          SetItemChecked(SelectedIndex, true);
          FireItemCheckChanged();

          SelectAllForward(FireItemCheckChanged);
        })
        {
          Enabled = SelectedItem != null,
        });

        cm.Items.Add(new ToolStripMenuItem("Select Forward Only", null, (_, _) =>
        {
          SelectAll(false);
          SetItemChecked(SelectedIndex, true);
          FireItemCheckChanged();

          SelectAllForward(FireItemCheckChanged);
        })
        {
          Enabled = SelectedItem != null,
        });

        cm.Items.Add(new ToolStripSeparator());

        // Highlighting
        foreach (var highlight in Highlight.StandardHighlights)
        {
          var h = highlight;
          var isChecked = Highlighted.ContainsKey(item) && Highlighted[item] == h;

          cm.Items.Add(new ToolStripMenuItem(h.Name, null, (_, _) =>
          {
            if (isChecked)
            {
              Highlighted.Remove(item);
            }
            else
            {
              Highlighted[item] = h;
            }

            Refresh();
            FireItemCheckChanged();
          })
          {
            Checked = isChecked,
          });
        }

        cm.Show(this, e.Location);
      };
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

    public void SelectAllForward(Action postAction)
    {
      foreach (var iclb in Forward)
      {
        iclb.SelectAll(true);
        postAction?.Invoke();
      }
    }

    public void SelectAll(bool value)
    {
      DoIgnoreItemCheck(() =>
      {
        for (var i = 0; i < Items.Count; i++)
        {
          SetItemChecked(i, value);
        }
      });
    }

    public void SetItems(List<string> items)
    {
      DoIgnoreItemCheck(() =>
      {
        var previouslyChecked = AllCheckedItems;
        var previouslySelected = (SelectedItem ?? "").ToString();

        Items.Clear();

        foreach (var item in items)
        {
          Items.Add(item, previouslyChecked.Contains(item));

          if (previouslySelected == item)
          {
            SelectedItem = item;
          }
        }
      });
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      if (e.Index < 0 || e.Index >= Items.Count)
      {
        base.OnDrawItem(e);
        return;
      }

      var checkSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.MixedNormal);
      var dx = (e.Bounds.Height - checkSize.Width) / 2;
      var item = Items[e.Index].ToString()!;

      using (var standardBrush = new SolidBrush(ForeColor))
      {
        var brush = Highlighted.ContainsKey(item) ? Highlighted[item].Brush : standardBrush;

        e.DrawBackground();
        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), GetItemChecked(e.Index) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
        e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height));
      }
    }

    void DoIgnoreItemCheck(Action? action)
    {
      try
      {
        IgnoreItemCheck = true;
        SuspendLayout();
        action?.Invoke();
      }
      finally
      {
        ResumeLayout();
        IgnoreItemCheck = false;
      }
    }

    void FireItemCheckChanged()
    {
      if (ItemCheckChanged != null)
        ItemCheckChanged();
    }

    public override string ToString() => $@"{Name} ({Items.Count})";
  }
}
