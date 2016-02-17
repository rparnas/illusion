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
  public partial class IllusionCheckedListBox : UserControl
  {
    public event Action ItemCheckChanged;
    bool IgnoreItemCheck = false;

    public string Label
    {
      get { return lbl_Field.Text; }
      set { lbl_Field.Text = value; }
    }

    public List<string> CheckedItems
    {
      get
      {
        var ret = new List<string>();
        foreach (var item in hclb.CheckedItems)
        {
          ret.Add(item.ToString());
        }
        return ret;
      }
    }

    public IllusionCheckedListBox()
    {
      InitializeComponent();
      hclb.ItemHighlightChanged += FireItemCheckChanged;
    }

    void FireItemCheckChanged()
    {
      if (ItemCheckChanged != null)
        ItemCheckChanged();
    }

    public void SetItems(List<string> items)
    {
      IgnoreItemCheck = true;

      var previouslyChecked = CheckedItems;
      var previouslySelected = (hclb.SelectedItem ?? "").ToString();

      hclb.Items.Clear();
      foreach (var item in items)
      {
        hclb.Items.Add(item, previouslyChecked.Contains(item));
        if (previouslySelected == item)
        {
          hclb.SelectedItem = item;
        }
      }

      IgnoreItemCheck = false;
    }

    void btn_All_Click(object sender, EventArgs e)
    {
      IgnoreItemCheck = true;
      for (int i = 0; i < hclb.Items.Count; i++)
        hclb.SetItemChecked(i, true);
      IgnoreItemCheck = false;
      FireItemCheckChanged();
    }

    void btn_None_Click(object sender, EventArgs e)
    {
      IgnoreItemCheck = true;
      for (int i = 0; i < hclb.Items.Count; i++)
        hclb.SetItemChecked(i, false);
      IgnoreItemCheck = false;
      FireItemCheckChanged();
    }

    void hclb_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!IgnoreItemCheck)
      {
        BeginInvoke((MethodInvoker)(() => FireItemCheckChanged()));
      }
    }

    public Brush GetHighlight(string item)
    {
      return hclb.Highlighted.ContainsKey(item) ? hclb.Highlighted[item].Brush : null;
    }
  }

  class HighlightableCheckedListBox : CheckedListBox
  {
    public event Action ItemHighlightChanged;

    public Dictionary<string, Highlight> Highlighted;

    public HighlightableCheckedListBox()
    {
      DoubleBuffered = true;
      Highlighted = new Dictionary<string, Highlight>();

      MouseUp += hclb_MouseUp;
    }

    void hclb_MouseUp(object sender, MouseEventArgs e)
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
      foreach (var highlight in MainForm.Highlights)
      {
        var h = highlight;
        var isChecked = Highlighted.ContainsKey(item) && Highlighted[item] == h;

        cm.MenuItems.Add(new MenuItem(h.Name, (sender2, e2) =>
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
          if (ItemHighlightChanged != null)
          {
            ItemHighlightChanged();
          }
        })
        { Checked = isChecked });
      }
      cm.Show(this, e.Location);
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
  }
}
