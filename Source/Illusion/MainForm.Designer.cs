namespace Illusion
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
      this.dtp_Start = new System.Windows.Forms.DateTimePicker();
      this.dtp_Stop = new System.Windows.Forms.DateTimePicker();
      this.lbl_Start = new System.Windows.Forms.Label();
      this.lbl_Stop = new System.Windows.Forms.Label();
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
      // dtp_Start
      // 
      this.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtp_Start.Location = new System.Drawing.Point(626, 437);
      this.dtp_Start.Name = "dtp_Start";
      this.dtp_Start.Size = new System.Drawing.Size(100, 20);
      this.dtp_Start.TabIndex = 5;
      this.dtp_Start.ValueChanged += new System.EventHandler(this.dtp_Start_ValueChanged);
      // 
      // dtp_Stop
      // 
      this.dtp_Stop.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtp_Stop.Location = new System.Drawing.Point(777, 437);
      this.dtp_Stop.Name = "dtp_Stop";
      this.dtp_Stop.Size = new System.Drawing.Size(100, 20);
      this.dtp_Stop.TabIndex = 6;
      this.dtp_Stop.ValueChanged += new System.EventHandler(this.dtp_Stop_ValueChanged);
      // 
      // lbl_Start
      // 
      this.lbl_Start.AutoSize = true;
      this.lbl_Start.Location = new System.Drawing.Point(591, 439);
      this.lbl_Start.Name = "lbl_Start";
      this.lbl_Start.Size = new System.Drawing.Size(29, 13);
      this.lbl_Start.TabIndex = 7;
      this.lbl_Start.Text = "Start";
      // 
      // lbl_Stop
      // 
      this.lbl_Stop.AutoSize = true;
      this.lbl_Stop.Location = new System.Drawing.Point(742, 439);
      this.lbl_Stop.Name = "lbl_Stop";
      this.lbl_Stop.Size = new System.Drawing.Size(29, 13);
      this.lbl_Stop.TabIndex = 8;
      this.lbl_Stop.Text = "Stop";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(946, 469);
      this.Controls.Add(this.lbl_Stop);
      this.Controls.Add(this.lbl_Start);
      this.Controls.Add(this.dtp_Stop);
      this.Controls.Add(this.dtp_Start);
      this.Controls.Add(this.dgv);
      this.Controls.Add(this.iclb_Activities);
      this.Controls.Add(this.iclb_Features);
      this.Controls.Add(this.iclb_Projects);
      this.Controls.Add(this.btn_Load);
      this.Name = "MainForm";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btn_Load;
    private IllusionCheckedListBox iclb_Projects;
    private IllusionCheckedListBox iclb_Features;
    private IllusionCheckedListBox iclb_Activities;
    private System.Windows.Forms.DataGridView dgv;
    private System.Windows.Forms.DateTimePicker dtp_Start;
    private System.Windows.Forms.DateTimePicker dtp_Stop;
    private System.Windows.Forms.Label lbl_Start;
    private System.Windows.Forms.Label lbl_Stop;
  }
}

