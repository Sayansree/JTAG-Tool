using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
    public partial class CreateBoardFile : Form
    {

        private List<string> serialFlashOptions = new List<string>
        {
            "None","SF_CSZ", "SF_CLK", "SF_DATA00", "SF_DATA01", "SF_DATA02", "SF_DATA03", "SF_DATA04", "SF_DATA06", "SF_DATA07"
        };
        private List<string>NoneFlashOptions = new List<string> {"None"};

        private List<string> parallelFlashOptions = new List<string>
        {
            "None","PF_DATA00", "PF_DATA01", "PF_DATA02", "PF_DATA03", "PF_DATA04", "PF_DATA05", "PF_DATA06", "PF_DATA07",
            "PF_DATA08", "PF_DATA09", "PF_DATA10", "PF_DATA11", "PF_DATA12", "PF_DATA13", "PF_DATA14", "PF_DATA15",
            "PF_ADDR00", "PF_ADDR01", "PF_ADDR02", "PF_ADDR03", "PF_ADDR04", "PF_ADDR05", "PF_ADDR06", "PF_ADDR07",
            "PF_ADDR08", "PF_ADDR09", "PF_ADDR10", "PF_ADDR11", "PF_ADDR12", "PF_ADDR13", "PF_ADDR14", "PF_ADDR15",
            "PF_ADDR16", "PF_ADDR17", "PF_ADDR18", "PF_ADDR19", "PF_ADDR20", "PF_ADDR21", "PF_ADDR22", "PF_ADDR23",
            "PF_CSZ", "PF_OEZ", "PF_WRZ", "PF_RESET"
        };
        Dictionary<int, PIN> pin_dic;
        Dictionary<string, DataGridViewRow> dataGridDict= new Dictionary<string, DataGridViewRow>();

        AddNewBoard sender;
        int ind;

        public CreateBoardFile(AddNewBoard sender,int ind)
        {
            this.sender= sender;this.ind=ind;
            InitializeComponent();
            comboBoxFlashInterface.SelectedIndex = 0;
            bobj = parser.transform();
            this.BringToFront();
            if(bobj == null)
            {
                    MessageBox.Show("Encountered some problem while parsing BSDL file\nPlease recheck the BSDL file is in proper format");
                //destroy this object
            }
            else
            {
                LoadControllerData();
                PopulateDataGridView();
                this.Show();
            }

        }
        public CreateBoardFile(AddNewBoard sender, string controlleNname,List<BoardPins> bp, string ft,int ind)
        {
            this.sender= sender;  this.ind = ind;
            InitializeComponent();
            comboBoxFlashInterface.SelectedIndex = 0;
            string fileName = "Controllers\\" + controlleNname + ".json";
            bobj = parser.readBDSL(fileName);
            if (bobj == null)
            {
                MessageBox.Show("Encountered some problem while parsing BSDL file\nPlease recheck the BSDL file is in proper format");
                //destroy this object
            }
            else
            {
                LoadControllerData();
                PopulateDataGridView();
                LoadDefaultState(bp,ft);
                this.Show();
            }
        }

        public CreateBoardFile(AddNewBoard sender,string controlleNname, int ind )
        {
            this.sender = sender; this.ind = ind;
            InitializeComponent();
            comboBoxFlashInterface.SelectedIndex = 0;
            string fileName = "Controllers\\" + controlleNname + ".json";
            bobj = parser.readBDSL(fileName);
            if (bobj == null)
            {
                MessageBox.Show("Encountered some problem while parsing BSDL file\nPlease recheck the BSDL file is in proper format");
                //destroy this object
            }
            else
            {
                LoadControllerData();
                PopulateDataGridView();
                LoadDefaultState(controlleNname);
                this.Show();
            }
        }

        private void LoadControllerData()
        {
            string strID = bobj.optional_register_description.idcode_register[0] + bobj.optional_register_description.idcode_register[1] + bobj.optional_register_description.idcode_register[2];
            labelDeviceId.Text +=string.Format("\t0x{0:X8}", Convert.ToInt64(strID, 2)) ;
            labelDeviceName.Text += bobj.component_name;
            labelInstructionBypass.Text += "0b"+bobj.instruction_register_description.instruction_opcodes[0].opcode_list[0];
            labelInstructionExtest.Text += "0b"+bobj.instruction_register_description.instruction_opcodes[1].opcode_list[0];
            labelInstructionLength.Text += bobj.instruction_register_description.instruction_length.ToString()+" bits";
        }

        private void PopulateDataGridView()
        {
            dataGridView1.Rows.Clear();
            Dictionary<string, string> pin_loc_dic = new Dictionary<string, string>();// pinName ,pin loc 
            pin_dic=new Dictionary<int,PIN>();
            foreach(var pinData in bobj.device_package_pin_mappings[0].pin_map)
            {
                if (pin_loc_dic.ContainsKey(pinData.port_name))
                    pin_loc_dic[pinData.port_name] += "," + pinData.pin_list[0];
                else
                    pin_loc_dic.Add(pinData.port_name, pinData.pin_list[0]);
            }
            int i = 0;
            foreach (var pinData in bobj.boundary_scan_register_description.fixed_boundary_stmts.boundary_register)
            {
                if (pinData.cell_info.cell_spec.function == "CONTROL") continue;
                string pin_name = pinData.cell_info.cell_spec.port_id;
                string pin_loc = (pin_loc_dic.ContainsKey(pin_name)) ? pin_loc_dic[pin_name] : "NA";
                dataGridView1.Rows.Add(
                            false,
                            pin_name,
                            pin_loc,
                            "",
                            true,   
                            ""
                        );
                //pin_dic = new Dictionary<int, PIN>();//<bcell,pin>//pin dict
                int bcn = Convert.ToInt32(pinData.cell_number);
                int ctr = (pinData.cell_info.input_or_disable_spec == null) ? -1 :
                    Convert.ToInt32(pinData.cell_info.input_or_disable_spec.control_cell);
                string v = pinData.cell_info.cell_spec.function;
                int val = (v == "BIDIR" || v == "INOUT") ? 0 : (v == "INPUT" || v == "IN") ? -1 : 1;
                pin_dic.Add(i, new PIN(ctr, bcn, bcn, val));//assumes same in out cell
                //pin mode 
                DataGridViewComboBoxCell dd = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells["labelPinType"]);
                if (val == 0)
                {
                    dd.DataSource =new List<string>(){ "INOUT","IN","OUT"};
                    dd.Selected = true;
                    dd.Value = "INOUT";
                }
                else if (val == 1)
                {
                    dd.DataSource = new List<string>() { "OUT" };
                    dd.Selected = true;
                    dd.Value = "OUT";
                }
                else
                {
                    dd.DataSource = new List<string>() { "INP" };
                    dd.Selected = true;
                    dd.Value = "INP";
                }
               
                //row dictionary
                dataGridDict.Add(pinData.cell_number, dataGridView1.Rows[i]);
                i++;
            }
            comboBoxFlashInterface_SelectedIndexChanged(null, null);
        }

        private void LoadDefaultState(string controllerName)
        {
            string filename = "Controllers\\default-" + controllerName + ".json";
            if (controllerName!=""&&File.Exists(filename))
                try
                {

                    List<ControllerDefault> load = JsonConvert.DeserializeObject<List<ControllerDefault>>(File.ReadAllText(filename));

                    comboBoxFlashInterface.SelectedItem = load[0].FlashType;
                    for (int i = 0; i < load.Count; i++)
                    {
                        dataGridDict[load[i].pinNumber].Cells["checkBoxAddToBoardFile"].Selected = load[i].selected;
                        dataGridDict[load[i].pinNumber].Cells["labelPinType"].Value = load[i].Type;
                        dataGridDict[load[i].pinNumber].Cells["checkBoxPinDefaultValue"].Value = load[i].safe;
                        dataGridDict[load[i].pinNumber].Cells["comboBoxFlashInterfaceCell"].Value = load[i].FlashInterface;
                    }
                }catch (Exception ex)
                {
                    //
                }
            
        }

        private void LoadDefaultState(List<BoardPins> bp,string flashType)
        {
            //string filename = "Controllers\\default-" + controllerName + ".json";
           // if (bp)
                try
                {
                    comboBoxFlashInterface.SelectedItem = flashType;
                    for (int i = 0; i < bp.Count; i++)
                    {
                    dataGridView1.Rows[i].Cells["checkBoxAddToBoardFile"].Value = bp[i].selected;
                    dataGridView1.Rows[i].Cells["labelPinType"].Value = bp[i].Type;
                    dataGridView1.Rows[i].Cells["checkBoxPinDefaultValue"].Value = bp[i].safe;
                    dataGridView1.Rows[i].Cells["comboBoxFlashInterfaceCell"].Value = bp[i].FlashInterface;
                    }
                }
                catch (Exception ex)
                {
                    //
                }

        }
        private void comboBoxFlashInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedFlashInterface = comboBoxFlashInterface.SelectedItem.ToString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["comboBoxFlashInterfaceCell"];
                // DataGridViewComboBoxCell comboBoxCell1 = (DataGridViewComboBoxCell)dataGridView1.Rows[1].Cells["comboBoxFlashInterfaceCell"];
                if (dataGridView1.Rows[i].Cells["labelPinType"].Value.ToString() == "INPUT" || dataGridView1.Rows[i].Cells["labelPinType"].Value.ToString() == "IN")
                {

                    comboBoxCell.Value = "None";
                    comboBoxCell.DataSource = NoneFlashOptions;
                    //comboBoxCell.ReadOnly = true;
                    continue;
                }

                if (selectedFlashInterface == "Serial")
                {
                    comboBoxCell.DataSource = serialFlashOptions;
                    comboBoxCell.Value = "None";
                }
                else if (selectedFlashInterface == "Parallel")
                {

                    comboBoxCell.DataSource = parallelFlashOptions;
                    comboBoxCell.Value = "None";
                }
                else
                {
                    comboBoxCell.Value = "None";
                    comboBoxCell.DataSource = NoneFlashOptions;

                }
            }

               
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (SaveAsDefaultCB.Checked)//if save option selected
            {
                saveDefaultState();
            }
            SaveControllerConfiguration();
            
        }

        private void saveDefaultState()
        {
            //logic to save default
            try
            {
                List<ControllerDefault> lis = new List<ControllerDefault>();
                foreach (KeyValuePair<string, DataGridViewRow> entry in dataGridDict)
                {
                    ControllerDefault obj = new ControllerDefault();
                    obj.pinNumber = entry.Key;
                    obj.selected = (bool)entry.Value.Cells["checkBoxAddToBoardFile"].Value;
                    obj.Type = entry.Value.Cells["labelPinType"].Value.ToString();
                    obj.safe = (bool)entry.Value.Cells["checkBoxPinDefaultValue"].Value;
                    obj.FlashType = comboBoxFlashInterface.SelectedItem.ToString();
                    obj.FlashInterface = entry.Value.Cells["comboBoxFlashInterfaceCell"].Value.ToString();
                    lis.Add(obj);

                }

                File.WriteAllText("Controllers\\default-" + bobj.component_name + ".json", JsonConvert.SerializeObject(lis));
                MessageBox.Show("Controller configuration saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save controller configuration. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SaveControllerConfiguration()
        {

            //logic to call parent function to transfer data
            //............................   
            List<BoardPins> l=new List<BoardPins>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                string dd = ((DataGridViewComboBoxCell)(row.Cells["labelPinType"])).Value.ToString();
                BoardPins boardPins = new BoardPins();
                PIN p = pin_dic[i];
                p.val=(dd == "INOUT") ? 0 : (dd == "OUT") ? 1 : -1;
                boardPins.pinMap = row.Cells["labelPinLocation"].Value.ToString(); ;
                boardPins.pinNumber=i;
                boardPins.pinName = row.Cells["labelPinName"].Value.ToString();
                boardPins.pin=p;
                boardPins.Type = dd;
                boardPins.selected = (bool)row.Cells["checkBoxAddToBoardFile"].Value;
                boardPins.safe = (bool)row.Cells["checkBoxPinDefaultValue"].Value;
                boardPins.FlashInterface= row.Cells["comboBoxFlashInterfaceCell"].Value?.ToString();
                l.Add(boardPins);
            }

            string fint=comboBoxFlashInterface.SelectedItem.ToString();

            sender.update(l,fint,ind);
            //List<ControllerConfiguration> controllerConfigurations = new List<ControllerConfiguration>();

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    bool addToBoardFile = (bool)row.Cells["checkBoxAddToBoardFile"].Value;
            //    string pinName = row.Cells["labelPinName"].Value.ToString();
            //    string pinLocation = row.Cells["labelPinLocation"].Value.ToString();
            //    string pinType = row.Cells["labelPinType"].Value.ToString();
            //    bool pinDefaultValue = (bool)row.Cells["checkBoxPinDefaultValue"].Value;
            //    string flashInterface = row.Cells["comboBoxFlashInterfaceCell"].Value?.ToString();

            //    ControllerConfiguration controllerConfiguration = new ControllerConfiguration
            //    {
            //        AddToBoardFile = addToBoardFile,
            //        PinName = pinName,
            //        PinLocation = pinLocation,
            //        PinType = pinType,
            //        PinDefaultValue = pinDefaultValue,
            //        FlashInterface = flashInterface
            //    };

            //    controllerConfigurations.Add(controllerConfiguration);
            //}

            //// Convert the controller configurations to JSON
            //string jsonContent = JsonConvert.SerializeObject(l);
            //// Save the JSON to a file
            //string jsonFilePath = "controllerConfigurations.json";
            //File.WriteAllText(jsonFilePath, jsonContent);
            
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

     
    }
    public class ControllerDefault
        {
            public string pinNumber { get; set; }
            public bool selected { get; set; }
            public string Type { get; set; }
            public bool safe { get; set; }
            public string FlashType { get; set; }
            public string FlashInterface { get; set; }
    }

    


    //public class PinData
    //{
    //    public string PinName { get; set; }
    //    public string PinLocation { get; set; }
    //    public string PinType { get; set; }
    //}

    //public class ControllerConfiguration
    //{
    //    public bool AddToBoardFile { get; set; }
    //    public string PinName { get; set; }
    //    public string PinLocation { get; set; }
    //    public string PinType { get; set; }
    //    public bool PinDefaultValue { get; set; }
    //    public string FlashInterface { get; set; }
    //}
}

