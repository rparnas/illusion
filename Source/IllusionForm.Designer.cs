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
      DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IllusionForm));
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
      btn_Load.Location = new Point(3594, 1852);
      btn_Load.Margin = new Padding(9, 12, 9, 12);
      btn_Load.Name = "btn_Load";
      btn_Load.Size = new Size(217, 68);
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
      dtp_Start.Location = new Point(146, 1862);
      dtp_Start.Margin = new Padding(9, 12, 9, 12);
      dtp_Start.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      dtp_Start.Name = "dtp_Start";
      dtp_Start.Size = new Size(313, 55);
      dtp_Start.TabIndex = 5;
      dtp_Start.ValueChanged += dtp_Start_ValueChanged;
      // 
      // dtp_Stop
      // 
      dtp_Stop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      dtp_Stop.CustomFormat = "MM/dd/yy";
      dtp_Stop.Format = DateTimePickerFormat.Custom;
      dtp_Stop.Location = new Point(549, 1862);
      dtp_Stop.Margin = new Padding(9, 12, 9, 12);
      dtp_Stop.Name = "dtp_Stop";
      dtp_Stop.Size = new Size(313, 55);
      dtp_Stop.TabIndex = 6;
      dtp_Stop.ValueChanged += dtp_Stop_ValueChanged;
      // 
      // lbl_Time
      // 
      lbl_Time.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Time.AutoSize = true;
      lbl_Time.Location = new Point(31, 1864);
      lbl_Time.Margin = new Padding(9, 0, 9, 0);
      lbl_Time.Name = "lbl_Time";
      lbl_Time.Size = new Size(98, 48);
      lbl_Time.TabIndex = 7;
      lbl_Time.Text = "Time";
      // 
      // cb_Grouping
      // 
      cb_Grouping.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_Grouping.FormattingEnabled = true;
      cb_Grouping.Location = new Point(1803, 1857);
      cb_Grouping.Margin = new Padding(9, 12, 9, 12);
      cb_Grouping.Name = "cb_Grouping";
      cb_Grouping.Size = new Size(201, 56);
      cb_Grouping.TabIndex = 12;
      cb_Grouping.SelectedIndexChanged += cb_Grouping_SelectedIndexChanged;
      // 
      // lbl_to
      // 
      lbl_to.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_to.AutoSize = true;
      lbl_to.Location = new Point(475, 1866);
      lbl_to.Margin = new Padding(9, 0, 9, 0);
      lbl_to.Name = "lbl_to";
      lbl_to.Size = new Size(53, 48);
      lbl_to.TabIndex = 13;
      lbl_to.Text = "to";
      // 
      // lbl_Grouping
      // 
      lbl_Grouping.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Grouping.AutoSize = true;
      lbl_Grouping.Location = new Point(1620, 1862);
      lbl_Grouping.Margin = new Padding(9, 0, 9, 0);
      lbl_Grouping.Name = "lbl_Grouping";
      lbl_Grouping.Size = new Size(170, 48);
      lbl_Grouping.TabIndex = 14;
      lbl_Grouping.Text = "Grouping";
      // 
      // lbl_Companies
      // 
      lbl_Companies.AutoSize = true;
      lbl_Companies.Location = new Point(40, 22);
      lbl_Companies.Margin = new Padding(9, 0, 9, 0);
      lbl_Companies.Name = "lbl_Companies";
      lbl_Companies.Size = new Size(196, 48);
      lbl_Companies.TabIndex = 15;
      lbl_Companies.Text = "Companies";
      // 
      // lbl_Projects
      // 
      lbl_Projects.AutoSize = true;
      lbl_Projects.Location = new Point(40, 561);
      lbl_Projects.Margin = new Padding(9, 0, 9, 0);
      lbl_Projects.Name = "lbl_Projects";
      lbl_Projects.Size = new Size(146, 48);
      lbl_Projects.TabIndex = 16;
      lbl_Projects.Text = "Projects";
      // 
      // lbl_Features
      // 
      lbl_Features.AutoSize = true;
      lbl_Features.Location = new Point(460, 22);
      lbl_Features.Margin = new Padding(9, 0, 9, 0);
      lbl_Features.Name = "lbl_Features";
      lbl_Features.Size = new Size(154, 48);
      lbl_Features.TabIndex = 17;
      lbl_Features.Text = "Features";
      // 
      // lbl_People
      // 
      lbl_People.AutoSize = true;
      lbl_People.Location = new Point(1080, 1210);
      lbl_People.Margin = new Padding(9, 0, 9, 0);
      lbl_People.Name = "lbl_People";
      lbl_People.Size = new Size(128, 48);
      lbl_People.TabIndex = 18;
      lbl_People.Text = "People";
      // 
      // lbl_Activities
      // 
      lbl_Activities.AutoSize = true;
      lbl_Activities.Location = new Point(1080, 22);
      lbl_Activities.Margin = new Padding(9, 0, 9, 0);
      lbl_Activities.Name = "lbl_Activities";
      lbl_Activities.Size = new Size(162, 48);
      lbl_Activities.TabIndex = 19;
      lbl_Activities.Text = "Activities";
      // 
      // cb_CollapseParenthesis
      // 
      cb_CollapseParenthesis.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_CollapseParenthesis.AutoSize = true;
      cb_CollapseParenthesis.Location = new Point(2025, 1862);
      cb_CollapseParenthesis.Margin = new Padding(9, 12, 9, 12);
      cb_CollapseParenthesis.Name = "cb_CollapseParenthesis";
      cb_CollapseParenthesis.Size = new Size(389, 52);
      cb_CollapseParenthesis.TabIndex = 20;
      cb_CollapseParenthesis.Text = "Collapse Parenthesis";
      cb_CollapseParenthesis.UseVisualStyleBackColor = true;
      cb_CollapseParenthesis.CheckedChanged += cb_CollapseParenthesis_CheckedChanged;
      // 
      // btn_Reload
      // 
      btn_Reload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      btn_Reload.Location = new Point(3358, 1852);
      btn_Reload.Margin = new Padding(9, 12, 9, 12);
      btn_Reload.Name = "btn_Reload";
      btn_Reload.Size = new Size(217, 68);
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
      cb_Time.Location = new Point(883, 1862);
      cb_Time.Margin = new Padding(9, 12, 9, 12);
      cb_Time.Name = "cb_Time";
      cb_Time.Size = new Size(270, 56);
      cb_Time.TabIndex = 22;
      cb_Time.SelectionChangeCommitted += cb_Time_SelectionChangeCommitted;
      // 
      // iclb_People
      // 
      iclb_People.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_People.CheckOnClick = true;
      iclb_People.Location = new Point(1080, 1270);
      iclb_People.Margin = new Padding(9, 12, 9, 12);
      iclb_People.Name = "iclb_People";
      iclb_People.Size = new Size(490, 544);
      iclb_People.TabIndex = 11;
      iclb_People.ItemCheckChanged += iclb_People_ItemCheckChanged;
      // 
      // iclb_Companies
      // 
      iclb_Companies.CheckOnClick = true;
      iclb_Companies.Location = new Point(40, 81);
      iclb_Companies.Margin = new Padding(9, 12, 9, 12);
      iclb_Companies.Name = "iclb_Companies";
      iclb_Companies.Size = new Size(390, 436);
      iclb_Companies.TabIndex = 10;
      iclb_Companies.ItemCheckChanged += iclb_Companies_ItemCheckChanged;
      // 
      // iclb_Activities
      // 
      iclb_Activities.CheckOnClick = true;
      iclb_Activities.Location = new Point(1080, 84);
      iclb_Activities.Margin = new Padding(9, 12, 9, 12);
      iclb_Activities.Name = "iclb_Activities";
      iclb_Activities.Size = new Size(490, 1084);
      iclb_Activities.TabIndex = 3;
      iclb_Activities.ItemCheckChanged += iclb_Activities_ItemCheckChanged;
      // 
      // iclb_Features
      // 
      iclb_Features.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_Features.CheckOnClick = true;
      iclb_Features.Location = new Point(460, 82);
      iclb_Features.Margin = new Padding(9, 12, 9, 12);
      iclb_Features.Name = "iclb_Features";
      iclb_Features.Size = new Size(590, 1732);
      iclb_Features.TabIndex = 2;
      iclb_Features.ItemCheckChanged += iclb_Features_ItemCheckChanged;
      // 
      // iclb_Projects
      // 
      iclb_Projects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      iclb_Projects.CheckOnClick = true;
      iclb_Projects.Location = new Point(40, 621);
      iclb_Projects.Margin = new Padding(9, 12, 9, 12);
      iclb_Projects.Name = "iclb_Projects";
      iclb_Projects.Size = new Size(390, 1192);
      iclb_Projects.TabIndex = 1;
      iclb_Projects.ItemCheckChanged += iclb_Projects_ItemCheckChanged;
      // 
      // tpVisualization
      // 
      tpVisualization.AutoScroll = true;
      tpVisualization.Controls.Add(lbl_Visualization);
      tpVisualization.Controls.Add(pnl_Visualization);
      tpVisualization.Location = new Point(12, 69);
      tpVisualization.Margin = new Padding(9, 12, 9, 12);
      tpVisualization.Name = "tpVisualization";
      tpVisualization.Padding = new Padding(9, 12, 9, 12);
      tpVisualization.Size = new Size(2987, 1716);
      tpVisualization.TabIndex = 1;
      tpVisualization.Text = "Visualization";
      tpVisualization.UseVisualStyleBackColor = true;
      // 
      // lbl_Visualization
      // 
      lbl_Visualization.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      lbl_Visualization.AutoSize = true;
      lbl_Visualization.Location = new Point(20, 1582);
      lbl_Visualization.Margin = new Padding(9, 0, 9, 0);
      lbl_Visualization.Name = "lbl_Visualization";
      lbl_Visualization.Size = new Size(64, 48);
      lbl_Visualization.TabIndex = 6;
      lbl_Visualization.Text = ". . .";
      // 
      // pnl_Visualization
      // 
      pnl_Visualization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      pnl_Visualization.AutoScroll = true;
      pnl_Visualization.Controls.Add(pb_Visualization);
      pnl_Visualization.Location = new Point(9, 22);
      pnl_Visualization.Margin = new Padding(9, 12, 9, 12);
      pnl_Visualization.Name = "pnl_Visualization";
      pnl_Visualization.Size = new Size(1974, 1554);
      pnl_Visualization.TabIndex = 1;
      // 
      // pb_Visualization
      // 
      pb_Visualization.Anchor = AnchorStyles.Top;
      pb_Visualization.Location = new Point(-40, 124);
      pb_Visualization.Margin = new Padding(9, 12, 9, 12);
      pb_Visualization.Name = "pb_Visualization";
      pb_Visualization.Size = new Size(1497, 1588);
      pb_Visualization.TabIndex = 0;
      pb_Visualization.TabStop = false;
      pb_Visualization.MouseLeave += pb_Visualization_MouseLeave;
      pb_Visualization.MouseMove += pb_Visualization_MouseMove;
      // 
      // tpStats
      // 
      tpStats.Controls.Add(dgv_Stats);
      tpStats.Location = new Point(12, 69);
      tpStats.Margin = new Padding(9, 12, 9, 12);
      tpStats.Name = "tpStats";
      tpStats.Padding = new Padding(9, 12, 9, 12);
      tpStats.Size = new Size(2208, 1716);
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
      dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = SystemColors.Control;
      dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      dgv_Stats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      dgv_Stats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = SystemColors.Window;
      dataGridViewCellStyle2.Font = new Font("Calibri", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      dgv_Stats.DefaultCellStyle = dataGridViewCellStyle2;
      dgv_Stats.Dock = DockStyle.Fill;
      dgv_Stats.Location = new Point(9, 12);
      dgv_Stats.Margin = new Padding(9, 12, 9, 12);
      dgv_Stats.Name = "dgv_Stats";
      dgv_Stats.RowHeadersVisible = false;
      dgv_Stats.RowHeadersWidth = 82;
      dgv_Stats.Size = new Size(2190, 1692);
      dgv_Stats.TabIndex = 4;
      dgv_Stats.SortCompare += dgv_SortCompare;
      // 
      // tc
      // 
      tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tc.Controls.Add(tpStats);
      tc.Controls.Add(tpVisualization);
      tc.Location = new Point(1600, 44);
      tc.Margin = new Padding(9, 12, 9, 12);
      tc.Name = "tc";
      tc.SelectedIndex = 0;
      tc.Size = new Size(2232, 1797);
      tc.TabIndex = 9;
      // 
      // cb_Income
      // 
      cb_Income.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      cb_Income.AutoSize = true;
      cb_Income.Location = new Point(2448, 1862);
      cb_Income.Margin = new Padding(9, 12, 9, 12);
      cb_Income.Name = "cb_Income";
      cb_Income.Size = new Size(184, 52);
      cb_Income.TabIndex = 23;
      cb_Income.Text = "Income";
      cb_Income.UseVisualStyleBackColor = true;
      cb_Income.CheckedChanged += cb_Income_CheckedChanged;
      // 
      // IllusionForm
      // 
      AutoScaleDimensions = new SizeF(20F, 48F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(3844, 1938);
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
      Icon = (Icon)resources.GetObject("$this.Icon");
      Margin = new Padding(9, 12, 9, 12);
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

