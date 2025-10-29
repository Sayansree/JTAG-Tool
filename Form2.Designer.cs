using System.Deployment.Internal;

namespace WindowsFormsApp1
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.pin_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pin_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cellsIOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state_w = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state_r = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.set_high = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.set_low = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.set_z = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.in_val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.out_val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctrl_val = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(601, 435);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Watch PINS";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(492, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 23);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Real Time Write\r\n";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(401, 14);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Write Changes";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pin_name,
            this.pin_type,
            this.cellsIOC,
            this.state_w,
            this.state_r,
            this.set_high,
            this.set_low,
            this.set_z,
            this.in_val,
            this.out_val,
            this.ctrl_val});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(10, 43);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.Size = new System.Drawing.Size(584, 379);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // pin_name
            // 
            this.pin_name.Frozen = true;
            this.pin_name.HeaderText = "PIN Name";
            this.pin_name.Name = "pin_name";
            this.pin_name.ReadOnly = true;
            this.pin_name.Width = 90;
            // 
            // pin_type
            // 
            this.pin_type.Frozen = true;
            this.pin_type.HeaderText = "Type";
            this.pin_type.Name = "pin_type";
            this.pin_type.ReadOnly = true;
            this.pin_type.Width = 60;
            // 
            // cellsIOC
            // 
            this.cellsIOC.HeaderText = "Cells (I,O,C)";
            this.cellsIOC.Name = "cellsIOC";
            this.cellsIOC.ReadOnly = true;
            // 
            // state_w
            // 
            this.state_w.HeaderText = "State (W)";
            this.state_w.Name = "state_w";
            this.state_w.ReadOnly = true;
            this.state_w.Width = 50;
            // 
            // state_r
            // 
            this.state_r.HeaderText = "State (R)";
            this.state_r.Name = "state_r";
            this.state_r.ReadOnly = true;
            this.state_r.Width = 50;
            // 
            // set_high
            // 
            this.set_high.HeaderText = "set HIGH";
            this.set_high.Name = "set_high";
            this.set_high.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.set_high.Width = 35;
            // 
            // set_low
            // 
            this.set_low.HeaderText = "Set LOW";
            this.set_low.Name = "set_low";
            this.set_low.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.set_low.Width = 35;
            // 
            // set_z
            // 
            this.set_z.HeaderText = "Set HI-Z";
            this.set_z.Name = "set_z";
            this.set_z.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.set_z.Width = 35;
            // 
            // in_val
            // 
            this.in_val.HeaderText = "IN";
            this.in_val.MinimumWidth = 40;
            this.in_val.Name = "in_val";
            this.in_val.ReadOnly = true;
            this.in_val.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.in_val.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.in_val.Width = 40;
            // 
            // out_val
            // 
            this.out_val.HeaderText = "OUT";
            this.out_val.MinimumWidth = 40;
            this.out_val.Name = "out_val";
            this.out_val.ReadOnly = true;
            this.out_val.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.out_val.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.out_val.Width = 40;
            // 
            // ctrl_val
            // 
            this.ctrl_val.HeaderText = "CTRL";
            this.ctrl_val.MinimumWidth = 40;
            this.ctrl_val.Name = "ctrl_val";
            this.ctrl_val.ReadOnly = true;
            this.ctrl_val.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ctrl_val.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ctrl_val.Width = 40;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(221, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Run Sampling";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Single Capture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Sample);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sampling Interval (ms)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "50",
            "100",
            "200",
            "300",
            "500",
            "1000",
            "2000",
            "5000",
            "10000"});
            this.comboBox1.Location = new System.Drawing.Point(127, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(85, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Sample);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 452);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form2";
            this.Text = "PIN Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }
        private Form1 parent;
        private Device device;
        private bool InternalChange=false;
        const byte Z = 2;
        const byte HIGH = 1;
        const byte LOW = 0;
        bool sampling = false;
        const int INPUT = -1;
        const int OUTPUT = 1;
        const int BIDIR = 0;
        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pin_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn pin_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn cellsIOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn state_w;
        private System.Windows.Forms.DataGridViewTextBoxColumn state_r;
        private System.Windows.Forms.DataGridViewCheckBoxColumn set_high;
        private System.Windows.Forms.DataGridViewCheckBoxColumn set_low;
        private System.Windows.Forms.DataGridViewCheckBoxColumn set_z;
        private System.Windows.Forms.DataGridViewTextBoxColumn in_val;
        private System.Windows.Forms.DataGridViewTextBoxColumn out_val;
        private System.Windows.Forms.DataGridViewTextBoxColumn ctrl_val;
    }
}