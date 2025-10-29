using WindowsFormsApp1;

namespace WindowsFormsApp1
{


    partial class CreateBoardFile

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
            this.groupBoxControllerInfo = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFlashInterface = new System.Windows.Forms.ComboBox();
            this.labelInstructionBypass = new System.Windows.Forms.Label();
            this.labelInstructionExtest = new System.Windows.Forms.Label();
            this.labelInstructionLength = new System.Windows.Forms.Label();
            this.labelDeviceId = new System.Windows.Forms.Label();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBoxAddToBoardFile = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelPinName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelPinLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelPinType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.checkBoxPinDefaultValue = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.comboBoxFlashInterfaceCell = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SaveAsDefaultCB = new System.Windows.Forms.CheckBox();
            this.groupBoxControllerInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxControllerInfo
            // 
            this.groupBoxControllerInfo.Controls.Add(this.label1);
            this.groupBoxControllerInfo.Controls.Add(this.comboBoxFlashInterface);
            this.groupBoxControllerInfo.Controls.Add(this.labelInstructionBypass);
            this.groupBoxControllerInfo.Controls.Add(this.labelInstructionExtest);
            this.groupBoxControllerInfo.Controls.Add(this.labelInstructionLength);
            this.groupBoxControllerInfo.Controls.Add(this.labelDeviceId);
            this.groupBoxControllerInfo.Controls.Add(this.labelDeviceName);
            this.groupBoxControllerInfo.Location = new System.Drawing.Point(10, 10);
            this.groupBoxControllerInfo.Name = "groupBoxControllerInfo";
            this.groupBoxControllerInfo.Size = new System.Drawing.Size(667, 83);
            this.groupBoxControllerInfo.TabIndex = 0;
            this.groupBoxControllerInfo.TabStop = false;
            this.groupBoxControllerInfo.Text = "Controller Information";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(438, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Flash Type:";
            // 
            // comboBoxFlashInterface
            // 
            this.comboBoxFlashInterface.DisplayMember = "None";
            this.comboBoxFlashInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFlashInterface.FormattingEnabled = true;
            this.comboBoxFlashInterface.Items.AddRange(new object[] {
            "None",
            "Parallel",
            "Serial"});
            this.comboBoxFlashInterface.Location = new System.Drawing.Point(539, 47);
            this.comboBoxFlashInterface.Name = "comboBoxFlashInterface";
            this.comboBoxFlashInterface.Size = new System.Drawing.Size(104, 21);
            this.comboBoxFlashInterface.TabIndex = 5;
            this.comboBoxFlashInterface.ValueMember = "None";
            this.comboBoxFlashInterface.SelectedIndexChanged += new System.EventHandler(this.comboBoxFlashInterface_SelectedIndexChanged);
            // 
            // labelInstructionBypass
            // 
            this.labelInstructionBypass.AutoSize = true;
            this.labelInstructionBypass.Location = new System.Drawing.Point(205, 52);
            this.labelInstructionBypass.Name = "labelInstructionBypass";
            this.labelInstructionBypass.Size = new System.Drawing.Size(102, 13);
            this.labelInstructionBypass.TabIndex = 4;
            this.labelInstructionBypass.Text = "Instruction Bypass  :";
            // 
            // labelInstructionExtest
            // 
            this.labelInstructionExtest.AutoSize = true;
            this.labelInstructionExtest.Location = new System.Drawing.Point(205, 28);
            this.labelInstructionExtest.Name = "labelInstructionExtest";
            this.labelInstructionExtest.Size = new System.Drawing.Size(100, 13);
            this.labelInstructionExtest.TabIndex = 3;
            this.labelInstructionExtest.Text = "Instruction Extest   :";
            // 
            // labelInstructionLength
            // 
            this.labelInstructionLength.AutoSize = true;
            this.labelInstructionLength.Location = new System.Drawing.Point(441, 28);
            this.labelInstructionLength.Name = "labelInstructionLength";
            this.labelInstructionLength.Size = new System.Drawing.Size(101, 13);
            this.labelInstructionLength.TabIndex = 2;
            this.labelInstructionLength.Text = "Instruction Length  :";
            // 
            // labelDeviceId
            // 
            this.labelDeviceId.AutoSize = true;
            this.labelDeviceId.Location = new System.Drawing.Point(5, 52);
            this.labelDeviceId.Name = "labelDeviceId";
            this.labelDeviceId.Size = new System.Drawing.Size(79, 13);
            this.labelDeviceId.TabIndex = 1;
            this.labelDeviceId.Text = "Device ID       :";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Location = new System.Drawing.Point(5, 26);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(78, 13);
            this.labelDeviceName.TabIndex = 0;
            this.labelDeviceName.Text = "Device Name :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkBoxAddToBoardFile,
            this.labelPinName,
            this.labelPinLocation,
            this.labelPinType,
            this.checkBoxPinDefaultValue,
            this.comboBoxFlashInterfaceCell});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(10, 98);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(665, 280);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // checkBoxAddToBoardFile
            // 
            this.checkBoxAddToBoardFile.HeaderText = "Add to Board File";
            this.checkBoxAddToBoardFile.MinimumWidth = 8;
            this.checkBoxAddToBoardFile.Name = "checkBoxAddToBoardFile";
            this.checkBoxAddToBoardFile.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.checkBoxAddToBoardFile.Width = 120;
            // 
            // labelPinName
            // 
            this.labelPinName.HeaderText = "Pin Name";
            this.labelPinName.MinimumWidth = 8;
            this.labelPinName.Name = "labelPinName";
            this.labelPinName.ReadOnly = true;
            this.labelPinName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.labelPinName.Width = 120;
            // 
            // labelPinLocation
            // 
            this.labelPinLocation.HeaderText = "Pin Location";
            this.labelPinLocation.MinimumWidth = 8;
            this.labelPinLocation.Name = "labelPinLocation";
            this.labelPinLocation.ReadOnly = true;
            this.labelPinLocation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.labelPinLocation.Width = 120;
            // 
            // labelPinType
            // 
            this.labelPinType.HeaderText = "Pin Type";
            this.labelPinType.MinimumWidth = 8;
            this.labelPinType.Name = "labelPinType";
            this.labelPinType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.labelPinType.Width = 120;
            // 
            // checkBoxPinDefaultValue
            // 
            this.checkBoxPinDefaultValue.HeaderText = "Pin Default Value";
            this.checkBoxPinDefaultValue.MinimumWidth = 8;
            this.checkBoxPinDefaultValue.Name = "checkBoxPinDefaultValue";
            this.checkBoxPinDefaultValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.checkBoxPinDefaultValue.Width = 120;
            // 
            // comboBoxFlashInterfaceCell
            // 
            this.comboBoxFlashInterfaceCell.HeaderText = "Flash Interface";
            this.comboBoxFlashInterfaceCell.MinimumWidth = 8;
            this.comboBoxFlashInterfaceCell.Name = "comboBoxFlashInterfaceCell";
            this.comboBoxFlashInterfaceCell.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.comboBoxFlashInterfaceCell.Width = 150;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(10, 392);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(86, 26);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SaveAsDefaultCB
            // 
            this.SaveAsDefaultCB.AutoSize = true;
            this.SaveAsDefaultCB.Location = new System.Drawing.Point(112, 395);
            this.SaveAsDefaultCB.Name = "SaveAsDefaultCB";
            this.SaveAsDefaultCB.Size = new System.Drawing.Size(183, 17);
            this.SaveAsDefaultCB.TabIndex = 3;
            this.SaveAsDefaultCB.Text = "Save as Default for this Controller";
            this.SaveAsDefaultCB.UseVisualStyleBackColor = true;
            // 
            // CreateBoardFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 428);
            this.Controls.Add(this.SaveAsDefaultCB);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBoxControllerInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CreateBoardFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controller Configuration";
            this.groupBoxControllerInfo.ResumeLayout(false);
            this.groupBoxControllerInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

             BSDLObject bobj;


            private System.Windows.Forms.GroupBox groupBoxControllerInfo;
            private System.Windows.Forms.Label labelDeviceName;
            private System.Windows.Forms.Label labelDeviceId;
            private System.Windows.Forms.Label labelInstructionLength;
            private System.Windows.Forms.Label labelInstructionExtest;
            private System.Windows.Forms.Label labelInstructionBypass;
            private System.Windows.Forms.ComboBox comboBoxFlashInterface;
            private System.Windows.Forms.DataGridView dataGridView1;
            private System.Windows.Forms.DataGridViewCheckBoxColumn checkBoxAddToBoardFile;
            private System.Windows.Forms.DataGridViewTextBoxColumn labelPinName;
            private System.Windows.Forms.DataGridViewTextBoxColumn labelPinLocation;
            private System.Windows.Forms.DataGridViewComboBoxColumn labelPinType;
            private System.Windows.Forms.DataGridViewCheckBoxColumn checkBoxPinDefaultValue;
            private System.Windows.Forms.DataGridViewComboBoxColumn comboBoxFlashInterfaceCell;
            private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox SaveAsDefaultCB;
    }
    }

