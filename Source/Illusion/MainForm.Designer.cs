﻿namespace Illusion
{
  partial class MainForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.btn_Load = new System.Windows.Forms.Button();
      this.dgv = new System.Windows.Forms.DataGridView();
      this.iclb_Activities = new Illusion.IllusionCheckedListBox();
      this.iclb_Features = new Illusion.IllusionCheckedListBox();
      this.iclb_Projects = new Illusion.IllusionCheckedListBox();
      ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
      this.SuspendLayout();
      // 
      // btn_Load
      // 
      this.btn_Load.Location = new System.Drawing.Point(480, 434);
      this.btn_Load.Name = "btn_Load";
      this.btn_Load.Size = new System.Drawing.Size(75, 23);
      this.btn_Load.TabIndex = 0;
      this.btn_Load.Text = "Load...";
      this.btn_Load.UseVisualStyleBackColor = true;
      this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
      // 
      // dgv
      // 
      this.dgv.AllowUserToAddRows = false;
      this.dgv.AllowUserToDeleteRows = false;
      this.dgv.AllowUserToResizeRows = false;
      this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv.ColumnHeadersVisible = false;
      this.dgv.Location = new System.Drawing.Point(480, 30);
      this.dgv.Name = "dgv";
      this.dgv.RowHeadersVisible = false;
      this.dgv.Size = new System.Drawing.Size(454, 398);
      this.dgv.TabIndex = 4;
      // 
      // iclb_Activities
      // 
      this.iclb_Activities.Label = "Activities";
      this.iclb_Activities.Location = new System.Drawing.Point(324, 12);
      this.iclb_Activities.Name = "iclb_Activities";
      this.iclb_Activities.Size = new System.Drawing.Size(150, 445);
      this.iclb_Activities.TabIndex = 3;
      this.iclb_Activities.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // iclb_Features
      // 
      this.iclb_Features.Label = "Features";
      this.iclb_Features.Location = new System.Drawing.Point(168, 12);
      this.iclb_Features.Name = "iclb_Features";
      this.iclb_Features.Size = new System.Drawing.Size(150, 445);
      this.iclb_Features.TabIndex = 2;
      this.iclb_Features.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // iclb_Projects
      // 
      this.iclb_Projects.Label = "Projects";
      this.iclb_Projects.Location = new System.Drawing.Point(12, 12);
      this.iclb_Projects.Name = "iclb_Projects";
      this.iclb_Projects.Size = new System.Drawing.Size(150, 445);
      this.iclb_Projects.TabIndex = 1;
      this.iclb_Projects.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(946, 469);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.iclb_Activities);
      this.Controls.Add(this.iclb_Features);
      this.Controls.Add(this.iclb_Projects);
      this.Controls.Add(this.btn_Load);
      this.Name = "MainForm";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btn_Load;
    private IllusionCheckedListBox iclb_Projects;
    private IllusionCheckedListBox iclb_Features;
    private IllusionCheckedListBox iclb_Activities;
    private System.Windows.Forms.DataGridView dgv;
  }
}
