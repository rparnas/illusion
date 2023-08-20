namespace Illusion
{
  partial class IllusionForm
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
      DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
      btn_Load = new Button();
      dtp_Start = new DateTimePicker();
      dtp_Stop = new DateTimePicker();
      lbl_Time = new Label();
      cb_Grouping = new ComboBox();
      lbl_to = new Label();
      lbl_Grouping = new Label();
      lbl_Companies = new Label();
      lbl_Projects = new Label();
      lbl_Features = new Label();
      lbl_People = new Label();
      lbl_Activities = new Label();
      cb_CollapseParenthesis = new CheckBox();
      btn_Reload = new Button();
      cb_Time = new ComboBox();
      iclb_People = new Controls.IllusionCheckedListBox();
      iclb_Companies = new Controls.IllusionCheckedListBox();
      iclb_Activities = new Controls.IllusionCheckedListBox();
      iclb_Features = new Controls.IllusionCheckedListBox();
      iclb_Projects = new Controls.IllusionCheckedListBox();
      tpVisualization = new TabPage();
      lbl_Visualization = new Label();
      pnl_Visualization = new Panel();
      pb_Visualization = new PictureBox();
      tpStats = new TabPage();
      dgv_Stats = new DataGridView();
      tc = new TabControl();
      cb_Income = new CheckBox();
      tpVisualization.SuspendLayout();
      pnl_Visualization.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)pb_Visualization).BeginInit();
      tpStats.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)dgv_Stats).BeginInit();
      tc.SuspendLayout();
      SuspendLayout();
      // 
      // btn_Load
      // 
      btn_Load.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      btn_Load.Location = new Point(2842, 1235);
      btn_Load.Margin = new Padding(6, 8, 6, 8);
      btn_Load.Name = "btn_Load";
      btn_Load.Size = new Size(141, 45);
      btn_Load.TabIndex = 0;
      btn_Load.Text = "Load...";
      btn_Load.UseVisualStyleBackColor = true;
      btn_Load.Click += btn_Load_Click;
      // 
      // dtp_Start
      // 
      dtp_Start.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      dtp_Start.CustomFormat = "MM/dd/yy";
      dtp_Start.Format = DateTimePickerFormat.Custom;
      dtp_Start.Location = new Point(95, 1241);
      dtp_Start.Margin = new Padding(6, 8, 6, 8);
      dtp_Start.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      dtp_Start.Name = "dtp_Start";
      dtp_Start.Size = new Size(205, 39);
      dtp_Start.TabIndex = 5;
      dtp_Start.ValueChanged += dtp_Start_ValueChanged;
      // 
      // dtp_Stop
      // 
      dtp_Stop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      dtp_Stop.CustomFormat = "MM/dd/yy";
      dtp_Stop.Format = DateTimePickerFormat.Custom;
      dtp_Stop.Location = new Point(357, 1241);
      dtp_Stop.Margin = new Padding(6, 8, 6, 8);
      dtp_Stop.Name = "dtp_Stop";
      dtp_Stop.Size = new Size(205, 39);
      dtp_Stop.TabIndex = 6;
      dtp_Stop.ValueChanged += dtp_Stop_ValueChanged;
      // 
      // lbl_Time
      // 
      lbl_Time.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Time.AutoSize = true;
      lbl_Time.Location = new Point(20, 1243);
      lbl_Time.Margin = new Padding(6, 0, 6, 0);
      lbl_Time.Name = "lbl_Time";
      lbl_Time.Size = new Size(67, 32);
      lbl_Time.TabIndex = 7;
      lbl_Time.Text = "Time";
      // 
      // cb_Grouping
      // 
      cb_Grouping.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_Grouping.FormattingEnabled = true;
      cb_Grouping.Location = new Point(1172, 1238);
      cb_Grouping.Margin = new Padding(6, 8, 6, 8);
      cb_Grouping.Name = "cb_Grouping";
      cb_Grouping.Size = new Size(132, 40);
      cb_Grouping.TabIndex = 12;
      cb_Grouping.SelectedIndexChanged += cb_Grouping_SelectedIndexChanged;
      // 
      // lbl_to
      // 
      lbl_to.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_to.AutoSize = true;
      lbl_to.Location = new Point(309, 1244);
      lbl_to.Margin = new Padding(6, 0, 6, 0);
      lbl_to.Name = "lbl_to";
      lbl_to.Size = new Size(36, 32);
      lbl_to.TabIndex = 13;
      lbl_to.Text = "to";
      // 
      // lbl_Grouping
      // 
      lbl_Grouping.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Grouping.AutoSize = true;
      lbl_Grouping.Location = new Point(1053, 1241);
      lbl_Grouping.Margin = new Padding(6, 0, 6, 0);
      lbl_Grouping.Name = "lbl_Grouping";
      lbl_Grouping.Size = new Size(114, 32);
      lbl_Grouping.TabIndex = 14;
      lbl_Grouping.Text = "Grouping";
      // 
      // lbl_Companies
      // 
      lbl_Companies.AutoSize = true;
      lbl_Companies.Location = new Point(26, 15);
      lbl_Companies.Margin = new Padding(6, 0, 6, 0);
      lbl_Companies.Name = "lbl_Companies";
      lbl_Companies.Size = new Size(133, 32);
      lbl_Companies.TabIndex = 15;
      lbl_Companies.Text = "Companies";
      // 
      // lbl_Projects
      // 
      lbl_Projects.AutoSize = true;
      lbl_Projects.Location = new Point(26, 374);
      lbl_Projects.Margin = new Padding(6, 0, 6, 0);
      lbl_Projects.Name = "lbl_Projects";
      lbl_Projects.Size = new Size(97, 32);
      lbl_Projects.TabIndex = 16;
      lbl_Projects.Text = "Projects";
      // 
      // lbl_Features
      // 
      lbl_Features.AutoSize = true;
      lbl_Features.Location = new Point(299, 15);
      lbl_Features.Margin = new Padding(6, 0, 6, 0);
      lbl_Features.Name = "lbl_Features";
      lbl_Features.Size = new Size(104, 32);
      lbl_Features.TabIndex = 17;
      lbl_Features.Text = "Features";
      // 
      // lbl_People
      // 
      lbl_People.AutoSize = true;
      lbl_People.Location = new Point(702, 807);
      lbl_People.Margin = new Padding(6, 0, 6, 0);
      lbl_People.Name = "lbl_People";
      lbl_People.Size = new Size(86, 32);
      lbl_People.TabIndex = 18;
      lbl_People.Text = "People";
      // 
      // lbl_Activities
      // 
      lbl_Activities.AutoSize = true;
      lbl_Activities.Location = new Point(702, 15);
      lbl_Activities.Margin = new Padding(6, 0, 6, 0);
      lbl_Activities.Name = "lbl_Activities";
      lbl_Activities.Size = new Size(109, 32);
      lbl_Activities.TabIndex = 19;
      lbl_Activities.Text = "Activities";
      // 
      // cb_CollapseParenthesis
      // 
      cb_CollapseParenthesis.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_CollapseParenthesis.AutoSize = true;
      cb_CollapseParenthesis.Location = new Point(1316, 1240);
      cb_CollapseParenthesis.Margin = new Padding(6, 8, 6, 8);
      cb_CollapseParenthesis.Name = "cb_CollapseParenthesis";
      cb_CollapseParenthesis.Size = new Size(263, 36);
      cb_CollapseParenthesis.TabIndex = 20;
      cb_CollapseParenthesis.Text = "Collapse Parenthesis";
      cb_CollapseParenthesis.UseVisualStyleBackColor = true;
      cb_CollapseParenthesis.CheckedChanged += cb_CollapseParenthesis_CheckedChanged;
      // 
      // btn_Reload
      // 
      btn_Reload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      btn_Reload.Location = new Point(2689, 1235);
      btn_Reload.Margin = new Padding(6, 8, 6, 8);
      btn_Reload.Name = "btn_Reload";
      btn_Reload.Size = new Size(141, 45);
      btn_Reload.TabIndex = 21;
      btn_Reload.Text = "Reload";
      btn_Reload.UseVisualStyleBackColor = true;
      btn_Reload.Click += btn_Reload_Click;
      // 
      // cb_Time
      // 
      cb_Time.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_Time.DropDownStyle = ComboBoxStyle.DropDownList;
      cb_Time.FormattingEnabled = true;
      cb_Time.Location = new Point(574, 1241);
      cb_Time.Margin = new Padding(6, 8, 6, 8);
      cb_Time.Name = "cb_Time";
      cb_Time.Size = new Size(177, 40);
      cb_Time.TabIndex = 22;
      cb_Time.SelectionChangeCommitted += cb_Time_SelectionChangeCommitted;
      // 
      // iclb_People
      // 
      iclb_People.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_People.CheckOnClick = true;
      iclb_People.Location = new Point(702, 847);
      iclb_People.Margin = new Padding(6, 8, 6, 8);
      iclb_People.Name = "iclb_People";
      iclb_People.Size = new Size(320, 364);
      iclb_People.TabIndex = 11;
      iclb_People.ItemCheckChanged += iclb_People_ItemCheckChanged;
      // 
      // iclb_Companies
      // 
      iclb_Companies.CheckOnClick = true;
      iclb_Companies.Location = new Point(26, 54);
      iclb_Companies.Margin = new Padding(6, 8, 6, 8);
      iclb_Companies.Name = "iclb_Companies";
      iclb_Companies.Size = new Size(255, 292);
      iclb_Companies.TabIndex = 10;
      iclb_Companies.ItemCheckChanged += iclb_Companies_ItemCheckChanged;
      // 
      // iclb_Activities
      // 
      iclb_Activities.CheckOnClick = true;
      iclb_Activities.Location = new Point(702, 56);
      iclb_Activities.Margin = new Padding(6, 8, 6, 8);
      iclb_Activities.Name = "iclb_Activities";
      iclb_Activities.Size = new Size(320, 724);
      iclb_Activities.TabIndex = 3;
      iclb_Activities.ItemCheckChanged += iclb_Activities_ItemCheckChanged;
      // 
      // iclb_Features
      // 
      iclb_Features.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_Features.CheckOnClick = true;
      iclb_Features.Location = new Point(299, 55);
      iclb_Features.Margin = new Padding(6, 8, 6, 8);
      iclb_Features.Name = "iclb_Features";
      iclb_Features.Size = new Size(385, 1156);
      iclb_Features.TabIndex = 2;
      iclb_Features.ItemCheckChanged += iclb_Features_ItemCheckChanged;
      // 
      // iclb_Projects
      // 
      iclb_Projects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_Projects.CheckOnClick = true;
      iclb_Projects.Location = new Point(26, 414);
      iclb_Projects.Margin = new Padding(6, 8, 6, 8);
      iclb_Projects.Name = "iclb_Projects";
      iclb_Projects.Size = new Size(255, 796);
      iclb_Projects.TabIndex = 1;
      iclb_Projects.ItemCheckChanged += iclb_Projects_ItemCheckChanged;
      // 
      // tpVisualization
      // 
      tpVisualization.AutoScroll = true;
      tpVisualization.Controls.Add(lbl_Visualization);
      tpVisualization.Controls.Add(pnl_Visualization);
      tpVisualization.Location = new Point(8, 46);
      tpVisualization.Margin = new Padding(6, 8, 6, 8);
      tpVisualization.Name = "tpVisualization";
      tpVisualization.Padding = new Padding(6, 8, 6, 8);
      tpVisualization.Size = new Size(1941, 1144);
      tpVisualization.TabIndex = 1;
      tpVisualization.Text = "Visualization";
      tpVisualization.UseVisualStyleBackColor = true;
      // 
      // lbl_Visualization
      // 
      lbl_Visualization.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Visualization.AutoSize = true;
      lbl_Visualization.Location = new Point(13, 1055);
      lbl_Visualization.Margin = new Padding(6, 0, 6, 0);
      lbl_Visualization.Name = "lbl_Visualization";
      lbl_Visualization.Size = new Size(43, 32);
      lbl_Visualization.TabIndex = 6;
      lbl_Visualization.Text = ". . .";
      // 
      // pnl_Visualization
      // 
      pnl_Visualization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      pnl_Visualization.AutoScroll = true;
      pnl_Visualization.Controls.Add(pb_Visualization);
      pnl_Visualization.Location = new Point(6, 15);
      pnl_Visualization.Margin = new Padding(6, 8, 6, 8);
      pnl_Visualization.Name = "pnl_Visualization";
      pnl_Visualization.Size = new Size(1283, 1036);
      pnl_Visualization.TabIndex = 1;
      // 
      // pb_Visualization
      // 
      pb_Visualization.Anchor = AnchorStyles.Top;
      pb_Visualization.Location = new Point(-1, 83);
      pb_Visualization.Margin = new Padding(6, 8, 6, 8);
      pb_Visualization.Name = "pb_Visualization";
      pb_Visualization.Size = new Size(973, 1059);
      pb_Visualization.TabIndex = 0;
      pb_Visualization.TabStop = false;
      pb_Visualization.MouseLeave += pb_Visualization_MouseLeave;
      pb_Visualization.MouseMove += pb_Visualization_MouseMove;
      // 
      // tpStats
      // 
      tpStats.Controls.Add(dgv_Stats);
      tpStats.Location = new Point(8, 46);
      tpStats.Margin = new Padding(6, 8, 6, 8);
      tpStats.Name = "tpStats";
      tpStats.Padding = new Padding(6, 8, 6, 8);
      tpStats.Size = new Size(1941, 1144);
      tpStats.TabIndex = 0;
      tpStats.Text = "Stats";
      tpStats.UseVisualStyleBackColor = true;
      // 
      // dgv_Stats
      // 
      dgv_Stats.AllowUserToAddRows = false;
      dgv_Stats.AllowUserToDeleteRows = false;
      dgv_Stats.AllowUserToResizeRows = false;
      dgv_Stats.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle3.BackColor = SystemColors.Control;
      dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      dgv_Stats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      dgv_Stats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = SystemColors.Window;
      dataGridViewCellStyle4.Font = new Font("Calibri", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
      dgv_Stats.DefaultCellStyle = dataGridViewCellStyle4;
      dgv_Stats.Dock = DockStyle.Fill;
      dgv_Stats.Location = new Point(6, 8);
      dgv_Stats.Margin = new Padding(6, 8, 6, 8);
      dgv_Stats.Name = "dgv_Stats";
      dgv_Stats.RowHeadersVisible = false;
      dgv_Stats.RowHeadersWidth = 82;
      dgv_Stats.Size = new Size(1929, 1128);
      dgv_Stats.TabIndex = 4;
      dgv_Stats.SortCompare += dgv_SortCompare;
      // 
      // tc
      // 
      tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tc.Controls.Add(tpStats);
      tc.Controls.Add(tpVisualization);
      tc.Location = new Point(1040, 29);
      tc.Margin = new Padding(6, 8, 6, 8);
      tc.Name = "tc";
      tc.SelectedIndex = 0;
      tc.Size = new Size(1957, 1198);
      tc.TabIndex = 9;
      // 
      // cb_Income
      // 
      cb_Income.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_Income.AutoSize = true;
      cb_Income.Location = new Point(1591, 1240);
      cb_Income.Margin = new Padding(6, 8, 6, 8);
      cb_Income.Name = "cb_Income";
      cb_Income.Size = new Size(125, 36);
      cb_Income.TabIndex = 23;
      cb_Income.Text = "Income";
      cb_Income.UseVisualStyleBackColor = true;
      cb_Income.CheckedChanged += cb_Income_CheckedChanged;
      // 
      // IllusionForm
      // 
      AutoScaleDimensions = new SizeF(13F, 32F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(3005, 1292);
      Controls.Add(cb_Income);
      Controls.Add(cb_Time);
      Controls.Add(btn_Reload);
      Controls.Add(cb_CollapseParenthesis);
      Controls.Add(lbl_Activities);
      Controls.Add(lbl_People);
      Controls.Add(lbl_Features);
      Controls.Add(lbl_Projects);
      Controls.Add(lbl_Companies);
      Controls.Add(lbl_Grouping);
      Controls.Add(lbl_to);
      Controls.Add(cb_Grouping);
      Controls.Add(iclb_People);
      Controls.Add(iclb_Companies);
      Controls.Add(lbl_Time);
      Controls.Add(dtp_Stop);
      Controls.Add(dtp_Start);
      Controls.Add(iclb_Activities);
      Controls.Add(iclb_Features);
      Controls.Add(iclb_Projects);
      Controls.Add(btn_Load);
      Controls.Add(tc);
      Margin = new Padding(6, 8, 6, 8);
      Name = "IllusionForm";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Illusion";
      tpVisualization.ResumeLayout(false);
      tpVisualization.PerformLayout();
      pnl_Visualization.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)pb_Visualization).EndInit();
      tpStats.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)dgv_Stats).EndInit();
      tc.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button btn_Load;
    private Illusion.Controls.IllusionCheckedListBox iclb_Activities;
    private DateTimePicker dtp_Start;
    private DateTimePicker dtp_Stop;
    private Label lbl_Time;
    private Illusion.Controls.IllusionCheckedListBox iclb_People;
    private ComboBox cb_Grouping;
    private Label lbl_to;
    private Label lbl_Grouping;
    private Illusion.Controls.IllusionCheckedListBox iclb_Features;
    private Illusion.Controls.IllusionCheckedListBox iclb_Projects;
    private Illusion.Controls.IllusionCheckedListBox iclb_Companies;
    private Label lbl_Companies;
    private Label lbl_Projects;
    private Label lbl_Features;
    private Label lbl_People;
    private Label lbl_Activities;
    private CheckBox cb_CollapseParenthesis;
    private Button btn_Reload;
    private ComboBox cb_Time;
    private TabPage tpVisualization;
    private Label lbl_Visualization;
    private Panel pnl_Visualization;
    private PictureBox pb_Visualization;
    private TabPage tpStats;
    private DataGridView dgv_Stats;
    private TabControl tc;
    private CheckBox cb_Income;
  }
}

