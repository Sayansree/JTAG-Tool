using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class AddNewBoard : Form
    {


        List<Controller> carr;
        BoardFile brd;
        bool newFile = true;

        public AddNewBoard()
        {
            carr = new List<Controller>();
            brd = new BoardFile();
            InitializeComponent();
            PopulateDropdownColumn();
        }

        //private Populate

        // Assuming you have a DataGridView control named dataGridView
        // Assuming you have a column named "Controller" in the DataGridView as a DataGridViewComboBoxColumn

        private void PopulateDropdownColumn()
        {
            // Folder path where the JSON files are located
           
            string folderPath ="Controllers";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            // Get the file names from the folder
            List<string> fileNamesList = Directory.GetFiles(folderPath, "*.json")
                                .Select(Path.GetFileName)
                                .Select(Path.GetFileNameWithoutExtension)
                                .Append("New").ToList();
            for(int i = 0; i < fileNamesList.Count; i++)
            {
                if (fileNamesList[i].Length>8&&fileNamesList[i].Substring(0, 8) == "default-")
                {
                    fileNamesList.Remove(fileNamesList[i]);
                    i--;
                }
            }
            // Assuming "Controller" is the name of the DataGridViewComboBoxColumn
            if (dataGridView_Board.Columns["Controller"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                // Clear existing items in the dropdown column
                comboBoxColumn.Items.Clear();

                // Add the file names as items in the dropdown column
                comboBoxColumn.Items.AddRange(fileNamesList.ToArray());
            }
        }

        private void PopulateDropdownColumn(BoardFile bf)
        {
            // Folder path where the JSON files are located

            string folderPath = "Controllers";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            // Get the file names from the folder
            List<string> fileNamesList = Directory.GetFiles(folderPath, "*.json")
                                .Select(Path.GetFileName)
                                .Select(Path.GetFileNameWithoutExtension)
                                .Append("New").ToList();
            for (int i = 0; i < fileNamesList.Count; i++)
            {
                if (fileNamesList[i].Length > 8 && fileNamesList[i].Substring(0, 8) == "default-")
                {
                    fileNamesList.Remove(fileNamesList[i]);
                    i--;
                }
            }
            // Assuming "Controller" is the name of the DataGridViewComboBoxColumn
            if (dataGridView_Board.Columns["Controller"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                // Clear existing items in the dropdown column
                comboBoxColumn.Items.Clear();

                // Add the file names as items in the dropdown column
                comboBoxColumn.Items.AddRange(fileNamesList.ToArray());
            }
        }

        // Board File Name Validation
        private void textBoxBoardFileName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxBoardFileName.Text))
            {
                e.Cancel = true;
                textBoxBoardFileName.Focus();
                errorProvider.SetError(textBoxBoardFileName, "Board File Name is required");
            }
            else
            {
                errorProvider.SetError(textBoxBoardFileName, null);
            }
        }


        // On Saving the Board File
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Handle the save button click event
            // Add your code here to save the data
            for (int i = 0; i < dataGridView_Board.Rows.Count; i++)
            {
                string controllerName = dataGridView_Board.Rows[i].Cells[1].Value.ToString();
                string filename = "Controllers\\" + controllerName + ".json";
                if (controllerName != "" && File.Exists(filename))
                {
                    try
                    {
                        BSDLObject load = JsonConvert.DeserializeObject<BSDLObject>(File.ReadAllText(filename));
                        foreach(InstructionOpcode io in load.instruction_register_description.instruction_opcodes)
                        {
                            if(io.instruction_name == "EXTEST")
                            {
                                carr[i].EXTEXT = io.opcode_list[0];
                            }
                        }
                        string strID = load.optional_register_description.idcode_register[0] + load.optional_register_description.idcode_register[1] + load.optional_register_description.idcode_register[2];
                        carr[i].devid = string.Format("\t0x{0:X8}", Convert.ToInt64(strID, 2));
                        carr[i].deviceName = load.component_name;
                        carr[i].IRLEN = Convert.ToInt32(load.instruction_register_description.instruction_length);
                        carr[i].BSLEN = Convert.ToInt32(load.boundary_scan_register_description.fixed_boundary_stmts.boundary_length);                       
                        // carr[i].flashType
                        // carr[i].pins;
                    }
                    catch (Exception ex)
                    {
                        //
                    }

                    brd.controllerCount = carr.Count;
                    brd.boardName =textBoxBoardFileName.Text;
                    brd.controllerArray = carr;
                     string jsonContent = JsonConvert.SerializeObject(brd);
                    string filepath = "Boards\\" + brd.boardName + ".json";             ///
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(filepath)))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filepath));
                    File.WriteAllText(filepath, jsonContent);
                }

                if (dataGridView_Board.Rows[i].Cells[1] == null)
                {
                    MessageBox.Show("Board File not created. Please select controllers before saving!", "Pending Controller Addition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ( carr[i].pins == null)
                {
                    MessageBox.Show("Board File not created. Please assign controllers pins before saving!", "Pending Controller Addition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            MessageBox.Show("Board Created successfully!");
        }

        // Add or Remove Row in Data Grid using numeric up and down
        private void numericUpDownDevices_ValueChanged(object sender, EventArgs e)
        {
            // Update the number of rows in the DataGridView based on the numeric up-down value
            int rowCount = Convert.ToInt32(numericUpDownDevices.Value);
            if (dataGridView_Board.Rows.Count < rowCount)
            {
                for (int i = dataGridView_Board.Rows.Count; i < rowCount; i++)
                {
                    dataGridView_Board.Rows.Add(i, null);
                    carr.Add(new Controller());
                }
            }
            else if (dataGridView_Board.Rows.Count > rowCount)
            {
                for (int i = dataGridView_Board.Rows.Count - 1; i >= rowCount; i--)
                {
                    dataGridView_Board.Rows.RemoveAt(i);
                    carr.RemoveAt(i);
                }
            }

        }



        /*
         * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
         * */
        private void toolStripButton_addboard_Click(object sender, EventArgs e)
        {
            newFile = true;
            numericUpDownDevices.Enabled = true;
            dataGridView_Board.Rows.Clear();
            textBoxBoardFileName.ReadOnly = false;
            textBoxBoardFileName.Text = "";
            numericUpDownDevices.ReadOnly = false;

        }
        private void toolStripButton_loadboard_Click(object sender, EventArgs e)
        {

            newFile = false;
            numericUpDownDevices.Enabled = false;
           // toolStripButton_loadboard.Text=
            string initialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = initialDirectory + "\\Boards";
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    try
                    {
                        string json = File.ReadAllText(selectedFilePath);

                        BoardFile load = JsonConvert.DeserializeObject<BoardFile>(json);

                        // Access and display the parameter values on the UI
                        string BoardName = load.boardName;
                        int NumberOfControllers = load.controllerCount;
                        textBoxBoardFileName.Text = BoardName;
                        numericUpDownDevices.Text = NumberOfControllers.ToString();
                        brd = load;
                        carr = load.controllerArray;

                        // Clear the existing rows in the DataGridView
                        dataGridView_Board.Rows.Clear();
                        // Iterate over the data array and add rows to the DataGridView
                        int i = 0;
                        foreach (Controller c in carr)
                        {
                            // Add a new row to the DataGridView
                            ///// Add button here ---------------------
                            dataGridView_Board.Rows.Add(i.ToString(),"", "");
                            
                            DataGridViewComboBoxCell dd = (DataGridViewComboBoxCell)(dataGridView_Board.Rows[i].Cells[1]);
                            dd.DataSource = new List<string>() { c.deviceName.ToString() };
                            dd.Selected = true;
                            dd.Value = c.deviceName.ToString();
                            i++;
                        }
                       
                        // Disabling writing in the parameters of the add board file
                        textBoxBoardFileName.ReadOnly = true;
                        numericUpDownDevices.ReadOnly = true;
                        foreach (DataGridViewRow row in dataGridView_Board.Rows)
                        {
                            row.ReadOnly = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading JSON file: " + ex.Message);
                    }
                }
            }
        }


        public void update(List<BoardPins>l,string fint,int ind)
        {
            carr[ind].flashType = fint;
            carr[ind].pins = l;

        }

        private void dataGridView_Board_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (!newFile)
                {
                    new CreateBoardFile(this, carr[e.RowIndex].deviceName, carr[e.RowIndex].pins, carr[e.RowIndex].flashType, e.RowIndex);
                }
                else
                {
                    string controllerName = dataGridView_Board.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();
                    if (controllerName == "New")
                    {
                        new CreateBoardFile(this, e.RowIndex);
                    }
                    else
                    {
                        new CreateBoardFile(this, controllerName, e.RowIndex);
                    }
                }
            }
            
        }


    }
}
