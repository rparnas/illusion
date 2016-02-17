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
      this.dgv_Stats = new System.Windows.Forms.DataGridView();
      this.dtp_Start = new System.Windows.Forms.DateTimePicker();
      this.dtp_Stop = new System.Windows.Forms.DateTimePicker();
      this.lbl_Start = new System.Windows.Forms.Label();
      this.lbl_Stop = new System.Windows.Forms.Label();
      this.tc = new System.Windows.Forms.TabControl();
      this.tpStats = new System.Windows.Forms.TabPage();
      this.tpVisualization = new System.Windows.Forms.TabPage();
      this.pnl_Visualization = new System.Windows.Forms.Panel();
      this.pb_Visualization = new System.Windows.Forms.PictureBox();
      this.iclb_Activities = new Illusion.IllusionCheckedListBox();
      this.iclb_Features = new Illusion.IllusionCheckedListBox();
      this.iclb_Projects = new Illusion.IllusionCheckedListBox();
      this.lbl_Visualization = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Stats)).BeginInit();
      this.tc.SuspendLayout();
      this.tpStats.SuspendLayout();
      this.tpVisualization.SuspendLayout();
      this.pnl_Visualization.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pb_Visualization)).BeginInit();
      this.SuspendLayout();
      // 
      // btn_Load
      // 
      this.btn_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btn_Load.Location = new System.Drawing.Point(480, 694);
      this.btn_Load.Name = "btn_Load";
      this.btn_Load.Size = new System.Drawing.Size(75, 23);
      this.btn_Load.TabIndex = 0;
      this.btn_Load.Text = "Load...";
      this.btn_Load.UseVisualStyleBackColor = true;
      this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
      // 
      // dgv_Stats
      // 
      this.dgv_Stats.AllowUserToAddRows = false;
      this.dgv_Stats.AllowUserToDeleteRows = false;
      this.dgv_Stats.AllowUserToResizeRows = false;
      this.dgv_Stats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_Stats.ColumnHeadersVisible = false;
      this.dgv_Stats.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv_Stats.Location = new System.Drawing.Point(3, 3);
      this.dgv_Stats.Name = "dgv_Stats";
      this.dgv_Stats.RowHeadersVisible = false;
      this.dgv_Stats.Size = new System.Drawing.Size(510, 644);
      this.dgv_Stats.TabIndex = 4;
      // 
      // dtp_Start
      // 
      this.dtp_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtp_Start.Location = new System.Drawing.Point(626, 697);
      this.dtp_Start.Name = "dtp_Start";
      this.dtp_Start.Size = new System.Drawing.Size(100, 20);
      this.dtp_Start.TabIndex = 5;
      this.dtp_Start.ValueChanged += new System.EventHandler(this.dtp_Start_ValueChanged);
      // 
      // dtp_Stop
      // 
      this.dtp_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.dtp_Stop.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtp_Stop.Location = new System.Drawing.Point(777, 697);
      this.dtp_Stop.Name = "dtp_Stop";
      this.dtp_Stop.Size = new System.Drawing.Size(100, 20);
      this.dtp_Stop.TabIndex = 6;
      this.dtp_Stop.ValueChanged += new System.EventHandler(this.dtp_Stop_ValueChanged);
      // 
      // lbl_Start
      // 
      this.lbl_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbl_Start.AutoSize = true;
      this.lbl_Start.Location = new System.Drawing.Point(591, 699);
      this.lbl_Start.Name = "lbl_Start";
      this.lbl_Start.Size = new System.Drawing.Size(29, 13);
      this.lbl_Start.TabIndex = 7;
      this.lbl_Start.Text = "Start";
      // 
      // lbl_Stop
      // 
      this.lbl_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbl_Stop.AutoSize = true;
      this.lbl_Stop.Location = new System.Drawing.Point(742, 699);
      this.lbl_Stop.Name = "lbl_Stop";
      this.lbl_Stop.Size = new System.Drawing.Size(29, 13);
      this.lbl_Stop.TabIndex = 8;
      this.lbl_Stop.Text = "Stop";
      // 
      // tc
      // 
      this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tc.Controls.Add(this.tpStats);
      this.tc.Controls.Add(this.tpVisualization);
      this.tc.Location = new System.Drawing.Point(480, 12);
      this.tc.Name = "tc";
      this.tc.SelectedIndex = 0;
      this.tc.Size = new System.Drawing.Size(524, 676);
      this.tc.TabIndex = 9;
      // 
      // tpStats
      // 
      this.tpStats.Controls.Add(this.dgv_Stats);
      this.tpStats.Location = new System.Drawing.Point(4, 22);
      this.tpStats.Name = "tpStats";
      this.tpStats.Padding = new System.Windows.Forms.Padding(3);
      this.tpStats.Size = new System.Drawing.Size(516, 650);
      this.tpStats.TabIndex = 0;
      this.tpStats.Text = "Stats";
      this.tpStats.UseVisualStyleBackColor = true;
      // 
      // tpVisualization
      // 
      this.tpVisualization.AutoScroll = true;
      this.tpVisualization.Controls.Add(this.lbl_Visualization);
      this.tpVisualization.Controls.Add(this.pnl_Visualization);
      this.tpVisualization.Location = new System.Drawing.Point(4, 22);
      this.tpVisualization.Name = "tpVisualization";
      this.tpVisualization.Padding = new System.Windows.Forms.Padding(3);
      this.tpVisualization.Size = new System.Drawing.Size(516, 650);
      this.tpVisualization.TabIndex = 1;
      this.tpVisualization.Text = "Visualization";
      this.tpVisualization.UseVisualStyleBackColor = true;
      // 
      // pnl_Visualization
      // 
      this.pnl_Visualization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnl_Visualization.AutoScroll = true;
      this.pnl_Visualization.Controls.Add(this.pb_Visualization);
      this.pnl_Visualization.Location = new System.Drawing.Point(3, 6);
      this.pnl_Visualization.Name = "pnl_Visualization";
      this.pnl_Visualization.Size = new System.Drawing.Size(510, 625);
      this.pnl_Visualization.TabIndex = 1;
      // 
      // pb_Visualization
      // 
      this.pb_Visualization.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pb_Visualization.Location = new System.Drawing.Point(3, 34);
      this.pb_Visualization.Name = "pb_Visualization";
      this.pb_Visualization.Size = new System.Drawing.Size(504, 430);
      this.pb_Visualization.TabIndex = 0;
      this.pb_Visualization.TabStop = false;
      this.pb_Visualization.MouseLeave += new System.EventHandler(this.pb_Visualization_MouseLeave);
      this.pb_Visualization.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_Visualization_MouseMove);
      // 
      // iclb_Activities
      // 
      this.iclb_Activities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_Activities.Label = "Activities";
      this.iclb_Activities.Location = new System.Drawing.Point(324, 12);
      this.iclb_Activities.Name = "iclb_Activities";
      this.iclb_Activities.Size = new System.Drawing.Size(150, 705);
      this.iclb_Activities.TabIndex = 3;
      this.iclb_Activities.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // iclb_Features
      // 
      this.iclb_Features.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_Features.Label = "Features";
      this.iclb_Features.Location = new System.Drawing.Point(168, 12);
      this.iclb_Features.Name = "iclb_Features";
      this.iclb_Features.Size = new System.Drawing.Size(150, 705);
      this.iclb_Features.TabIndex = 2;
      this.iclb_Features.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // iclb_Projects
      // 
      this.iclb_Projects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_Projects.Label = "Projects";
      this.iclb_Projects.Location = new System.Drawing.Point(12, 12);
      this.iclb_Projects.Name = "iclb_Projects";
      this.iclb_Projects.Size = new System.Drawing.Size(150, 705);
      this.iclb_Projects.TabIndex = 1;
      this.iclb_Projects.ItemCheckChanged += new System.Action(this.Setup);
      // 
      // lbl_Visualization
      // 
      this.lbl_Visualization.AutoSize = true;
      this.lbl_Visualization.Location = new System.Drawing.Point(6, 633);
      this.lbl_Visualization.Name = "lbl_Visualization";
      this.lbl_Visualization.Size = new System.Drawing.Size(22, 13);
      this.lbl_Visualization.TabIndex = 6;
      this.lbl_Visualization.Text = ". . .";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 729);
      this.Controls.Add(this.tc);
      this.Controls.Add(this.lbl_Stop);
      this.Controls.Add(this.lbl_Start);
      this.Controls.Add(this.dtp_Stop);
      this.Controls.Add(this.dtp_Start);
      this.Controls.Add(this.iclb_Activities);
      this.Controls.Add(this.iclb_Features);
      this.Controls.Add(this.iclb_Projects);
      this.Controls.Add(this.btn_Load);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Illusion";
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Stats)).EndInit();
      this.tc.ResumeLayout(false);
      this.tpStats.ResumeLayout(false);
      this.tpVisualization.ResumeLayout(false);
      this.tpVisualization.PerformLayout();
      this.pnl_Visualization.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pb_Visualization)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btn_Load;
    private IllusionCheckedListBox iclb_Projects;
    private IllusionCheckedListBox iclb_Features;
    private IllusionCheckedListBox iclb_Activities;
    private System.Windows.Forms.DataGridView dgv_Stats;
    private System.Windows.Forms.DateTimePicker dtp_Start;
    private System.Windows.Forms.DateTimePicker dtp_Stop;
    private System.Windows.Forms.Label lbl_Start;
    private System.Windows.Forms.Label lbl_Stop;
    private System.Windows.Forms.TabControl tc;
    private System.Windows.Forms.TabPage tpStats;
    private System.Windows.Forms.TabPage tpVisualization;
    private System.Windows.Forms.PictureBox pb_Visualization;
    private System.Windows.Forms.Panel pnl_Visualization;
    private System.Windows.Forms.Label lbl_Visualization;
  }
}

