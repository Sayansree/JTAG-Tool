using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class AddNewBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label_BoardFileName;
        private System.Windows.Forms.TextBox textBoxBoardFileName;
        private System.Windows.Forms.Label label_NumberOfDevices;
        private System.Windows.Forms.NumericUpDown numericUpDownDevices;
        private System.Windows.Forms.DataGridView dataGridView_Board;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button saveButton;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewBoard));
            this.label_BoardFileName = new System.Windows.Forms.Label();
            this.textBoxBoardFileName = new System.Windows.Forms.TextBox();
            this.label_NumberOfDevices = new System.Windows.Forms.Label();
            this.numericUpDownDevices = new System.Windows.Forms.NumericUpDown();
            this.dataGridView_Board = new System.Windows.Forms.DataGridView();
            this.ChainID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Controller = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.saveButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_loadboard = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_addboard = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Board)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_BoardFileName
            // 
            this.label_BoardFileName.AutoSize = true;
            this.label_BoardFileName.Location = new System.Drawing.Point(5, 18);
            this.label_BoardFileName.Name = "label_BoardFileName";
            this.label_BoardFileName.Size = new System.Drawing.Size(88, 13);
            this.label_BoardFileName.TabIndex = 0;
            this.label_BoardFileName.Text = "Board File Name:";
            // 
            // textBoxBoardFileName
            // 
            this.textBoxBoardFileName.Location = new System.Drawing.Point(97, 14);
            this.textBoxBoardFileName.Name = "textBoxBoardFileName";
            this.textBoxBoardFileName.Size = new System.Drawing.Size(241, 20);
            this.textBoxBoardFileName.TabIndex = 1;
            this.textBoxBoardFileName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxBoardFileName_Validating);
            // 
            // label_NumberOfDevices
            // 
            this.label_NumberOfDevices.AutoSize = true;
            this.label_NumberOfDevices.Location = new System.Drawing.Point(5, 44);
            this.label_NumberOfDevices.Name = "label_NumberOfDevices";
            this.label_NumberOfDevices.Size = new System.Drawing.Size(101, 13);
            this.label_NumberOfDevices.TabIndex = 2;
            this.label_NumberOfDevices.Text = "Number of Devices:";
            // 
            // numericUpDownDevices
            // 
            this.numericUpDownDevices.Location = new System.Drawing.Point(108, 42);
            this.numericUpDownDevices.Name = "numericUpDownDevices";
            this.numericUpDownDevices.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownDevices.TabIndex = 3;
            this.numericUpDownDevices.ValueChanged += new System.EventHandler(this.numericUpDownDevices_ValueChanged);
            // 
            // dataGridView_Board
            // 
            this.dataGridView_Board.AllowUserToAddRows = false;
            this.dataGridView_Board.AllowUserToDeleteRows = false;
            this.dataGridView_Board.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Board.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChainID,
            this.Controller,
            this.Column1});
            this.dataGridView_Board.Location = new System.Drawing.Point(12, 96);
            this.dataGridView_Board.Name = "dataGridView_Board";
            this.dataGridView_Board.RowHeadersWidth = 62;
            this.dataGridView_Board.Size = new System.Drawing.Size(357, 228);
            this.dataGridView_Board.TabIndex = 4;
            this.dataGridView_Board.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Board_CellContentClick);
            // 
            // ChainID
            // 
            this.ChainID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ChainID.HeaderText = "Chain ID";
            this.ChainID.MinimumWidth = 8;
            this.ChainID.Name = "ChainID";
            this.ChainID.ReadOnly = true;
            this.ChainID.Width = 73;
            // 
            // Controller
            // 
            this.Controller.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Controller.HeaderText = "Controller";
            this.Controller.MaxDropDownItems = 15;
            this.Controller.MinimumWidth = 150;
            this.Controller.Name = "Controller";
            this.Controller.Width = 150;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.HeaderText = "Assign Pins";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            this.Column1.Width = 67;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 331);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(113, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Add Board";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_BoardFileName);
            this.groupBox1.Controls.Add(this.textBoxBoardFileName);
            this.groupBox1.Controls.Add(this.label_NumberOfDevices);
            this.groupBox1.Controls.Add(this.numericUpDownDevices);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(357, 65);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_loadboard,
            this.toolStripSeparator1,
            this.toolStripButton_addboard});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(391, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_loadboard
            // 
            this.toolStripButton_loadboard.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripButton_loadboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_loadboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_loadboard.Name = "toolStripButton_loadboard";
            this.toolStripButton_loadboard.Size = new System.Drawing.Size(71, 22);
            this.toolStripButton_loadboard.Text = "Load Board";
            this.toolStripButton_loadboard.ToolTipText = "Load Board";
            this.toolStripButton_loadboard.Click += new System.EventHandler(this.toolStripButton_loadboard_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_addboard
            // 
            this.toolStripButton_addboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_addboard.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_addboard.Image")));
            this.toolStripButton_addboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_addboard.Name = "toolStripButton_addboard";
            this.toolStripButton_addboard.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton_addboard.Text = "Add Board";
            this.toolStripButton_addboard.Click += new System.EventHandler(this.toolStripButton_addboard_Click);
            // 
            // AddNewBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 363);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.dataGridView_Board);
            this.Name = "AddNewBoard";
            this.Text = "Add New Board";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Board)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_loadboard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChainID;
        private System.Windows.Forms.DataGridViewComboBoxColumn Controller;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private ToolStripButton toolStripButton_addboard;
    }
}