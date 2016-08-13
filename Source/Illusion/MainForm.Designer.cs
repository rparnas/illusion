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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.btn_Load = new System.Windows.Forms.Button();
      this.dgv_Stats = new System.Windows.Forms.DataGridView();
      this.dtp_Start = new System.Windows.Forms.DateTimePicker();
      this.dtp_Stop = new System.Windows.Forms.DateTimePicker();
      this.lbl_Time = new System.Windows.Forms.Label();
      this.tc = new System.Windows.Forms.TabControl();
      this.tpStats = new System.Windows.Forms.TabPage();
      this.tpVisualization = new System.Windows.Forms.TabPage();
      this.lbl_Visualization = new System.Windows.Forms.Label();
      this.pnl_Visualization = new System.Windows.Forms.Panel();
      this.pb_Visualization = new System.Windows.Forms.PictureBox();
      this.cb_Grouping = new System.Windows.Forms.ComboBox();
      this.lbl_to = new System.Windows.Forms.Label();
      this.lbl_Grouping = new System.Windows.Forms.Label();
      this.lbl_Companies = new System.Windows.Forms.Label();
      this.lbl_Projects = new System.Windows.Forms.Label();
      this.lbl_Features = new System.Windows.Forms.Label();
      this.lbl_People = new System.Windows.Forms.Label();
      this.lbl_Activities = new System.Windows.Forms.Label();
      this.iclb_People = new Illusion.IllusionCheckedListBox();
      this.iclb_Companies = new Illusion.IllusionCheckedListBox();
      this.iclb_Activities = new Illusion.IllusionCheckedListBox();
      this.iclb_Features = new Illusion.IllusionCheckedListBox();
      this.iclb_Projects = new Illusion.IllusionCheckedListBox();
      this.cb_IgnoreParenthesis = new System.Windows.Forms.CheckBox();
      this.tp_Overview = new System.Windows.Forms.TabPage();
      this.dgv_Overview = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Stats)).BeginInit();
      this.tc.SuspendLayout();
      this.tpStats.SuspendLayout();
      this.tpVisualization.SuspendLayout();
      this.pnl_Visualization.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pb_Visualization)).BeginInit();
      this.tp_Overview.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Overview)).BeginInit();
      this.SuspendLayout();
      // 
      // btn_Load
      // 
      this.btn_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_Load.Location = new System.Drawing.Point(867, 694);
      this.btn_Load.Name = "btn_Load";
      this.btn_Load.Size = new System.Drawing.Size(65, 23);
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
      this.dgv_Stats.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_Stats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgv_Stats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Stats.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgv_Stats.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv_Stats.Location = new System.Drawing.Point(3, 3);
      this.dgv_Stats.Name = "dgv_Stats";
      this.dgv_Stats.RowHeadersVisible = false;
      this.dgv_Stats.Size = new System.Drawing.Size(446, 644);
      this.dgv_Stats.TabIndex = 4;
      // 
      // dtp_Start
      // 
      this.dtp_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.dtp_Start.CustomFormat = "MM/dd/yy";
      this.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtp_Start.Location = new System.Drawing.Point(45, 697);
      this.dtp_Start.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dtp_Start.Name = "dtp_Start";
      this.dtp_Start.Size = new System.Drawing.Size(84, 20);
      this.dtp_Start.TabIndex = 5;
      this.dtp_Start.ValueChanged += new System.EventHandler(this.dtp_Start_ValueChanged);
      // 
      // dtp_Stop
      // 
      this.dtp_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.dtp_Stop.CustomFormat = "MM/dd/yy";
      this.dtp_Stop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtp_Stop.Location = new System.Drawing.Point(157, 696);
      this.dtp_Stop.Name = "dtp_Stop";
      this.dtp_Stop.Size = new System.Drawing.Size(83, 20);
      this.dtp_Stop.TabIndex = 6;
      this.dtp_Stop.ValueChanged += new System.EventHandler(this.dtp_Stop_ValueChanged);
      // 
      // lbl_Time
      // 
      this.lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbl_Time.AutoSize = true;
      this.lbl_Time.Location = new System.Drawing.Point(9, 699);
      this.lbl_Time.Name = "lbl_Time";
      this.lbl_Time.Size = new System.Drawing.Size(30, 13);
      this.lbl_Time.TabIndex = 7;
      this.lbl_Time.Text = "Time";
      // 
      // tc
      // 
      this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tc.Controls.Add(this.tpStats);
      this.tc.Controls.Add(this.tp_Overview);
      this.tc.Controls.Add(this.tpVisualization);
      this.tc.Location = new System.Drawing.Point(480, 12);
      this.tc.Name = "tc";
      this.tc.SelectedIndex = 0;
      this.tc.Size = new System.Drawing.Size(460, 676);
      this.tc.TabIndex = 9;
      // 
      // tpStats
      // 
      this.tpStats.Controls.Add(this.dgv_Stats);
      this.tpStats.Location = new System.Drawing.Point(4, 22);
      this.tpStats.Name = "tpStats";
      this.tpStats.Padding = new System.Windows.Forms.Padding(3);
      this.tpStats.Size = new System.Drawing.Size(452, 650);
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
      this.tpVisualization.Size = new System.Drawing.Size(452, 650);
      this.tpVisualization.TabIndex = 1;
      this.tpVisualization.Text = "Visualization";
      this.tpVisualization.UseVisualStyleBackColor = true;
      // 
      // lbl_Visualization
      // 
      this.lbl_Visualization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbl_Visualization.AutoSize = true;
      this.lbl_Visualization.Location = new System.Drawing.Point(6, 633);
      this.lbl_Visualization.Name = "lbl_Visualization";
      this.lbl_Visualization.Size = new System.Drawing.Size(22, 13);
      this.lbl_Visualization.TabIndex = 6;
      this.lbl_Visualization.Text = ". . .";
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
      this.pnl_Visualization.Size = new System.Drawing.Size(449, 625);
      this.pnl_Visualization.TabIndex = 1;
      // 
      // pb_Visualization
      // 
      this.pb_Visualization.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pb_Visualization.Location = new System.Drawing.Point(0, 34);
      this.pb_Visualization.Name = "pb_Visualization";
      this.pb_Visualization.Size = new System.Drawing.Size(449, 430);
      this.pb_Visualization.TabIndex = 0;
      this.pb_Visualization.TabStop = false;
      this.pb_Visualization.MouseLeave += new System.EventHandler(this.pb_Visualization_MouseLeave);
      this.pb_Visualization.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_Visualization_MouseMove);
      // 
      // cb_Grouping
      // 
      this.cb_Grouping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cb_Grouping.FormattingEnabled = true;
      this.cb_Grouping.Location = new System.Drawing.Point(537, 699);
      this.cb_Grouping.Name = "cb_Grouping";
      this.cb_Grouping.Size = new System.Drawing.Size(60, 21);
      this.cb_Grouping.TabIndex = 12;
      this.cb_Grouping.SelectedIndexChanged += new System.EventHandler(this.cb_Grouping_SelectedIndexChanged);
      // 
      // lbl_to
      // 
      this.lbl_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbl_to.AutoSize = true;
      this.lbl_to.Location = new System.Drawing.Point(135, 699);
      this.lbl_to.Name = "lbl_to";
      this.lbl_to.Size = new System.Drawing.Size(16, 13);
      this.lbl_to.TabIndex = 13;
      this.lbl_to.Text = "to";
      // 
      // lbl_Grouping
      // 
      this.lbl_Grouping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lbl_Grouping.AutoSize = true;
      this.lbl_Grouping.Location = new System.Drawing.Point(481, 702);
      this.lbl_Grouping.Name = "lbl_Grouping";
      this.lbl_Grouping.Size = new System.Drawing.Size(50, 13);
      this.lbl_Grouping.TabIndex = 14;
      this.lbl_Grouping.Text = "Grouping";
      // 
      // lbl_Companies
      // 
      this.lbl_Companies.AutoSize = true;
      this.lbl_Companies.Location = new System.Drawing.Point(12, 6);
      this.lbl_Companies.Name = "lbl_Companies";
      this.lbl_Companies.Size = new System.Drawing.Size(59, 13);
      this.lbl_Companies.TabIndex = 15;
      this.lbl_Companies.Text = "Companies";
      // 
      // lbl_Projects
      // 
      this.lbl_Projects.AutoSize = true;
      this.lbl_Projects.Location = new System.Drawing.Point(16, 168);
      this.lbl_Projects.Name = "lbl_Projects";
      this.lbl_Projects.Size = new System.Drawing.Size(45, 13);
      this.lbl_Projects.TabIndex = 16;
      this.lbl_Projects.Text = "Projects";
      // 
      // lbl_Features
      // 
      this.lbl_Features.AutoSize = true;
      this.lbl_Features.Location = new System.Drawing.Point(135, 6);
      this.lbl_Features.Name = "lbl_Features";
      this.lbl_Features.Size = new System.Drawing.Size(48, 13);
      this.lbl_Features.TabIndex = 17;
      this.lbl_Features.Text = "Features";
      // 
      // lbl_People
      // 
      this.lbl_People.AutoSize = true;
      this.lbl_People.Location = new System.Drawing.Point(326, 337);
      this.lbl_People.Name = "lbl_People";
      this.lbl_People.Size = new System.Drawing.Size(40, 13);
      this.lbl_People.TabIndex = 18;
      this.lbl_People.Text = "People";
      // 
      // lbl_Activities
      // 
      this.lbl_Activities.AutoSize = true;
      this.lbl_Activities.Location = new System.Drawing.Point(326, 6);
      this.lbl_Activities.Name = "lbl_Activities";
      this.lbl_Activities.Size = new System.Drawing.Size(49, 13);
      this.lbl_Activities.TabIndex = 19;
      this.lbl_Activities.Text = "Activities";
      // 
      // iclb_People
      // 
      this.iclb_People.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_People.CheckOnClick = true;
      this.iclb_People.Location = new System.Drawing.Point(324, 352);
      this.iclb_People.Name = "iclb_People";
      this.iclb_People.Size = new System.Drawing.Size(150, 334);
      this.iclb_People.TabIndex = 11;
      this.iclb_People.ItemCheckChanged += new System.Action(this.DisplayBlocks);
      // 
      // iclb_Companies
      // 
      this.iclb_Companies.CheckOnClick = true;
      this.iclb_Companies.Location = new System.Drawing.Point(12, 22);
      this.iclb_Companies.Name = "iclb_Companies";
      this.iclb_Companies.Size = new System.Drawing.Size(120, 139);
      this.iclb_Companies.TabIndex = 10;
      this.iclb_Companies.ItemCheckChanged += new System.Action(this.DisplayBlocks);
      // 
      // iclb_Activities
      // 
      this.iclb_Activities.CheckOnClick = true;
      this.iclb_Activities.Location = new System.Drawing.Point(324, 23);
      this.iclb_Activities.Name = "iclb_Activities";
      this.iclb_Activities.Size = new System.Drawing.Size(150, 304);
      this.iclb_Activities.TabIndex = 3;
      this.iclb_Activities.ItemCheckChanged += new System.Action(this.DisplayBlocks);
      // 
      // iclb_Features
      // 
      this.iclb_Features.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_Features.CheckOnClick = true;
      this.iclb_Features.Location = new System.Drawing.Point(138, 22);
      this.iclb_Features.Name = "iclb_Features";
      this.iclb_Features.Size = new System.Drawing.Size(180, 664);
      this.iclb_Features.TabIndex = 2;
      this.iclb_Features.ItemCheckChanged += new System.Action(this.DisplayBlocks);
      // 
      // iclb_Projects
      // 
      this.iclb_Projects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.iclb_Projects.CheckOnClick = true;
      this.iclb_Projects.Location = new System.Drawing.Point(12, 185);
      this.iclb_Projects.Name = "iclb_Projects";
      this.iclb_Projects.Size = new System.Drawing.Size(120, 499);
      this.iclb_Projects.TabIndex = 1;
      this.iclb_Projects.ItemCheckChanged += new System.Action(this.DisplayBlocks);
      // 
      // cb_IgnoreParenthesis
      // 
      this.cb_IgnoreParenthesis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.cb_IgnoreParenthesis.AutoSize = true;
      this.cb_IgnoreParenthesis.Location = new System.Drawing.Point(252, 699);
      this.cb_IgnoreParenthesis.Name = "cb_IgnoreParenthesis";
      this.cb_IgnoreParenthesis.Size = new System.Drawing.Size(114, 17);
      this.cb_IgnoreParenthesis.TabIndex = 20;
      this.cb_IgnoreParenthesis.Text = "Ignore Parenthesis";
      this.cb_IgnoreParenthesis.UseVisualStyleBackColor = true;
      this.cb_IgnoreParenthesis.CheckedChanged += new System.EventHandler(this.cb_IgnoreParenthesis_CheckedChanged);
      // 
      // tp_Overview
      // 
      this.tp_Overview.Controls.Add(this.dgv_Overview);
      this.tp_Overview.Location = new System.Drawing.Point(4, 22);
      this.tp_Overview.Name = "tp_Overview";
      this.tp_Overview.Padding = new System.Windows.Forms.Padding(3);
      this.tp_Overview.Size = new System.Drawing.Size(452, 650);
      this.tp_Overview.TabIndex = 2;
      this.tp_Overview.Text = "Overview";
      this.tp_Overview.UseVisualStyleBackColor = true;
      // 
      // dgv_Overview
      // 
      this.dgv_Overview.AllowUserToAddRows = false;
      this.dgv_Overview.AllowUserToDeleteRows = false;
      this.dgv_Overview.AllowUserToResizeRows = false;
      this.dgv_Overview.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_Overview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dgv_Overview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Overview.DefaultCellStyle = dataGridViewCellStyle4;
      this.dgv_Overview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgv_Overview.Location = new System.Drawing.Point(3, 3);
      this.dgv_Overview.Name = "dgv_Overview";
      this.dgv_Overview.RowHeadersVisible = false;
      this.dgv_Overview.Size = new System.Drawing.Size(446, 644);
      this.dgv_Overview.TabIndex = 5;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(944, 729);
      this.Controls.Add(this.cb_IgnoreParenthesis);
      this.Controls.Add(this.lbl_Activities);
      this.Controls.Add(this.lbl_People);
      this.Controls.Add(this.lbl_Features);
      this.Controls.Add(this.lbl_Projects);
      this.Controls.Add(this.lbl_Companies);
      this.Controls.Add(this.lbl_Grouping);
      this.Controls.Add(this.lbl_to);
      this.Controls.Add(this.cb_Grouping);
      this.Controls.Add(this.iclb_People);
      this.Controls.Add(this.iclb_Companies);
      this.Controls.Add(this.lbl_Time);
      this.Controls.Add(this.dtp_Stop);
      this.Controls.Add(this.dtp_Start);
      this.Controls.Add(this.iclb_Activities);
      this.Controls.Add(this.iclb_Features);
      this.Controls.Add(this.iclb_Projects);
      this.Controls.Add(this.btn_Load);
      this.Controls.Add(this.tc);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Illusion";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Stats)).EndInit();
      this.tc.ResumeLayout(false);
      this.tpStats.ResumeLayout(false);
      this.tpVisualization.ResumeLayout(false);
      this.tpVisualization.PerformLayout();
      this.pnl_Visualization.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pb_Visualization)).EndInit();
      this.tp_Overview.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Overview)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btn_Load;
    private IllusionCheckedListBox iclb_Activities;
    private System.Windows.Forms.DataGridView dgv_Stats;
    private System.Windows.Forms.DateTimePicker dtp_Start;
    private System.Windows.Forms.DateTimePicker dtp_Stop;
    private System.Windows.Forms.Label lbl_Time;
    private System.Windows.Forms.TabControl tc;
    private System.Windows.Forms.TabPage tpStats;
    private System.Windows.Forms.TabPage tpVisualization;
    private System.Windows.Forms.PictureBox pb_Visualization;
    private System.Windows.Forms.Panel pnl_Visualization;
    private System.Windows.Forms.Label lbl_Visualization;
    private IllusionCheckedListBox iclb_People;
    private System.Windows.Forms.ComboBox cb_Grouping;
    private System.Windows.Forms.Label lbl_to;
    private System.Windows.Forms.Label lbl_Grouping;
    private IllusionCheckedListBox iclb_Features;
    private IllusionCheckedListBox iclb_Projects;
    private IllusionCheckedListBox iclb_Companies;
    private System.Windows.Forms.Label lbl_Companies;
    private System.Windows.Forms.Label lbl_Projects;
    private System.Windows.Forms.Label lbl_Features;
    private System.Windows.Forms.Label lbl_People;
    private System.Windows.Forms.Label lbl_Activities;
    private System.Windows.Forms.CheckBox cb_IgnoreParenthesis;
    private System.Windows.Forms.TabPage tp_Overview;
    private System.Windows.Forms.DataGridView dgv_Overview;
  }
}

