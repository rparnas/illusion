using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Illusion
{
  public class IllusionCheckedListBox : CheckedListBox
  {
    public List<IllusionCheckedListBox> Forward;
    Dictionary<string, Highlight> Highlighted;
    bool IgnoreItemCheck;
    public event Action ItemCheckChanged;

    public HashSet<string> AllCheckedItems
    {
      get
      {
        var ret = new HashSet<string>();
        foreach (var item in CheckedItems)
        {
          ret.Add(item.ToString());
        }
        return ret;
      }
    }

    public IllusionCheckedListBox()
    {
      Forward = new List<IllusionCheckedListBox>();
      Highlighted = new Dictionary<string, Highlight>();
      IgnoreItemCheck = false;

      CheckOnClick = true;
      DoubleBuffered = true;
      ItemCheck += IllusionCheckedListBox_ItemCheck;
      MouseUp += IllusionCheckedListBox_MouseUp;
    }

    void FireItemCheckChanged()
    {
      if (ItemCheckChanged != null)
        ItemCheckChanged();
    }

    public Brush GetHighlight(IEnumerable<string> items)
    {
      foreach (var item in items)
      {
        if (Highlighted.ContainsKey(item))
        {
          return Highlighted[item].Brush;
        }
      }
      return null;
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
      var item = Items[e.Index].ToString();

      using (var standardBrush = new SolidBrush(ForeColor))
      {
        var brush = Highlighted.ContainsKey(item) ? Highlighted[item].Brush : standardBrush;

        e.DrawBackground();
        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), GetItemChecked(e.Index) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
        e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height));
      }
    }

    public void SelectAllForward()
    {
      foreach (var iclb in Forward)
      {
        iclb.SelectAll(true);
      }
    }

    public void SelectAll(bool value)
    {
      IgnoreItemCheck = true;
      for (int i = 0; i < Items.Count; i++)
        SetItemChecked(i, value);
      IgnoreItemCheck = false;
      FireItemCheckChanged();
    }

    public void SetItems(List<string> items)
    {
      IgnoreItemCheck = true;

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

      IgnoreItemCheck = false;
    }

    void IllusionCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!IgnoreItemCheck)
      {
        BeginInvoke((MethodInvoker)(() => FireItemCheckChanged()));
      }
    }

    void IllusionCheckedListBox_MouseUp(object sender, MouseEventArgs e)
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

      var item = (SelectedItem ?? "").ToString();

      var cm = new ContextMenu();

      // Add Select All, None controls.
      cm.MenuItems.Add(new MenuItem("Select All", (s2, e2) => SelectAll(true)));
      cm.MenuItems.Add(new MenuItem("Deselect All", (s2, e2) => SelectAll(false)));
      cm.MenuItems.Add(new MenuItem("Select Forward", (s2, e2) =>
      {
        SetItemChecked(SelectedIndex, true);
        FireItemCheckChanged();
        SelectAllForward();
      }) { Enabled = SelectedItem != null });
      cm.MenuItems.Add(new MenuItem("-"));

      // Highlighting
      foreach (var highlight in MainForm.Highlights)
      {
        var h = highlight;
        var isChecked = Highlighted.ContainsKey(item) && Highlighted[item] == h;

        cm.MenuItems.Add(new MenuItem(h.Name, (s2, e2) =>
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
        { Checked = isChecked });
      }
      cm.Show(this, e.Location);
    }
  }
}
