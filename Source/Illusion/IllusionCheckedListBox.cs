using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        foreach (var item in clb.CheckedItems)
        {
          ret.Add(item.ToString());
        }
        return ret;
      }
    }

    public List<string> SelectedItems
    {
      get
      {
        var ret = new List<string>();
        foreach (var item in clb.SelectedItems)
        {
          ret.Add(item.ToString());
        }
        return ret;
      }
    }

    public IllusionCheckedListBox()
    {
      InitializeComponent();
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
      var previouslySelected = SelectedItems;

      clb.Items.Clear();
      foreach (var item in items)
      {
        clb.Items.Add(item, previouslyChecked.Contains(item));
        if (previouslySelected.Contains(item))
        {
          clb.SelectedItems.Add(item);
        }
      }

      IgnoreItemCheck = false;
    }

    void btn_All_Click(object sender, EventArgs e)
    {
      IgnoreItemCheck = true;
      for (int i = 0; i < clb.Items.Count; i++)
        clb.SetItemChecked(i, true);
      IgnoreItemCheck = false;
      FireItemCheckChanged();
    }

    void btn_None_Click(object sender, EventArgs e)
    {
      IgnoreItemCheck = true;
      for (int i = 0; i < clb.Items.Count; i++)
        clb.SetItemChecked(i, false);
      IgnoreItemCheck = false;
      FireItemCheckChanged();
    }

    void clb_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!IgnoreItemCheck)
      {
        BeginInvoke((MethodInvoker)(() => FireItemCheckChanged()));
      }
    }
  }
}
