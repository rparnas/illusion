namespace Illusion
{
  partial class IllusionCheckedListBox
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.hclb = new Illusion.HighlightableCheckedListBox();
      this.btn_All = new System.Windows.Forms.Button();
      this.btn_None = new System.Windows.Forms.Button();
      this.lbl_Field = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // hclb
      // 
      this.hclb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.hclb.CheckOnClick = true;
      this.hclb.FormattingEnabled = true;
      this.hclb.Location = new System.Drawing.Point(0, 19);
      this.hclb.Name = "hclb";
      this.hclb.Size = new System.Drawing.Size(128, 169);
      this.hclb.TabIndex = 0;
      this.hclb.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.hclb_ItemCheck);
      // 
      // btn_All
      // 
      this.btn_All.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btn_All.Location = new System.Drawing.Point(0, 189);
      this.btn_All.Name = "btn_All";
      this.btn_All.Size = new System.Drawing.Size(50, 23);
      this.btn_All.TabIndex = 1;
      this.btn_All.Text = "All";
      this.btn_All.UseVisualStyleBackColor = true;
      this.btn_All.Click += new System.EventHandler(this.btn_All_Click);
      // 
      // btn_None
      // 
      this.btn_None.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btn_None.Location = new System.Drawing.Point(56, 189);
      this.btn_None.Name = "btn_None";
      this.btn_None.Size = new System.Drawing.Size(50, 23);
      this.btn_None.TabIndex = 2;
      this.btn_None.Text = "None";
      this.btn_None.UseVisualStyleBackColor = true;
      this.btn_None.Click += new System.EventHandler(this.btn_None_Click);
      // 
      // lbl_Field
      // 
      this.lbl_Field.AutoSize = true;
      this.lbl_Field.Location = new System.Drawing.Point(4, 4);
      this.lbl_Field.Name = "lbl_Field";
      this.lbl_Field.Size = new System.Drawing.Size(29, 13);
      this.lbl_Field.TabIndex = 3;
      this.lbl_Field.Text = "Field";
      // 
      // IllusionCheckedListBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lbl_Field);
      this.Controls.Add(this.btn_None);
      this.Controls.Add(this.btn_All);
      this.Controls.Add(this.hclb);
      this.Name = "IllusionCheckedListBox";
      this.Size = new System.Drawing.Size(128, 212);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private HighlightableCheckedListBox hclb;
    private System.Windows.Forms.Button btn_All;
    private System.Windows.Forms.Button btn_None;
    private System.Windows.Forms.Label lbl_Field;
  }
}
