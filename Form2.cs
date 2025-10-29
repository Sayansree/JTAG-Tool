using Eto.Parse;
using Eto.Parse.Grammars;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(Form1 parent,Device device)
        {
            InitializeComponent();
            this.parent = parent;
            this.device = device;
            parent.Hide();
            comboBox1.SelectedIndex = 8;
            initTable();
            
        }



        
        private void initTable()
        {
            InternalChange = true;
            //for (int i= 0;i<device.ADDR.Length;i++)
            //{
            //    device.util.Add(("A"+i), device.ADDR[i]);
            //}
            //for (int i = 0; i < device.DATA.Length; i++)
            //{
            //    device.util.Add(("D" + i), device.DATA[i]);
            //}
            //device.util.Add("CE", device.CE);
            //device.util.Add("OE", device.OE);
            //device.util.Add("WE", device.WE);
            //device.util.Add("RST", device.RST);
            foreach (KeyValuePair<string,PIN> entry in device.util)
            {
          
                String cells = "(" + entry.Value.input + ", " + entry.Value.output + ", " + entry.Value.ctr + ")";

                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Resizable = DataGridViewTriState.False;
                dataGridView1.Rows[index].Cells["pin_name"].Value = entry.Key;
                dataGridView1.Rows[index].Cells["cellsIOC"].Value = cells;
                
                if (entry.Value.val == INPUT)
                {
                    dataGridView1.Rows[index].Cells["pin_type"].Value = "IN";
                    dataGridView1.Rows[index].Cells["pin_type"].Style.BackColor = Color.LightGreen;
                    if(entry.Value.ctr == -1) 
                    {
                        dataGridView1.Rows[index].Cells["state_w"].Value = "X";
                        dataGridView1.Rows[index].Cells["state_w"].Style.BackColor = Color.DarkGray;
                    }
                    else
                    {
                        dataGridView1.Rows[index].Cells["state_w"].Value = "Z";
                        dataGridView1.Rows[index].Cells["state_w"].Style.BackColor = Color.DarkGray;
                    }
                    

                    disableButton(dataGridView1.Rows[index].Cells["set_high"]);
                    disableButton(dataGridView1.Rows[index].Cells["set_low"]);
                    disableButton(dataGridView1.Rows[index].Cells["set_z"]);
                    dataGridView1.Rows[index].Cells["set_z"].Value = true;
                }
                else if (entry.Value.val == OUTPUT)
                {
                    dataGridView1.Rows[index].Cells["pin_type"].Value = "OUT";
                    dataGridView1.Rows[index].Cells["pin_type"].Style.BackColor = Color.LightPink;
                    if (entry.Value.input == -1)
                    {
                        dataGridView1.Rows[index].Cells["state_r"].Value = "X";
                        dataGridView1.Rows[index].Cells["state_r"].Style.BackColor = Color.DarkGray;
                    }
                    else
                    {
                        dataGridView1.Rows[index].Cells["state_r"].Style.BackColor = Color.DarkGray;
                    }
                    if (entry.Value.ctr == -1)
                    {
                        resetButton(dataGridView1.Rows[index].Cells["set_high"]);
                        //dataGridView1.Rows[index].Cells["set_high"].Value = true;
                        setButton(dataGridView1.Rows[index].Cells["set_low"]);
                        disableButton(dataGridView1.Rows[index].Cells["set_low"]);
                        disableButton(dataGridView1.Rows[index].Cells["set_z"]);
                    }
                    else
                    {
                        setButton(dataGridView1.Rows[index].Cells["set_z"]);
                        resetButton(dataGridView1.Rows[index].Cells["set_low"]);
                        resetButton(dataGridView1.Rows[index].Cells["set_high"]);
                    }
                }
                else if (entry.Value.val == BIDIR)
                {
                    dataGridView1.Rows[index].Cells["pin_type"].Value = "INOUT";
                    dataGridView1.Rows[index].Cells["pin_type"].Style.BackColor = Color.Tan;
                    resetButton(dataGridView1.Rows[index].Cells["set_z"]);
                    resetButton(dataGridView1.Rows[index].Cells["set_low"]);
                    resetButton(dataGridView1.Rows[index].Cells["set_high"]);
                    if (entry.Value.input == -1)
                    {
                        dataGridView1.Rows[index].Cells["state_r"].Value = "X";
                        dataGridView1.Rows[index].Cells["state_r"].Style.BackColor = Color.DarkGray;
                        disableButton(dataGridView1.Rows[index].Cells["set_z"]);
                    }
                    else
                    {
                        setButton(dataGridView1.Rows[index].Cells["set_z"]);
                    }
                    if (entry.Value.output == -1)
                    {
                        dataGridView1.Rows[index].Cells["state_w"].Value = "X";
                        dataGridView1.Rows[index].Cells["state_w"].Style.BackColor = Color.DarkGray;
                        disableButton(dataGridView1.Rows[index].Cells["set_low"]);
                        disableButton(dataGridView1.Rows[index].Cells["set_high"]);
                    }else if(entry.Value.input == -1)
                    {
                        setButton(dataGridView1.Rows[index].Cells["set_low"]);
                    }
                }
                //if(entry.Key=="CE"||entry.Key=="WE"||entry.Key=="RST"||entry.Key=="OE") 
                //{
                //    setButton(dataGridView1.Rows[index].Cells["set_high"]);
                //    resetButton(dataGridView1.Rows[index].Cells["set_low"]);
                //    resetButton(dataGridView1.Rows[index].Cells["set_z"]);
                //    disableButton(dataGridView1.Rows[index].Cells["set_z"]);
                //}
                if (entry.Key == "SELECT")
                {
                    setButton(dataGridView1.Rows[index].Cells["set_high"]);
                    resetButton(dataGridView1.Rows[index].Cells["set_low"]);
                    resetButton(dataGridView1.Rows[index].Cells["set_z"]);
                }
                if (entry.Value.ctr == -1)
                {
                    dataGridView1.Rows[index].Cells["ctrl_val"].Value = "X";
                    dataGridView1.Rows[index].Cells["ctrl_val"].Style.BackColor = Color.DarkGray;
                }
                if(entry.Value.input == -1)
                {
                    dataGridView1.Rows[index].Cells["in_val"].Value = "X";
                    dataGridView1.Rows[index].Cells["in_val"].Style.BackColor = Color.DarkGray;
                }
                if(entry.Value.output == -1)
                {
                    dataGridView1.Rows[index].Cells["out_val"].Value = "X";
                    dataGridView1.Rows[index].Cells["out_val"].Style.BackColor = Color.DarkGray;
                }
                
            }
            InternalChange=false;
            button3_Click(null, null);
        }


        byte[] pack (byte[] data,byte slaveAddr, byte[] addr)
        {
            int len = data.Length;
            int addrLen = 4;//addr.Length;
            byte []res=new byte [3+len+addrLen];
            res[0]= slaveAddr;
            res[1] = 0x01; //write enum variable
            for (int i = 0; i < addrLen; i++)
                res[i+2]= addr[i];

            res[2 + addrLen] = (byte)((len == 256) ? 0 : len);
            //assuming 1,2,4x data
            for (int i = 0; i < len; i++)
            {
                res[3 + addrLen] = data[i];

            }
            return res;
        }
      
        private void disableButton(DataGridViewCell cell)
        {
            cell.Style.BackColor = Color.DarkGray;
            cell.Style.ForeColor = Color.DarkGray;
            cell.Style.SelectionBackColor = Color.DarkGray;
            cell.Style.SelectionForeColor = Color.DarkGray;
            cell.ReadOnly = true;
        }
        private void resetButton(DataGridViewCell cell)
        {
            cell.ReadOnly = false;
            cell.Value = false;
            stateColor(cell, 3);
        }
        private void setButton(DataGridViewCell cell)
        {
            cell.ReadOnly = true;
            cell.Value = true;
            byte state= (cell.ColumnIndex == 7) ? Z : (cell.ColumnIndex == 6) ? LOW : HIGH;
            stateColor(cell,state);

        }
        private bool isDisabled(DataGridViewCell cell)
        {
            return cell.Style.BackColor == Color.DarkGray;
        }
        private byte getState(int i)//changed
        {
            if ((bool)dataGridView1.Rows[i].Cells[5].Value == true) return HIGH;
            if ((bool)dataGridView1.Rows[i].Cells[6].Value == true) return LOW;
            if ((bool)dataGridView1.Rows[i].Cells[7].Value == true) return Z;
            return HIGH;
        }
        private void stateColor(DataGridViewCell cell,byte state)
        {
            if(state == 0)
            {
                cell.Style.BackColor = Color.PaleTurquoise;
                cell.Style.SelectionBackColor = Color.PaleTurquoise;
            }
            else if(state == 1) {
                cell.Style.BackColor = Color.LightPink;
                cell.Style.SelectionBackColor = Color.LightPink;
            }
            else if (state == 2)
            {
                cell.Style.BackColor = Color.LightYellow;
                cell.Style.SelectionBackColor = Color.LightYellow;
            }
            else
            {
                cell.Style.BackColor = Color.White;
                cell.Style.SelectionBackColor = Color.White;
            }
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Show();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             timer1.Interval=  Convert.ToInt32(comboBox1.SelectedItem.ToString());
        }

       

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//change
        {
            if (InternalChange) return;
            InternalChange=true;
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            if (row >= 0 && (col == 5||col==6||col==7))
            {
                //if (col==5&&(bool)dataGridView1.Rows[row].Cells[5].Value)
                //{
                //    resetButton(dataGridView1.Rows[row].Cells[6]);
                //    disableButton(dataGridView1.Rows[row].Cells[5]);
                //    dataGridView1.Rows[row].Cells[6].Value = false;
                //}
                //if (col==6&&(bool)dataGridView1.Rows[row].Cells[6].Value)
                //{
                //    resetButton(dataGridView1.Rows[row].Cells[5]);
                //    disableButton(dataGridView1.Rows[row].Cells[6]);
                //    dataGridView1.Rows[row].Cells[5].Value= false;
                //}
                resetButton(dataGridView1.Rows[row].Cells[5]);
                resetButton(dataGridView1.Rows[row].Cells[6]);
                if (!isDisabled(dataGridView1.Rows[row].Cells[7]))
                    resetButton(dataGridView1.Rows[row].Cells[7]);
                setButton(dataGridView1.Rows[row].Cells[col]);
            }
            InternalChange = false;
            if(checkBox1.Checked)
                button3_Click(sender, e);
            else
                button3.Enabled = true;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void Sample(object sender, EventArgs e) //single capture
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string pin_name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                PIN pin = device.util[pin_name];
                if (device.util[pin_name].input != -1)
                { }
            }
                    device.ReadBoundaryCells();
            //device.ReadandRestoreBoundaryCells();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string pin_name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                PIN pin = device.util[pin_name];
                if (device.util[pin_name].input != -1)
                {
                    byte val = device.getPinRead(pin);
                    dataGridView1.Rows[i].Cells["state_r"].Value = (val == HIGH) ? "H" : "L";
                    stateColor(dataGridView1.Rows[i].Cells["state_r"], val);
                    dataGridView1.Rows[i].Cells["in_val"].Value = val.ToString();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)//run sampling
        {
            if (sampling)
            {
                button2.Text = "Run Sampling";
                sampling = false;
                timer1.Stop();
                button1.Enabled = true;
            }
            else
            {
                button2.Text = "Stop Sampling";
                sampling = true;
                timer1.Start();
                button1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)//singlecapture     write change
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                string pin_name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                PIN pin = device.util[pin_name];
                if (device.util[pin_name].val != INPUT)
                {
                    byte val = getState(i);
                    device.setPinWrite(pin, val);
                    dataGridView1.Rows[i].Cells["state_w"].Value = (val == HIGH) ? "H" : (val == LOW) ? "L" : "Z";
                    stateColor(dataGridView1.Rows[i].Cells["state_w"], val);

                }
                if (pin.ctr != -1) dataGridView1.Rows[i].Cells["ctrl_val"].Value =device.readCell(pin.ctr).ToString();
                if (pin.output!= -1) dataGridView1.Rows[i].Cells["out_val"].Value = device.readCell(pin.output);

            }
            device.WriteBoundaryCells();
            button3.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (button3.Enabled)
            {
                button3_Click(sender, e);
            }
        }

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    device.flashRead(0x00);
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    device.flashWrite(0x00,0x00);
        //}

        //private int getAddress()
        //{
        //    return (Convert.ToInt32(textBox4.Text, 16) << 24) + (Convert.ToInt32(textBox2.Text, 16) << 16)
        //        + (Convert.ToInt32(textBox1.Text, 16) << 8) + Convert.ToInt32(textBox3.Text, 16);
        //}
        //private void setAddress(int x)
        //{
        //    string s = String.Format("{0:X8}", x);
        //    textBox4.Text = s.Substring(0, 2);
        //    textBox2.Text = s.Substring(2, 2);
        //    textBox1.Text = s.Substring(4, 2);
        //    textBox3.Text = s.Substring(6, 2);
        //}
        //private int getData()
        //{
        //    return (Convert.ToInt32(textBox5.Text, 16) << 24) + (Convert.ToInt32(textBox6.Text, 16) << 16)
        //          + (Convert.ToInt32(textBox7.Text, 16) << 8) + Convert.ToInt32(textBox8.Text, 16);
        //}
        //private void setData(int x)
        //{
        //    string s = String.Format("{0:X8}", x);
        //    textBox5.Text = s.Substring(0, 2);
        //    textBox6.Text = s.Substring(2, 2);
        //    textBox7.Text = s.Substring(4, 2);
        //    textBox8.Text = s.Substring(6, 2);
        //}
        //private bool isHex(string s)
        //{
        //    char[] allowedChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        //    foreach (char c in s.ToUpper())
        //    {

        //        if (!allowedChars.Contains(c) || !allowedChars.Contains(c))
        //        {
        //           return false;
        //        }
        //    }
        //    return true;
        //}
        //private void AddrBox_Validating(object sender, CancelEventArgs e)
        //{
        //    int maxADDR = (1 << device.ADDR.Length) - 1;
        //    TextBox t = sender as TextBox;
        //    if(!isHex(t.Text))
        //    {
        //        MessageBox.Show("'"+t.Text + "' is not Not a hexadecimal Number");
        //        e.Cancel = true;
        //        setAddress(maxADDR);
        //    }
        //    int currVal = getAddress();
        //    if (currVal>maxADDR)
        //    {
        //        e.Cancel = true;
        //        MessageBox.Show(String.Format("Address Value Cannot Exceed {0:X8}",maxADDR));
        //        setAddress(maxADDR);
        //    }

        //}
        //private void DataBox_Validating(object sender, CancelEventArgs e)
        //{
        //    int maxDATA = (1 << device.DATA.Length) - 1;
        //    TextBox t = sender as TextBox;
        //    if (!isHex(t.Text))
        //    {
        //        MessageBox.Show("'" + t.Text + "' is not Not a hexadecimal Number");
        //        e.Cancel = true;
        //        setData(maxDATA);
        //    }
        //    int currVal = getData();
        //    if (currVal > maxDATA)
        //    {
        //        e.Cancel = true;
        //        MessageBox.Show(String.Format("Data Value Cannot Exceed {0:X8}", maxDATA));
        //        setData(maxDATA);
        //    }
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    int word= device.flashRead(getAddress());
        //    setData(word);
        //    label8.Text =String.Format(" Read <{1:X8}> from\nlocation [{0:X8}] ", getAddress(),getData());

        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    device.flashWrite(getAddress(), getData());
        //    label8.Text = String.Format(" Wrote <{1:X8}> to\nlocation [{0:X8}] ", getAddress(), getData());
        //}
    }
}
